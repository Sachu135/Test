namespace DockSample
{
    partial class ToolWindow
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
            this.contextMenuOutput = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.contextMenuClose = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuOutput.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuOutput
            // 
            this.contextMenuOutput.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contextMenuClose});
            this.contextMenuOutput.Name = "contextMenuStrip1";
            this.contextMenuOutput.Size = new System.Drawing.Size(181, 48);
            this.contextMenuOutput.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.contextMenuStrip1_ItemClicked);
            // 
            // contextMenuClose
            // 
            this.contextMenuClose.Name = "contextMenuClose";
            this.contextMenuClose.Size = new System.Drawing.Size(180, 22);
            this.contextMenuClose.Text = "&Close";
            // 
            // ToolWindow
            // 
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)(((((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockTop) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom)));
            this.Name = "ToolWindow";
            this.TabPageContextMenuStrip = this.contextMenuOutput;
            this.TabText = "ToolWindow";
            this.Text = "ToolWindow";
            this.contextMenuOutput.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuOutput;
        private System.Windows.Forms.ToolStripMenuItem contextMenuClose;
    }
}