namespace DockSample
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being usedshow.
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.menuItemFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemClose = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemCloseAll = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemCloseAllButThisOne = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSaveAll = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSaveClose = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemView = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSolutionExplorer = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemOutputWindow = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemTaskList = new System.Windows.Forms.ToolStripMenuItem();
            this.advanceanalyticsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemToolBar = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemStatusBar = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemTools = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemLockLayout = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemShowDocumentIcon = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemSchemaVS2015Light = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSchemaVS2015Blue = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSchemaVS2015Dark = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemConfiguration = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSchemaVS2013Light = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSchemaVS2013Blue = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSchemaVS2013Dark = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSchemaVS2012Light = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSchemaVS2012Blue = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSchemaVS2012Dark = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSchemaVS2005 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSchemaVS2003 = new System.Windows.Forms.ToolStripMenuItem();
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.toolBar = new System.Windows.Forms.ToolStrip();
            this.toolBarButtonSolutionExplorer = new System.Windows.Forms.ToolStripButton();
            this.toolBarButtonOutputWindow = new System.Windows.Forms.ToolStripButton();
            this.toolBarButtonTaskList = new System.Windows.Forms.ToolStripButton();
            this.toolBarButtonSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolBarButtonLinuxTerminal = new FontAwesome.Sharp.IconToolStripButton();
            this.iconToolStripButton2 = new FontAwesome.Sharp.IconToolStripButton();
            this.tsSchedular = new FontAwesome.Sharp.IconToolStripButton();
            this.tsHealthCheck = new FontAwesome.Sharp.IconToolStripButton();
            this.tsClusterSetup = new FontAwesome.Sharp.IconToolStripButton();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolBarcbDatabase = new System.Windows.Forms.ToolStripComboBox();
            this.dockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.vS2015DarkTheme1 = new WeifenLuo.WinFormsUI.Docking.VS2015DarkTheme();
            this.vS2015BlueTheme1 = new WeifenLuo.WinFormsUI.Docking.VS2015BlueTheme();
            this.vsToolStripExtender1 = new WeifenLuo.WinFormsUI.Docking.VisualStudioToolStripExtender(this.components);
            this.mainMenu.SuspendLayout();
            this.toolBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemFile,
            this.menuItemView,
            this.menuItemTools,
            this.menuItemHelp});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(879, 24);
            this.mainMenu.TabIndex = 7;
            // 
            // menuItemFile
            // 
            this.menuItemFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemOpen,
            this.menuItemClose,
            this.menuItemCloseAll,
            this.menuItemCloseAllButThisOne,
            this.menuItemSaveAll,
            this.menuItemSaveClose,
            this.menuItem4,
            this.menuItemExit});
            this.menuItemFile.Name = "menuItemFile";
            this.menuItemFile.Size = new System.Drawing.Size(37, 20);
            this.menuItemFile.Text = "&File";
            this.menuItemFile.DropDownOpening += new System.EventHandler(this.menuItemFile_Popup);
            // 
            // menuItemOpen
            // 
            this.menuItemOpen.Name = "menuItemOpen";
            this.menuItemOpen.Size = new System.Drawing.Size(190, 22);
            this.menuItemOpen.Text = "&Open";
            // 
            // menuItemClose
            // 
            this.menuItemClose.Name = "menuItemClose";
            this.menuItemClose.Size = new System.Drawing.Size(190, 22);
            this.menuItemClose.Text = "&Close";
            this.menuItemClose.Click += new System.EventHandler(this.menuItemClose_Click);
            // 
            // menuItemCloseAll
            // 
            this.menuItemCloseAll.Name = "menuItemCloseAll";
            this.menuItemCloseAll.Size = new System.Drawing.Size(190, 22);
            this.menuItemCloseAll.Text = "Close &All";
            this.menuItemCloseAll.Click += new System.EventHandler(this.menuItemCloseAll_Click);
            // 
            // menuItemCloseAllButThisOne
            // 
            this.menuItemCloseAllButThisOne.Name = "menuItemCloseAllButThisOne";
            this.menuItemCloseAllButThisOne.Size = new System.Drawing.Size(190, 22);
            this.menuItemCloseAllButThisOne.Text = "Close All &But This One";
            this.menuItemCloseAllButThisOne.Click += new System.EventHandler(this.menuItemCloseAllButThisOne_Click);
            // 
            // menuItemSaveAll
            // 
            this.menuItemSaveAll.Name = "menuItemSaveAll";
            this.menuItemSaveAll.Size = new System.Drawing.Size(190, 22);
            this.menuItemSaveAll.Text = "&Save All";
            this.menuItemSaveAll.Click += new System.EventHandler(this.menuItemSaveAll_Click);
            // 
            // menuItemSaveClose
            // 
            this.menuItemSaveClose.Name = "menuItemSaveClose";
            this.menuItemSaveClose.Size = new System.Drawing.Size(190, 22);
            this.menuItemSaveClose.Text = "&Save && Close";
            this.menuItemSaveClose.Visible = false;
            this.menuItemSaveClose.Click += new System.EventHandler(this.menuItemSaveClose_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Name = "menuItem4";
            this.menuItem4.Size = new System.Drawing.Size(187, 6);
            // 
            // menuItemExit
            // 
            this.menuItemExit.Name = "menuItemExit";
            this.menuItemExit.Size = new System.Drawing.Size(190, 22);
            this.menuItemExit.Text = "&Exit";
            this.menuItemExit.Click += new System.EventHandler(this.menuItemExit_Click);
            // 
            // menuItemView
            // 
            this.menuItemView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemSolutionExplorer,
            this.menuItemOutputWindow,
            this.menuItemTaskList,
            this.advanceanalyticsToolStripMenuItem,
            this.menuItem1,
            this.menuItemToolBar,
            this.menuItemStatusBar,
            this.menuItem2});
            this.menuItemView.MergeIndex = 1;
            this.menuItemView.Name = "menuItemView";
            this.menuItemView.Size = new System.Drawing.Size(44, 20);
            this.menuItemView.Text = "&View";
            // 
            // menuItemSolutionExplorer
            // 
            this.menuItemSolutionExplorer.Name = "menuItemSolutionExplorer";
            this.menuItemSolutionExplorer.Size = new System.Drawing.Size(171, 22);
            this.menuItemSolutionExplorer.Text = "&Solution Explorer";
            this.menuItemSolutionExplorer.Click += new System.EventHandler(this.menuItemSolutionExplorer_Click);
            // 
            // menuItemOutputWindow
            // 
            this.menuItemOutputWindow.Name = "menuItemOutputWindow";
            this.menuItemOutputWindow.Size = new System.Drawing.Size(171, 22);
            this.menuItemOutputWindow.Text = "&Output Window";
            this.menuItemOutputWindow.Click += new System.EventHandler(this.menuItemOutputWindow_Click);
            // 
            // menuItemTaskList
            // 
            this.menuItemTaskList.Name = "menuItemTaskList";
            this.menuItemTaskList.Size = new System.Drawing.Size(171, 22);
            this.menuItemTaskList.Text = "Task &List";
            this.menuItemTaskList.Click += new System.EventHandler(this.menuItemTaskList_Click);
            // 
            // advanceanalyticsToolStripMenuItem
            // 
            this.advanceanalyticsToolStripMenuItem.Name = "advanceanalyticsToolStripMenuItem";
            this.advanceanalyticsToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.advanceanalyticsToolStripMenuItem.Text = "Advance Analytics";
            this.advanceanalyticsToolStripMenuItem.Click += new System.EventHandler(this.advanceanalyticsToolStripMenuItem_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Name = "menuItem1";
            this.menuItem1.Size = new System.Drawing.Size(168, 6);
            // 
            // menuItemToolBar
            // 
            this.menuItemToolBar.Checked = true;
            this.menuItemToolBar.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuItemToolBar.Name = "menuItemToolBar";
            this.menuItemToolBar.Size = new System.Drawing.Size(171, 22);
            this.menuItemToolBar.Text = "Tool &Bar";
            this.menuItemToolBar.Click += new System.EventHandler(this.menuItemToolBar_Click);
            // 
            // menuItemStatusBar
            // 
            this.menuItemStatusBar.Checked = true;
            this.menuItemStatusBar.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuItemStatusBar.Name = "menuItemStatusBar";
            this.menuItemStatusBar.Size = new System.Drawing.Size(171, 22);
            this.menuItemStatusBar.Text = "Status B&ar";
            this.menuItemStatusBar.Click += new System.EventHandler(this.menuItemStatusBar_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Name = "menuItem2";
            this.menuItem2.Size = new System.Drawing.Size(168, 6);
            // 
            // menuItemTools
            // 
            this.menuItemTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemLockLayout,
            this.menuItemShowDocumentIcon,
            this.menuItem3,
            this.menuItemSchemaVS2015Light,
            this.menuItemSchemaVS2015Blue,
            this.menuItemSchemaVS2015Dark,
            this.menuItem5,
            this.menuItemConfiguration});
            this.menuItemTools.MergeIndex = 2;
            this.menuItemTools.Name = "menuItemTools";
            this.menuItemTools.Size = new System.Drawing.Size(46, 20);
            this.menuItemTools.Text = "&Tools";
            this.menuItemTools.DropDownOpening += new System.EventHandler(this.menuItemTools_Popup);
            // 
            // menuItemLockLayout
            // 
            this.menuItemLockLayout.Name = "menuItemLockLayout";
            this.menuItemLockLayout.Size = new System.Drawing.Size(188, 22);
            this.menuItemLockLayout.Text = "&Lock Layout";
            this.menuItemLockLayout.Click += new System.EventHandler(this.menuItemLockLayout_Click);
            // 
            // menuItemShowDocumentIcon
            // 
            this.menuItemShowDocumentIcon.Name = "menuItemShowDocumentIcon";
            this.menuItemShowDocumentIcon.Size = new System.Drawing.Size(188, 22);
            this.menuItemShowDocumentIcon.Text = "&Show Document Icon";
            this.menuItemShowDocumentIcon.Click += new System.EventHandler(this.menuItemShowDocumentIcon_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Name = "menuItem3";
            this.menuItem3.Size = new System.Drawing.Size(185, 6);
            this.menuItem3.Visible = false;
            // 
            // menuItemSchemaVS2015Light
            // 
            this.menuItemSchemaVS2015Light.Name = "menuItemSchemaVS2015Light";
            this.menuItemSchemaVS2015Light.Size = new System.Drawing.Size(188, 22);
            this.menuItemSchemaVS2015Light.Text = "Light Theme";
            this.menuItemSchemaVS2015Light.Visible = false;
            this.menuItemSchemaVS2015Light.Click += new System.EventHandler(this.SetSchema);
            // 
            // menuItemSchemaVS2015Blue
            // 
            this.menuItemSchemaVS2015Blue.Name = "menuItemSchemaVS2015Blue";
            this.menuItemSchemaVS2015Blue.Size = new System.Drawing.Size(188, 22);
            this.menuItemSchemaVS2015Blue.Text = "Blue Theme";
            this.menuItemSchemaVS2015Blue.Visible = false;
            this.menuItemSchemaVS2015Blue.Click += new System.EventHandler(this.SetSchema);
            // 
            // menuItemSchemaVS2015Dark
            // 
            this.menuItemSchemaVS2015Dark.Name = "menuItemSchemaVS2015Dark";
            this.menuItemSchemaVS2015Dark.Size = new System.Drawing.Size(188, 22);
            this.menuItemSchemaVS2015Dark.Text = "Dark Theme";
            this.menuItemSchemaVS2015Dark.Visible = false;
            this.menuItemSchemaVS2015Dark.Click += new System.EventHandler(this.SetSchema);
            // 
            // menuItem5
            // 
            this.menuItem5.Name = "menuItem5";
            this.menuItem5.Size = new System.Drawing.Size(185, 6);
            // 
            // menuItemConfiguration
            // 
            this.menuItemConfiguration.Name = "menuItemConfiguration";
            this.menuItemConfiguration.Size = new System.Drawing.Size(188, 22);
            this.menuItemConfiguration.Text = "Open &Configuration";
            this.menuItemConfiguration.Click += new System.EventHandler(this.menuItemConfiguration_Click);
            // 
            // menuItemHelp
            // 
            this.menuItemHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemAbout});
            this.menuItemHelp.MergeIndex = 3;
            this.menuItemHelp.Name = "menuItemHelp";
            this.menuItemHelp.Size = new System.Drawing.Size(44, 20);
            this.menuItemHelp.Text = "&Help";
            // 
            // menuItemAbout
            // 
            this.menuItemAbout.Name = "menuItemAbout";
            this.menuItemAbout.Size = new System.Drawing.Size(184, 22);
            this.menuItemAbout.Text = "&About KockpitStudio";
            this.menuItemAbout.Click += new System.EventHandler(this.menuItemAbout_Click);
            // 
            // menuItemSchemaVS2013Light
            // 
            this.menuItemSchemaVS2013Light.Name = "menuItemSchemaVS2013Light";
            this.menuItemSchemaVS2013Light.Size = new System.Drawing.Size(255, 22);
            this.menuItemSchemaVS2013Light.Text = "Schema: VS2013 Light";
            this.menuItemSchemaVS2013Light.Click += new System.EventHandler(this.SetSchema);
            // 
            // menuItemSchemaVS2013Blue
            // 
            this.menuItemSchemaVS2013Blue.Name = "menuItemSchemaVS2013Blue";
            this.menuItemSchemaVS2013Blue.Size = new System.Drawing.Size(255, 22);
            this.menuItemSchemaVS2013Blue.Text = "Schema: VS2013 Blue";
            this.menuItemSchemaVS2013Blue.Click += new System.EventHandler(this.SetSchema);
            // 
            // menuItemSchemaVS2013Dark
            // 
            this.menuItemSchemaVS2013Dark.Name = "menuItemSchemaVS2013Dark";
            this.menuItemSchemaVS2013Dark.Size = new System.Drawing.Size(255, 22);
            this.menuItemSchemaVS2013Dark.Text = "Schema: VS2013 Dark";
            this.menuItemSchemaVS2013Dark.Click += new System.EventHandler(this.SetSchema);
            // 
            // menuItemSchemaVS2012Light
            // 
            this.menuItemSchemaVS2012Light.Name = "menuItemSchemaVS2012Light";
            this.menuItemSchemaVS2012Light.Size = new System.Drawing.Size(255, 22);
            this.menuItemSchemaVS2012Light.Text = "Schema: VS2012 Light";
            this.menuItemSchemaVS2012Light.Click += new System.EventHandler(this.SetSchema);
            // 
            // menuItemSchemaVS2012Blue
            // 
            this.menuItemSchemaVS2012Blue.Name = "menuItemSchemaVS2012Blue";
            this.menuItemSchemaVS2012Blue.Size = new System.Drawing.Size(255, 22);
            this.menuItemSchemaVS2012Blue.Text = "Schema: VS2012 Blue";
            this.menuItemSchemaVS2012Blue.Click += new System.EventHandler(this.SetSchema);
            // 
            // menuItemSchemaVS2012Dark
            // 
            this.menuItemSchemaVS2012Dark.Name = "menuItemSchemaVS2012Dark";
            this.menuItemSchemaVS2012Dark.Size = new System.Drawing.Size(255, 22);
            this.menuItemSchemaVS2012Dark.Text = "Schema: VS2012 Dark";
            this.menuItemSchemaVS2012Dark.Click += new System.EventHandler(this.SetSchema);
            // 
            // menuItemSchemaVS2005
            // 
            this.menuItemSchemaVS2005.Checked = true;
            this.menuItemSchemaVS2005.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuItemSchemaVS2005.Name = "menuItemSchemaVS2005";
            this.menuItemSchemaVS2005.Size = new System.Drawing.Size(255, 22);
            this.menuItemSchemaVS2005.Text = "Schema: VS200&5";
            this.menuItemSchemaVS2005.Click += new System.EventHandler(this.SetSchema);
            // 
            // menuItemSchemaVS2003
            // 
            this.menuItemSchemaVS2003.Name = "menuItemSchemaVS2003";
            this.menuItemSchemaVS2003.Size = new System.Drawing.Size(255, 22);
            this.menuItemSchemaVS2003.Text = "Schema: VS200&3";
            this.menuItemSchemaVS2003.Click += new System.EventHandler(this.SetSchema);
            // 
            // statusBar
            // 
            this.statusBar.BackColor = System.Drawing.Color.Black;
            this.statusBar.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusBar.Location = new System.Drawing.Point(0, 387);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(879, 22);
            this.statusBar.TabIndex = 4;
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "");
            this.imageList.Images.SetKeyName(1, "");
            this.imageList.Images.SetKeyName(2, "");
            this.imageList.Images.SetKeyName(3, "");
            this.imageList.Images.SetKeyName(4, "");
            this.imageList.Images.SetKeyName(5, "");
            this.imageList.Images.SetKeyName(6, "");
            this.imageList.Images.SetKeyName(7, "");
            this.imageList.Images.SetKeyName(8, "");
            // 
            // toolBar
            // 
            this.toolBar.ImageList = this.imageList;
            this.toolBar.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolBarButtonSolutionExplorer,
            this.toolBarButtonOutputWindow,
            this.toolBarButtonTaskList,
            this.toolBarButtonSeparator2,
            this.toolBarButtonLinuxTerminal,
            this.iconToolStripButton2,
            this.tsSchedular,
            this.tsHealthCheck,
            this.tsClusterSetup,
            this.toolStripLabel1,
            this.toolBarcbDatabase});
            this.toolBar.Location = new System.Drawing.Point(0, 24);
            this.toolBar.Name = "toolBar";
            this.toolBar.Size = new System.Drawing.Size(879, 27);
            this.toolBar.TabIndex = 6;
            this.toolBar.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolBar_ButtonClick);
            // 
            // toolBarButtonSolutionExplorer
            // 
            this.toolBarButtonSolutionExplorer.ImageIndex = 2;
            this.toolBarButtonSolutionExplorer.Name = "toolBarButtonSolutionExplorer";
            this.toolBarButtonSolutionExplorer.Size = new System.Drawing.Size(24, 24);
            this.toolBarButtonSolutionExplorer.ToolTipText = "Solution Explorer";
            // 
            // toolBarButtonOutputWindow
            // 
            this.toolBarButtonOutputWindow.ImageIndex = 5;
            this.toolBarButtonOutputWindow.Name = "toolBarButtonOutputWindow";
            this.toolBarButtonOutputWindow.Size = new System.Drawing.Size(24, 24);
            this.toolBarButtonOutputWindow.ToolTipText = "Output Window";
            // 
            // toolBarButtonTaskList
            // 
            this.toolBarButtonTaskList.ImageIndex = 6;
            this.toolBarButtonTaskList.Name = "toolBarButtonTaskList";
            this.toolBarButtonTaskList.Size = new System.Drawing.Size(24, 24);
            this.toolBarButtonTaskList.ToolTipText = "Task List";
            // 
            // toolBarButtonSeparator2
            // 
            this.toolBarButtonSeparator2.Name = "toolBarButtonSeparator2";
            this.toolBarButtonSeparator2.Size = new System.Drawing.Size(6, 27);
            // 
            // toolBarButtonLinuxTerminal
            // 
            this.toolBarButtonLinuxTerminal.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.toolBarButtonLinuxTerminal.IconChar = FontAwesome.Sharp.IconChar.Terminal;
            this.toolBarButtonLinuxTerminal.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(97)))), ((int)(((byte)(179)))));
            this.toolBarButtonLinuxTerminal.IconSize = 16;
            this.toolBarButtonLinuxTerminal.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.toolBarButtonLinuxTerminal.Name = "toolBarButtonLinuxTerminal";
            this.toolBarButtonLinuxTerminal.Rotation = 0D;
            this.toolBarButtonLinuxTerminal.Size = new System.Drawing.Size(76, 24);
            this.toolBarButtonLinuxTerminal.Text = "Terminal";
            this.toolBarButtonLinuxTerminal.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal;
            // 
            // iconToolStripButton2
            // 
            this.iconToolStripButton2.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.iconToolStripButton2.IconChar = FontAwesome.Sharp.IconChar.Table;
            this.iconToolStripButton2.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(97)))), ((int)(((byte)(179)))));
            this.iconToolStripButton2.IconSize = 16;
            this.iconToolStripButton2.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.iconToolStripButton2.Name = "iconToolStripButton2";
            this.iconToolStripButton2.Rotation = 0D;
            this.iconToolStripButton2.Size = new System.Drawing.Size(95, 24);
            this.iconToolStripButton2.Text = "Spreadsheet";
            // 
            // tsSchedular
            // 
            this.tsSchedular.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.tsSchedular.IconChar = FontAwesome.Sharp.IconChar.Clock;
            this.tsSchedular.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(97)))), ((int)(((byte)(179)))));
            this.tsSchedular.IconSize = 16;
            this.tsSchedular.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.tsSchedular.Name = "tsSchedular";
            this.tsSchedular.Rotation = 0D;
            this.tsSchedular.Size = new System.Drawing.Size(83, 24);
            this.tsSchedular.Text = "Scheduler";
            // 
            // tsHealthCheck
            // 
            this.tsHealthCheck.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.tsHealthCheck.IconChar = FontAwesome.Sharp.IconChar.Medkit;
            this.tsHealthCheck.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(97)))), ((int)(((byte)(179)))));
            this.tsHealthCheck.IconSize = 16;
            this.tsHealthCheck.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.tsHealthCheck.Name = "tsHealthCheck";
            this.tsHealthCheck.Rotation = 0D;
            this.tsHealthCheck.Size = new System.Drawing.Size(99, 24);
            this.tsHealthCheck.Text = "HealthCheck";
            // 
            // tsClusterSetup
            // 
            this.tsClusterSetup.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.tsClusterSetup.IconChar = FontAwesome.Sharp.IconChar.Cogs;
            this.tsClusterSetup.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(97)))), ((int)(((byte)(179)))));
            this.tsClusterSetup.IconSize = 16;
            this.tsClusterSetup.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.tsClusterSetup.Name = "tsClusterSetup";
            this.tsClusterSetup.Rotation = 0D;
            this.tsClusterSetup.Size = new System.Drawing.Size(98, 24);
            this.tsClusterSetup.Text = "ClusterSetup";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(60, 24);
            this.toolStripLabel1.Text = "Databases";
            // 
            // toolBarcbDatabase
            // 
            this.toolBarcbDatabase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolBarcbDatabase.Name = "toolBarcbDatabase";
            this.toolBarcbDatabase.Size = new System.Drawing.Size(100, 27);
            this.toolBarcbDatabase.SelectedIndexChanged += new System.EventHandler(this.toolBarcbDatabase_SelectedIndexChanged);
            // 
            // dockPanel
            // 
            this.dockPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dockPanel.DockBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(57)))), ((int)(((byte)(85)))));
            this.dockPanel.DockBottomPortion = 150D;
            this.dockPanel.DockLeftPortion = 200D;
            this.dockPanel.DockRightPortion = 200D;
            this.dockPanel.DockTopPortion = 150D;
            this.dockPanel.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World, ((byte)(0)));
            this.dockPanel.Location = new System.Drawing.Point(0, 51);
            this.dockPanel.Name = "dockPanel";
            this.dockPanel.RightToLeftLayout = true;
            this.dockPanel.ShowAutoHideContentOnHover = false;
            this.dockPanel.Size = new System.Drawing.Size(879, 336);
            this.dockPanel.TabIndex = 0;
            // 
            // vsToolStripExtender1
            // 
            this.vsToolStripExtender1.DefaultRenderer = null;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(879, 409);
            this.Controls.Add(this.dockPanel);
            this.Controls.Add(this.toolBar);
            this.Controls.Add(this.mainMenu);
            this.Controls.Add(this.statusBar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.mainMenu;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Kockpit Studio";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Closing += new System.ComponentModel.CancelEventHandler(this.MainForm_Closing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.SizeChanged += new System.EventHandler(this.MainForm_SizeChanged);
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.toolBar.ResumeLayout(false);
            this.toolBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.ToolStrip toolBar;
        private System.Windows.Forms.ToolStripButton toolBarButtonSolutionExplorer;
        private System.Windows.Forms.ToolStripButton toolBarButtonOutputWindow;
        private System.Windows.Forms.ToolStripButton toolBarButtonTaskList;
        private System.Windows.Forms.ToolStripSeparator toolBarButtonSeparator2;
        private System.Windows.Forms.ToolStripComboBox toolBarcbDatabase;
        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem menuItemFile;
        private System.Windows.Forms.ToolStripMenuItem menuItemClose;
        private System.Windows.Forms.ToolStripMenuItem menuItemCloseAll;
        private System.Windows.Forms.ToolStripMenuItem menuItemCloseAllButThisOne;
        private System.Windows.Forms.ToolStripSeparator menuItem4;
        private System.Windows.Forms.ToolStripMenuItem menuItemExit;
        private System.Windows.Forms.ToolStripMenuItem menuItemView;
        private System.Windows.Forms.ToolStripMenuItem menuItemSolutionExplorer;
        private System.Windows.Forms.ToolStripMenuItem menuItemOutputWindow;
        private System.Windows.Forms.ToolStripMenuItem menuItemTaskList;
        private System.Windows.Forms.ToolStripSeparator menuItem1;
        private System.Windows.Forms.ToolStripMenuItem menuItemToolBar;
        private System.Windows.Forms.ToolStripMenuItem menuItemStatusBar;
        private System.Windows.Forms.ToolStripSeparator menuItem2;
        private System.Windows.Forms.ToolStripMenuItem menuItemTools;
        private System.Windows.Forms.ToolStripMenuItem menuItemLockLayout;
        private System.Windows.Forms.ToolStripSeparator menuItem3;
        private System.Windows.Forms.ToolStripMenuItem menuItemSchemaVS2005;
        private System.Windows.Forms.ToolStripMenuItem menuItemSchemaVS2003;
        private System.Windows.Forms.ToolStripSeparator menuItem5;
        private System.Windows.Forms.ToolStripMenuItem menuItemShowDocumentIcon;
        private System.Windows.Forms.ToolStripMenuItem menuItemHelp;
        private System.Windows.Forms.ToolStripMenuItem menuItemAbout;
        private System.Windows.Forms.StatusStrip statusBar;
        private System.Windows.Forms.ToolStripMenuItem menuItemSchemaVS2012Light;
        private System.Windows.Forms.ToolStripMenuItem menuItemSchemaVS2012Blue;
        private System.Windows.Forms.ToolStripMenuItem menuItemSchemaVS2012Dark;
        private System.Windows.Forms.ToolStripMenuItem menuItemSchemaVS2013Light;
        private System.Windows.Forms.ToolStripMenuItem menuItemSchemaVS2013Blue;
        private System.Windows.Forms.ToolStripMenuItem menuItemSchemaVS2013Dark;
        private System.Windows.Forms.ToolStripMenuItem menuItemSchemaVS2015Light;
        private System.Windows.Forms.ToolStripMenuItem menuItemSchemaVS2015Blue;
        private System.Windows.Forms.ToolStripMenuItem menuItemSchemaVS2015Dark;
        private WeifenLuo.WinFormsUI.Docking.VS2015LightTheme vS2015LightTheme1;
        private WeifenLuo.WinFormsUI.Docking.VS2015BlueTheme vS2015BlueTheme1;
        private WeifenLuo.WinFormsUI.Docking.VS2015DarkTheme vS2015DarkTheme1;
        //private WeifenLuo.WinFormsUI.Docking.VS2013LightTheme vS2013LightTheme1;
        //private WeifenLuo.WinFormsUI.Docking.VS2013BlueTheme vS2013BlueTheme1;
        //private WeifenLuo.WinFormsUI.Docking.VS2013DarkTheme vS2013DarkTheme1;
        //private WeifenLuo.WinFormsUI.Docking.VS2012LightTheme vS2012LightTheme1;
        //private WeifenLuo.WinFormsUI.Docking.VS2012BlueTheme vS2012BlueTheme1;
        //private WeifenLuo.WinFormsUI.Docking.VS2012DarkTheme vS2012DarkTheme1;
        //private WeifenLuo.WinFormsUI.Docking.VS2003Theme vS2003Theme1;
        //private WeifenLuo.WinFormsUI.Docking.VS2005Theme vS2005Theme1;
        private WeifenLuo.WinFormsUI.Docking.VisualStudioToolStripExtender vsToolStripExtender1;
        private FontAwesome.Sharp.IconToolStripButton toolBarButtonLinuxTerminal;
        private FontAwesome.Sharp.IconToolStripButton tsHealthCheck;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private FontAwesome.Sharp.IconToolStripButton iconToolStripButton2;
        private FontAwesome.Sharp.IconToolStripButton tsSchedular;
        private System.Windows.Forms.ToolStripMenuItem menuItemConfiguration;
        private System.Windows.Forms.ToolStripMenuItem menuItemSaveClose;
        private System.Windows.Forms.ToolStripMenuItem menuItemOpen;
        private FontAwesome.Sharp.IconToolStripButton tsClusterSetup;
        private System.Windows.Forms.ToolStripMenuItem menuItemSaveAll;
        private System.Windows.Forms.ToolStripMenuItem advanceanalyticsToolStripMenuItem;
        public WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel;
    }
}