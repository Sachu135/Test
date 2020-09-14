namespace DockSample.Controls
{
    partial class ConfigurationForm
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
            this.lblMsg = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tbDatabase = new System.Windows.Forms.TabPage();
            this.gpBoxNewConn = new System.Windows.Forms.GroupBox();
            this.button4 = new System.Windows.Forms.Button();
            this.txtDbName = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtConnKey = new System.Windows.Forms.TextBox();
            this.connKey = new System.Windows.Forms.Label();
            this.btnSaveConnection = new System.Windows.Forms.Button();
            this.cbConnDbType = new System.Windows.Forms.ComboBox();
            this.txtConnPassword = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtConnUserName = new System.Windows.Forms.TextBox();
            this.txtConnServerName = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.gpBoxSavedConnections = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.tbProjects = new System.Windows.Forms.TabPage();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.button6 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.txtHealthCheck = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtAirflow = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtTerminalUrl = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.txtSshPass = new System.Windows.Forms.TextBox();
            this.txtSshUser = new System.Windows.Forms.TextBox();
            this.txtSshIP = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnDefault = new System.Windows.Forms.Button();
            this.txtExplorerServiceUrl = new System.Windows.Forms.TextBox();
            this.lblServiceUrl = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.txtProLocation = new System.Windows.Forms.TextBox();
            this.cmbServerType = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txtProName = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tbControl = new System.Windows.Forms.TabControl();
            this.panel1.SuspendLayout();
            this.tbDatabase.SuspendLayout();
            this.gpBoxNewConn.SuspendLayout();
            this.gpBoxSavedConnections.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tbProjects.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.tbControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblMsg
            // 
            this.lblMsg.AutoSize = true;
            this.lblMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMsg.ForeColor = System.Drawing.Color.Maroon;
            this.lblMsg.Location = new System.Drawing.Point(12, 2);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(0, 16);
            this.lblMsg.TabIndex = 10;
            this.lblMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(80)))), ((int)(((byte)(141)))));
            this.button2.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.button2.Location = new System.Drawing.Point(587, 573);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(111, 33);
            this.button2.TabIndex = 9;
            this.button2.Text = "Close";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(80)))), ((int)(((byte)(141)))));
            this.button1.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.button1.Location = new System.Drawing.Point(445, 572);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(122, 33);
            this.button1.TabIndex = 8;
            this.button1.Text = "Save";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(237)))), ((int)(((byte)(200)))));
            this.panel1.Controls.Add(this.lblMsg);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(708, 20);
            this.panel1.TabIndex = 11;
            // 
            // tbDatabase
            // 
            this.tbDatabase.Controls.Add(this.gpBoxNewConn);
            this.tbDatabase.Controls.Add(this.gpBoxSavedConnections);
            this.tbDatabase.Location = new System.Drawing.Point(4, 22);
            this.tbDatabase.Name = "tbDatabase";
            this.tbDatabase.Padding = new System.Windows.Forms.Padding(3);
            this.tbDatabase.Size = new System.Drawing.Size(700, 519);
            this.tbDatabase.TabIndex = 1;
            this.tbDatabase.Text = "Database";
            this.tbDatabase.UseVisualStyleBackColor = true;
            // 
            // gpBoxNewConn
            // 
            this.gpBoxNewConn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gpBoxNewConn.Controls.Add(this.button4);
            this.gpBoxNewConn.Controls.Add(this.txtDbName);
            this.gpBoxNewConn.Controls.Add(this.label7);
            this.gpBoxNewConn.Controls.Add(this.txtConnKey);
            this.gpBoxNewConn.Controls.Add(this.connKey);
            this.gpBoxNewConn.Controls.Add(this.btnSaveConnection);
            this.gpBoxNewConn.Controls.Add(this.cbConnDbType);
            this.gpBoxNewConn.Controls.Add(this.txtConnPassword);
            this.gpBoxNewConn.Controls.Add(this.label13);
            this.gpBoxNewConn.Controls.Add(this.txtConnUserName);
            this.gpBoxNewConn.Controls.Add(this.txtConnServerName);
            this.gpBoxNewConn.Controls.Add(this.label10);
            this.gpBoxNewConn.Controls.Add(this.label11);
            this.gpBoxNewConn.Controls.Add(this.label12);
            this.gpBoxNewConn.Font = new System.Drawing.Font("Microsoft Tai Le", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gpBoxNewConn.Location = new System.Drawing.Point(6, 3);
            this.gpBoxNewConn.Name = "gpBoxNewConn";
            this.gpBoxNewConn.Size = new System.Drawing.Size(688, 213);
            this.gpBoxNewConn.TabIndex = 6;
            this.gpBoxNewConn.TabStop = false;
            this.gpBoxNewConn.Text = "New/Edit Connection";
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(80)))), ((int)(((byte)(141)))));
            this.button4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button4.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.button4.Location = new System.Drawing.Point(409, 148);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(137, 33);
            this.button4.TabIndex = 10;
            this.button4.Text = "Test Connection";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // txtDbName
            // 
            this.txtDbName.Font = new System.Drawing.Font("Microsoft Tai Le", 9F);
            this.txtDbName.Location = new System.Drawing.Point(474, 69);
            this.txtDbName.Name = "txtDbName";
            this.txtDbName.Size = new System.Drawing.Size(200, 23);
            this.txtDbName.TabIndex = 4;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Tai Le", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(354, 72);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(62, 16);
            this.label7.TabIndex = 0;
            this.label7.Text = "Database :";
            // 
            // txtConnKey
            // 
            this.txtConnKey.Font = new System.Drawing.Font("Microsoft Tai Le", 9F);
            this.txtConnKey.Location = new System.Drawing.Point(124, 36);
            this.txtConnKey.Name = "txtConnKey";
            this.txtConnKey.Size = new System.Drawing.Size(200, 23);
            this.txtConnKey.TabIndex = 1;
            // 
            // connKey
            // 
            this.connKey.AutoSize = true;
            this.connKey.Font = new System.Drawing.Font("Microsoft Tai Le", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.connKey.Location = new System.Drawing.Point(16, 39);
            this.connKey.Name = "connKey";
            this.connKey.Size = new System.Drawing.Size(78, 16);
            this.connKey.TabIndex = 0;
            this.connKey.Text = "Conn Name :";
            // 
            // btnSaveConnection
            // 
            this.btnSaveConnection.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(80)))), ((int)(((byte)(141)))));
            this.btnSaveConnection.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveConnection.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnSaveConnection.Location = new System.Drawing.Point(552, 148);
            this.btnSaveConnection.Name = "btnSaveConnection";
            this.btnSaveConnection.Size = new System.Drawing.Size(122, 33);
            this.btnSaveConnection.TabIndex = 9;
            this.btnSaveConnection.Text = "Save";
            this.btnSaveConnection.UseVisualStyleBackColor = false;
            this.btnSaveConnection.Click += new System.EventHandler(this.btnSaveConnection_Click);
            // 
            // cbConnDbType
            // 
            this.cbConnDbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbConnDbType.Font = new System.Drawing.Font("Microsoft Tai Le", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbConnDbType.FormattingEnabled = true;
            this.cbConnDbType.Items.AddRange(new object[] {
            "Sql Server",
            "Postgres",
            "MySql",
            "Mongo Db",
            "Json",
            "Xml",
            "Streaming Data",
            "Cloud Storage"});
            this.cbConnDbType.Location = new System.Drawing.Point(474, 36);
            this.cbConnDbType.Name = "cbConnDbType";
            this.cbConnDbType.Size = new System.Drawing.Size(200, 24);
            this.cbConnDbType.TabIndex = 2;
            this.cbConnDbType.SelectedIndexChanged += new System.EventHandler(this.cbConnDbType_SelectedIndexChanged);
            // 
            // txtConnPassword
            // 
            this.txtConnPassword.Font = new System.Drawing.Font("Microsoft Tai Le", 9F);
            this.txtConnPassword.Location = new System.Drawing.Point(474, 103);
            this.txtConnPassword.Name = "txtConnPassword";
            this.txtConnPassword.PasswordChar = '*';
            this.txtConnPassword.Size = new System.Drawing.Size(200, 23);
            this.txtConnPassword.TabIndex = 6;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Tai Le", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(354, 106);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(64, 16);
            this.label13.TabIndex = 0;
            this.label13.Text = "Password :";
            // 
            // txtConnUserName
            // 
            this.txtConnUserName.Font = new System.Drawing.Font("Microsoft Tai Le", 9F);
            this.txtConnUserName.Location = new System.Drawing.Point(123, 103);
            this.txtConnUserName.Name = "txtConnUserName";
            this.txtConnUserName.Size = new System.Drawing.Size(200, 23);
            this.txtConnUserName.TabIndex = 5;
            // 
            // txtConnServerName
            // 
            this.txtConnServerName.Font = new System.Drawing.Font("Microsoft Tai Le", 9F);
            this.txtConnServerName.Location = new System.Drawing.Point(123, 69);
            this.txtConnServerName.Name = "txtConnServerName";
            this.txtConnServerName.Size = new System.Drawing.Size(200, 23);
            this.txtConnServerName.TabIndex = 3;
            this.txtConnServerName.TextChanged += new System.EventHandler(this.txtConnServerName_TextChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Tai Le", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(16, 106);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(72, 16);
            this.label10.TabIndex = 0;
            this.label10.Text = "User Name :";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Tai Le", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(16, 72);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(101, 16);
            this.label11.TabIndex = 0;
            this.label11.Text = "Server Name/Url :";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Tai Le", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(354, 39);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(91, 16);
            this.label12.TabIndex = 0;
            this.label12.Text = "Database Type :";
            // 
            // gpBoxSavedConnections
            // 
            this.gpBoxSavedConnections.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gpBoxSavedConnections.Controls.Add(this.dataGridView1);
            this.gpBoxSavedConnections.Font = new System.Drawing.Font("Microsoft Tai Le", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gpBoxSavedConnections.Location = new System.Drawing.Point(6, 222);
            this.gpBoxSavedConnections.Name = "gpBoxSavedConnections";
            this.gpBoxSavedConnections.Size = new System.Drawing.Size(688, 291);
            this.gpBoxSavedConnections.TabIndex = 1;
            this.gpBoxSavedConnections.TabStop = false;
            this.gpBoxSavedConnections.Text = "Saved Connections";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(3, 20);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(682, 265);
            this.dataGridView1.TabIndex = 0;
            // 
            // tbProjects
            // 
            this.tbProjects.Controls.Add(this.groupBox5);
            this.tbProjects.Controls.Add(this.groupBox4);
            this.tbProjects.Location = new System.Drawing.Point(4, 22);
            this.tbProjects.Name = "tbProjects";
            this.tbProjects.Size = new System.Drawing.Size(700, 519);
            this.tbProjects.TabIndex = 2;
            this.tbProjects.Text = "Workspaces";
            this.tbProjects.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.Controls.Add(this.dataGridView2);
            this.groupBox5.Font = new System.Drawing.Font("Microsoft Tai Le", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox5.Location = new System.Drawing.Point(7, 359);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(687, 157);
            this.groupBox5.TabIndex = 8;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Existing Workspaces";
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.AllowUserToDeleteRows = false;
            this.dataGridView2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(3, 20);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.ReadOnly = true;
            this.dataGridView2.Size = new System.Drawing.Size(681, 134);
            this.dataGridView2.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.button6);
            this.groupBox4.Controls.Add(this.button5);
            this.groupBox4.Controls.Add(this.groupBox9);
            this.groupBox4.Controls.Add(this.button3);
            this.groupBox4.Controls.Add(this.groupBox8);
            this.groupBox4.Controls.Add(this.groupBox7);
            this.groupBox4.Controls.Add(this.cmbServerType);
            this.groupBox4.Controls.Add(this.label15);
            this.groupBox4.Controls.Add(this.txtProName);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Font = new System.Drawing.Font("Microsoft Tai Le", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.Location = new System.Drawing.Point(7, 3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(691, 355);
            this.groupBox4.TabIndex = 7;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "New Workspace";
            // 
            // button6
            // 
            this.button6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(80)))), ((int)(((byte)(141)))));
            this.button6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button6.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.button6.Location = new System.Drawing.Point(106, 317);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(81, 33);
            this.button6.TabIndex = 15;
            this.button6.Text = "Cancel";
            this.button6.UseVisualStyleBackColor = false;
            this.button6.Visible = false;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(80)))), ((int)(((byte)(141)))));
            this.button5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button5.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.button5.Location = new System.Drawing.Point(8, 317);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(92, 33);
            this.button5.TabIndex = 14;
            this.button5.Text = "Update";
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Visible = false;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.txtHealthCheck);
            this.groupBox9.Controls.Add(this.label14);
            this.groupBox9.Controls.Add(this.txtAirflow);
            this.groupBox9.Controls.Add(this.label8);
            this.groupBox9.Controls.Add(this.txtTerminalUrl);
            this.groupBox9.Controls.Add(this.label20);
            this.groupBox9.Font = new System.Drawing.Font("Microsoft Tai Le", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox9.Location = new System.Drawing.Point(351, 172);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(334, 143);
            this.groupBox9.TabIndex = 13;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Services Info";
            // 
            // txtHealthCheck
            // 
            this.txtHealthCheck.Font = new System.Drawing.Font("Microsoft Tai Le", 9F);
            this.txtHealthCheck.Location = new System.Drawing.Point(117, 94);
            this.txtHealthCheck.Name = "txtHealthCheck";
            this.txtHealthCheck.Size = new System.Drawing.Size(200, 23);
            this.txtHealthCheck.TabIndex = 9;
            this.txtHealthCheck.Tag = "http://23.101.24.36:9045";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Tai Le", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(32, 97);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(85, 16);
            this.label14.TabIndex = 8;
            this.label14.Text = "Health Check :";
            // 
            // txtAirflow
            // 
            this.txtAirflow.Font = new System.Drawing.Font("Microsoft Tai Le", 9F);
            this.txtAirflow.Location = new System.Drawing.Point(117, 64);
            this.txtAirflow.Name = "txtAirflow";
            this.txtAirflow.Size = new System.Drawing.Size(200, 23);
            this.txtAirflow.TabIndex = 7;
            this.txtAirflow.Tag = "http://23.101.24.36:9046";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Tai Le", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(32, 67);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(52, 16);
            this.label8.TabIndex = 6;
            this.label8.Text = "Airflow :";
            // 
            // txtTerminalUrl
            // 
            this.txtTerminalUrl.Font = new System.Drawing.Font("Microsoft Tai Le", 9F);
            this.txtTerminalUrl.Location = new System.Drawing.Point(117, 31);
            this.txtTerminalUrl.Name = "txtTerminalUrl";
            this.txtTerminalUrl.Size = new System.Drawing.Size(200, 23);
            this.txtTerminalUrl.TabIndex = 3;
            this.txtTerminalUrl.Tag = "http://23.101.24.36:8080";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Microsoft Tai Le", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.Location = new System.Drawing.Point(32, 34);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(61, 16);
            this.label20.TabIndex = 0;
            this.label20.Text = "Terminal :";
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(80)))), ((int)(((byte)(141)))));
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.button3.Location = new System.Drawing.Point(562, 317);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(122, 33);
            this.button3.TabIndex = 9;
            this.button3.Text = "Add New";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.txtSshPass);
            this.groupBox8.Controls.Add(this.txtSshUser);
            this.groupBox8.Controls.Add(this.txtSshIP);
            this.groupBox8.Controls.Add(this.label17);
            this.groupBox8.Controls.Add(this.label18);
            this.groupBox8.Controls.Add(this.label19);
            this.groupBox8.Font = new System.Drawing.Font("Microsoft Tai Le", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox8.Location = new System.Drawing.Point(8, 172);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(337, 143);
            this.groupBox8.TabIndex = 12;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "SSH Client Info";
            // 
            // txtSshPass
            // 
            this.txtSshPass.Font = new System.Drawing.Font("Microsoft Tai Le", 9F);
            this.txtSshPass.Location = new System.Drawing.Point(121, 94);
            this.txtSshPass.Name = "txtSshPass";
            this.txtSshPass.PasswordChar = '*';
            this.txtSshPass.Size = new System.Drawing.Size(200, 23);
            this.txtSshPass.TabIndex = 5;
            this.txtSshPass.Tag = "JunkyardUbuntu!";
            this.txtSshPass.Validated += new System.EventHandler(this.txtSshPass_Validated);
            // 
            // txtSshUser
            // 
            this.txtSshUser.Font = new System.Drawing.Font("Microsoft Tai Le", 9F);
            this.txtSshUser.Location = new System.Drawing.Point(121, 64);
            this.txtSshUser.Name = "txtSshUser";
            this.txtSshUser.Size = new System.Drawing.Size(200, 23);
            this.txtSshUser.TabIndex = 4;
            this.txtSshUser.Tag = "abhishek";
            this.txtSshUser.Validated += new System.EventHandler(this.txtSshUser_Validated);
            // 
            // txtSshIP
            // 
            this.txtSshIP.Font = new System.Drawing.Font("Microsoft Tai Le", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSshIP.Location = new System.Drawing.Point(121, 31);
            this.txtSshIP.Name = "txtSshIP";
            this.txtSshIP.Size = new System.Drawing.Size(200, 23);
            this.txtSshIP.TabIndex = 3;
            this.txtSshIP.Tag = "23.101.24.36";
            this.txtSshIP.Validated += new System.EventHandler(this.txtSshIP_Validated);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Tai Le", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(23, 97);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(64, 16);
            this.label17.TabIndex = 2;
            this.label17.Text = "Password :";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Microsoft Tai Le", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(23, 67);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(72, 16);
            this.label18.TabIndex = 1;
            this.label18.Text = "User Name :";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Microsoft Tai Le", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.Location = new System.Drawing.Point(23, 34);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(69, 16);
            this.label19.TabIndex = 0;
            this.label19.Text = "IP Address :";
            // 
            // groupBox7
            // 
            this.groupBox7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox7.Controls.Add(this.btnBrowse);
            this.groupBox7.Controls.Add(this.btnDefault);
            this.groupBox7.Controls.Add(this.txtExplorerServiceUrl);
            this.groupBox7.Controls.Add(this.lblServiceUrl);
            this.groupBox7.Controls.Add(this.label16);
            this.groupBox7.Controls.Add(this.txtProLocation);
            this.groupBox7.Font = new System.Drawing.Font("Microsoft Tai Le", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox7.Location = new System.Drawing.Point(8, 75);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(677, 91);
            this.groupBox7.TabIndex = 11;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Explorer Service Info";
            // 
            // btnBrowse
            // 
            this.btnBrowse.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(80)))), ((int)(((byte)(141)))));
            this.btnBrowse.Enabled = false;
            this.btnBrowse.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrowse.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnBrowse.Location = new System.Drawing.Point(327, 23);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(102, 28);
            this.btnBrowse.TabIndex = 16;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = false;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnDefault
            // 
            this.btnDefault.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(80)))), ((int)(((byte)(141)))));
            this.btnDefault.Enabled = false;
            this.btnDefault.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDefault.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnDefault.Location = new System.Drawing.Point(435, 23);
            this.btnDefault.Name = "btnDefault";
            this.btnDefault.Size = new System.Drawing.Size(102, 28);
            this.btnDefault.TabIndex = 16;
            this.btnDefault.Text = "Set Default";
            this.btnDefault.UseVisualStyleBackColor = false;
            this.btnDefault.Click += new System.EventHandler(this.btnDefault_Click);
            // 
            // txtExplorerServiceUrl
            // 
            this.txtExplorerServiceUrl.Font = new System.Drawing.Font("Microsoft Tai Le", 9F);
            this.txtExplorerServiceUrl.Location = new System.Drawing.Point(121, 57);
            this.txtExplorerServiceUrl.Name = "txtExplorerServiceUrl";
            this.txtExplorerServiceUrl.Size = new System.Drawing.Size(200, 23);
            this.txtExplorerServiceUrl.TabIndex = 5;
            this.txtExplorerServiceUrl.Tag = "http://23.101.24.36:5000";
            // 
            // lblServiceUrl
            // 
            this.lblServiceUrl.AutoSize = true;
            this.lblServiceUrl.Font = new System.Drawing.Font("Microsoft Tai Le", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblServiceUrl.Location = new System.Drawing.Point(23, 60);
            this.lblServiceUrl.Name = "lblServiceUrl";
            this.lblServiceUrl.Size = new System.Drawing.Size(69, 16);
            this.lblServiceUrl.TabIndex = 4;
            this.lblServiceUrl.Text = "Service Url :";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Tai Le", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(23, 31);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(60, 16);
            this.label16.TabIndex = 0;
            this.label16.Text = "Location :";
            // 
            // txtProLocation
            // 
            this.txtProLocation.Font = new System.Drawing.Font("Microsoft Tai Le", 9F);
            this.txtProLocation.Location = new System.Drawing.Point(121, 26);
            this.txtProLocation.Name = "txtProLocation";
            this.txtProLocation.Size = new System.Drawing.Size(200, 23);
            this.txtProLocation.TabIndex = 3;
            // 
            // cmbServerType
            // 
            this.cmbServerType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbServerType.Font = new System.Drawing.Font("Microsoft Tai Le", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbServerType.FormattingEnabled = true;
            this.cmbServerType.Items.AddRange(new object[] {
            "Windows",
            "Linux"});
            this.cmbServerType.Location = new System.Drawing.Point(468, 36);
            this.cmbServerType.Name = "cmbServerType";
            this.cmbServerType.Size = new System.Drawing.Size(200, 24);
            this.cmbServerType.TabIndex = 2;
            this.cmbServerType.SelectedIndexChanged += new System.EventHandler(this.cmbServerType_SelectedIndexChanged);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Tai Le", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(378, 39);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(75, 16);
            this.label15.TabIndex = 10;
            this.label15.Text = "Server Type :";
            // 
            // txtProName
            // 
            this.txtProName.Font = new System.Drawing.Font("Microsoft Tai Le", 9F);
            this.txtProName.Location = new System.Drawing.Point(129, 36);
            this.txtProName.Name = "txtProName";
            this.txtProName.ShortcutsEnabled = false;
            this.txtProName.Size = new System.Drawing.Size(200, 23);
            this.txtProName.TabIndex = 1;
            this.txtProName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtProName_KeyPress);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Tai Le", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(13, 39);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(107, 16);
            this.label9.TabIndex = 0;
            this.label9.Text = "Workspace Name :";
            // 
            // tbControl
            // 
            this.tbControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbControl.Controls.Add(this.tbProjects);
            this.tbControl.Controls.Add(this.tbDatabase);
            this.tbControl.Location = new System.Drawing.Point(0, 21);
            this.tbControl.Name = "tbControl";
            this.tbControl.SelectedIndex = 0;
            this.tbControl.Size = new System.Drawing.Size(708, 545);
            this.tbControl.TabIndex = 11;
            // 
            // ConfigurationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(708, 618);
            this.Controls.Add(this.tbControl);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "ConfigurationForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cluster Config";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ConfigurationForm_FormClosing);
            this.Load += new System.EventHandler(this.ConfigurationForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tbDatabase.ResumeLayout(false);
            this.gpBoxNewConn.ResumeLayout(false);
            this.gpBoxNewConn.PerformLayout();
            this.gpBoxSavedConnections.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tbProjects.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.tbControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lblMsg;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TabPage tbDatabase;
        private System.Windows.Forms.GroupBox gpBoxNewConn;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TextBox txtDbName;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtConnKey;
        private System.Windows.Forms.Label connKey;
        private System.Windows.Forms.Button btnSaveConnection;
        private System.Windows.Forms.ComboBox cbConnDbType;
        private System.Windows.Forms.TextBox txtConnPassword;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtConnUserName;
        private System.Windows.Forms.TextBox txtConnServerName;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.GroupBox gpBoxSavedConnections;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TabPage tbProjects;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.TextBox txtTerminalUrl;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.TextBox txtSshPass;
        private System.Windows.Forms.TextBox txtSshUser;
        private System.Windows.Forms.TextBox txtSshIP;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.TextBox txtExplorerServiceUrl;
        private System.Windows.Forms.Label lblServiceUrl;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txtProLocation;
        private System.Windows.Forms.ComboBox cmbServerType;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtProName;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TabControl tbControl;
        private System.Windows.Forms.TextBox txtHealthCheck;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtAirflow;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button btnDefault;
        private System.Windows.Forms.Button btnBrowse;
    }
}