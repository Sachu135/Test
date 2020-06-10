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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblMsg = new System.Windows.Forms.Label();
            this.tbControl = new System.Windows.Forms.TabControl();
            this.tablinuxCtrl = new System.Windows.Forms.TabPage();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.tbDatabase = new System.Windows.Forms.TabPage();
            this.gpBoxNewConn = new System.Windows.Forms.GroupBox();
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tbControl.SuspendLayout();
            this.tablinuxCtrl.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tbDatabase.SuspendLayout();
            this.gpBoxNewConn.SuspendLayout();
            this.gpBoxSavedConnections.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Tai Le", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(94, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP Address :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Tai Le", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(91, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "User Name :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Tai Le", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(99, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "Password :";
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Microsoft Tai Le", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(228, 37);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(181, 23);
            this.textBox1.TabIndex = 3;
            this.textBox1.Text = "23.101.24.36";
            // 
            // textBox2
            // 
            this.textBox2.Font = new System.Drawing.Font("Microsoft Tai Le", 9F);
            this.textBox2.Location = new System.Drawing.Point(228, 67);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(181, 23);
            this.textBox2.TabIndex = 4;
            this.textBox2.Text = "abhishek";
            // 
            // textBox3
            // 
            this.textBox3.Font = new System.Drawing.Font("Microsoft Tai Le", 9F);
            this.textBox3.Location = new System.Drawing.Point(228, 97);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(181, 23);
            this.textBox3.TabIndex = 5;
            this.textBox3.Text = "JunkyardUbuntu!";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.textBox3);
            this.groupBox1.Controls.Add(this.textBox2);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Tai Le", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(612, 144);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "SSH Client Info";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Tai Le", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(82, 40);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(78, 16);
            this.label6.TabIndex = 0;
            this.label6.Text = "Project Path :";
            // 
            // textBox6
            // 
            this.textBox6.Font = new System.Drawing.Font("Microsoft Tai Le", 9F);
            this.textBox6.Location = new System.Drawing.Point(228, 37);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(181, 23);
            this.textBox6.TabIndex = 3;
            this.textBox6.Text = "/home/abhishek/Kockpit";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Tai Le", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(91, 70);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 16);
            this.label5.TabIndex = 4;
            this.label5.Text = "Service Url :";
            // 
            // textBox5
            // 
            this.textBox5.Font = new System.Drawing.Font("Microsoft Tai Le", 9F);
            this.textBox5.Location = new System.Drawing.Point(228, 67);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(181, 23);
            this.textBox5.TabIndex = 5;
            this.textBox5.Text = "http://23.101.24.36:5000";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.textBox5);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.textBox6);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Tai Le", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(9, 156);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(606, 109);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Project Info";
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
            // tbControl
            // 
            this.tbControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbControl.Controls.Add(this.tablinuxCtrl);
            this.tbControl.Controls.Add(this.tbDatabase);
            this.tbControl.Location = new System.Drawing.Point(0, 21);
            this.tbControl.Name = "tbControl";
            this.tbControl.SelectedIndex = 0;
            this.tbControl.Size = new System.Drawing.Size(626, 426);
            this.tbControl.TabIndex = 11;
            // 
            // tablinuxCtrl
            // 
            this.tablinuxCtrl.Controls.Add(this.groupBox1);
            this.tablinuxCtrl.Controls.Add(this.groupBox2);
            this.tablinuxCtrl.Controls.Add(this.button2);
            this.tablinuxCtrl.Controls.Add(this.groupBox3);
            this.tablinuxCtrl.Controls.Add(this.button1);
            this.tablinuxCtrl.Location = new System.Drawing.Point(4, 22);
            this.tablinuxCtrl.Name = "tablinuxCtrl";
            this.tablinuxCtrl.Padding = new System.Windows.Forms.Padding(3);
            this.tablinuxCtrl.Size = new System.Drawing.Size(618, 400);
            this.tablinuxCtrl.TabIndex = 0;
            this.tablinuxCtrl.Text = "Linux Config";
            this.tablinuxCtrl.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(80)))), ((int)(((byte)(141)))));
            this.button2.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.button2.Location = new System.Drawing.Point(495, 364);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(111, 33);
            this.button2.TabIndex = 9;
            this.button2.Text = "Close";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.textBox4);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Tai Le", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(9, 271);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(606, 90);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Terminal Info";
            // 
            // textBox4
            // 
            this.textBox4.Font = new System.Drawing.Font("Microsoft Tai Le", 9F);
            this.textBox4.Location = new System.Drawing.Point(228, 37);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(181, 23);
            this.textBox4.TabIndex = 3;
            this.textBox4.Text = "http://23.101.24.36:8080";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Tai Le", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(131, 40);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 16);
            this.label4.TabIndex = 0;
            this.label4.Text = "Url :";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(80)))), ((int)(((byte)(141)))));
            this.button1.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.button1.Location = new System.Drawing.Point(367, 364);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(122, 33);
            this.button1.TabIndex = 8;
            this.button1.Text = "Save";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tbDatabase
            // 
            this.tbDatabase.Controls.Add(this.gpBoxNewConn);
            this.tbDatabase.Controls.Add(this.gpBoxSavedConnections);
            this.tbDatabase.Location = new System.Drawing.Point(4, 22);
            this.tbDatabase.Name = "tbDatabase";
            this.tbDatabase.Padding = new System.Windows.Forms.Padding(3);
            this.tbDatabase.Size = new System.Drawing.Size(618, 400);
            this.tbDatabase.TabIndex = 1;
            this.tbDatabase.Text = "Database";
            this.tbDatabase.UseVisualStyleBackColor = true;
            // 
            // gpBoxNewConn
            // 
            this.gpBoxNewConn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
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
            this.gpBoxNewConn.Size = new System.Drawing.Size(609, 196);
            this.gpBoxNewConn.TabIndex = 6;
            this.gpBoxNewConn.TabStop = false;
            this.gpBoxNewConn.Text = "New/Edit Connection";
            // 
            // txtDbName
            // 
            this.txtDbName.Font = new System.Drawing.Font("Microsoft Tai Le", 9F);
            this.txtDbName.Location = new System.Drawing.Point(455, 69);
            this.txtDbName.Name = "txtDbName";
            this.txtDbName.Size = new System.Drawing.Size(138, 23);
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
            this.txtConnKey.Location = new System.Drawing.Point(123, 36);
            this.txtConnKey.Name = "txtConnKey";
            this.txtConnKey.Size = new System.Drawing.Size(138, 23);
            this.txtConnKey.TabIndex = 1;
            // 
            // connKey
            // 
            this.connKey.AutoSize = true;
            this.connKey.Font = new System.Drawing.Font("Microsoft Tai Le", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.connKey.Location = new System.Drawing.Point(9, 39);
            this.connKey.Name = "connKey";
            this.connKey.Size = new System.Drawing.Size(78, 16);
            this.connKey.TabIndex = 0;
            this.connKey.Text = "Conn Name :";
            // 
            // btnSaveConnection
            // 
            this.btnSaveConnection.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(80)))), ((int)(((byte)(141)))));
            this.btnSaveConnection.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnSaveConnection.Location = new System.Drawing.Point(471, 145);
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
            "Postgres"});
            this.cbConnDbType.Location = new System.Drawing.Point(455, 36);
            this.cbConnDbType.Name = "cbConnDbType";
            this.cbConnDbType.Size = new System.Drawing.Size(138, 24);
            this.cbConnDbType.TabIndex = 2;
            // 
            // txtConnPassword
            // 
            this.txtConnPassword.Font = new System.Drawing.Font("Microsoft Tai Le", 9F);
            this.txtConnPassword.Location = new System.Drawing.Point(455, 103);
            this.txtConnPassword.Name = "txtConnPassword";
            this.txtConnPassword.PasswordChar = '*';
            this.txtConnPassword.Size = new System.Drawing.Size(138, 23);
            this.txtConnPassword.TabIndex = 6;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Tai Le", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(352, 106);
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
            this.txtConnUserName.Size = new System.Drawing.Size(138, 23);
            this.txtConnUserName.TabIndex = 5;
            // 
            // txtConnServerName
            // 
            this.txtConnServerName.Font = new System.Drawing.Font("Microsoft Tai Le", 9F);
            this.txtConnServerName.Location = new System.Drawing.Point(123, 69);
            this.txtConnServerName.Name = "txtConnServerName";
            this.txtConnServerName.Size = new System.Drawing.Size(138, 23);
            this.txtConnServerName.TabIndex = 3;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Tai Le", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(15, 106);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(72, 16);
            this.label10.TabIndex = 0;
            this.label10.Text = "User Name :";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Tai Le", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(6, 72);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(81, 16);
            this.label11.TabIndex = 0;
            this.label11.Text = "Server Name :";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Tai Le", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(325, 39);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(91, 16);
            this.label12.TabIndex = 0;
            this.label12.Text = "Database Type :";
            // 
            // gpBoxSavedConnections
            // 
            this.gpBoxSavedConnections.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gpBoxSavedConnections.Controls.Add(this.dataGridView1);
            this.gpBoxSavedConnections.Font = new System.Drawing.Font("Microsoft Tai Le", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gpBoxSavedConnections.Location = new System.Drawing.Point(6, 198);
            this.gpBoxSavedConnections.Name = "gpBoxSavedConnections";
            this.gpBoxSavedConnections.Size = new System.Drawing.Size(609, 196);
            this.gpBoxSavedConnections.TabIndex = 1;
            this.gpBoxSavedConnections.TabStop = false;
            this.gpBoxSavedConnections.Text = "Saved Connections";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 20);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(603, 173);
            this.dataGridView1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(237)))), ((int)(((byte)(200)))));
            this.panel1.Controls.Add(this.lblMsg);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(626, 20);
            this.panel1.TabIndex = 11;
            // 
            // ConfigurationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(626, 450);
            this.Controls.Add(this.tbControl);
            this.Controls.Add(this.panel1);
            this.Name = "ConfigurationForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ConfigurationForm";
            this.Load += new System.EventHandler(this.ConfigurationForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tbControl.ResumeLayout(false);
            this.tablinuxCtrl.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.tbDatabase.ResumeLayout(false);
            this.gpBoxNewConn.ResumeLayout(false);
            this.gpBoxNewConn.PerformLayout();
            this.gpBoxSavedConnections.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblMsg;
        private System.Windows.Forms.TabControl tbControl;
        private System.Windows.Forms.TabPage tablinuxCtrl;
        private System.Windows.Forms.TabPage tbDatabase;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox gpBoxNewConn;
        private System.Windows.Forms.ComboBox cbConnDbType;
        private System.Windows.Forms.TextBox txtConnPassword;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtConnUserName;
        private System.Windows.Forms.TextBox txtConnServerName;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.GroupBox gpBoxSavedConnections;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtConnKey;
        private System.Windows.Forms.Label connKey;
        private System.Windows.Forms.Button btnSaveConnection;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox txtDbName;
        private System.Windows.Forms.Label label7;
    }
}