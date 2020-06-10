using DockSample.Controls;

namespace DockSample
{
    partial class DbConnectorDoc
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>

		#region Designer Fields
		private System.Windows.Forms.StatusBarPanel panRunStatus;
		private System.Windows.Forms.StatusBarPanel panExecTime;
		private System.Windows.Forms.StatusBar statusBar;
		private System.Windows.Forms.TabControl tabControl;
		private EasyScintilla.SimpleEditor txtQuery;
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Timer tmrExecTime;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.SaveFileDialog saveFileDialog;
		private System.Windows.Forms.StatusBarPanel panRows;
		private System.Windows.Forms.SaveFileDialog saveResultsDialog;
		private System.Windows.Forms.Splitter splQuery;
		private System.Windows.Forms.Panel panBrowser;
		private System.Windows.Forms.TreeView treeView;
		private System.Windows.Forms.Splitter splBrowser;
		private System.Windows.Forms.Panel panDatabase;
		private System.Windows.Forms.ComboBox cboDatabase;
		private System.Windows.Forms.Label label1;
		//private XButton btnCloseBrowser;
		private System.Windows.Forms.ContextMenu cmRefresh;
		private System.Windows.Forms.MenuItem miRefresh;
		private System.Windows.Forms.ImageList imageList;
		#endregion

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.panRows = new System.Windows.Forms.StatusBarPanel();
            this.txtQuery = new EasyScintilla.SimpleEditor();
            this.cboDatabase = new System.Windows.Forms.ComboBox();
            this.cmRefresh = new System.Windows.Forms.ContextMenu();
            this.miRefresh = new System.Windows.Forms.MenuItem();
            this.panBrowser = new System.Windows.Forms.Panel();
            this.treeView = new System.Windows.Forms.TreeView();
            this.panDatabase = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.statusBar = new System.Windows.Forms.StatusBar();
            this.panRunStatus = new System.Windows.Forms.StatusBarPanel();
            this.panExecTime = new System.Windows.Forms.StatusBarPanel();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.pnlToolStrip = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton6 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton7 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton8 = new System.Windows.Forms.ToolStripButton();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.tmrExecTime = new System.Windows.Forms.Timer(this.components);
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.splQuery = new System.Windows.Forms.Splitter();
            this.saveResultsDialog = new System.Windows.Forms.SaveFileDialog();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.splBrowser = new System.Windows.Forms.Splitter();
            this.pnlQueryAndResult = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.panRows)).BeginInit();
            this.panBrowser.SuspendLayout();
            this.panDatabase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panRunStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panExecTime)).BeginInit();
            this.pnlToolStrip.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.pnlQueryAndResult.SuspendLayout();
            this.SuspendLayout();
            // 
            // panRows
            // 
            this.panRows.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
            this.panRows.MinWidth = 60;
            this.panRows.Name = "panRows";
            this.panRows.Width = 60;
            // 
            // txtQuery
            // 
            this.txtQuery.AllowDrop = true;
            this.txtQuery.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtQuery.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtQuery.Location = new System.Drawing.Point(0, 28);
            this.txtQuery.Name = "txtQuery";
            this.txtQuery.Size = new System.Drawing.Size(576, 307);
            this.txtQuery.Styler = null;
            this.txtQuery.TabIndex = 0;
            this.txtQuery.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtQuery_DragDrop);
            this.txtQuery.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtQuery_DragEnter);
            // 
            // cboDatabase
            // 
            this.cboDatabase.ContextMenu = this.cmRefresh;
            this.cboDatabase.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.cboDatabase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDatabase.DropDownWidth = 128;
            this.cboDatabase.ItemHeight = 13;
            this.cboDatabase.Location = new System.Drawing.Point(0, 19);
            this.cboDatabase.Name = "cboDatabase";
            this.cboDatabase.Size = new System.Drawing.Size(228, 21);
            this.cboDatabase.TabIndex = 1;
            this.cboDatabase.SelectedIndexChanged += new System.EventHandler(this.cboDatabase_SelectedIndexChanged);
            this.cboDatabase.Enter += new System.EventHandler(this.cboDatabase_Enter);
            // 
            // cmRefresh
            // 
            this.cmRefresh.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.miRefresh});
            // 
            // miRefresh
            // 
            this.miRefresh.Index = 0;
            this.miRefresh.Text = "&Refresh Browser";
            this.miRefresh.Click += new System.EventHandler(this.miRefresh_Click);
            // 
            // panBrowser
            // 
            this.panBrowser.Controls.Add(this.treeView);
            this.panBrowser.Controls.Add(this.panDatabase);
            this.panBrowser.Dock = System.Windows.Forms.DockStyle.Left;
            this.panBrowser.Location = new System.Drawing.Point(0, 0);
            this.panBrowser.Name = "panBrowser";
            this.panBrowser.Size = new System.Drawing.Size(228, 499);
            this.panBrowser.TabIndex = 3;
            // 
            // treeView
            // 
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.Location = new System.Drawing.Point(0, 40);
            this.treeView.Name = "treeView";
            this.treeView.Size = new System.Drawing.Size(228, 459);
            this.treeView.TabIndex = 0;
            this.treeView.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView_BeforeExpand);
            this.treeView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeView_ItemDrag);
            this.treeView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeView_MouseDown);
            this.treeView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.treeView_MouseUp);
            // 
            // panDatabase
            // 
            this.panDatabase.ContextMenu = this.cmRefresh;
            this.panDatabase.Controls.Add(this.label1);
            this.panDatabase.Controls.Add(this.cboDatabase);
            this.panDatabase.Dock = System.Windows.Forms.DockStyle.Top;
            this.panDatabase.Location = new System.Drawing.Point(0, 0);
            this.panDatabase.Name = "panDatabase";
            this.panDatabase.Size = new System.Drawing.Size(228, 40);
            this.panDatabase.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(-1, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Database:";
            // 
            // statusBar
            // 
            this.statusBar.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusBar.Location = new System.Drawing.Point(0, 480);
            this.statusBar.Name = "statusBar";
            this.statusBar.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
            this.panRunStatus,
            this.panExecTime,
            this.panRows});
            this.statusBar.ShowPanels = true;
            this.statusBar.Size = new System.Drawing.Size(576, 19);
            this.statusBar.SizingGrip = false;
            this.statusBar.TabIndex = 2;
            // 
            // panRunStatus
            // 
            this.panRunStatus.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
            this.panRunStatus.Name = "panRunStatus";
            this.panRunStatus.Text = "Ready";
            this.panRunStatus.Width = 308;
            // 
            // panExecTime
            // 
            this.panExecTime.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
            this.panExecTime.MinWidth = 80;
            this.panExecTime.Name = "panExecTime";
            this.panExecTime.Width = 80;
            // 
            // tabControl
            // 
            this.tabControl.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 341);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(576, 139);
            this.tabControl.TabIndex = 1;
            this.tabControl.TabStop = false;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
            // 
            // pnlToolStrip
            // 
            this.pnlToolStrip.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.pnlToolStrip.Controls.Add(this.toolStrip1);
            this.pnlToolStrip.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlToolStrip.Location = new System.Drawing.Point(0, 0);
            this.pnlToolStrip.Name = "pnlToolStrip";
            this.pnlToolStrip.Size = new System.Drawing.Size(576, 28);
            this.pnlToolStrip.TabIndex = 5;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripSeparator1,
            this.toolStripButton3,
            this.toolStripButton4,
            this.toolStripSeparator2,
            this.toolStripButton5,
            this.toolStripButton6,
            this.toolStripButton7,
            this.toolStripButton8});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(576, 28);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = global::DockSample.Properties.Resources.open;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 25);
            this.toolStripButton1.Text = "toolStripButton1";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = global::DockSample.Properties.Resources.save;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(23, 25);
            this.toolStripButton2.Text = "toolStripButton2";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 28);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton3.Image = global::DockSample.Properties.Resources.sqlexecute;
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(23, 25);
            this.toolStripButton3.Text = "toolStripButton3";
            this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton4.Image = global::DockSample.Properties.Resources.cancel;
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(23, 25);
            this.toolStripButton4.Text = "toolStripButton4";
            this.toolStripButton4.Click += new System.EventHandler(this.toolStripButton4_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 28);
            // 
            // toolStripButton5
            // 
            this.toolStripButton5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton5.Image = global::DockSample.Properties.Resources.textresults;
            this.toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton5.Name = "toolStripButton5";
            this.toolStripButton5.Size = new System.Drawing.Size(23, 25);
            this.toolStripButton5.Text = "toolStripButton5";
            this.toolStripButton5.Click += new System.EventHandler(this.toolStripButton5_Click);
            // 
            // toolStripButton6
            // 
            this.toolStripButton6.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton6.Image = global::DockSample.Properties.Resources.gridresults;
            this.toolStripButton6.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton6.Name = "toolStripButton6";
            this.toolStripButton6.Size = new System.Drawing.Size(23, 25);
            this.toolStripButton6.Text = "toolStripButton6";
            this.toolStripButton6.Click += new System.EventHandler(this.toolStripButton6_Click);
            // 
            // toolStripButton7
            // 
            this.toolStripButton7.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton7.Image = global::DockSample.Properties.Resources.hideresults;
            this.toolStripButton7.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton7.Name = "toolStripButton7";
            this.toolStripButton7.Size = new System.Drawing.Size(23, 25);
            this.toolStripButton7.Text = "toolStripButton7";
            this.toolStripButton7.Click += new System.EventHandler(this.toolStripButton7_Click);
            // 
            // toolStripButton8
            // 
            this.toolStripButton8.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton8.Image = global::DockSample.Properties.Resources.hidebrowser;
            this.toolStripButton8.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton8.Name = "toolStripButton8";
            this.toolStripButton8.Size = new System.Drawing.Size(23, 25);
            this.toolStripButton8.Text = "toolStripButton8";
            this.toolStripButton8.Click += new System.EventHandler(this.toolStripButton8_Click);
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "sql";
            this.saveFileDialog.Filter = "SQL files|*.sql|Text files|*.txt|All files|*.*";
            // 
            // tmrExecTime
            // 
            this.tmrExecTime.Interval = 1000;
            this.tmrExecTime.Tick += new System.EventHandler(this.tmrExecTime_Tick);
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "SQL";
            this.openFileDialog.Filter = "SQL files|*.sql|Text files|*.txt|All files|*.*";
            // 
            // splQuery
            // 
            this.splQuery.Dock = System.Windows.Forms.DockStyle.Top;
            this.splQuery.Location = new System.Drawing.Point(0, 335);
            this.splQuery.MinExtra = 0;
            this.splQuery.Name = "splQuery";
            this.splQuery.Size = new System.Drawing.Size(576, 6);
            this.splQuery.TabIndex = 1;
            this.splQuery.TabStop = false;
            this.splQuery.Paint += new System.Windows.Forms.PaintEventHandler(this.splQuery_Paint);
            this.splQuery.Resize += new System.EventHandler(this.splQuery_Resize);
            // 
            // saveResultsDialog
            // 
            this.saveResultsDialog.Filter = "CSV Format|*.csv|XML|*.xml|All files|*.*";
            this.saveResultsDialog.Title = "Save Query Results";
            // 
            // imageList
            // 
            this.imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // splBrowser
            // 
            this.splBrowser.BackColor = System.Drawing.SystemColors.Control;
            this.splBrowser.Location = new System.Drawing.Point(228, 0);
            this.splBrowser.Name = "splBrowser";
            this.splBrowser.Size = new System.Drawing.Size(10, 499);
            this.splBrowser.TabIndex = 4;
            this.splBrowser.TabStop = false;
            // 
            // pnlQueryAndResult
            // 
            this.pnlQueryAndResult.Controls.Add(this.tabControl);
            this.pnlQueryAndResult.Controls.Add(this.splQuery);
            this.pnlQueryAndResult.Controls.Add(this.txtQuery);
            this.pnlQueryAndResult.Controls.Add(this.statusBar);
            this.pnlQueryAndResult.Controls.Add(this.pnlToolStrip);
            this.pnlQueryAndResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlQueryAndResult.Location = new System.Drawing.Point(238, 0);
            this.pnlQueryAndResult.Name = "pnlQueryAndResult";
            this.pnlQueryAndResult.Size = new System.Drawing.Size(576, 499);
            this.pnlQueryAndResult.TabIndex = 5;
            // 
            // DbConnectorDoc
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(814, 499);
            this.Controls.Add(this.pnlQueryAndResult);
            this.Controls.Add(this.splBrowser);
            this.Controls.Add(this.panBrowser);
            this.KeyPreview = true;
            this.Name = "DbConnectorDoc";

            this.ClientSize = new System.Drawing.Size(255, 365);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)(((((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft)
                        | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight)
                        | WeifenLuo.WinFormsUI.Docking.DockAreas.DockTop)
                        | WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom)));
            //this.HideOnClose = true;
            this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockBottomAutoHide;
            //this.TabText = "Output";
            //this.Text = "Output";


            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "QueryForm";
            this.Activated += new System.EventHandler(this.DbConnectorDoc_Activated);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.QueryForm_Closing);
            this.Load += new System.EventHandler(this.DbConnectorDoc_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.QueryForm_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.QueryForm_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.panRows)).EndInit();
            this.panBrowser.ResumeLayout(false);
            this.panDatabase.ResumeLayout(false);
            this.panDatabase.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panRunStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panExecTime)).EndInit();
            this.pnlToolStrip.ResumeLayout(false);
            this.pnlToolStrip.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.pnlQueryAndResult.ResumeLayout(false);
            this.ResumeLayout(false);

		}


        #endregion

        private System.Windows.Forms.Panel pnlToolStrip;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButton5;
        private System.Windows.Forms.ToolStripButton toolStripButton6;
        private System.Windows.Forms.ToolStripButton toolStripButton7;
        private System.Windows.Forms.ToolStripButton toolStripButton8;
        private System.Windows.Forms.Panel pnlQueryAndResult;
    }
}