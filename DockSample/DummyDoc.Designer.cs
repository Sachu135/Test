namespace DockSample
{
    partial class DummyDoc
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DummyDoc));
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.menuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemCheckTest = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuTabPage = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.txtCodeEditor = new EasyScintilla.SimpleEditor();
            this.contextMenuForEditor = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.selecTAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.conectionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripComboBox1 = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.pnlMsg = new System.Windows.Forms.Panel();
            this.lblMsg = new System.Windows.Forms.Label();
            this.mainMenu.SuspendLayout();
            this.contextMenuTabPage.SuspendLayout();
            this.contextMenuForEditor.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.pnlMsg.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.BackColor = System.Drawing.Color.White;
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItem1});
            this.mainMenu.Location = new System.Drawing.Point(0, 4);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(448, 24);
            this.mainMenu.TabIndex = 1;
            this.mainMenu.Visible = false;
            // 
            // menuItem1
            // 
            this.menuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItem2,
            this.menuItemCheckTest});
            this.menuItem1.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.menuItem1.MergeIndex = 1;
            this.menuItem1.Name = "menuItem1";
            this.menuItem1.Size = new System.Drawing.Size(100, 20);
            this.menuItem1.Text = "&MDI Document";
            // 
            // menuItem2
            // 
            this.menuItem2.Name = "menuItem2";
            this.menuItem2.Size = new System.Drawing.Size(131, 22);
            this.menuItem2.Text = "Test";
            this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
            // 
            // menuItemCheckTest
            // 
            this.menuItemCheckTest.Name = "menuItemCheckTest";
            this.menuItemCheckTest.Size = new System.Drawing.Size(131, 22);
            this.menuItemCheckTest.Text = "Check Test";
            this.menuItemCheckTest.Click += new System.EventHandler(this.menuItemCheckTest_Click);
            // 
            // contextMenuTabPage
            // 
            this.contextMenuTabPage.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItem3,
            this.menuItem4,
            this.menuItem5});
            this.contextMenuTabPage.Name = "contextMenuTabPage";
            this.contextMenuTabPage.Size = new System.Drawing.Size(121, 70);
            // 
            // menuItem3
            // 
            this.menuItem3.Name = "menuItem3";
            this.menuItem3.Size = new System.Drawing.Size(120, 22);
            this.menuItem3.Text = "Option &1";
            // 
            // menuItem4
            // 
            this.menuItem4.Name = "menuItem4";
            this.menuItem4.Size = new System.Drawing.Size(120, 22);
            this.menuItem4.Text = "Option &2";
            // 
            // menuItem5
            // 
            this.menuItem5.Name = "menuItem5";
            this.menuItem5.Size = new System.Drawing.Size(120, 22);
            this.menuItem5.Text = "Option &3";
            // 
            // txtCodeEditor
            // 
            this.txtCodeEditor.ContextMenuStrip = this.contextMenuForEditor;
            this.txtCodeEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCodeEditor.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCodeEditor.IndentationGuides = ScintillaNET.IndentView.LookBoth;
            this.txtCodeEditor.Location = new System.Drawing.Point(0, 4);
            this.txtCodeEditor.Name = "txtCodeEditor";
            this.txtCodeEditor.Size = new System.Drawing.Size(448, 389);
            this.txtCodeEditor.Styler = null;
            this.txtCodeEditor.TabIndex = 2;
            this.txtCodeEditor.CharAdded += new System.EventHandler<ScintillaNET.CharAddedEventArgs>(this.txtCodeEditor_CharAdded);
            this.txtCodeEditor.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCodeEditor_KeyDown);
            this.txtCodeEditor.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCodeEditor_KeyPress);
            // 
            // contextMenuForEditor
            // 
            this.contextMenuForEditor.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem4,
            this.toolStripMenuItem5,
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem,
            this.toolStripMenuItem1,
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.toolStripMenuItem2,
            this.selecTAllToolStripMenuItem,
            this.toolStripMenuItem3,
            this.conectionsToolStripMenuItem});
            this.contextMenuForEditor.Name = "contextMenuForEditor";
            this.contextMenuForEditor.Size = new System.Drawing.Size(135, 226);
            this.contextMenuForEditor.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuForEditor_Opening);
            this.contextMenuForEditor.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.contextMenuForEditor_ItemClicked);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(134, 22);
            this.toolStripMenuItem4.Text = "Find";
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(131, 6);
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.undoToolStripMenuItem.Text = "Undo";
            // 
            // redoToolStripMenuItem
            // 
            this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            this.redoToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.redoToolStripMenuItem.Text = "Redo";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(131, 6);
            // 
            // cutToolStripMenuItem
            // 
            this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            this.cutToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.cutToolStripMenuItem.Text = "Cut";
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.pasteToolStripMenuItem.Text = "Paste";
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(131, 6);
            // 
            // selecTAllToolStripMenuItem
            // 
            this.selecTAllToolStripMenuItem.Name = "selecTAllToolStripMenuItem";
            this.selecTAllToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.selecTAllToolStripMenuItem.Text = "Select All";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(131, 6);
            // 
            // conectionsToolStripMenuItem
            // 
            this.conectionsToolStripMenuItem.Name = "conectionsToolStripMenuItem";
            this.conectionsToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.conectionsToolStripMenuItem.Text = "Conections";
            this.conectionsToolStripMenuItem.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.conectionsToolStripMenuItem_DropDownItemClicked);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator1,
            this.toolStripButton1,
            this.toolStripLabel1,
            this.toolStripComboBox1,
            this.toolStripButton2,
            this.toolStripButton3});
            this.toolStrip1.Location = new System.Drawing.Point(0, 4);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(448, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = global::DockSample.Properties.Resources.saveHS;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "Save";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(75, 22);
            this.toolStripLabel1.Text = "Environment";
            // 
            // toolStripComboBox1
            // 
            this.toolStripComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripComboBox1.Items.AddRange(new object[] {
            "python",
            "python3",
            "spark submit"});
            this.toolStripComboBox1.Name = "toolStripComboBox1";
            this.toolStripComboBox1.Size = new System.Drawing.Size(121, 25);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.Image = global::DockSample.Properties.Resources.sqlexecute;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(67, 22);
            this.toolStripButton2.Text = "Execute";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.Enabled = false;
            this.toolStripButton3.Image = global::DockSample.Properties.Resources.cancel;
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(51, 22);
            this.toolStripButton3.Text = "Stop";
            this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // pnlMsg
            // 
            this.pnlMsg.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(237)))), ((int)(((byte)(200)))));
            this.pnlMsg.Controls.Add(this.lblMsg);
            this.pnlMsg.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlMsg.Location = new System.Drawing.Point(0, 29);
            this.pnlMsg.Name = "pnlMsg";
            this.pnlMsg.Size = new System.Drawing.Size(448, 20);
            this.pnlMsg.TabIndex = 4;
            this.pnlMsg.Visible = false;
            // 
            // lblMsg
            // 
            this.lblMsg.AutoSize = true;
            this.lblMsg.ForeColor = System.Drawing.Color.Maroon;
            this.lblMsg.Location = new System.Drawing.Point(4, 4);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(0, 13);
            this.lblMsg.TabIndex = 0;
            // 
            // DummyDoc
            // 
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(448, 393);
            this.Controls.Add(this.pnlMsg);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.txtCodeEditor);
            this.Controls.Add(this.mainMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mainMenu;
            this.Name = "DummyDoc";
            this.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.TabPageContextMenuStrip = this.contextMenuTabPage;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DummyDoc_FormClosing);
            this.Load += new System.EventHandler(this.DummyDoc_Load);
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.contextMenuTabPage.ResumeLayout(false);
            this.contextMenuForEditor.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.pnlMsg.ResumeLayout(false);
            this.pnlMsg.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem menuItem1;
        private System.Windows.Forms.ToolStripMenuItem menuItem2;
        private System.Windows.Forms.ContextMenuStrip contextMenuTabPage;
        private System.Windows.Forms.ToolStripMenuItem menuItem3;
        private System.Windows.Forms.ToolStripMenuItem menuItem4;
        private System.Windows.Forms.ToolStripMenuItem menuItem5;
        private System.Windows.Forms.ToolStripMenuItem menuItemCheckTest;
        private System.Windows.Forms.ToolTip toolTip;
        private EasyScintilla.SimpleEditor txtCodeEditor;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBox1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ContextMenuStrip contextMenuForEditor;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selecTAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem conectionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redoToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.Panel pnlMsg;
        private System.Windows.Forms.Label lblMsg;
    }
}