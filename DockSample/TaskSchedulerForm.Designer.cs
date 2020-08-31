namespace DockSample
{
    partial class TaskSchedulerForm
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
            this.textBoxlabelOneTimeOnlyTag = new System.Windows.Forms.TextBox();
            this.labelOneTimeOnlyTag = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dateTimePickerTriggerTime = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerEndDate = new System.Windows.Forms.DateTimePicker();
            this.labelEndDate = new System.Windows.Forms.Label();
            this.dateTimePickerStartDate = new System.Windows.Forms.DateTimePicker();
            this.labelStartDate = new System.Windows.Forms.Label();
            this.buttonCreateTrigger = new System.Windows.Forms.Button();
            this.buttonReset = new System.Windows.Forms.Button();
            this.buttonStopService = new System.Windows.Forms.Button();
            this.buttonStartService = new System.Windows.Forms.Button();
            this.checkBoxEnabled = new System.Windows.Forms.CheckBox();
            this.buttonInstallService = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblServiceStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTagName = new System.Windows.Forms.TextBox();
            this.dgvItems = new System.Windows.Forms.DataGridView();
            this.grpOneTime = new System.Windows.Forms.GroupBox();
            this.checkBoxOneTimeOnlyActive = new System.Windows.Forms.CheckBox();
            this.labelOneTimeOnlyDay = new System.Windows.Forms.Label();
            this.dateTimePickerOneTimeOnlyDay = new System.Windows.Forms.DateTimePicker();
            this.grpDaily = new System.Windows.Forms.GroupBox();
            this.numericUpDownDaily = new System.Windows.Forms.NumericUpDown();
            this.labelDailyEvery = new System.Windows.Forms.Label();
            this.labelDailyDay = new System.Windows.Forms.Label();
            this.grpWeekly = new System.Windows.Forms.GroupBox();
            this.labelWeeklyDays = new System.Windows.Forms.Label();
            this.checkedListBoxWeeklyDays = new System.Windows.Forms.CheckedListBox();
            this.grpMonthly = new System.Windows.Forms.GroupBox();
            this.tabControlMonthlyMode = new System.Windows.Forms.TabControl();
            this.tabPageMonthlyDayOfMonth = new System.Windows.Forms.TabPage();
            this.checkedListBoxMonthlyDays = new System.Windows.Forms.CheckedListBox();
            this.tabPageMonthlyWeekDay = new System.Windows.Forms.TabPage();
            this.checkedListBoxMonthlyWeekNumber = new System.Windows.Forms.CheckedListBox();
            this.checkedListBoxMonthlyWeekDay = new System.Windows.Forms.CheckedListBox();
            this.labelMonthlyMonth = new System.Windows.Forms.Label();
            this.checkedListBoxMonthlyMonths = new System.Windows.Forms.CheckedListBox();
            this.btnPreview = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).BeginInit();
            this.grpOneTime.SuspendLayout();
            this.grpDaily.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDaily)).BeginInit();
            this.grpWeekly.SuspendLayout();
            this.grpMonthly.SuspendLayout();
            this.tabControlMonthlyMode.SuspendLayout();
            this.tabPageMonthlyDayOfMonth.SuspendLayout();
            this.tabPageMonthlyWeekDay.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxlabelOneTimeOnlyTag
            // 
            this.textBoxlabelOneTimeOnlyTag.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxlabelOneTimeOnlyTag.Location = new System.Drawing.Point(10, 323);
            this.textBoxlabelOneTimeOnlyTag.Multiline = true;
            this.textBoxlabelOneTimeOnlyTag.Name = "textBoxlabelOneTimeOnlyTag";
            this.textBoxlabelOneTimeOnlyTag.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxlabelOneTimeOnlyTag.Size = new System.Drawing.Size(628, 78);
            this.textBoxlabelOneTimeOnlyTag.TabIndex = 8;
            this.textBoxlabelOneTimeOnlyTag.Enter += new System.EventHandler(this.textBoxlabelOneTimeOnlyTag_Enter);
            this.textBoxlabelOneTimeOnlyTag.Leave += new System.EventHandler(this.textBoxlabelOneTimeOnlyTag_Leave);
            // 
            // labelOneTimeOnlyTag
            // 
            this.labelOneTimeOnlyTag.AutoSize = true;
            this.labelOneTimeOnlyTag.Location = new System.Drawing.Point(7, 307);
            this.labelOneTimeOnlyTag.Name = "labelOneTimeOnlyTag";
            this.labelOneTimeOnlyTag.Size = new System.Drawing.Size(102, 13);
            this.labelOneTimeOnlyTag.TabIndex = 7;
            this.labelOneTimeOnlyTag.Text = "ETL jobs references";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Time:";
            // 
            // dateTimePickerTriggerTime
            // 
            this.dateTimePickerTriggerTime.CustomFormat = "";
            this.dateTimePickerTriggerTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dateTimePickerTriggerTime.Location = new System.Drawing.Point(58, 54);
            this.dateTimePickerTriggerTime.Name = "dateTimePickerTriggerTime";
            this.dateTimePickerTriggerTime.ShowUpDown = true;
            this.dateTimePickerTriggerTime.Size = new System.Drawing.Size(96, 20);
            this.dateTimePickerTriggerTime.TabIndex = 5;
            // 
            // dateTimePickerEndDate
            // 
            this.dateTimePickerEndDate.Location = new System.Drawing.Point(58, 29);
            this.dateTimePickerEndDate.Name = "dateTimePickerEndDate";
            this.dateTimePickerEndDate.Size = new System.Drawing.Size(200, 20);
            this.dateTimePickerEndDate.TabIndex = 3;
            this.dateTimePickerEndDate.Value = new System.DateTime(2010, 7, 12, 17, 25, 0, 0);
            // 
            // labelEndDate
            // 
            this.labelEndDate.AutoSize = true;
            this.labelEndDate.Location = new System.Drawing.Point(10, 33);
            this.labelEndDate.Name = "labelEndDate";
            this.labelEndDate.Size = new System.Drawing.Size(29, 13);
            this.labelEndDate.TabIndex = 2;
            this.labelEndDate.Text = "End:";
            // 
            // dateTimePickerStartDate
            // 
            this.dateTimePickerStartDate.Location = new System.Drawing.Point(58, 5);
            this.dateTimePickerStartDate.Name = "dateTimePickerStartDate";
            this.dateTimePickerStartDate.Size = new System.Drawing.Size(200, 20);
            this.dateTimePickerStartDate.TabIndex = 1;
            // 
            // labelStartDate
            // 
            this.labelStartDate.AutoSize = true;
            this.labelStartDate.Location = new System.Drawing.Point(11, 7);
            this.labelStartDate.Name = "labelStartDate";
            this.labelStartDate.Size = new System.Drawing.Size(32, 13);
            this.labelStartDate.TabIndex = 0;
            this.labelStartDate.Text = "Start:";
            // 
            // buttonCreateTrigger
            // 
            this.buttonCreateTrigger.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCreateTrigger.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(80)))), ((int)(((byte)(141)))));
            this.buttonCreateTrigger.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.buttonCreateTrigger.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.buttonCreateTrigger.Location = new System.Drawing.Point(644, 355);
            this.buttonCreateTrigger.Name = "buttonCreateTrigger";
            this.buttonCreateTrigger.Size = new System.Drawing.Size(130, 32);
            this.buttonCreateTrigger.TabIndex = 9;
            this.buttonCreateTrigger.Text = "Create Task";
            this.buttonCreateTrigger.UseVisualStyleBackColor = false;
            this.buttonCreateTrigger.Click += new System.EventHandler(this.buttonCreateTrigger_Click);
            // 
            // buttonReset
            // 
            this.buttonReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonReset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(80)))), ((int)(((byte)(141)))));
            this.buttonReset.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.buttonReset.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.buttonReset.Location = new System.Drawing.Point(780, 323);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(130, 32);
            this.buttonReset.TabIndex = 10;
            this.buttonReset.Text = "Reset Jobs";
            this.buttonReset.UseVisualStyleBackColor = false;
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
            // 
            // buttonStopService
            // 
            this.buttonStopService.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonStopService.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(80)))), ((int)(((byte)(141)))));
            this.buttonStopService.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.buttonStopService.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.buttonStopService.Location = new System.Drawing.Point(780, 440);
            this.buttonStopService.Name = "buttonStopService";
            this.buttonStopService.Size = new System.Drawing.Size(130, 32);
            this.buttonStopService.TabIndex = 16;
            this.buttonStopService.Text = "Stop service";
            this.buttonStopService.UseVisualStyleBackColor = false;
            this.buttonStopService.Click += new System.EventHandler(this.buttonStopService_Click);
            // 
            // buttonStartService
            // 
            this.buttonStartService.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonStartService.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(80)))), ((int)(((byte)(141)))));
            this.buttonStartService.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.buttonStartService.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.buttonStartService.Location = new System.Drawing.Point(644, 440);
            this.buttonStartService.Name = "buttonStartService";
            this.buttonStartService.Size = new System.Drawing.Size(130, 32);
            this.buttonStartService.TabIndex = 15;
            this.buttonStartService.Text = "Start service";
            this.buttonStartService.UseVisualStyleBackColor = false;
            this.buttonStartService.Click += new System.EventHandler(this.buttonStartService_Click);
            // 
            // checkBoxEnabled
            // 
            this.checkBoxEnabled.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxEnabled.AutoSize = true;
            this.checkBoxEnabled.Checked = true;
            this.checkBoxEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxEnabled.Location = new System.Drawing.Point(644, 504);
            this.checkBoxEnabled.Name = "checkBoxEnabled";
            this.checkBoxEnabled.Size = new System.Drawing.Size(115, 17);
            this.checkBoxEnabled.TabIndex = 17;
            this.checkBoxEnabled.Text = "Scheduler enabled";
            this.checkBoxEnabled.UseVisualStyleBackColor = true;
            // 
            // buttonInstallService
            // 
            this.buttonInstallService.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonInstallService.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(80)))), ((int)(((byte)(141)))));
            this.buttonInstallService.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.buttonInstallService.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.buttonInstallService.Location = new System.Drawing.Point(644, 407);
            this.buttonInstallService.Name = "buttonInstallService";
            this.buttonInstallService.Size = new System.Drawing.Size(130, 32);
            this.buttonInstallService.TabIndex = 13;
            this.buttonInstallService.Text = "Install service";
            this.buttonInstallService.UseVisualStyleBackColor = false;
            this.buttonInstallService.Click += new System.EventHandler(this.buttonInstallService_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblServiceStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 536);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(919, 22);
            this.statusStrip1.TabIndex = 18;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblServiceStatus
            // 
            this.lblServiceStatus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblServiceStatus.Name = "lblServiceStatus";
            this.lblServiceStatus.Size = new System.Drawing.Size(22, 17);
            this.lblServiceStatus.Text = ".....";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 277);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "Job Name:";
            // 
            // txtTagName
            // 
            this.txtTagName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTagName.Location = new System.Drawing.Point(71, 274);
            this.txtTagName.Name = "txtTagName";
            this.txtTagName.Size = new System.Drawing.Size(321, 20);
            this.txtTagName.TabIndex = 20;
            // 
            // dgvItems
            // 
            this.dgvItems.AllowUserToAddRows = false;
            this.dgvItems.AllowUserToDeleteRows = false;
            this.dgvItems.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvItems.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvItems.Location = new System.Drawing.Point(10, 407);
            this.dgvItems.Name = "dgvItems";
            this.dgvItems.ReadOnly = true;
            this.dgvItems.Size = new System.Drawing.Size(627, 114);
            this.dgvItems.TabIndex = 21;
            this.dgvItems.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvItems_CellClick);
            this.dgvItems.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvItems_CellPainting);
            // 
            // grpOneTime
            // 
            this.grpOneTime.Controls.Add(this.checkBoxOneTimeOnlyActive);
            this.grpOneTime.Controls.Add(this.labelOneTimeOnlyDay);
            this.grpOneTime.Controls.Add(this.dateTimePickerOneTimeOnlyDay);
            this.grpOneTime.Location = new System.Drawing.Point(13, 84);
            this.grpOneTime.Name = "grpOneTime";
            this.grpOneTime.Size = new System.Drawing.Size(226, 187);
            this.grpOneTime.TabIndex = 22;
            this.grpOneTime.TabStop = false;
            this.grpOneTime.Tag = "1";
            this.grpOneTime.Text = "One time only";
            // 
            // checkBoxOneTimeOnlyActive
            // 
            this.checkBoxOneTimeOnlyActive.AutoSize = true;
            this.checkBoxOneTimeOnlyActive.Location = new System.Drawing.Point(9, 66);
            this.checkBoxOneTimeOnlyActive.Name = "checkBoxOneTimeOnlyActive";
            this.checkBoxOneTimeOnlyActive.Size = new System.Drawing.Size(56, 17);
            this.checkBoxOneTimeOnlyActive.TabIndex = 3;
            this.checkBoxOneTimeOnlyActive.Text = "Active";
            this.checkBoxOneTimeOnlyActive.UseVisualStyleBackColor = true;
            this.checkBoxOneTimeOnlyActive.CheckedChanged += new System.EventHandler(this.checkBoxOneTimeOnlyActive_CheckedChanged);
            // 
            // labelOneTimeOnlyDay
            // 
            this.labelOneTimeOnlyDay.AutoSize = true;
            this.labelOneTimeOnlyDay.Location = new System.Drawing.Point(6, 23);
            this.labelOneTimeOnlyDay.Name = "labelOneTimeOnlyDay";
            this.labelOneTimeOnlyDay.Size = new System.Drawing.Size(29, 13);
            this.labelOneTimeOnlyDay.TabIndex = 5;
            this.labelOneTimeOnlyDay.Text = "Day:";
            // 
            // dateTimePickerOneTimeOnlyDay
            // 
            this.dateTimePickerOneTimeOnlyDay.Location = new System.Drawing.Point(9, 39);
            this.dateTimePickerOneTimeOnlyDay.Name = "dateTimePickerOneTimeOnlyDay";
            this.dateTimePickerOneTimeOnlyDay.Size = new System.Drawing.Size(189, 20);
            this.dateTimePickerOneTimeOnlyDay.TabIndex = 4;
            // 
            // grpDaily
            // 
            this.grpDaily.Controls.Add(this.numericUpDownDaily);
            this.grpDaily.Controls.Add(this.labelDailyEvery);
            this.grpDaily.Controls.Add(this.labelDailyDay);
            this.grpDaily.Location = new System.Drawing.Point(245, 84);
            this.grpDaily.Name = "grpDaily";
            this.grpDaily.Size = new System.Drawing.Size(147, 187);
            this.grpDaily.TabIndex = 22;
            this.grpDaily.TabStop = false;
            this.grpDaily.Tag = "2";
            this.grpDaily.Text = "Daily";
            // 
            // numericUpDownDaily
            // 
            this.numericUpDownDaily.Location = new System.Drawing.Point(9, 39);
            this.numericUpDownDaily.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numericUpDownDaily.Name = "numericUpDownDaily";
            this.numericUpDownDaily.Size = new System.Drawing.Size(49, 20);
            this.numericUpDownDaily.TabIndex = 6;
            this.numericUpDownDaily.ValueChanged += new System.EventHandler(this.numericUpDownDaily_ValueChanged);
            this.numericUpDownDaily.Enter += new System.EventHandler(this.numericUpDownDaily_Enter);
            // 
            // labelDailyEvery
            // 
            this.labelDailyEvery.AutoSize = true;
            this.labelDailyEvery.Location = new System.Drawing.Point(6, 23);
            this.labelDailyEvery.Name = "labelDailyEvery";
            this.labelDailyEvery.Size = new System.Drawing.Size(34, 13);
            this.labelDailyEvery.TabIndex = 7;
            this.labelDailyEvery.Text = "Every";
            // 
            // labelDailyDay
            // 
            this.labelDailyDay.AutoSize = true;
            this.labelDailyDay.Location = new System.Drawing.Point(64, 45);
            this.labelDailyDay.Name = "labelDailyDay";
            this.labelDailyDay.Size = new System.Drawing.Size(24, 13);
            this.labelDailyDay.TabIndex = 8;
            this.labelDailyDay.Text = "day";
            // 
            // grpWeekly
            // 
            this.grpWeekly.Controls.Add(this.labelWeeklyDays);
            this.grpWeekly.Controls.Add(this.checkedListBoxWeeklyDays);
            this.grpWeekly.Location = new System.Drawing.Point(398, 84);
            this.grpWeekly.Name = "grpWeekly";
            this.grpWeekly.Size = new System.Drawing.Size(133, 187);
            this.grpWeekly.TabIndex = 22;
            this.grpWeekly.TabStop = false;
            this.grpWeekly.Tag = "3";
            this.grpWeekly.Text = "Weekly";
            // 
            // labelWeeklyDays
            // 
            this.labelWeeklyDays.AutoSize = true;
            this.labelWeeklyDays.Location = new System.Drawing.Point(6, 24);
            this.labelWeeklyDays.Name = "labelWeeklyDays";
            this.labelWeeklyDays.Size = new System.Drawing.Size(34, 13);
            this.labelWeeklyDays.TabIndex = 30;
            this.labelWeeklyDays.Text = "Days:";
            // 
            // checkedListBoxWeeklyDays
            // 
            this.checkedListBoxWeeklyDays.FormattingEnabled = true;
            this.checkedListBoxWeeklyDays.Items.AddRange(new object[] {
            "Sunday",
            "Monday",
            "Tuesday",
            "Wednesday",
            "Thursday",
            "Friday",
            "Saturday"});
            this.checkedListBoxWeeklyDays.Location = new System.Drawing.Point(6, 40);
            this.checkedListBoxWeeklyDays.Name = "checkedListBoxWeeklyDays";
            this.checkedListBoxWeeklyDays.Size = new System.Drawing.Size(104, 109);
            this.checkedListBoxWeeklyDays.TabIndex = 29;
            this.checkedListBoxWeeklyDays.SelectedIndexChanged += new System.EventHandler(this.checkedListBoxWeeklyDays_SelectedIndexChanged);
            // 
            // grpMonthly
            // 
            this.grpMonthly.Controls.Add(this.tabControlMonthlyMode);
            this.grpMonthly.Controls.Add(this.labelMonthlyMonth);
            this.grpMonthly.Controls.Add(this.checkedListBoxMonthlyMonths);
            this.grpMonthly.Location = new System.Drawing.Point(537, 84);
            this.grpMonthly.Name = "grpMonthly";
            this.grpMonthly.Size = new System.Drawing.Size(370, 187);
            this.grpMonthly.TabIndex = 22;
            this.grpMonthly.TabStop = false;
            this.grpMonthly.Tag = "4";
            this.grpMonthly.Text = "Monthly";
            // 
            // tabControlMonthlyMode
            // 
            this.tabControlMonthlyMode.Controls.Add(this.tabPageMonthlyDayOfMonth);
            this.tabControlMonthlyMode.Controls.Add(this.tabPageMonthlyWeekDay);
            this.tabControlMonthlyMode.Location = new System.Drawing.Point(137, 38);
            this.tabControlMonthlyMode.Name = "tabControlMonthlyMode";
            this.tabControlMonthlyMode.SelectedIndex = 0;
            this.tabControlMonthlyMode.Size = new System.Drawing.Size(216, 143);
            this.tabControlMonthlyMode.TabIndex = 31;
            // 
            // tabPageMonthlyDayOfMonth
            // 
            this.tabPageMonthlyDayOfMonth.Controls.Add(this.checkedListBoxMonthlyDays);
            this.tabPageMonthlyDayOfMonth.Location = new System.Drawing.Point(4, 22);
            this.tabPageMonthlyDayOfMonth.Name = "tabPageMonthlyDayOfMonth";
            this.tabPageMonthlyDayOfMonth.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMonthlyDayOfMonth.Size = new System.Drawing.Size(208, 117);
            this.tabPageMonthlyDayOfMonth.TabIndex = 0;
            this.tabPageMonthlyDayOfMonth.Text = "Day of Month";
            this.tabPageMonthlyDayOfMonth.UseVisualStyleBackColor = true;
            // 
            // checkedListBoxMonthlyDays
            // 
            this.checkedListBoxMonthlyDays.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListBoxMonthlyDays.FormattingEnabled = true;
            this.checkedListBoxMonthlyDays.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "30",
            "31",
            "Last Day"});
            this.checkedListBoxMonthlyDays.Location = new System.Drawing.Point(3, 3);
            this.checkedListBoxMonthlyDays.Name = "checkedListBoxMonthlyDays";
            this.checkedListBoxMonthlyDays.Size = new System.Drawing.Size(202, 111);
            this.checkedListBoxMonthlyDays.TabIndex = 29;
            // 
            // tabPageMonthlyWeekDay
            // 
            this.tabPageMonthlyWeekDay.Controls.Add(this.checkedListBoxMonthlyWeekNumber);
            this.tabPageMonthlyWeekDay.Controls.Add(this.checkedListBoxMonthlyWeekDay);
            this.tabPageMonthlyWeekDay.Location = new System.Drawing.Point(4, 22);
            this.tabPageMonthlyWeekDay.Name = "tabPageMonthlyWeekDay";
            this.tabPageMonthlyWeekDay.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMonthlyWeekDay.Size = new System.Drawing.Size(208, 117);
            this.tabPageMonthlyWeekDay.TabIndex = 1;
            this.tabPageMonthlyWeekDay.Text = "Weekday";
            this.tabPageMonthlyWeekDay.UseVisualStyleBackColor = true;
            // 
            // checkedListBoxMonthlyWeekNumber
            // 
            this.checkedListBoxMonthlyWeekNumber.FormattingEnabled = true;
            this.checkedListBoxMonthlyWeekNumber.Items.AddRange(new object[] {
            "First",
            "Second",
            "Third",
            "Fourth",
            "Last"});
            this.checkedListBoxMonthlyWeekNumber.Location = new System.Drawing.Point(3, 6);
            this.checkedListBoxMonthlyWeekNumber.Name = "checkedListBoxMonthlyWeekNumber";
            this.checkedListBoxMonthlyWeekNumber.Size = new System.Drawing.Size(98, 79);
            this.checkedListBoxMonthlyWeekNumber.TabIndex = 33;
            // 
            // checkedListBoxMonthlyWeekDay
            // 
            this.checkedListBoxMonthlyWeekDay.FormattingEnabled = true;
            this.checkedListBoxMonthlyWeekDay.Items.AddRange(new object[] {
            "Sunday",
            "Monday",
            "Tuesday",
            "Wednesday",
            "Thursday",
            "Friday",
            "Saturday"});
            this.checkedListBoxMonthlyWeekDay.Location = new System.Drawing.Point(107, 6);
            this.checkedListBoxMonthlyWeekDay.Name = "checkedListBoxMonthlyWeekDay";
            this.checkedListBoxMonthlyWeekDay.Size = new System.Drawing.Size(94, 109);
            this.checkedListBoxMonthlyWeekDay.TabIndex = 34;
            // 
            // labelMonthlyMonth
            // 
            this.labelMonthlyMonth.AutoSize = true;
            this.labelMonthlyMonth.Location = new System.Drawing.Point(3, 24);
            this.labelMonthlyMonth.Name = "labelMonthlyMonth";
            this.labelMonthlyMonth.Size = new System.Drawing.Size(40, 13);
            this.labelMonthlyMonth.TabIndex = 32;
            this.labelMonthlyMonth.Text = "Month:";
            // 
            // checkedListBoxMonthlyMonths
            // 
            this.checkedListBoxMonthlyMonths.FormattingEnabled = true;
            this.checkedListBoxMonthlyMonths.Items.AddRange(new object[] {
            "January",
            "February",
            "March",
            "April",
            "May",
            "June",
            "July",
            "August",
            "September",
            "October",
            "November",
            "December"});
            this.checkedListBoxMonthlyMonths.Location = new System.Drawing.Point(6, 40);
            this.checkedListBoxMonthlyMonths.Name = "checkedListBoxMonthlyMonths";
            this.checkedListBoxMonthlyMonths.Size = new System.Drawing.Size(120, 109);
            this.checkedListBoxMonthlyMonths.TabIndex = 30;
            this.checkedListBoxMonthlyMonths.SelectedIndexChanged += new System.EventHandler(this.checkedListBoxMonthlyMonths_SelectedIndexChanged);
            // 
            // btnPreview
            // 
            this.btnPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPreview.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(80)))), ((int)(((byte)(141)))));
            this.btnPreview.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.btnPreview.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnPreview.Location = new System.Drawing.Point(644, 323);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(130, 32);
            this.btnPreview.TabIndex = 23;
            this.btnPreview.Text = "Preview";
            this.btnPreview.UseVisualStyleBackColor = false;
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // btnNew
            // 
            this.btnNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNew.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(80)))), ((int)(((byte)(141)))));
            this.btnNew.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.btnNew.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnNew.Location = new System.Drawing.Point(780, 355);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(130, 32);
            this.btnNew.TabIndex = 24;
            this.btnNew.Text = "Cancel";
            this.btnNew.UseVisualStyleBackColor = false;
            this.btnNew.Visible = false;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // TaskSchedulerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(919, 558);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.btnPreview);
            this.Controls.Add(this.grpMonthly);
            this.Controls.Add(this.grpWeekly);
            this.Controls.Add(this.grpDaily);
            this.Controls.Add(this.grpOneTime);
            this.Controls.Add(this.dgvItems);
            this.Controls.Add(this.txtTagName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.buttonInstallService);
            this.Controls.Add(this.buttonStopService);
            this.Controls.Add(this.buttonStartService);
            this.Controls.Add(this.checkBoxEnabled);
            this.Controls.Add(this.buttonCreateTrigger);
            this.Controls.Add(this.buttonReset);
            this.Controls.Add(this.textBoxlabelOneTimeOnlyTag);
            this.Controls.Add(this.labelOneTimeOnlyTag);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dateTimePickerTriggerTime);
            this.Controls.Add(this.dateTimePickerEndDate);
            this.Controls.Add(this.labelEndDate);
            this.Controls.Add(this.dateTimePickerStartDate);
            this.Controls.Add(this.labelStartDate);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "TaskSchedulerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TaskScheduler";
            this.Shown += new System.EventHandler(this.TaskSchedulerForm_Shown);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).EndInit();
            this.grpOneTime.ResumeLayout(false);
            this.grpOneTime.PerformLayout();
            this.grpDaily.ResumeLayout(false);
            this.grpDaily.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDaily)).EndInit();
            this.grpWeekly.ResumeLayout(false);
            this.grpWeekly.PerformLayout();
            this.grpMonthly.ResumeLayout(false);
            this.grpMonthly.PerformLayout();
            this.tabControlMonthlyMode.ResumeLayout(false);
            this.tabPageMonthlyDayOfMonth.ResumeLayout(false);
            this.tabPageMonthlyWeekDay.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxlabelOneTimeOnlyTag;
        //private DockSample.Controls.PlaceHolderTextBox textBoxlabelOneTimeOnlyTag;
        private System.Windows.Forms.Label labelOneTimeOnlyTag;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dateTimePickerTriggerTime;
        private System.Windows.Forms.DateTimePicker dateTimePickerEndDate;
        private System.Windows.Forms.Label labelEndDate;
        private System.Windows.Forms.DateTimePicker dateTimePickerStartDate;
        private System.Windows.Forms.Label labelStartDate;
        private System.Windows.Forms.Button buttonCreateTrigger;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.Button buttonStopService;
        private System.Windows.Forms.Button buttonStartService;
        private System.Windows.Forms.CheckBox checkBoxEnabled;
        private System.Windows.Forms.Button buttonInstallService;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblServiceStatus;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTagName;
        private System.Windows.Forms.DataGridView dgvItems;
        private System.Windows.Forms.GroupBox grpOneTime;
        private System.Windows.Forms.GroupBox grpDaily;
        private System.Windows.Forms.GroupBox grpWeekly;
        private System.Windows.Forms.GroupBox grpMonthly;
        private System.Windows.Forms.CheckBox checkBoxOneTimeOnlyActive;
        private System.Windows.Forms.Label labelOneTimeOnlyDay;
        private System.Windows.Forms.DateTimePicker dateTimePickerOneTimeOnlyDay;
        private System.Windows.Forms.NumericUpDown numericUpDownDaily;
        private System.Windows.Forms.Label labelDailyEvery;
        private System.Windows.Forms.Label labelDailyDay;
        private System.Windows.Forms.Label labelWeeklyDays;
        private System.Windows.Forms.CheckedListBox checkedListBoxWeeklyDays;
        private System.Windows.Forms.TabControl tabControlMonthlyMode;
        private System.Windows.Forms.TabPage tabPageMonthlyDayOfMonth;
        private System.Windows.Forms.CheckedListBox checkedListBoxMonthlyDays;
        private System.Windows.Forms.TabPage tabPageMonthlyWeekDay;
        private System.Windows.Forms.CheckedListBox checkedListBoxMonthlyWeekNumber;
        private System.Windows.Forms.CheckedListBox checkedListBoxMonthlyWeekDay;
        private System.Windows.Forms.Label labelMonthlyMonth;
        private System.Windows.Forms.CheckedListBox checkedListBoxMonthlyMonths;
        private System.Windows.Forms.Button btnPreview;
        private System.Windows.Forms.Button btnNew;
    }
}