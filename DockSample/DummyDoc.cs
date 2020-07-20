using System;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.IO;
using EasyScintilla.Stylers;
using ScintillaNET;
using System.Threading.Tasks;
using DockSample.lib;
using SFTPEntities;
using UIFunctionality;
using UIFunctionality.Common;
using System.ComponentModel;
using Renci.SshNet;
using System.Threading;
using DockSample.Controls;

namespace DockSample
{
    public partial class DummyDoc :  DockContent
    {
        StudioConfig studioConfig;
        //bool isDocDirty;
        public ToolStrip ToolStrip { get; set; }

        FindReplace ctrlFindReplace;
        SSHManager sshManager;
        public EasyScintilla.SimpleEditor TxtEditor { get; set; }

        public DummyDoc()
        {
            InitializeComponent();
            AutoScaleMode = AutoScaleMode.Dpi;
            DockAreas = DockAreas.Document | DockAreas.Float;
            
        }

        public DummyDoc(eFileType fileType)
        {
            InitializeComponent();
            if (fileType == eFileType.Python)
            {
                this.toolStrip1.Visible = true;
                ToolStrip = this.toolStrip1;
            }
            else
            {
                this.toolStrip1.Visible = false;
            }
            
            AutoScaleMode = AutoScaleMode.Dpi;
            DockAreas = DockAreas.Document | DockAreas.Float;
            FileType = fileType;
            txtCodeEditor.Text = string.Empty;
            TxtEditor = txtCodeEditor;
            toolStripComboBox1.SelectedIndex = 0;
            studioConfig = new StudioConfig(AppDomain.CurrentDomain.BaseDirectory).GetStudioConfigFromFile();
            ctrlFindReplace = new FindReplace(txtCodeEditor);
        }

        public MainForm mainFrm { get; set; }
        
        public string FileName { get; set; }

        public DummyOutputWindow outputWindow { get; set; }
        public SFTPEntities.eFileType FileType { get; set; }
        public string FullPath { get; set; }
        
        // workaround of RichTextbox control's bug:
        // If load file before the control showed, all the text format will be lost
        // re-load the file after it get showed.
        private bool m_resetText = true;
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (m_resetText)
            {
                m_resetText = false;
                FileName = FileName;
            }
        }

        //protected override string GetPersistString()
        //{
        //    // Add extra information into the persist string for this document
        //    // so that it is available when deserialized.
        //    return GetType().ToString() + "," + FileName + "," + Text;
        //}

        private void menuItem2_Click(object sender, System.EventArgs e)
        {
            MessageBox.Show("This is to demostrate menu item has been successfully merged into the main form. Form Text=" + Text);
        }

        private void menuItemCheckTest_Click(object sender, System.EventArgs e)
        {
            menuItemCheckTest.Checked = !menuItemCheckTest.Checked;
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            this.txtCodeEditor.Text = Text;
            //if (FileName == string.Empty)
                
        }

        private void DummyDoc_Load(object sender, EventArgs e)
        {
            //ctrlFindReplace.Visible = false;
            if (studioConfig.databaseConnections != null)
            {
                if (studioConfig.databaseConnections.Count > 0)
                {
                    foreach (var item in studioConfig.databaseConnections)
                    {
                        ToolStripMenuItem dpItem = new ToolStripMenuItem();
                        dpItem.Name = item.ConnName;
                        dpItem.Text = item.ConnName;
                        switch (item.DbType)
                        {
                            case "Sql Server":
                                //dpItem.Tag = "'Server=" + item.ServerName + ";Database=" + item.DbName + ";User Id=" + item.UserName + ";Password=" + item.Password + ";'";
                                dpItem.Tag = "'DRIVER={ODBC Driver 17 for SQL Server};SERVER=" + item.ServerName + ";DATABASE=" + item.DbName + ";UID=" + item.UserName + ";PWD=" + item.Password + ";'";
                                break;
                            case "Postgres":
                                dpItem.Tag = "'Host=" + item.ServerName + "Port=5432;Database=" + item.DbName + ";User Id=" + item.UserName + ";Password=" + item.Password + ";Integrated Security=False'";
                                break;
                            case "MySql":
                                dpItem.Tag = "'Server=" + item.ServerName + ";Port=1234;Database=" + item.DbName + ";Uid=" + item.UserName + ";Pwd=" + item.Password + ";'";
                                break;
                            case "Mongo Db":
                                dpItem.Tag = "'mongodb://" + item.UserName + ":" + item.Password + "@" + item.ServerName + ":27017/" + item.DbName + "'";
                                //mongodb://admin:password@localhost:27017/db
                                break;
                            case "Json":
                            case "Xml":
                            case "Cloud Storage":
                            case "Streaming Data":
                                dpItem.Tag = ("'" + item.ServerName + "'");
                                break;
                        }
                        conectionsToolStripMenuItem.DropDownItems.Add(dpItem);
                    }
                    //var ddd = txtCodeEditor.ContextMenu.MenuItems;
                    
                }
            }

            switch (FileType)
            {
                case SFTPEntities.eFileType.Python:
                    this.txtCodeEditor.Styler = new PythonStyler();
                    txtCodeEditor.Lexer = Lexer.Python;
                    break;
                case SFTPEntities.eFileType.Text:
                    break;
                case SFTPEntities.eFileType.Xml:
                    this.txtCodeEditor.Styler = new EasyScintilla.Stylers.HtmlStyler();
                    txtCodeEditor.Lexer = Lexer.Xml;
                    break;
                case SFTPEntities.eFileType.Json:
                    this.txtCodeEditor.Styler = new EasyScintilla.Stylers.JsonStyler();
                    txtCodeEditor.Lexer = Lexer.Json;
                    break;
                case SFTPEntities.eFileType.Csv:

                    break;
                case SFTPEntities.eFileType.Excel:

                    break;
                default:

                    break;
            }
        }

        private async void txtCodeEditor_CharAdded(object sender, CharAddedEventArgs e)
        {
            Task t = new Task(() =>
            {
                int currentPos = 0, wordStartPos = 0;
                // Find the word start
                txtCodeEditor.PerformSafely(() =>
                {
                    if (txtCodeEditor.Lexer != Lexer.Python)
                    {
                        return;
                    }
                    currentPos = txtCodeEditor.CurrentPosition;
                    wordStartPos = txtCodeEditor.WordStartPosition(currentPos, true);
                });


                // Display the autocompletion list
                var lenEntered = currentPos - wordStartPos;
                if (lenEntered > 0)
                {
                    txtCodeEditor.PerformSafely(() =>
                    {
                        if (!txtCodeEditor.AutoCActive)
                            txtCodeEditor.AutoCShow(lenEntered, "and as assert break class continue def del elif else except finally for from global if import in is lambda nonlocal not or pass print raise return try while with yield False None True");
                    });
                }
            });
            t.Start();
            await t;
        }

        private void DummyDoc_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.outputWindow != null)
            {
                this.outputWindow.Close();
            }
            
        }

        private async void toolStripButton1_Click(object sender, EventArgs e)
        {
            //Save button
            await Task.Run(async () =>
            {
                toolStrip1.PerformSafely(() =>
                {
                    toolStripButton1.Enabled = false;
                    toolStripComboBox1.Enabled = false;
                    toolStripButton2.Enabled = false;
                    toolStripButton3.Enabled = false;
                });
                txtCodeEditor.PerformSafely(() => {
                    txtCodeEditor.Enabled = false;
                });

                pnlMsg.PerformSafely(() => {
                    pnlMsg.Visible = true;
                });

                lblMsg.PerformSafely(() => {
                    lblMsg.Text = "Saving file...";
                });

                try
                {
                    outputWindow.OutputTextControl.PerformSafely(() =>
                    {
                        outputWindow.OutputTextControl.Text += string.Format("{0}: {1}", DateTime.Now.ToString(), "saving file..");
                        outputWindow.OutputTextControl.Text += Environment.NewLine;
                    });
                    string fileText = string.Empty;
                    txtCodeEditor.PerformSafely(() =>
                    {
                        fileText = txtCodeEditor.Text;
                    });
                    sshManager = new SSHManager();
                    sshManager.WriteFileContentMethod(studioConfig.sSHClientInfo.IPAddress, studioConfig.sSHClientInfo.UserName, studioConfig.sSHClientInfo.Password, ToolTipText, fileText);

                    //if (isDocDirty)
                    //{
                    //    TabText = TabText.Substring(0, TabText.Length-1);
                    //    isDocDirty = false;
                    //}
                    
                    outputWindow.OutputTextControl.PerformSafely(() =>
                    {
                        outputWindow.OutputTextControl.Text += string.Format("{0}: {1}", DateTime.Now.ToString(), "File saved..");
                        outputWindow.OutputTextControl.Text += Environment.NewLine;
                    });

                }
                catch (Exception ex)
                {
                    outputWindow.OutputTextControl.PerformSafely(() =>
                    {
                        outputWindow.OutputTextControl.Text += string.Format("{0}: {1}", "Error found", ex.Message);
                    });
                }
                finally
                {
                    outputWindow.OutputTextControl.PerformSafely(() =>
                    {
                        outputWindow.OutputTextControl.Text += "---------------------------------------------------------------------";
                        outputWindow.OutputTextControl.Text += Environment.NewLine;
                    });
                    toolStrip1.PerformSafely(() =>
                    {
                        toolStripButton1.Enabled = true;
                        toolStripComboBox1.Enabled = true;
                        toolStripButton2.Enabled = true;
                        toolStripButton3.Enabled = false;
                    });
                    txtCodeEditor.PerformSafely(() => {
                        txtCodeEditor.Enabled = true;
                    });
                    pnlMsg.PerformSafely(() => {
                        pnlMsg.Visible = false;
                    });
                }
            });
        }

        private async void toolStripButton2_Click(object sender, EventArgs e)
        {
            await Task.Run(async () =>
            {
                toolStrip1.PerformSafely(() =>
                {
                    toolStripButton1.Enabled = false;
                    toolStripComboBox1.Enabled = false;
                    toolStripButton2.Enabled = false;
                    toolStripButton3.Enabled = true;
                });
                pnlMsg.PerformSafely(() => {
                    pnlMsg.Visible = true;
                });

                lblMsg.PerformSafely(() => {
                    lblMsg.Text = "Saving file...";
                });
                try
                {
                    txtCodeEditor.PerformSafely(() => {
                        txtCodeEditor.Enabled = false;
                    });
                    
                    outputWindow.OutputTextControl.PerformSafely(() =>
                    {
                        outputWindow.OutputTextControl.Text += string.Format("{0}: {1}", DateTime.Now.ToString(), "saving file..");
                        outputWindow.OutputTextControl.Text += Environment.NewLine;
                    });
                    string fileText = string.Empty;
                    txtCodeEditor.PerformSafely(() =>
                    {
                        fileText = txtCodeEditor.Text;
                    });
                    sshManager = new SSHManager();
                    sshManager.WriteFileContentMethod(studioConfig.sSHClientInfo.IPAddress, studioConfig.sSHClientInfo.UserName, studioConfig.sSHClientInfo.Password, ToolTipText, fileText);

                    outputWindow.OutputTextControl.PerformSafely(() =>
                    {
                        outputWindow.OutputTextControl.Text += string.Format("{0}: {1}", DateTime.Now.ToString(), "File saved..");
                        outputWindow.OutputTextControl.Text += Environment.NewLine;
                    });

                    outputWindow.OutputTextControl.PerformSafely(() =>
                    {
                        outputWindow.OutputTextControl.Text += "---------------------------------------------------------------------";
                        outputWindow.OutputTextControl.Text += Environment.NewLine;
                    });

                    outputWindow.OutputTextControl.PerformSafely(() =>
                    {
                        outputWindow.OutputTextControl.Text += string.Format("{0}: {1}", DateTime.Now.ToString(), "executing script file..");
                        outputWindow.OutputTextControl.Text += Environment.NewLine;
                    });

                    outputWindow.OutputTextControl.PerformSafely(() =>
                    {
                        outputWindow.OutputTextControl.Text += "---------------------------------------------------------------------";
                        outputWindow.OutputTextControl.Text += Environment.NewLine;
                        //Task.Delay(5000).Wait();
                    });

                    string compilerText = string.Empty;
                    toolStrip1.PerformSafely(() =>
                    {
                        compilerText = this.toolStripComboBox1.SelectedItem.ToString();
                    });

                    lblMsg.PerformSafely(() => {
                        lblMsg.Text = "Executing file...";
                    });

                    sshManager.ExecuteCommandOnConsoleMethod(studioConfig.sSHClientInfo.IPAddress, studioConfig.sSHClientInfo.UserName, studioConfig.sSHClientInfo.Password, compilerText, ToolTipText, outputWindow.OutputTextControl,
                        () => {
                            outputWindow.OutputTextControl.PerformSafely(() =>
                            {
                                outputWindow.OutputTextControl.Text += "---------------------------------------------------------------------";
                                outputWindow.OutputTextControl.Text += Environment.NewLine;
                                outputWindow.OutputTextControl.SelectionStart = outputWindow.OutputTextControl.Text.Length;
                                outputWindow.OutputTextControl.ScrollToCaret();
                            });
                            toolStrip1.PerformSafely(() =>
                            {
                                toolStripButton1.Enabled = true;
                                toolStripComboBox1.Enabled = true;
                                toolStripButton2.Enabled = true;
                                toolStripButton3.Enabled = false;
                            });
                            txtCodeEditor.PerformSafely(() => {
                                txtCodeEditor.Enabled = true;
                                
                            });
                            pnlMsg.PerformSafely(() => {
                                pnlMsg.Visible = false;
                            });
                        });
                }
                catch (Exception ex)
                {
                    outputWindow.OutputTextControl.PerformSafely(() =>
                    {
                        outputWindow.OutputTextControl.Text += string.Format("{0}: {1}", "Error found", ex.Message);
                    });
                    toolStrip1.PerformSafely(() =>
                    {
                        toolStripButton1.Enabled = true;
                        toolStripComboBox1.Enabled = true;
                        toolStripButton2.Enabled = true;
                        toolStripButton3.Enabled = false;
                    });
                    txtCodeEditor.PerformSafely(() => {
                        txtCodeEditor.Enabled = true;
                    });
                }
                finally
                {

                }
            });
           
        }

        private async void toolStripButton3_Click(object sender, EventArgs e)
        {
            var t = new Task(() =>
            {
                if (sshManager != null)
                {
                    sshManager.Dispose();
                }
               
                outputWindow.OutputTextControl.PerformSafely(() =>
                {
                    outputWindow.OutputTextControl.Text += "Script execution halt by user..";
                    outputWindow.OutputTextControl.Text += Environment.NewLine;
                });
                toolStrip1.PerformSafely(() =>
                {
                    toolStripButton1.Enabled = true;
                    toolStripComboBox1.Enabled = true;
                    toolStripButton2.Enabled = true;
                    toolStripButton3.Enabled = false;
                });
                txtCodeEditor.PerformSafely(() => {
                    txtCodeEditor.Enabled = true;
                });

                pnlMsg.PerformSafely(() => {
                    pnlMsg.Visible = false;
                });

            });
            t.Start();
            //Stop button
        }

        private async void contextMenuForEditor_Opening(object sender, CancelEventArgs e)
        {
            bool showCutDelete = false;
            if (txtCodeEditor.Selections.Count > 0)
            {
                if ((txtCodeEditor.Selections[0].End - txtCodeEditor.Selections[0].Start) > 0)
                {
                    showCutDelete = true;
                }
            }
            undoToolStripMenuItem.Enabled = txtCodeEditor.CanUndo;
            redoToolStripMenuItem.Enabled = txtCodeEditor.CanRedo;
            cutToolStripMenuItem.Enabled = showCutDelete;
            pasteToolStripMenuItem.Enabled = txtCodeEditor.CanPaste;
            deleteToolStripMenuItem.Enabled = showCutDelete;
            selecTAllToolStripMenuItem.Enabled = txtCodeEditor.TextLength > 0;
        }

        private async void contextMenuForEditor_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Text)
            {
                case "Find":
                    ctrlFindReplace.ShowDialog(this);
                    break;
                case "Undo":
                    this.txtCodeEditor.Undo();
                    break;
                case "Redo":
                    this.txtCodeEditor.Redo();
                    break;
                case "Cut":
                    this.txtCodeEditor.Cut();
                    break;
                case "Copy":
                    this.txtCodeEditor.Copy();
                    break;
                case "Paste":
                    this.txtCodeEditor.Paste();
                    break;
                case "Select All":
                    this.txtCodeEditor.SelectAll();
                    break;
            }
        }

        private async void conectionsToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            Clipboard.SetText(e.ClickedItem.Tag.ToString());
            txtCodeEditor.Paste();
        }

        private async void txtCodeEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F && e.Control)
            {
                ctrlFindReplace.ShowDialog(this);
            }
            else if (e.KeyCode == Keys.S && e.Control)
            {
                toolStripButton1_Click(null, null);
            }
            else
                e.SuppressKeyPress = false;
        }

        private async void txtCodeEditor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar < 32)
            {
                e.Handled = true;
                return;
            }
        }
    }
}