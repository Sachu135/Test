using DockSample.lib;
using ScintillaNET;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UIFunctionality.Common;
using WeifenLuo.WinFormsUI.Docking;

namespace DockSample
{
    public partial class DbConnectorDoc : DockContent//Form
	{
		#region Private Fields
		DbClient dbClient;                                  // DbClient object used to talk to database server
		IBrowser browser;                                   // Browser object for displaying object browser (may be null)
		DateTime queryStartTime;                    // For  the timer showing running query time
		string fileName;                                        // Filename for when query is saved
		static int untitledCount = 1;                   // For default new filenames (Untited-1, Untitled-2, etc)
		bool realFileName = false;                  // true if default name of "untitled-x" - forces Save As... when Save is requested
		bool resultsInText = false;                 // text based results rather than grid based
		RichTextBox txtResultsBox;              // handle to the rich textbox used to display text results
		bool hideBrowser = false;                       // hide the treeview, if available
		bool initializing = true;                           // to prevent multiple updates during startup
		bool error = false;                                 // true if an error was encountered
		string lastDatabase;                                // ...so we can tell when the database has changed
		StudioConfig studioConfig;
		string connName;

		Action activated = null;
		#endregion

		#region Events

		/// <summary>
		/// Fires when a public property has changed.  This is used for enabled/disabling buttons
		/// on a toolbar.
		/// </summary>
		public event EventHandler PropertyChanged;

		#endregion

		#region Properties

		/// <summary>Returns the database client object provided in construction</summary>
		public DbClient DbClient
		{
			get { return dbClient; }
		}

		/// <summary>Returns the database browser object provided in construction</summary>
		public IBrowser Browser
		{
			get { return browser; }
		}

		DataSet DSResults
		{
			get { return DbClient.DataSet; }
		}

		/// <summary>The current state of query execution</summary>
		public RunState RunState
		{
			get { return dbClient.RunState; }
		}

		/// <summary>The filename given to the SQL query</summary>
		public string FileName
		{
			get { return fileName; }
			set
			{
				fileName = value;
				UpdateFormText();
				FirePropertyChanged();
			}
		}

		/// <summary>True if results should be displayed in textbox rather than in a grid</summary>
		public bool ResultsInText
		{
			get { return resultsInText; }
			set
			{
				resultsInText = value;
				FirePropertyChanged();
			}
		}

		#endregion
		private void DoBrowserAction(object sender, EventArgs e)
		{
			// This is called from the context menu activated by the TreeView's right-click
			// event handler (treeView_MouseUp) and appends text to the query textbox
			// applicable to the selected menu item.
			MenuItem mi = (MenuItem)sender;
			// Ask the browser for the text to append, applicable to the selected node and menu item text.
			string s = Browser.GetActionText(treeView.SelectedNode, mi.Text);
			if (s == null) return;
			if (s.Length > 200) HideResults = true;
			if (txtQuery.Text != "") txtQuery.AppendText("\r\n\r\n");
			int start = txtQuery.SelectionStart;
			txtQuery.AppendText(s);
			txtQuery.SelectionStart = start;
			//txtQuery.SelectedText.Length
			//txtQuery.SelectionLength = s.Length;
			txtQuery.SetSavePoint();
			//txtQuery.Modified = true;
			txtQuery.Focus();
		}
		void CancelDone()
		{
			SetRunning(false);
			panRunStatus.Text = "Query batch was cancelled.";
		}
		void SetRunning(bool running)
		{
			// Start the timer in the status bar
			if (running)
			{
				queryStartTime = DateTime.Now;
				UpdateExecTime();
			}
			tmrExecTime.Enabled = running;
			if (!running) CheckDatabase();
			FirePropertyChanged();
		}
		void AddTextResults()
		{
			// Note: we give this method via a delegate to our DbClient object.  The DbClient object then
			// invokes this delegate from its worker thread, as results become available.
			if (RunState == RunState.Cancelling) return;
			txtResultsBox.AppendText(dbClient.TextResults.ToString());
		}

		void AddGrid(DataTable dt)      // called by the method above
		{
			DataGrid dataGrid = new DataGrid();

			// Due to a bug in the grid control, we must add the grid to the tabpage before assigning a datasource.
			// This bug was introduced in Beta 1, was fixed for Beta 2, then reared its ugly head again in RTM.
			TabPage tabPage = new TabPage("Result Set " + (tabControl.TabCount).ToString());
			tabPage.Controls.Add(dataGrid);
			tabControl.TabPages.Add(tabPage);

			dataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			dataGrid.CaptionVisible = false;
			dataGrid.ReadOnly = true;

			DataGridTableStyle ts = new DataGridTableStyle();
			ts.MappingName = dt.TableName;
			dataGrid.TableStyles.Clear();
			dataGrid.TableStyles.Add(ts);
			dataGrid.SetDataBinding(dt, "");

			// The auto sizing feature below is no longer supported in RTM.
			// dataGrid.PreferredColumnWidth = -1;
			// Instead we'll have to size each column manually.
			// A graphics object is required to measure text so we can size the grid columns correctly
			System.Drawing.Graphics g = CreateGraphics();

			// For each column, determine the largest visible text string, and use that to size the column
			// We'll be measuring text for each row that's visible in the grid
			int maxRows = Math.Min(dataGrid.VisibleRowCount, dt.Rows.Count);
			GridColumnStylesCollection cols = ts.GridColumnStyles;
			const int margin = 6;       // allow 6 pixels per column, for grid lines and some white space
			int colNum = 0;
			if (cols.Count == 1)
				cols[0].Width = dataGrid.Width;
			else
				foreach (DataGridColumnStyle col in cols)
				{
					int maxWidth = (int)g.MeasureString(col.HeaderText, dataGrid.Font).Width + margin;
					for (int row = 0; row < maxRows; row++)
					{
						string s = dt.Rows[row][colNum, DataRowVersion.Current].ToString();
						int length = (int)g.MeasureString(s, dataGrid.Font).Width + margin;
						maxWidth = Math.Max(maxWidth, length);
					}
					// Assign length of longest string to the column width, but don't exceed width of actual grid.
					col.Width = Math.Min(dataGrid.Width, maxWidth);
					colNum++;
				}
			g.Dispose();

			// Set datetime columns to show the time as well as the date
			colNum = 0;
			foreach (DataGridColumnStyle col in cols)
			{
				DataGridTextBoxColumn textCol = col as DataGridTextBoxColumn;
				if (textCol != null && dt.Columns[colNum].DataType == typeof(DateTime))
					// Display the date in short format (ie using numbers), and the time in long
					// format (ie including seconds).  This is done using the 'G' format string.
					textCol.Format = "G";
				colNum++;
			}
		}

		void AddGridResults()
		{
			// Note: we give this method via a delegate to our DbClient object.  The DbClient object then
			// invokes this delegate from its worker thread, when results have become available.
			const int MaxResultSets = 20;

			// Create a new tab page and grid for each new result set.  In case this has already been called,
			// (as will be the case with multiple queries, separated with the 'GO' construct) only add tab
			// pages for new result sets.
			for (int page = tabControl.TabCount - 1; page < Math.Min(MaxResultSets, DSResults.Tables.Count); page++)
				AddGrid(DSResults.Tables[page]);
		}
		/// <summary>
		/// Called when a query has successfully finished executing.
		/// </summary>
		void QueryDone()
		{
			panRunStatus.Text = "Query batch completed" + (error ? " with errors" : ".");
			// If there were no results from query, display message to provide feedback to user
			if (!ResultsInText && !error && dbClient.Messages.Count == 0)
				txtResultsBox.AppendText("The command(s) completed successfully.");
			if (dbClient.Messages.Count > 0)
			{
				if (txtResultsBox.Text.Length > 0) txtResultsBox.AppendText("\r\n");
				foreach (string msg in dbClient.Messages)
					txtResultsBox.AppendText(msg + "\r\n");
			}
			if (!ResultsInText)
				tabControl.SelectedIndex = error ? 0 : 1;
			ShowRowCount();
			SetRunning(false);
			txtQuery.Focus();
		}

		/// <summary>
		/// Called when a query has returned errors.
		/// </summary>
		void QueryFailed()
		{
			error = true;
			txtResultsBox.AppendText(dbClient.Error + "\r\n\r\n");
		}
		public void Execute()
		{
			if (RunState != RunState.Idle)
				return;

			if (HideResults) HideResults = false;
			error = false;

			// Delete any previously defined tab pages and their child controls
			tabControl.TabPages.Clear();

			TabPage tabPage = new TabPage(ResultsInText ? "Results" : "Messages");
			// We'll need a rich textbox because an ordinary textbox has limited capacity
			txtResultsBox = new RichTextBox();
			txtResultsBox.AutoSize = false;
			txtResultsBox.Dock = DockStyle.Fill;
			txtResultsBox.Multiline = true;
			txtResultsBox.WordWrap = false;
			txtResultsBox.Font = new Font("Courier New", 8);
			txtResultsBox.ScrollBars = RichTextBoxScrollBars.Both;
			txtResultsBox.MaxLength = 0;
			txtResultsBox.Text = "";
			tabControl.TabPages.Add(tabPage);
			tabPage.Controls.Add(txtResultsBox);

			// If the user has selected text within the query window, just execute the
			// selected text.  Otherwise, execute the contents of the whole textbox.
			string query = txtQuery.SelectedText.Length == 0 ? txtQuery.Text : txtQuery.SelectedText;
			if (query.Trim() == "") return;

			// Use the database client class to execute the query.  Create delegates which will be invoked
			// when the query completes or cancels with an error.

			MethodInvoker results, done, failed;

			if (ResultsInText)
				results = new MethodInvoker(AddTextResults);
			else
				results = new MethodInvoker(AddGridResults);

			done = new MethodInvoker(QueryDone);
			failed = new MethodInvoker(QueryFailed);

			// dbClient.Execute runs asynchronously, so control will return immediately to the calling method.

			Cursor oldCursor = Cursor;
			Cursor = Cursors.WaitCursor;
			panRunStatus.Text = "Executing Query Batch...";
			dbClient.Execute(this, results, done, failed, query, ResultsInText);        // this does the work
			SetRunning(true);
			Cursor = oldCursor;
		}
		public void Cancel()
		{
			panRunStatus.Text = "Cancelling...";
			dbClient.Cancel(new MethodInvoker(CancelDone));
			// Control will return immediately, and CancelDone will be invoked when the cancel is complete.
			FirePropertyChanged();
		}
		/// <summary>True if the "hide results" option has been selected (manually or automatically)</summary>
		public bool HideResults
		{
			get { return !tabControl.Visible; }
			set
			{
				tabControl.Visible = !value;
				txtQuery.Dock = value ? DockStyle.Fill : DockStyle.Top;
				splQuery.Visible = !value;
				FirePropertyChanged();

				//HACK: Work around bug in splitter control, where it loses its SplitPosition the first time it's made invisible
				if (splQuery.SplitPosition < 0 || splQuery.SplitPosition > ClientSize.Height - statusBar.Height - 10)
					splQuery.SplitPosition = ClientSize.Height / 2;
				else
					// If you take the following line out, the results window will be invisible when running
					// the compiled executable on another differently setup PC.
					//  This is a beta 2 bug - not sure if still present in RTM
					splQuery.SplitPosition += 0;
			}
		}

		/// <summary>True if the "hide browser" option has been selected</summary>
		public bool HideBrowser
		{
			get { return hideBrowser; }
			set
			{
				if (Browser == null && !value) return;      // Can't show browser if not available!
				hideBrowser = value;
				panBrowser.Visible = !value;                        // show/hide the browser panel containing the treeview
				splBrowser.Visible = !value;                        // show/hide the splitter
				if (!value) PopulateBrowser();
				FirePropertyChanged();
			}
		}

		// Private properties
		public DbConnectorDoc()
        {
            InitializeComponent();
        }

		public DbConnectorDoc(StudioConfig st, string connName, Action activated)
		{
			InitializeComponent();
			AutoScaleMode = AutoScaleMode.Dpi;
			DockAreas = DockAreas.Document | DockAreas.Float;
			studioConfig = st;
			this.connName = connName;
			this.activated = activated;

			IClientFactory clientFactory;
			string connectString, connectDescription;

			var dbConn = studioConfig.databaseConnections.First(c => c.ConnName == this.connName);
			clientFactory = new SqlFactory();

			//connectString = "Data Source=" + dbConn.ServerName + ";app=Query Express" + "; User ID=" + dbConn.UserName + ";Password=" + dbConn.Password;
			connectString = "Data Source=" + dbConn.ServerName + ";app=Query Express" + ";Initial Catalog=" + dbConn.DbName + "; User ID=" + dbConn.UserName + ";Password=" + dbConn.Password;
			
			connectDescription = dbConn.ServerName + " (" + dbConn.UserName + ")";
			dbClient = new DbClient(clientFactory, connectString, connectDescription);
			//dbClient.Database = dbConn.DbName;
			Cursor oldCursor = Cursor;
			Cursor = Cursors.WaitCursor;

			bool success = dbClient.Connect();
			Cursor = oldCursor;

			browser = new SqlBrowser(dbClient);

			this.txtQuery.Styler = new EasyScintilla.Stylers.SqlStyler();
			txtQuery.Lexer = Lexer.Sql;

			//QueryForm qf = new QueryForm(cf.DbClient, cf.Browser, cf.LowBandwidth);
			//qf.MdiParent = this;
			// This is so that we can update the toolbar and menu as the state of the QueryForm changes.
			PropertyChanged += new EventHandler(ChildPropertyChanged);
			SetRunning(false);
			//qf.Show();
		}

		/// <summary>
		/// We'll attach the PropertyChanged event of the QueryForms to this handler, so we can
		/// update the toolbar and menu items when the state of a QueryForm changes.
		/// </summary>
		void ChildPropertyChanged(object sender, EventArgs e)
		{
			EnableControls();
		}

		/// <summary>
		/// Enable / Disable toolbar and menu items
		/// </summary>
		void EnableControls()
		{
			/*QueryForm q;
			bool active = IsChildActive();
			if (active) q = GetQueryChild(); else q = null;

			miOpen.Enabled = tbOpen.Enabled =
				miSaveResults.Enabled = (active && q.RunState == RunState.Idle);

			miNew.Enabled = tbNew.Enabled =
				miSave.Enabled = tbSave.Enabled =
				miSaveAs.Enabled = active;

			miDisconnect.Enabled = tbDisconnect.Enabled = (active && q.RunState != RunState.Cancelling);

			miExecute.Enabled = tbExecute.Enabled = (active && q.RunState == RunState.Idle);

			miCancel.Enabled = tbCancel.Enabled = (active && q.RunState == RunState.Running);

			miResultsText.Enabled = tbResultsText.Enabled =
				miResultsGrid.Enabled = tbResultsGrid.Enabled = active;

			miResultsText.Checked = tbResultsText.Pushed = (active && q.ResultsInText);
			miResultsGrid.Checked = tbResultsGrid.Pushed = (active && !q.ResultsInText);

			miNextPane.Enabled = miPrevPane.Enabled = active;

			miHideResults.Enabled = tbHideResults.Enabled = active;
			miHideBrowser.Enabled = tbHideBrowser.Enabled =
				(active && q.Browser != null && q.RunState == RunState.Idle);

			miHideResults.Checked = tbHideResults.Pushed = (active && q.HideResults);
			miHideBrowser.Checked = tbHideBrowser.Pushed = (active && q.HideBrowser);
			*/
		}


		void UpdateFormText()
		{
			Text = dbClient.ConnectDescription + " - " + dbClient.Database + " - " + fileName;
		}

		void FirePropertyChanged()
		{
			if (!initializing && PropertyChanged != null)
				PropertyChanged(this, EventArgs.Empty);     // fire event
		}
		void UpdateExecTime()
		{
			TimeSpan t = DateTime.Now.Subtract(queryStartTime);
			panExecTime.Text = String.Format("Exec Time: {0}:{1}:{2}"
				, t.Hours.ToString("00"), t.Minutes.ToString("00"), t.Seconds.ToString("00"));
		}

		void ShowRowCount()
		{
			if (ResultsInText || tabControl.SelectedIndex < 1)
				panRows.Text = "";
			else
			{
				int rows;
				if (DSResults.Tables.Count == 0 || tabControl.SelectedIndex < 0)
					rows = 0;
				else
					rows = DSResults.Tables[tabControl.SelectedIndex - 1].Rows.Count;
				panRows.Text = rows == 0 ? "" : rows.ToString() + " row" + (rows == 1 ? "" : "s");
			}
		}

		bool ClientBusy
		{
			get { return RunState != RunState.Idle; }
		}

		private void txtQuery_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
			if (e.Data.GetDataPresent(typeof(string)))
			{
				string s = (string)e.Data.GetData(typeof(string));
				// Have the newly inserted text highlighted
				int start = txtQuery.SelectionStart;
				//txtQuery.SelectedText = s;
				txtQuery.SelectionStart = start;
				//txtQuery.SelectionLength = s.Length;
				//txtQuery.Modified = true;
				txtQuery.Focus();
			}
		}

		private void txtQuery_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
		{
			if (e.Data.GetDataPresent(typeof(string)))
			{
				e.Effect = DragDropEffects.Copy;
			}
		}

		private void cboDatabase_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (cboDatabase.SelectedIndex == 0)
				PopulateBrowser();
			else
				DbClient.Database = cboDatabase.Text;
			CheckDatabase();
		}
		private void cboDatabase_Enter(object sender, System.EventArgs e)
		{
			if (ClientBusy) txtQuery.Focus();
		}
		private void treeView_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			// When right-clicking, first select the node under the mouse.
			if (e.Button == MouseButtons.Right)
			{
				TreeNode tn = treeView.GetNodeAt(e.X, e.Y);
				if (tn != null)
					treeView.SelectedNode = tn;
			}
		}

		private void treeView_BeforeExpand(object sender, System.Windows.Forms.TreeViewCancelEventArgs e)
		{
			// If a browser has been installed, see if it has a sub object hierarchy for us at the point of expansion
			if (Browser == null) return;
			TreeNode[] subtree = Browser.GetSubObjectHierarchy(e.Node);
			if (subtree != null)
			{
				e.Node.Nodes.Clear();
				e.Node.Nodes.AddRange(subtree);
			}
		}

		private void treeView_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (Browser == null) return;
			// Display a context menu if the browser has an action list for the selected node
			if (e.Button == MouseButtons.Right && treeView.SelectedNode != null)
			{
				StringCollection actions = Browser.GetActionList(treeView.SelectedNode);
				if (actions != null)
				{
					System.Windows.Forms.ContextMenu cm = new ContextMenu();
					foreach (string action in actions)
						cm.MenuItems.Add(action, new EventHandler(DoBrowserAction));
					cm.Show(treeView, new Point(e.X, e.Y));
				}
			}
		}

		private void treeView_ItemDrag(object sender, System.Windows.Forms.ItemDragEventArgs e)
		{
			// Allow objects to be dragged from the browser to the query textbox.
			if (e.Button == MouseButtons.Left && e.Item is TreeNode)
			{
				// Ask the browser object for a string applicable to dragging onto the query window.
				string dragText = Browser.GetDragText((TreeNode)e.Item);
				// We'll use a simple string-type DataObject
				if (dragText != "")
					treeView.DoDragDrop(new DataObject(dragText), DragDropEffects.Copy);
			}
		}
		private void btnCloseBrowser_Click(object sender, System.EventArgs e)
		{
			HideBrowser = true;
		}
		private void tabControl_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			// Workaround: there's a bug in the grid control, whereby the scrollbars in grids in tabpages
			// don't resize when the parent control is resized with another tabpage active.
			this.Height += 1;
			this.Height -= 1;
			// If there is more than one result set, show the row count in the currently selected table.
			//tabControl.SelectedTab.inRefresh();
			ShowRowCount();
		}
		private void tmrExecTime_Tick(object sender, System.EventArgs e)
		{
			UpdateExecTime();
		}
		private void splQuery_Resize(object sender, System.EventArgs e)
		{
			// Force a re-paint
			Invalidate();
		}
		private void splQuery_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			// We need a 3D border effect on the bottom of this control to look right.
			e.Graphics.Clear(splQuery.BackColor);
			ControlPaint.DrawBorder3D(e.Graphics, e.ClipRectangle, Border3DStyle.Raised, Border3DSide.Bottom);
		}
		private void miRefresh_Click(object sender, System.EventArgs e)
		{
			PopulateBrowser();
		}
		private void QueryForm_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			// Check for Alt+Break combination (alternative shortcut for cancelling a query)
			if (e.Alt && e.KeyCode == Keys.Pause && RunState == RunState.Running)
			{
				Cancel();
				e.Handled = true;
			}
		}

		private void QueryForm_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			// Check for Control+E keypress (alternative to F5 for executing a  query)
			// Because this keystroke does get received in the KeyPress event, we are obliged to trap
			// it here (rather than in KeyDown) so we can set Handled to true to prevent the
			// default behaviour (ie a beep).
			if (e.KeyChar == '\x005' && RunState == RunState.Idle)
			{
				Execute();
				e.Handled = true;
			}
		}

		/// <summary>Returns false if user cancelled or save failed</summary>
		public bool SaveAs()
		{
			saveFileDialog.FileName = FileName;
			if (saveFileDialog.ShowDialog() == DialogResult.OK)
			{
				FileName = saveFileDialog.FileName;
				realFileName = true;
				return SaveFile(FileName);
			}
			else return false;
		}

		bool SaveFile(string fileName)
		{
			if (FileUtil.WriteToFile(fileName, txtQuery.Text))
			{
				txtQuery.SetSavePoint();
				//txtQuery.Modified = false;
				return true;
			}
			else
			{
				MessageBox.Show(FileUtil.Error, "Error saving file", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return false;
			}
		}

		/// <summary>Returns false if user cancelled or save failed</summary>
		public bool Save()
		{
			if (!realFileName)
				return SaveAs();
			else
				return SaveFile(FileName);
		}

		bool CloseQuery()
		{
			// Check to see if a query is running, and warn user that the query will be cancelled.
			if (RunState != RunState.Idle)
				if (MessageBox.Show(FileName + " is currently executing.\nWould you like to cancel the query?",
					"Query Express", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
					// The Dispose method in DbClient will actually do the Cancel
					return false;

			// If the query text has been modified, give option of saving changes.
			// Don't nag the user in the case of simple queries of less than 30 characters.
			if (txtQuery.Modified && txtQuery.Text.Length > 30)
			{
				DialogResult dr = MessageBox.Show("Save changes to " + FileName + "?", Text,
					MessageBoxButtons.YesNoCancel);
				if (dr == DialogResult.Yes)
				{
					if (!Save()) return false;
				}
				else if (dr == DialogResult.Cancel)
					return false;
			}
			return true;
		}
		private void QueryForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (!CloseQuery()) e.Cancel = true;
		}

		public void SwitchPane(bool forward)
		{
			if (ResultsInText)
			{
				if (txtQuery.Focused)
					txtResultsBox.Focus();
				else
					txtQuery.Focus();
				return;
			}
			if (forward)
			{
				if (txtQuery.Focused)
				{
					tabControl.Focus();
					tabControl.SelectedIndex = 0;
				}
				else
				{
					if (tabControl.SelectedIndex < tabControl.TabCount - 1)
						tabControl.SelectedIndex++;
					else
						txtQuery.Focus();
				}
			}
			else
			{
				if (txtQuery.Focused)
				{
					tabControl.Focus();
					tabControl.SelectedIndex = tabControl.TabCount - 1;
				}
				else
				{
					if (tabControl.SelectedIndex > 0)
						tabControl.SelectedIndex--;
					else
						txtQuery.Focus();
				}
			}
			if (!txtQuery.Focused)
				tabControl.SelectedTab.Controls[0].Focus();
		}

		void CheckDatabase()
		{
			if (lastDatabase != dbClient.Database)
			{
				lastDatabase = dbClient.Database;
				UpdateFormText();
				PopulateBrowser();
			}
		}

		void PopulateBrowser()
		{
			if (Browser != null && !HideBrowser && !ClientBusy)
				try
				{
					treeView.Nodes.Clear();
					TreeNode[] tn = Browser.GetObjectHierarchy();
					if (tn == null) HideBrowser = true;
					else
					{
						treeView.Nodes.AddRange(tn);
						treeView.Nodes[0].Expand();             // Expand the top level of hierarchy
						cboDatabase.Items.Clear();
						cboDatabase.Items.Add("<refresh list...>");
						cboDatabase.Items.AddRange(Browser.GetDatabases());
						try { cboDatabase.Text = DbClient.Database; }
						catch { }
					}
				}
				catch { }
		}


		public bool Open()
		{
			openFileDialog.FileName = "*.sql";
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				string f = openFileDialog.FileName;
				if (System.IO.Path.GetExtension(f) == "") f += ".sql";
				return OpenFile(f);
			}
			else
				return false;
		}

		/// <summary>Returns false if user cancelled or open failed </summary>
		bool OpenFile(string fileName)
		{
			string s;
			if (FileUtil.ReadFromFile(fileName, out s) && CloseQuery())
			{
				txtQuery.Text = s;
				//txtQuery.M = false;
				this.FileName = fileName;
				realFileName = true;
				return true;
			}
			else
			{
				MessageBox.Show(FileUtil.Error, "Error opening file", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return false;
			}
		}

		private void toolStripButton3_Click(object sender, EventArgs e)
		{
			Execute();
		}

		private void toolStripButton4_Click(object sender, EventArgs e)
		{
			Cancel();
		}

		private void toolStripButton7_Click(object sender, EventArgs e)
		{
			HideResults = !HideResults;
			//SwitchPane(true);
		}

		private void toolStripButton8_Click(object sender, EventArgs e)
		{
			HideBrowser = !HideBrowser;
			//SwitchPane(false);
		}

		private void toolStripButton5_Click(object sender, EventArgs e)
		{
			ResultsInText = true;
		}

		private void toolStripButton6_Click(object sender, EventArgs e)
		{
			ResultsInText = false;
		}

		private void DbConnectorDoc_Load(object sender, EventArgs e)
		{
			
		}

		private void toolStripButton2_Click(object sender, EventArgs e)
		{
			Save();
		}

		private void toolStripButton1_Click(object sender, EventArgs e)
		{
			Open();
		}

		private void DbConnectorDoc_Activated(object sender, EventArgs e)
		{
			activated?.Invoke();
		}
	}
}
