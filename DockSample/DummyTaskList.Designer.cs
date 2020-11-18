namespace DockSample
{
    partial class DummyTaskList
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DummyTaskList));
            this.panel1 = new System.Windows.Forms.Panel();
            this.tsParent = new System.Windows.Forms.ToolStrip();
            this.btnAddNewTaskList = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.cmbTaskList = new System.Windows.Forms.ToolStripComboBox();
            this.btnDeleteTaskList = new System.Windows.Forms.ToolStripButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tsSub = new System.Windows.Forms.ToolStrip();
            this.btnAddNewTask = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.panel3 = new System.Windows.Forms.Panel();
            this.dgvList = new System.Windows.Forms.DataGridView();
            this.panel1.SuspendLayout();
            this.tsParent.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tsSub.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.tsParent);
            this.panel1.Location = new System.Drawing.Point(0, 1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(698, 25);
            this.panel1.TabIndex = 1;
            // 
            // tsParent
            // 
            this.tsParent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tsParent.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAddNewTaskList,
            this.toolStripSeparator1,
            this.toolStripLabel1,
            this.cmbTaskList,
            this.btnDeleteTaskList});
            this.tsParent.Location = new System.Drawing.Point(0, 0);
            this.tsParent.Name = "tsParent";
            this.tsParent.Size = new System.Drawing.Size(698, 25);
            this.tsParent.TabIndex = 0;
            this.tsParent.Text = "toolStrip2";
            // 
            // btnAddNewTaskList
            // 
            this.btnAddNewTaskList.Image = global::DockSample.Properties.Resources.plus;
            this.btnAddNewTaskList.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddNewTaskList.Name = "btnAddNewTaskList";
            this.btnAddNewTaskList.Size = new System.Drawing.Size(92, 22);
            this.btnAddNewTaskList.Text = "Add Task list";
            this.btnAddNewTaskList.Click += new System.EventHandler(this.btnAddNewTaskList_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(86, 22);
            this.toolStripLabel1.Text = "Select task list :";
            // 
            // cmbTaskList
            // 
            this.cmbTaskList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTaskList.Name = "cmbTaskList";
            this.cmbTaskList.Size = new System.Drawing.Size(200, 25);
            this.cmbTaskList.SelectedIndexChanged += new System.EventHandler(this.cmbTaskList_SelectedIndexChanged);
            // 
            // btnDeleteTaskList
            // 
            this.btnDeleteTaskList.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDeleteTaskList.Image = global::DockSample.Properties.Resources.clear;
            this.btnDeleteTaskList.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDeleteTaskList.Name = "btnDeleteTaskList";
            this.btnDeleteTaskList.Size = new System.Drawing.Size(23, 22);
            this.btnDeleteTaskList.Text = "Delete tasklist";
            this.btnDeleteTaskList.ToolTipText = "Delete task list";
            this.btnDeleteTaskList.Visible = false;
            this.btnDeleteTaskList.Click += new System.EventHandler(this.btnDeleteTaskList_Click);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.tsSub);
            this.panel2.Location = new System.Drawing.Point(0, 28);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(698, 25);
            this.panel2.TabIndex = 3;
            // 
            // tsSub
            // 
            this.tsSub.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tsSub.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAddNewTask,
            this.toolStripSeparator2});
            this.tsSub.Location = new System.Drawing.Point(0, 0);
            this.tsSub.Name = "tsSub";
            this.tsSub.Size = new System.Drawing.Size(698, 25);
            this.tsSub.TabIndex = 0;
            this.tsSub.Text = "toolStrip1";
            // 
            // btnAddNewTask
            // 
            this.btnAddNewTask.Image = global::DockSample.Properties.Resources.plus;
            this.btnAddNewTask.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddNewTask.Name = "btnAddNewTask";
            this.btnAddNewTask.Size = new System.Drawing.Size(74, 22);
            this.btnAddNewTask.Text = "Add Task";
            this.btnAddNewTask.Click += new System.EventHandler(this.btnAddNewTask_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.Controls.Add(this.dgvList);
            this.panel3.Location = new System.Drawing.Point(0, 53);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(698, 325);
            this.panel3.TabIndex = 4;
            // 
            // dgvList
            // 
            this.dgvList.AllowUserToAddRows = false;
            this.dgvList.AllowUserToDeleteRows = false;
            this.dgvList.AllowUserToOrderColumns = true;
            this.dgvList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvList.BackgroundColor = System.Drawing.Color.White;
            this.dgvList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.LightCyan;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvList.ColumnHeadersHeight = 25;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.Transparent;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvList.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvList.EnableHeadersVisualStyles = false;
            this.dgvList.GridColor = System.Drawing.Color.White;
            this.dgvList.Location = new System.Drawing.Point(0, 0);
            this.dgvList.MultiSelect = false;
            this.dgvList.Name = "dgvList";
            this.dgvList.ReadOnly = true;
            this.dgvList.RowHeadersVisible = false;
            this.dgvList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvList.Size = new System.Drawing.Size(698, 325);
            this.dgvList.TabIndex = 1;
            this.dgvList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvList_CellClick);
            this.dgvList.SelectionChanged += new System.EventHandler(this.dgvList_SelectionChanged);
            // 
            // DummyTaskList
            // 
            this.ClientSize = new System.Drawing.Size(699, 376);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DummyTaskList";
            this.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.Text = "Task List";
            this.Shown += new System.EventHandler(this.DummyTaskList_Shown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tsParent.ResumeLayout(false);
            this.tsParent.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tsSub.ResumeLayout(false);
            this.tsSub.PerformLayout();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ToolStrip tsSub;
        private System.Windows.Forms.ToolStrip tsParent;
        private System.Windows.Forms.ToolStripButton btnAddNewTaskList;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox cmbTaskList;
        private System.Windows.Forms.ToolStripButton btnAddNewTask;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.DataGridView dgvList;
        private System.Windows.Forms.ToolStripButton btnDeleteTaskList;
    }
}