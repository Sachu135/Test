namespace DockSample.Controls
{
    partial class GitSwitch
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
            this.cmbLocalBranches = new System.Windows.Forms.ComboBox();
            this.lblLocalBranch = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSwitch = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblCurrentBranch = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbLocalBranches
            // 
            this.cmbLocalBranches.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLocalBranches.FormattingEnabled = true;
            this.cmbLocalBranches.Location = new System.Drawing.Point(74, 88);
            this.cmbLocalBranches.Name = "cmbLocalBranches";
            this.cmbLocalBranches.Size = new System.Drawing.Size(271, 21);
            this.cmbLocalBranches.TabIndex = 13;
            // 
            // lblLocalBranch
            // 
            this.lblLocalBranch.AutoSize = true;
            this.lblLocalBranch.Location = new System.Drawing.Point(71, 72);
            this.lblLocalBranch.Name = "lblLocalBranch";
            this.lblLocalBranch.Size = new System.Drawing.Size(127, 13);
            this.lblLocalBranch.TabIndex = 12;
            this.lblLocalBranch.Text = "Switch/Checkout Branch";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Controls.Add(this.btnSwitch);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 153);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(428, 57);
            this.panel1.TabIndex = 14;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(115)))), ((int)(((byte)(172)))));
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(141)))), ((int)(((byte)(248)))));
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnClose.Location = new System.Drawing.Point(310, 14);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 30);
            this.btnClose.TabIndex = 18;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSwitch
            // 
            this.btnSwitch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSwitch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(115)))), ((int)(((byte)(172)))));
            this.btnSwitch.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(141)))), ((int)(((byte)(248)))));
            this.btnSwitch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSwitch.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSwitch.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnSwitch.Location = new System.Drawing.Point(209, 14);
            this.btnSwitch.Name = "btnSwitch";
            this.btnSwitch.Size = new System.Drawing.Size(100, 30);
            this.btnSwitch.TabIndex = 17;
            this.btnSwitch.Text = "Switch/CheckOut";
            this.btnSwitch.UseVisualStyleBackColor = false;
            this.btnSwitch.Click += new System.EventHandler(this.btnSwitch_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(71, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "Current Branch";
            // 
            // lblCurrentBranch
            // 
            this.lblCurrentBranch.AutoSize = true;
            this.lblCurrentBranch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentBranch.Location = new System.Drawing.Point(71, 39);
            this.lblCurrentBranch.Name = "lblCurrentBranch";
            this.lblCurrentBranch.Size = new System.Drawing.Size(109, 16);
            this.lblCurrentBranch.TabIndex = 16;
            this.lblCurrentBranch.Text = "Current Branch";
            // 
            // GitSwitch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(428, 210);
            this.ControlBox = false;
            this.Controls.Add(this.lblCurrentBranch);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.cmbLocalBranches);
            this.Controls.Add(this.lblLocalBranch);
            this.MaximumSize = new System.Drawing.Size(444, 249);
            this.MinimumSize = new System.Drawing.Size(444, 249);
            this.Name = "GitSwitch";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GitSwitch";
            this.Load += new System.EventHandler(this.GitSwitch_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbLocalBranches;
        private System.Windows.Forms.Label lblLocalBranch;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblCurrentBranch;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSwitch;
    }
}