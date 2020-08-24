using CustomControls;
using DockSample.Controls;
using DockSample.lib;
using SFTPEntities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UIFunctionality.Common;
using unvell.ReoGrid.Editor;
using WeifenLuo.WinFormsUI.Docking;

namespace DockSample
{
    public partial class MainForm : Form
    {
        private bool m_bSaveLayout = true;
        private DeserializeDockContent m_deserializeDockContent;
        //private DummySolutionExplorer m_solutionExplorer;
        private SolutionExplorer m_solutionExplorer;
        private DummyPropertyWindow m_propertyWindow;
        private DummyToolbox m_toolbox;
        //private DummyOutputWindow m_outputWindow;
        private DummyTaskList m_taskList;
        private bool _showSplash;
        private SplashScreen _splashScreen;
        StudioConfig studioConfig;
        public ProjectInfo CurrentProj { get; set; }

        Action loadComplete;

        UCLoaderForm _waitLoader = new UCLoaderForm();
        public UCLoaderForm WaitLoader { get { return _waitLoader; } }

        public MainForm(StudioConfig sc, Action action)
        {
            studioConfig = sc;
            InitializeComponent();

            AutoScaleMode = AutoScaleMode.Dpi;

            //SetSplashScreen();
            CreateStandardControls();

            showRightToLeft.Checked = (RightToLeft == RightToLeft.Yes);
            RightToLeftLayout = showRightToLeft.Checked;
            //m_solutionExplorer.RightToLeftLayout = RightToLeftLayout;
            m_solutionExplorer.DockState = DockState.DockLeft;
            m_deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);

            vsToolStripExtender1.DefaultRenderer = _toolStripProfessionalRenderer;
            this.Activated += MainForm_Activated;
            loadComplete = action;

            this.MdiChildActivate += MainForm_MdiChildActivate;

        }

        private void MainForm_MdiChildActivate(object sender, EventArgs e)
        {
            var child = this.ActiveMdiChild;

            if (child != null)
            {
                ToolStripManager.RevertMerge(toolBar);
                if (child is DummyDoc)
                {
                    DummyDoc dummyDoc = (DummyDoc)child;
                    if (dummyDoc.ToolStrip != null)
                    {
                        ToolStripManager.Merge(dummyDoc.ToolStrip, toolBar);
                        dummyDoc.ToolStrip.Hide();
                    }
                    
                    dummyDoc.FormClosing += delegate (object sender2, FormClosingEventArgs fe)
                    {
                        var dummyDocWindow = (DummyDoc)sender2;
                        if (dummyDocWindow.ToolStrip != null)
                        {
                            dummyDocWindow.ToolStrip.Show();
                            ToolStripManager.RevertMerge(toolBar, dummyDocWindow.ToolStrip);
                        }
                    };
                    //child.LostFocus += delegate (object sender2, EventArgs fe)
                    //{
                    //    var dummyDocWindow = (DummyDoc)sender2;
                    //    ToolStripManager.RevertMerge(toolBar, dummyDocWindow.ToolStrip);
                    //};
                }
                else if(child is ReoGridEditor)
                {
                    ReoGridEditor editor = (ReoGridEditor)child;
                    ToolStripManager.Merge(editor.ToolStrip, toolBar);
                    ToolStripManager.Merge(editor.FontToolStrip, toolBar);
                    editor.ToolStrip.Hide();
                    editor.FontToolStrip.Hide();

                    editor.FormClosing += delegate (object sender2, FormClosingEventArgs fe)
                    {
                        var editorWindow = (ReoGridEditor)sender2;
                        editorWindow.ToolStrip.Show();
                        editorWindow.FontToolStrip.Show();
                        ToolStripManager.RevertMerge(toolBar, editorWindow.ToolStrip);
                        ToolStripManager.RevertMerge(toolBar, editorWindow.FontToolStrip);

                    };
                }
            }
            //throw new NotImplementedException();
        }

        private void MainForm_Activated(object sender, EventArgs e)
        {
            if (loadComplete != null)
            {
                loadComplete();
            }
        }

        public MainForm()
        {
            
        }

        #region Methods

        private IDockContent FindDocument(string text)
        {
            if (dockPanel.DocumentStyle == DocumentStyle.SystemMdi)
            {
                foreach (Form form in MdiChildren)
                    if (form.Text == text)
                        return form as IDockContent;

                return null;
            }
            else
            {
                foreach (IDockContent content in dockPanel.Documents)
                {
                    //if(content is TaskSchedulerForm)
                    if (content.DockHandler.TabText == text)
                        return content;
                }
                    
                return null;
            }
        }

        public IDockContent FindDocumentWithPath(string fullPath)
        {
            foreach (IDockContent content in dockPanel.Documents)
                if (content.DockHandler.ToolTipText == fullPath)
                    return content;

            return null;
        }

        public List<IDockContent> FindAllDocumentsInDirectory(string fullPath)
        {
            List<IDockContent> docs = new List<IDockContent>();
            foreach (IDockContent content in dockPanel.Documents)
            {
                if (content.DockHandler.ToolTipText.Contains(fullPath))
                    docs.Add(content);
            }
            return docs;
        }

        public bool CheckTerminalOrConfiguratorOpen(string tabText)
        {
            foreach (IDockContent content in dockPanel.Documents)
            {
                if (content.DockHandler.TabText == tabText)
                {
                    return true;
                }
            }
            return false;   
        }

        private DummyDoc CreateNewDocument()
        {
            DummyDoc dummyDoc = new DummyDoc();

            int count = 1;
            string text = $"Document{count}";
            while (FindDocument(text) != null)
            {
                count++;
                text = $"Document{count}";
            }

            dummyDoc.Text = text;
            return dummyDoc;
        }

        private DummyDoc CreateNewDocument(string text, eFileType fileType = eFileType.UnKnown)
        {
            DummyDoc dummyDoc = new DummyDoc(fileType);
            dummyDoc.Text = text;
            return dummyDoc;
        }

        public void ShowOutputWindow(ToolWindow tw)
        {
            tw.PerformSafely(() =>
            {
                tw.Show(dockPanel);
            });
            
        }
        public void CreateDocumentAndShow(string text, string flName, string filePath, eFileType fileType, bool isWindows)
        {
            dockPanel.PerformSafely(() =>
            {               
                DummyDoc dummyDoc = CreateNewDocument(text, fileType);
                //dummyDoc.Text = text;
                dummyDoc.FileName = flName;
                dummyDoc.TabText = flName;
                dummyDoc.FullPath = filePath;
                dummyDoc.ToolTipText = filePath;
                dummyDoc.IsWindows = isWindows;
                dummyDoc.mainFrm = this;
                if (fileType == eFileType.Python)
                {
                    var outputWindow = new DummyOutputWindow();
                    outputWindow.ParentDoc = dummyDoc;
                    outputWindow.mainFrm = this;
                    outputWindow.TabText = (flName + " " + "Output");
                    dummyDoc.outputWindow = outputWindow;
                    ShowOutputWindow(outputWindow);
                }

                if (dockPanel.DocumentStyle == DocumentStyle.SystemMdi)
                {
                    dummyDoc.MdiParent = this;
                    dummyDoc.Show();
                }
                else
                    dummyDoc.Show(dockPanel);
            });
            
        }

        public void OpenExcelCsvDoc(string flName, string filePath, eFileType fileType)
        {
            OpenExcelCsvDoc(filePath, null);
        }

        public void OpenExcelCsvDoc(string content)
        {
            OpenExcelCsvDoc(null, content);
        }
        private void CloseAllDocuments()
        {
            if (dockPanel.DocumentStyle == DocumentStyle.SystemMdi)
            {
                foreach (Form form in MdiChildren)
                    form.Close();
            }
            else
            {
                foreach (IDockContent document in dockPanel.DocumentsToArray())
                {
                    // IMPORANT: dispose all panes.
                    document.DockHandler.DockPanel = null;
                    document.DockHandler.Close();
                }
            }
        }

        public bool HasDocuments()
        {
            return (dockPanel.DocumentsToArray().Length > 0);
        }
        public void CloseAllDocumentsExceptSolutionExplorer()
        {
            if (dockPanel.DocumentStyle == DocumentStyle.SystemMdi)
            {
                foreach (Form form in MdiChildren)
                    form.Close();
            }
            else
            {
                foreach (IDockContent document in dockPanel.DocumentsToArray())
                {
                    // IMPORANT: dispose all panes.
                    document.DockHandler.DockPanel = null;
                    document.DockHandler.Close();
                }
            }
        }


        public void CloseDocumentTab(string fullPath)
        {
            if (dockPanel.DocumentStyle == DocumentStyle.SystemMdi)
            {
                foreach (Form form in MdiChildren)
                {
                    if (form is DummyDoc)
                    {
                        var tab = (DummyDoc)form;
                        if(tab.FullPath.Equals(fullPath))
                            form.Close();
                    }
                }
            }
            else
            {
                foreach (IDockContent document in dockPanel.DocumentsToArray())
                {
                    // IMPORANT: dispose all panes.
                    if (document is DummyDoc)
                    {
                        var tab = (DummyDoc)document;
                        if (tab.FullPath.Equals(fullPath))
                        {
                            this.PerformSafely(() =>
                            {
                                document.DockHandler.DockPanel = null;
                                document.DockHandler.Close();
                            });
                        }
                    }
                }
            }
        }

        private IDockContent GetContentFromPersistString(string persistString)
        {
            if (persistString == typeof(SolutionExplorer).ToString())
                return m_solutionExplorer;
            else if (persistString == typeof(DummyPropertyWindow).ToString())
                return m_propertyWindow;
            else if (persistString == typeof(DummyToolbox).ToString())
                return m_toolbox;
            //else if (persistString == typeof(DummyOutputWindow).ToString())
            //    return m_outputWindow;
            else if (persistString == typeof(DummyTaskList).ToString())
                return m_taskList;
            else
            {
                // DummyDoc overrides GetPersistString to add extra information into persistString.
                // Any DockContent may override this value to add any needed information for deserialization.

                string[] parsedStrings = persistString.Split(new char[] { ',' });
                if (parsedStrings.Length != 3)
                    return null;

                if (parsedStrings[0] != typeof(DummyDoc).ToString())
                    return null;

                DummyDoc dummyDoc = new DummyDoc();
                if (parsedStrings[1] != string.Empty)
                    dummyDoc.FileName = parsedStrings[1];
                if (parsedStrings[2] != string.Empty)
                    dummyDoc.Text = parsedStrings[2];

                return dummyDoc;
            }
        }

        public void CloseAllContents()
        {
            // we don't want to create another instance of tool window, set DockPanel to null
            m_solutionExplorer.DockPanel = null;
            m_propertyWindow.DockPanel = null;
            m_toolbox.DockPanel = null;
            //m_outputWindow.DockPanel = null;
            m_taskList.DockPanel = null;

            // Close all other document windows
            CloseAllDocuments();

            // IMPORTANT: dispose all float windows.
            foreach (var window in dockPanel.FloatWindows.ToList())
                window.Dispose();

            System.Diagnostics.Debug.Assert(dockPanel.Panes.Count == 0);
            System.Diagnostics.Debug.Assert(dockPanel.Contents.Count == 0);
            System.Diagnostics.Debug.Assert(dockPanel.FloatWindows.Count == 0);
        }

        private readonly ToolStripRenderer _toolStripProfessionalRenderer = new ToolStripProfessionalRenderer();
        
        private void SetSchema(object sender, System.EventArgs e)
        {
            // Persist settings when rebuilding UI
            //string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "DockPanel.temp.config");

            //dockPanel.SaveAsXml(configFile);
            CloseAllContents();
            /*
            if (sender == this.menuItemSchemaVS2005)
            {
                this.dockPanel.Theme = this.vS2005Theme1;
                this.EnableVSRenderer(VisualStudioToolStripExtender.VsVersion.Vs2005, vS2005Theme1);
            }
            else if (sender == this.menuItemSchemaVS2003)
            {
                this.dockPanel.Theme = this.vS2003Theme1;
                this.EnableVSRenderer(VisualStudioToolStripExtender.VsVersion.Vs2003, vS2003Theme1);
            }
            else if (sender == this.menuItemSchemaVS2012Light)
            {
                this.dockPanel.Theme = this.vS2012LightTheme1;
                this.EnableVSRenderer(VisualStudioToolStripExtender.VsVersion.Vs2012, vS2012LightTheme1);
            }
            else if (sender == this.menuItemSchemaVS2012Blue)
            {
                this.dockPanel.Theme = this.vS2012BlueTheme1;
                this.EnableVSRenderer(VisualStudioToolStripExtender.VsVersion.Vs2012, vS2012BlueTheme1);
            }
            else if (sender == this.menuItemSchemaVS2012Dark)
            {
                this.dockPanel.Theme = this.vS2012DarkTheme1;
                this.EnableVSRenderer(VisualStudioToolStripExtender.VsVersion.Vs2012, vS2012DarkTheme1);
            }
            else if (sender == this.menuItemSchemaVS2013Blue)
            {
                this.dockPanel.Theme = this.vS2013BlueTheme1;
                this.EnableVSRenderer(VisualStudioToolStripExtender.VsVersion.Vs2013, vS2013BlueTheme1);
            }
            else if (sender == this.menuItemSchemaVS2013Light)
            {
                this.dockPanel.Theme = this.vS2013LightTheme1;
                this.EnableVSRenderer(VisualStudioToolStripExtender.VsVersion.Vs2013, vS2013LightTheme1);
            }
            else if (sender == this.menuItemSchemaVS2013Dark)
            {
                this.dockPanel.Theme = this.vS2013DarkTheme1;
                this.EnableVSRenderer(VisualStudioToolStripExtender.VsVersion.Vs2013, vS2013DarkTheme1);
            }
            else if (sender == this.menuItemSchemaVS2015Blue)
            {
                this.dockPanel.Theme = this.vS2015BlueTheme1;
                this.EnableVSRenderer(VisualStudioToolStripExtender.VsVersion.Vs2015, vS2015BlueTheme1);
            }
            else if (sender == this.menuItemSchemaVS2015Light)
            {
                this.dockPanel.Theme = this.vS2015LightTheme1;
                this.EnableVSRenderer(VisualStudioToolStripExtender.VsVersion.Vs2015, vS2015LightTheme1);
            }
            else if (sender == this.menuItemSchemaVS2015Dark)
            {
                this.dockPanel.Theme = this.vS2015DarkTheme1;
                this.EnableVSRenderer(VisualStudioToolStripExtender.VsVersion.Vs2015, vS2015DarkTheme1);
            }
            */
            if (sender == this.menuItemSchemaVS2015Blue)
            {
                this.dockPanel.Theme = this.vS2015BlueTheme1;
                this.EnableVSRenderer(VisualStudioToolStripExtender.VsVersion.Vs2015, vS2015BlueTheme1);
            }
            else if (sender == this.menuItemSchemaVS2015Light)
            {
                this.dockPanel.Theme = this.vS2015LightTheme1;
                this.EnableVSRenderer(VisualStudioToolStripExtender.VsVersion.Vs2015, vS2015LightTheme1);
            }
            else if (sender == this.menuItemSchemaVS2015Dark)
            {
                this.dockPanel.Theme = this.vS2015DarkTheme1;
                this.EnableVSRenderer(VisualStudioToolStripExtender.VsVersion.Vs2015, vS2015DarkTheme1);
            }
            //this.dockPanel.Theme = this.vS2015BlueTheme1;
            //this.EnableVSRenderer(VisualStudioToolStripExtender.VsVersion.Vs2015, vS2015BlueTheme1);
            menuItemSchemaVS2005.Checked = (sender == menuItemSchemaVS2005);
            menuItemSchemaVS2003.Checked = (sender == menuItemSchemaVS2003);
            menuItemSchemaVS2012Light.Checked = (sender == menuItemSchemaVS2012Light);
            menuItemSchemaVS2012Blue.Checked = (sender == menuItemSchemaVS2012Blue);
            menuItemSchemaVS2012Dark.Checked = (sender == menuItemSchemaVS2012Dark);
            menuItemSchemaVS2013Light.Checked = (sender == menuItemSchemaVS2013Light);
            menuItemSchemaVS2013Blue.Checked = (sender == menuItemSchemaVS2013Blue);
            menuItemSchemaVS2013Dark.Checked = (sender == menuItemSchemaVS2013Dark);
            menuItemSchemaVS2015Light.Checked = (sender == menuItemSchemaVS2015Light);
            menuItemSchemaVS2015Blue.Checked = (sender == menuItemSchemaVS2015Blue);
            menuItemSchemaVS2015Dark.Checked = (sender == menuItemSchemaVS2015Dark);
            if (dockPanel.Theme.ColorPalette != null)
            {
                statusBar.BackColor = dockPanel.Theme.ColorPalette.MainWindowStatusBarDefault.Background;
            }

            //if (File.Exists(configFile))
            //    dockPanel.LoadFromXml(configFile, m_deserializeDockContent);
        }

        private void EnableVSRenderer(VisualStudioToolStripExtender.VsVersion version, ThemeBase theme)
        {
            vsToolStripExtender1.SetStyle(mainMenu, version, theme);
            vsToolStripExtender1.SetStyle(toolBar, version, theme);
            vsToolStripExtender1.SetStyle(statusBar, version, theme);
        }

        private void SetDocumentStyle(object sender, System.EventArgs e)
        {
            DocumentStyle oldStyle = dockPanel.DocumentStyle;
            DocumentStyle newStyle;
            if (sender == menuItemDockingMdi)
                newStyle = DocumentStyle.DockingMdi;
            else if (sender == menuItemDockingWindow)
                newStyle = DocumentStyle.DockingWindow;
            else if (sender == menuItemDockingSdi)
                newStyle = DocumentStyle.DockingSdi;
            else
                newStyle = DocumentStyle.SystemMdi;

            if (oldStyle == newStyle)
                return;

            if (oldStyle == DocumentStyle.SystemMdi || newStyle == DocumentStyle.SystemMdi)
                CloseAllDocuments();

            dockPanel.DocumentStyle = newStyle;
            menuItemDockingMdi.Checked = (newStyle == DocumentStyle.DockingMdi);
            menuItemDockingWindow.Checked = (newStyle == DocumentStyle.DockingWindow);
            menuItemDockingSdi.Checked = (newStyle == DocumentStyle.DockingSdi);
            menuItemSystemMdi.Checked = (newStyle == DocumentStyle.SystemMdi);
            //toolBarButtonLayoutByCode.Enabled = (newStyle != DocumentStyle.SystemMdi);
            //toolBarButtonLayoutByXml.Enabled = (newStyle != DocumentStyle.SystemMdi);
            toolBarButtonLinuxTerminal.Enabled = (newStyle != DocumentStyle.SystemMdi);
            tsHealthCheck.Enabled = (newStyle != DocumentStyle.SystemMdi);
        }

        #endregion

        #region Event Handlers

        private void menuItemExit_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        private void menuItemSolutionExplorer_Click(object sender, System.EventArgs e)
        {
            if (m_solutionExplorer.DockState == DockState.DockLeftAutoHide || m_solutionExplorer.DockState == DockState.Unknown)
            {
                m_solutionExplorer.Show(dockPanel, DockState.DockLeftAutoHide);
                m_solutionExplorer.Show(dockPanel, DockState.DockLeft);
            }
            else
                m_solutionExplorer.Show(dockPanel, DockState.DockLeftAutoHide);
        }

        private void menuItemPropertyWindow_Click(object sender, System.EventArgs e)
        {
            m_propertyWindow.Show(dockPanel);
        }

        private async void menuItemToolbox_Click(object sender, System.EventArgs e)
        {
            if (HasDocuments())
            {
                if (MessageBox.Show(this, "Operation will close all opened documents, Are you sure?",
                                string.Empty, MessageBoxButtons.OKCancel)
                                == System.Windows.Forms.DialogResult.OK)
                {
                    CloseAllDocumentsExceptSolutionExplorer();
                    ConfigurationForm frm = new ConfigurationForm(string.Empty, () =>
                    {
                        this.Hide();
                    });
                    frm.ShowDialog(this);
                }
            }
            else
            {
                ConfigurationForm frm = new ConfigurationForm(string.Empty, () =>
                {
                    this.Hide();
                });
                frm.ShowDialog(this);

            }

        }

        private async void menuItemOutputWindow_Click(object sender, System.EventArgs e)
        {
            var isShown = false;
            foreach (IDockContent document in dockPanel.DocumentsToArray())
            {
                if (document is DummyDoc)
                {
                    isShown = (isShown || ((DummyDoc)document).outputWindow.DockState == DockState.DockBottom);
                }
            }
            foreach (IDockContent document in dockPanel.DocumentsToArray())
            {
                if (document is DummyDoc)
                {
                    if (!isShown)
                        ((DummyDoc)document).outputWindow.Show(dockPanel, DockState.DockBottom);
                    else
                        ((DummyDoc)document).outputWindow.Show(dockPanel, DockState.DockBottomAutoHide);
                }
            }
        }

        private async void menuItemTaskList_Click(object sender, System.EventArgs e)
        {
            if (m_taskList.DockState == DockState.DockTopAutoHide)
                m_taskList.Show(dockPanel, DockState.DockTop);
            else
                m_taskList.Show(dockPanel, DockState.DockTopAutoHide);
        }

        private void menuItemAbout_Click(object sender, System.EventArgs e)
        {
            AboutDialog aboutDialog = new AboutDialog();
            aboutDialog.ShowDialog(this);
        }

        private void menuItemNew_Click(object sender, System.EventArgs e)
        {
            DummyDoc dummyDoc = CreateNewDocument();
            if (dockPanel.DocumentStyle == DocumentStyle.SystemMdi)
            {
                dummyDoc.MdiParent = this;
                dummyDoc.Show();
            }
            else
                dummyDoc.Show(dockPanel);
        }

        private void menuItemOpen_Click(object sender, System.EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();

            openFile.InitialDirectory = Application.ExecutablePath;
            openFile.Filter = "rtf files (*.rtf)|*.rtf|txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFile.FilterIndex = 1;
            openFile.RestoreDirectory = true;

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                string fullName = openFile.FileName;
                string fileName = Path.GetFileName(fullName);

                if (FindDocument(fileName) != null)
                {
                    MessageBox.Show("The document: " + fileName + " has already opened!");
                    return;
                }

                DummyDoc dummyDoc = new DummyDoc();
                dummyDoc.Text = fileName;
                if (dockPanel.DocumentStyle == DocumentStyle.SystemMdi)
                {
                    dummyDoc.MdiParent = this;
                    dummyDoc.Show();
                }
                else
                    dummyDoc.Show(dockPanel);
                try
                {
                    dummyDoc.FileName = fullName;
                }
                catch (Exception exception)
                {
                    dummyDoc.Close();
                    MessageBox.Show(exception.Message);
                }

            }
        }

        private void menuItemFile_Popup(object sender, System.EventArgs e)
        {
            if (dockPanel.DocumentStyle == DocumentStyle.SystemMdi)
            {
                menuItemClose.Enabled = 
                    menuItemCloseAll.Enabled =
                    menuItemCloseAllButThisOne.Enabled = (ActiveMdiChild != null);
            }
            else
            {
                menuItemClose.Enabled = (dockPanel.ActiveDocument != null);
                menuItemCloseAll.Enabled =
                    menuItemCloseAllButThisOne.Enabled = (dockPanel.DocumentsCount > 0);
            }
        }

        private void menuItemClose_Click(object sender, System.EventArgs e)
        {
            if (dockPanel.DocumentStyle == DocumentStyle.SystemMdi)
                ActiveMdiChild.Close();
            else if (dockPanel.ActiveDocument != null)
                dockPanel.ActiveDocument.DockHandler.Close();
        }

        private void menuItemCloseAll_Click(object sender, System.EventArgs e)
        {
            CloseAllDocuments();
        }

        private void MainForm_Load(object sender, System.EventArgs e)
        {
            SetSchema(this.menuItemSchemaVS2015Blue, null);

            string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "DockPanel.config");

            if (File.Exists(configFile))
                dockPanel.LoadFromXml(configFile, m_deserializeDockContent);

            //Mahesh  to show the default solution explorer on page load
            menuItemSolutionExplorer_Click(sender, e);
        }

        public void EnableDisableControls()
        {
            this.PerformSafely(() =>
            {
                tsSchedular.Enabled = false;
                tsHealthCheck.Enabled = false;
                toolBarButtonLinuxTerminal.Enabled = false;
                
                if (CurrentProj.otherServices != null)
                {
                    if (!string.IsNullOrEmpty(CurrentProj.otherServices.AirflowService))
                    {
                        tsSchedular.Enabled = true;
                    }
                    if (!string.IsNullOrEmpty(CurrentProj.otherServices.HealthCheckService))
                    {
                        tsHealthCheck.Enabled = true;
                    }
                }
                if (CurrentProj.terminalInfo != null)
                {
                    if (!string.IsNullOrEmpty(CurrentProj.terminalInfo.Url))
                    {
                        toolBarButtonLinuxTerminal.Enabled = true;
                    }
                }

                if (CurrentProj.IsWindows)
                {
                    tsSchedular.Enabled = true;
                }
            });
            
        }
        private void MainForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "DockPanel.config");
            //if (m_bSaveLayout)
            //    dockPanel.SaveAsXml(configFile);
            //else if (File.Exists(configFile))
            //    File.Delete(configFile);
            Environment.Exit(0);
            //Application.Exit();
        }

        private void menuItemToolBar_Click(object sender, System.EventArgs e)
        {
            toolBar.Visible = menuItemToolBar.Checked = !menuItemToolBar.Checked;
        }

        private void menuItemStatusBar_Click(object sender, System.EventArgs e)
        {
            statusBar.Visible = menuItemStatusBar.Checked = !menuItemStatusBar.Checked;
        }

        private void toolBar_ButtonClick(object sender, System.Windows.Forms.ToolStripItemClickedEventArgs e)
        {
            //if (e.ClickedItem == toolBarButtonNew)
            //    menuItemNew_Click(null, null);
            //else if (e.ClickedItem == toolBarButtonOpen)
            //    menuItemOpen_Click(null, null);
            //else 
            if (e.ClickedItem == toolBarButtonSolutionExplorer)
                menuItemSolutionExplorer_Click(null, null);
            //else if (e.ClickedItem == toolBarButtonPropertyWindow)
            //    menuItemPropertyWindow_Click(null, null);
            //else if (e.ClickedItem == toolBarButtonToolbox)
            //    menuItemToolbox_Click(null, null);
            else if (e.ClickedItem == toolBarButtonOutputWindow)
                menuItemOutputWindow_Click(null, null);
            else if (e.ClickedItem == toolBarButtonTaskList)
                menuItemTaskList_Click(null, null);
            //else if (e.ClickedItem == toolBarButtonLayoutByCode)
            //    menuItemLayoutByCode_Click(null, null);
            //else if (e.ClickedItem == toolBarButtonLayoutByXml)
            //    menuItemLayoutByXml_Click(null, null);
            else if (e.ClickedItem == toolBarButtonLinuxTerminal)
                menuItemLayoutLinuxTerminal_Click(null, null);
            //else if (e.ClickedItem == toolBarButtonConfigurator)
            //    menuItemLayoutConfirator_Click(null, null);
            else if (e.ClickedItem == iconToolStripButton2)
                menuItemLayoutSpreadSheetViewer_Click(null, null);
            else if (e.ClickedItem == tsSchedular)
                tsSchedular_Click(null, null);
            else if (e.ClickedItem == tsHealthCheck)
                tshealthCheck_Click(null, null);
        }

        private void menuItemNewWindow_Click(object sender, System.EventArgs e)
        {
            MainForm newWindow = new MainForm();
            newWindow.Text = newWindow.Text + " - New";
            newWindow.Show();
        }

        private void menuItemTools_Popup(object sender, System.EventArgs e)
        {
            menuItemLockLayout.Checked = !this.dockPanel.AllowEndUserDocking;
        }

        private void menuItemLockLayout_Click(object sender, System.EventArgs e)
        {
            dockPanel.AllowEndUserDocking = !dockPanel.AllowEndUserDocking;
        }

        private void menuItemLayoutByCode_Click(object sender, System.EventArgs e)
        {
            dockPanel.SuspendLayout(true);

            CloseAllContents();

            CreateStandardControls();

            m_solutionExplorer.Show(dockPanel, DockState.DockLeft);
            m_propertyWindow.Show(m_solutionExplorer.Pane, m_solutionExplorer);
            m_toolbox.Show(dockPanel, new Rectangle(98, 133, 200, 383));
            //m_outputWindow.Show(m_solutionExplorer.Pane, DockAlignment.Bottom, 0.35);
            m_taskList.Show(m_toolbox.Pane, DockAlignment.Left, 0.4);

            DummyDoc doc1 = CreateNewDocument("Document1");
            DummyDoc doc2 = CreateNewDocument("Document2");
            DummyDoc doc3 = CreateNewDocument("Document3");
            DummyDoc doc4 = CreateNewDocument("Document4");
            doc1.Show(dockPanel, DockState.Document);
            doc2.Show(doc1.Pane, null);
            doc3.Show(doc1.Pane, DockAlignment.Bottom, 0.5);
            doc4.Show(doc3.Pane, DockAlignment.Right, 0.5);

            dockPanel.ResumeLayout(true, true);
        }

        private void SetSplashScreen()
        {
            _showSplash = true;
            _splashScreen = new SplashScreen();
            ResizeSplash();
            _splashScreen.Visible = true;
            _splashScreen.TopMost = true;

            System.Windows.Forms.Timer _timer = new System.Windows.Forms.Timer();
            _timer.Tick += (sender, e) =>
            {
                _splashScreen.Visible = false;
                _timer.Enabled = false;
                _showSplash = false;
            };
            _timer.Interval = 4000;
            _timer.Enabled = true;
        }

        private void ResizeSplash()
        {
            if (_showSplash) {
                
            var centerXMain = (this.Location.X + this.Width) / 2.0;
            var LocationXSplash = Math.Max(0, centerXMain - (_splashScreen.Width / 2.0));

            var centerYMain = (this.Location.Y + this.Height) / 2.0;
            var LocationYSplash = Math.Max(0, centerYMain - (_splashScreen.Height / 2.0));

            //_splashScreen.Location = new Point((int)Math.Round(LocationXSplash), (int)Math.Round(LocationYSplash));
            }
        }

        private void CreateStandardControls()
        {
            m_solutionExplorer = new SolutionExplorer(studioConfig, this);
            //m_solutionExplorer = new DummySolutionExplorer();
            //m_solutionExplorer.Show(dockPanel, DockState.DockLeft);
            //m_solutionExplorer.DockState = DockState.DockLeft;
            m_propertyWindow = new DummyPropertyWindow();
            m_toolbox = new DummyToolbox();
            //m_outputWindow = new DummyOutputWindow();          
            m_taskList = new DummyTaskList();
            m_solutionExplorer.Show(dockPanel, DockState.DockLeft);
            if (studioConfig.databaseConnections != null)
            {
                var dbConns = studioConfig.databaseConnections.Where(c => c.DbType.Equals("Sql Server") || c.DbType.Equals("Postgres") || c.DbType.Equals("MySql")).Select(c => c.ConnName).ToList(); 
                dbConns.Insert(0, "-Select-");
                toolBarcbDatabase.ComboBox.DataSource = dbConns;
            }
            
        }

        private async void toolBarcbDatabase_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            new Task(() =>
            {
                UCLoaderForm loader = new UCLoaderForm();
                int index=-1;
                this.PerformSafely(() =>
                {
                    index = toolBarcbDatabase.SelectedIndex;
                });
                if (index > 0)
                {
                    new Task(() =>
                    {
                        this.PerformSafely(() =>
                        {
                            loader.ShowDialog(this);
                        });
                    }).Start();
                    string selectedVal = string.Empty;
                    this.PerformSafely(() =>
                    {
                        selectedVal = toolBarcbDatabase.ComboBox.SelectedValue.ToString();
                        index = toolBarcbDatabase.SelectedIndex;
                    });
                    
                    new Task(() =>
                    {
                        dockPanel.PerformSafely(() =>
                        {
                            DbConnectorDoc dbDoc = new DbConnectorDoc(studioConfig, selectedVal,
                                () =>
                                {
                                    this.PerformSafely(() =>
                                    {
                                        loader.Hide();
                                    });
                                });
                            dbDoc.Show(dockPanel);
                        });
                    }).Start();

                }
            }).Start();
        }

        private void menuItemLayoutLinuxTerminal_Click(object sender, System.EventArgs e)
        {
            //var loader = new UCLoaderForm();
            new Task(() =>
            {
                var tabText = "Terminal";
                if (CheckTerminalOrConfiguratorOpen(tabText))
                {
                    MessageBox.Show("Terminal already opened!");
                    return;
                }
                dockPanel.PerformSafely(() =>
                {
                    BrowserDoc dummyDoc = new BrowserDoc(CurrentProj.terminalInfo.Url, tabText);
                    if (dockPanel.DocumentStyle == DocumentStyle.SystemMdi)
                    {
                        dummyDoc.MdiParent = this;
                        dummyDoc.Show();
                    }
                    else
                        dummyDoc.Show(dockPanel);

                });
            }).Start();
            //
        }

        private void menuItemLayoutConfirator_Click(object sender, System.EventArgs e)
        {
            new Task(() =>
            {
                var tabText = "Configurator";
                if (CheckTerminalOrConfiguratorOpen(tabText))
                {
                    MessageBox.Show("Configurator already opened!");
                    return;
                }
                dockPanel.PerformSafely(() =>
                {
                    BrowserDoc dummyDoc = new BrowserDoc("http://localhost:9090", tabText);
                    if (dockPanel.DocumentStyle == DocumentStyle.SystemMdi)
                    {
                        dummyDoc.MdiParent = this;
                        dummyDoc.Show();
                    }
                    else
                        dummyDoc.Show(dockPanel);

                });
            }).Start();
        }

        void OpenExcelCsvDoc(string filePath, string content = "")
        {
            new Task(() =>
            {
                this.PerformSafely(() =>
                {
                    WaitLoader.ShowDialog(this);
                });
            }).Start();
            new Task(() =>
            {
                dockPanel.PerformSafely(() =>
                {
                    if (string.IsNullOrEmpty(content))
                    {
                        ReoGridEditor editor = new ReoGridEditor(filePath, string.Empty, this.CurrentProj,
                        () =>
                        {
                            WaitLoader.Hide();
                        });
                        editor.Show(dockPanel);
                    }
                    else
                    {
                        ReoGridEditor editor = new ReoGridEditor(null, content, this.CurrentProj,
                           () =>
                           {
                               WaitLoader.Hide();
                           });
                        editor.Show(dockPanel);
                    }

                });
            }).Start();
            
        }

        private void menuItemLayoutSpreadSheetViewer_Click(object sender, System.EventArgs e)
        {
            OpenExcelCsvDoc(null);
        }

        public void ReloadControls()
        {
            dockPanel.SuspendLayout(true);

            // In order to load layout from XML, we need to close all the DockContents
            CloseAllContents();

            CreateStandardControls();

            Assembly assembly = Assembly.GetAssembly(typeof(MainForm));
            Stream xmlStream = assembly.GetManifestResourceStream("DockSample.Resources.DockPanel.xml");
            dockPanel.LoadFromXml(xmlStream, m_deserializeDockContent);
            xmlStream.Close();

            dockPanel.ResumeLayout(true, true);
        }
        private void menuItemLayoutByXml_Click(object sender, System.EventArgs e)
        {
            ReloadControls();
        }

        private void menuItemCloseAllButThisOne_Click(object sender, System.EventArgs e)
        {
            if (dockPanel.DocumentStyle == DocumentStyle.SystemMdi)
            {
                Form activeMdi = ActiveMdiChild;
                foreach (Form form in MdiChildren)
                {
                    if (form != activeMdi)
                        form.Close();
                }
            }
            else
            {
                foreach (IDockContent document in dockPanel.DocumentsToArray())
                {
                    if (!document.DockHandler.IsActivated)
                        document.DockHandler.Close();
                }
            }
        }

        private void menuItemShowDocumentIcon_Click(object sender, System.EventArgs e)
        {
            dockPanel.ShowDocumentIcon = menuItemShowDocumentIcon.Checked = !menuItemShowDocumentIcon.Checked;
        }

        private void showRightToLeft_Click(object sender, EventArgs e)
        {
            CloseAllContents();
            if (showRightToLeft.Checked)
            {
                this.RightToLeft = RightToLeft.No;
                this.RightToLeftLayout = false;
            }
            else
            {
                this.RightToLeft = RightToLeft.Yes;
                this.RightToLeftLayout = true;
            }
            m_solutionExplorer.RightToLeftLayout = this.RightToLeftLayout;
            showRightToLeft.Checked = !showRightToLeft.Checked;
        }

        private void exitWithoutSavingLayout_Click(object sender, EventArgs e)
        {
            m_bSaveLayout = false;
            Close();
            m_bSaveLayout = true;
        }

        #endregion

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            ResizeSplash();
        }

        private async void tsSchedular_Click(object sender, EventArgs e)
        {
            new Task(() =>
            {
                var tabText = "Scheduler";
                if (!CurrentProj.IsWindows)
                {
                    if (CheckTerminalOrConfiguratorOpen(tabText))
                    {
                        MessageBox.Show("Scheduler already opened!");
                        return;
                    }
                }
                else
                {
                    //Code to check Tab is already open or not
                    foreach (IDockContent content in dockPanel.Documents)
                    {
                        if(content is TaskSchedulerForm)
                        {
                            MessageBox.Show("Task Scheduler already opened!");
                            return;
                        }    
                    }
                }
                
                dockPanel.PerformSafely(() =>
                {
                    if (!CurrentProj.IsWindows)
                    {
                        if (CurrentProj.otherServices.AirflowService.Length > 0)
                        {
                            BrowserDoc dummyDoc = new BrowserDoc(CurrentProj.otherServices.AirflowService, tabText);
                            if (dockPanel.DocumentStyle == DocumentStyle.SystemMdi)
                            {
                                dummyDoc.MdiParent = this;
                                dummyDoc.Show();
                            }
                            else
                                dummyDoc.Show(dockPanel);
                        }
                    }
                    else 
                    {
                        dockPanel.PerformSafely(() =>
                        {
                            TaskSchedulerForm taskSchedulerForm = new TaskSchedulerForm(AppDomain.CurrentDomain.BaseDirectory);
                            //taskSchedulerForm.Show(dockPanel);
                            if (dockPanel.DocumentStyle == DocumentStyle.SystemMdi)
                            {
                                taskSchedulerForm.MdiParent = this;
                                taskSchedulerForm.Show();
                            }
                            else
                                taskSchedulerForm.Show(dockPanel);
                        });
                    }
                });
            }).Start();
        }

        private async void tshealthCheck_Click(object sender, EventArgs e)
        {
            new Task(() =>
            {
                var tabText = "Health Check";
                if (CheckTerminalOrConfiguratorOpen(tabText))
                {
                    MessageBox.Show("Health Check already opened!");
                    return;
                }
                dockPanel.PerformSafely(() =>
                {
                    if (CurrentProj.otherServices.HealthCheckService.Length > 0)
                    {
                        BrowserDoc dummyDoc = new BrowserDoc(CurrentProj.otherServices.HealthCheckService, tabText);
                        if (dockPanel.DocumentStyle == DocumentStyle.SystemMdi)
                        {
                            dummyDoc.MdiParent = this;
                            dummyDoc.Show();
                        }
                        else
                            dummyDoc.Show(dockPanel);
                    }


                });
            }).Start();
        }

    }
}