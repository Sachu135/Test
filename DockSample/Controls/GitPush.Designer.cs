namespace DockSample.Controls
{
    partial class GitPush
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
            this.btnClose = new System.Windows.Forms.Button();
            this.btnPush = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.txtBranch = new System.Windows.Forms.TextBox();
            this.cmbBranches = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsBtnAddNewBranch = new System.Windows.Forms.ToolStripButton();
            this.pnlNewBranch = new System.Windows.Forms.Panel();
            this.btnCancelAddNewBranch = new System.Windows.Forms.Button();
            this.btnSaveNewBranch = new System.Windows.Forms.Button();
            this.toolStrip1.SuspendLayout();
            this.pnlNewBranch.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(115)))), ((int)(((byte)(172)))));
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(141)))), ((int)(((byte)(248)))));
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnClose.Location = new System.Drawing.Point(195, 114);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 33);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnPush
            // 
            this.btnPush.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(115)))), ((int)(((byte)(172)))));
            this.btnPush.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(141)))), ((int)(((byte)(248)))));
            this.btnPush.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPush.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPush.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnPush.Location = new System.Drawing.Point(89, 114);
            this.btnPush.Name = "btnPush";
            this.btnPush.Size = new System.Drawing.Size(100, 33);
            this.btnPush.TabIndex = 4;
            this.btnPush.Text = "Push";
            this.btnPush.UseVisualStyleBackColor = false;
            this.btnPush.Click += new System.EventHandler(this.btnPush_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 6);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(71, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "New branch :";
            // 
            // txtBranch
            // 
            this.txtBranch.Location = new System.Drawing.Point(89, 3);
            this.txtBranch.Multiline = true;
            this.txtBranch.Name = "txtBranch";
            this.txtBranch.Size = new System.Drawing.Size(204, 21);
            this.txtBranch.TabIndex = 3;
            // 
            // cmbBranches
            // 
            this.cmbBranches.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBranches.FormattingEnabled = true;
            this.cmbBranches.Location = new System.Drawing.Point(89, 70);
            this.cmbBranches.Name = "cmbBranches";
            this.cmbBranches.Size = new System.Drawing.Size(271, 21);
            this.cmbBranches.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 73);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Branch";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsBtnAddNewBranch});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(428, 25);
            this.toolStrip1.TabIndex = 8;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsBtnAddNewBranch
            // 
            this.tsBtnAddNewBranch.Image = global::DockSample.Properties.Resources.plus;
            this.tsBtnAddNewBranch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsBtnAddNewBranch.Name = "tsBtnAddNewBranch";
            this.tsBtnAddNewBranch.Size = new System.Drawing.Size(114, 22);
            this.tsBtnAddNewBranch.Text = "Add new branch";
            this.tsBtnAddNewBranch.Click += new System.EventHandler(this.tsBtnAddNewBranch_Click);
            // 
            // pnlNewBranch
            // 
            this.pnlNewBranch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(237)))), ((int)(((byte)(200)))));
            this.pnlNewBranch.Controls.Add(this.btnCancelAddNewBranch);
            this.pnlNewBranch.Controls.Add(this.btnSaveNewBranch);
            this.pnlNewBranch.Controls.Add(this.txtBranch);
            this.pnlNewBranch.Controls.Add(this.label7);
            this.pnlNewBranch.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlNewBranch.Location = new System.Drawing.Point(0, 25);
            this.pnlNewBranch.Name = "pnlNewBranch";
            this.pnlNewBranch.Size = new System.Drawing.Size(428, 27);
            this.pnlNewBranch.TabIndex = 9;
            this.pnlNewBranch.Visible = false;
            // 
            // btnCancelAddNewBranch
            // 
            this.btnCancelAddNewBranch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(115)))), ((int)(((byte)(172)))));
            this.btnCancelAddNewBranch.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(141)))), ((int)(((byte)(248)))));
            this.btnCancelAddNewBranch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelAddNewBranch.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelAddNewBranch.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnCancelAddNewBranch.Location = new System.Drawing.Point(360, 2);
            this.btnCancelAddNewBranch.Name = "btnCancelAddNewBranch";
            this.btnCancelAddNewBranch.Size = new System.Drawing.Size(64, 23);
            this.btnCancelAddNewBranch.TabIndex = 7;
            this.btnCancelAddNewBranch.Text = "Cancel";
            this.btnCancelAddNewBranch.UseVisualStyleBackColor = false;
            this.btnCancelAddNewBranch.Click += new System.EventHandler(this.btnCancelAddNewBranch_Click);
            // 
            // btnSaveNewBranch
            // 
            this.btnSaveNewBranch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(115)))), ((int)(((byte)(172)))));
            this.btnSaveNewBranch.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(141)))), ((int)(((byte)(248)))));
            this.btnSaveNewBranch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveNewBranch.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveNewBranch.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnSaveNewBranch.Location = new System.Drawing.Point(295, 2);
            this.btnSaveNewBranch.Name = "btnSaveNewBranch";
            this.btnSaveNewBranch.Size = new System.Drawing.Size(64, 23);
            this.btnSaveNewBranch.TabIndex = 6;
            this.btnSaveNewBranch.Text = "Save";
            this.btnSaveNewBranch.UseVisualStyleBackColor = false;
            this.btnSaveNewBranch.Click += new System.EventHandler(this.btnSaveNewBranch_Click);
            // 
            // GitPush
            // 
            this.AcceptButton = this.btnPush;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(428, 210);
            this.ControlBox = false;
            this.Controls.Add(this.pnlNewBranch);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbBranches);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnPush);
            this.Name = "GitPush";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GitPush";
            this.Load += new System.EventHandler(this.GitPush_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.pnlNewBranch.ResumeLayout(false);
            this.pnlNewBranch.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnPush;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtBranch;
        private System.Windows.Forms.ComboBox cmbBranches;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsBtnAddNewBranch;
        private System.Windows.Forms.Panel pnlNewBranch;
        private System.Windows.Forms.Button btnSaveNewBranch;
        private System.Windows.Forms.Button btnCancelAddNewBranch;
    }
}