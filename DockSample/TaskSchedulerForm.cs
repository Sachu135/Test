using DockSample.lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Windows.Forms;
using UIFunctionality.Common;
using WeifenLuo.WinFormsUI.Docking;
using static UIFunctionality.Common.TaskScheduler;

namespace DockSample
{
    public partial class TaskSchedulerForm : DockContent //Form
    {
        public string AppPath { get; set; }
        private string schedulerFileName = "KockpitStudioTaskSchedulerServices.xml";

        private TaskScheduler _taskScheduler;

        public TaskSchedulerForm(string appPath)
        {
            this.AppPath = appPath;

            InitializeComponent();
            _taskScheduler = new TaskScheduler();

            // Set the synchronizing object to get trigger events within the main thread.
            // Important if you are using Windows Forms
            _taskScheduler.SynchronizingObject = this;

            dateTimePickerStartDate.Value = DateTime.Today;
            dateTimePickerEndDate.Value = DateTime.Today.AddYears(1);
            dateTimePickerTriggerTime.Value = DateTime.Now.AddMinutes(10); // Add 10 Minutes for testing
        }

        void triggerItem_OnTrigger(object sender, TaskScheduler.OnTriggerEventArgs e)
        {
            String nextTrigger = String.Empty;
            if (e.Item.GetNextTriggerDateTime() != DateTime.MaxValue)
                nextTrigger = e.Item.GetNextTriggerDateTime().DayOfWeek.ToString() + ", " + e.Item.GetNextTriggerDateTime().ToString();
            else
                nextTrigger = "Never";
            //textBoxEvents.AppendText(e.TriggerDate.ToString() + ": " + e.Item.Tag + ", next trigger: " + nextTrigger + "\r\n");
            UpdateTaskList();
        }

        #region [Methods]
        private void CreateSchedulerItem()
        {
            TaskScheduler.TriggerItem triggerItem = new TaskScheduler.TriggerItem();
            var tagName = txtTagName.Text.Trim();
            var existingObj = _taskScheduler.TriggerItems.Cast<TriggerItem>()
                .FirstOrDefault(c => c.TagName.ToString().Trim().ToLower().Equals(tagName.Trim().ToLower()));
            if (existingObj != null)
            {
                //code to show message for already exists..
                MessageBox.Show("Already a job with a same name is exists..", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                string[] liTags = textBoxlabelOneTimeOnlyTag.Text.ToString().Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                triggerItem.TagName = tagName;
                triggerItem.Tag = string.Join(",", liTags); //textBoxlabelOneTimeOnlyTag.Text;
                triggerItem.StartDate = dateTimePickerStartDate.Value;
                triggerItem.EndDate = dateTimePickerEndDate.Value;
                triggerItem.TriggerTime = dateTimePickerTriggerTime.Value;
                triggerItem.OnTrigger += new TaskScheduler.TriggerItem.OnTriggerEventHandler(triggerItem_OnTrigger); // And the trigger-Event :)

                // Set OneTimeOnly - Active and Date
                triggerItem.TriggerSettings.OneTimeOnly.Active = checkBoxOneTimeOnlyActive.Checked;
                triggerItem.TriggerSettings.OneTimeOnly.Date = dateTimePickerOneTimeOnlyDay.Value.Date;

                // Set the interval for daily trigger
                triggerItem.TriggerSettings.Daily.Interval = (ushort)numericUpDownDaily.Value;

                // Set the active days for weekly trigger
                for (byte day = 0; day < 7; day++) // Set the active Days
                    triggerItem.TriggerSettings.Weekly.DaysOfWeek[day] = checkedListBoxWeeklyDays.GetItemChecked(day);

                // Set the active months for monthly trigger
                for (byte month = 0; month < 12; month++)
                    triggerItem.TriggerSettings.Monthly.Month[month] = checkedListBoxMonthlyMonths.GetItemChecked(month);

                // Set active Days (0..30 = Days, 31=last Day) for monthly trigger
                for (byte day = 0; day < 32; day++)
                    triggerItem.TriggerSettings.Monthly.DaysOfMonth[day] = checkedListBoxMonthlyDays.GetItemChecked(day);

                // Set the active weekNumber and DayOfWeek for monthly trigger
                // f.e. the first monday, or the last friday...
                for (byte weekNumber = 0; weekNumber < 5; weekNumber++) // 0..4: first, second, third, fourth or last week
                    triggerItem.TriggerSettings.Monthly.WeekDay.WeekNumber[weekNumber] = checkedListBoxMonthlyWeekNumber.GetItemChecked(weekNumber);
                for (byte day = 0; day < 7; day++)
                    triggerItem.TriggerSettings.Monthly.WeekDay.DayOfWeek[day] = checkedListBoxMonthlyWeekDay.GetItemChecked(day);

                triggerItem.Enabled = true; // Set the Item-Active - State
                _taskScheduler.AddTrigger(triggerItem); // Add the trigger to List
                _taskScheduler.Enabled = checkBoxEnabled.Checked; // Start the Scheduler

                UpdateTaskList();
                SaveConfig();
            }
        }
        private void UpdateTaskList()
        {
            DataTable tData = new DataTable();
            tData.Columns.Add("JobName", typeof(string));
            tData.Columns.Add("Job", typeof(string));
            tData.Columns.Add("JobTrigger", typeof(string));

            dgvItems.PerformSafely(() =>
            {
                dgvItems.DataSource = null;
                dgvItems.Rows.Clear();
                dgvItems.Columns.Clear();

                if (_taskScheduler.TriggerItems != null)
                {
                    if (_taskScheduler.TriggerItems.Count > 0)
                    {
                        foreach (TaskScheduler.TriggerItem item in _taskScheduler.TriggerItems)
                        {
                            DataRow dr = tData.NewRow();
                            dr["JobName"] = item.TagName;
                            dr["Job"] = item.Tag;
                            DateTime nextDate = item.GetNextTriggerDateTime();
                            if (nextDate != DateTime.MaxValue)
                                dr["JobTrigger"] = nextDate.ToString();
                            else
                                dr["JobTrigger"] = "Never";

                            tData.Rows.Add(dr);
                        }

                        dgvItems.DataSource = tData;

                        //DataGridViewLinkColumn InfoLink = new DataGridViewLinkColumn();
                        //InfoLink.UseColumnTextForLinkValue = true;
                        //InfoLink.HeaderText = "Info Action";
                        //InfoLink.DataPropertyName = "lnkInfoColumn";
                        //InfoLink.LinkBehavior = LinkBehavior.SystemDefault;
                        //InfoLink.Text = "Info";
                        //InfoLink.Tag = "Info";
                        //dgvItems.Columns.Add(InfoLink);

                        //DataGridViewLinkColumn Deletelink = new DataGridViewLinkColumn();
                        //Deletelink.UseColumnTextForLinkValue = true;
                        //Deletelink.HeaderText = "Delete Action";
                        //Deletelink.DataPropertyName = "lnkColumn";
                        //Deletelink.LinkBehavior = LinkBehavior.SystemDefault;
                        //Deletelink.Text = "Delete";
                        //Deletelink.Tag = "Delete";
                        //Deletelink.Columns.Add(Deletelink);

                        DataGridViewButtonColumn InfoLink = new DataGridViewButtonColumn();
                        InfoLink.HeaderText = "Info";
                        //InfoLink.Text = "Info";
                        InfoLink.Tag = "Info";
                        InfoLink.Width = 20;
                        InfoLink.UseColumnTextForButtonValue = true;
                        dgvItems.Columns.Add(InfoLink);

                        DataGridViewButtonColumn Deletelink = new DataGridViewButtonColumn();
                        Deletelink.HeaderText = "Delete";
                        //Deletelink.Text = "Delete";
                        Deletelink.Tag = "Delete";
                        Deletelink.Width = 20;
                        Deletelink.UseColumnTextForButtonValue = true;
                        dgvItems.Columns.Add(Deletelink);
                    }
                }
            });
        }
        private void ResetScheduler()
        {
            _taskScheduler.Enabled = false;
            _taskScheduler.TriggerItems.Clear();
            UpdateTaskList();
            SaveConfig();
            //textBoxEvents.Clear();
        }
        private string ExportCollectionToXML()
        {
            String xmlString = String.Empty;
            try
            {
                xmlString = _taskScheduler.TriggerItems.ToXML();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: generate XML: " + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return xmlString;
        }
        private void ImportCollectionFromXML()
        {
            _taskScheduler.TriggerItems.Clear();
            try
            {
                TriggerItemCollection items = TriggerItemCollection.FromXML(ReadXML());
                _taskScheduler.TriggerItems.AddRange(items, new TaskScheduler.TriggerItem.OnTriggerEventHandler(triggerItem_OnTrigger));
                _taskScheduler.Enabled = checkBoxEnabled.Checked;
                UpdateTaskList();
            }
            catch (Exception ex)
            {
            }
        }
        private void ShowAllTriggerDates(TriggerItem triggerItem)
        {
            if (triggerItem != null)
            {
                TriggerItem item = (TriggerItem)triggerItem;
                Form form = new Form();
                form.Text = item.TagName.ToString();
                form.Width = 400;
                form.Height = 450;
                form.StartPosition = FormStartPosition.CenterScreen;

                TabControl tabControl = new TabControl();
                tabControl.Parent = form;
                tabControl.Dock = DockStyle.Fill;

                TabPage Page1 = new TabPage();
                Page1.Text = "Scheduled Jobs";
                Page1.Name = "tPage1";

                ListView listView = new ListView();
                listView.FullRowSelect = true;
                listView.Parent = Page1;
                listView.Dock = DockStyle.Fill;
                listView.View = View.Details;
                listView.Columns.Add("Date", 200);
                DateTime date = dateTimePickerStartDate.Value.Date;
                while (date <= dateTimePickerEndDate.Value.Date)
                {
                    if (item.CheckDate(date)) // probe this date
                        listView.Items.Add(date.ToLongDateString());
                    date = date.AddDays(1);
                }

                TabPage Page2 = new TabPage();
                Page2.Text = "All Jobs";
                Page2.Name = "tPage2";

                ListView listAllView = new ListView();
                listAllView.FullRowSelect = true;
                listAllView.Parent = Page2;
                listAllView.Dock = DockStyle.Fill;
                listAllView.View = View.Details;
                listAllView.Columns.Add("JobName", 100);
                listAllView.Columns.Add("Job", 200);
                listAllView.Columns.Add("Trigger", 100);
                foreach (TriggerItem jobitem in _taskScheduler.TriggerItems)
                {
                    ListViewItem listItem = listAllView.Items.Add(jobitem.TagName.ToString());
                    listItem.SubItems.Add(jobitem.Tag.ToString());
                    listItem.Tag = item;
                    DateTime nextDate = item.GetNextTriggerDateTime();
                    if (nextDate != DateTime.MaxValue)
                        listItem.SubItems.Add(nextDate.ToString());
                    else
                        listItem.SubItems.Add("Never");
                }

                tabControl.TabPages.Add(Page1);
                tabControl.TabPages.Add(Page2);
                form.Show();
            }
            else
                MessageBox.Show("Please select a trigger!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void SaveAsServiceConfig()
        {
            //if (_taskScheduler.TriggerItems.Count == 0)
            //{
            //    MessageBox.Show("Please create trigger items!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}

            String xmlString = ExportCollectionToXML();
            String configFile = Path.Combine(AppPath, schedulerFileName);
            String directory = Path.GetDirectoryName(configFile);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            using (StreamWriter outfile = new StreamWriter(configFile))
            {
                try
                {
                    outfile.Write(xmlString);
                    MessageBox.Show("Configuration saved successfully!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: write XML: " + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void SaveConfig()
        {
            ///code to check
            ///if service not installed then go as same.
            ///if service installed and not started then go as same
            ///if service installed and started then confirmation message for ""
            ///if YES
            ///then stop service , update XML , start service
            ///else
            ///do nothing
            ///

            if (IsServiceInstalled("KockpitStudioService") && IsServiceRunning("KockpitStudioService").Item1)
            {
                DialogResult dialogResult = MessageBox.Show("All the background running task will terminated. " + Environment.NewLine + " Are you sure want to restart the service ?",
                    "Restart service", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    SaveAsServiceConfig();
                    Cursor.Current = Cursors.WaitCursor;
                    KockpitStudioServiceAssistant.StopService();
                    KockpitStudioServiceAssistant.StartService();
                    EnableControls();
                }
            }
            else
            {
                SaveAsServiceConfig();
            }
        }
        private static void InstallService()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                var asmPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WindowsService.exe");
                KockpitStudioServiceAssistant.Install(Assembly.LoadFrom(asmPath));
                MessageBox.Show("Service installation successful", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: install service: " + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Cursor.Current = Cursors.Default;
        }
        private static void UninstallService()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                var asmPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WindowsService.exe");
                KockpitStudioServiceAssistant.Uninstall(Assembly.LoadFrom(asmPath));
                MessageBox.Show("Service removed successfuly", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: uninstall service: " + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Cursor.Current = Cursors.Default;
        }
        private static void StartService()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                KockpitStudioServiceAssistant.StartService();
                MessageBox.Show("Service start successful", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: start service: " + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Cursor.Current = Cursors.Default;
        }
        private static void StopService()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                KockpitStudioServiceAssistant.StopService();
                MessageBox.Show("Service stop successful", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: stop service: " + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Cursor.Current = Cursors.Default;
        }
        private bool ValidateEntry()
        {
            DateTime tStart = Convert.ToDateTime(dateTimePickerStartDate.Text.Trim());
            DateTime tEnd = Convert.ToDateTime(dateTimePickerEndDate.Text.Trim());
            if(tEnd < tStart)
            {
                MessageBox.Show("End date should be greater or equal to start date.");
                return false;
            }

            if (string.IsNullOrEmpty(txtTagName.Text.Trim()))
            {
                MessageBox.Show("Please mention Job name.");
                return false;
            }

            if (string.IsNullOrEmpty(textBoxlabelOneTimeOnlyTag.Text.Trim()) ||
                textBoxlabelOneTimeOnlyTag.Text == @"spark-submit D:\Workspace\Stage1\DataIngestion.py " + Environment.NewLine + @"spark - submit D:\Workspace\Stage2\AR.py" + Environment.NewLine + @"spark - submit D:\Workspace\Stage3\AP.py" + Environment.NewLine + "...")
            {
                MessageBox.Show("Please mention the ETL job references.");
                return false;
            }

            return true;
        }
        private bool IsServiceInstalled(string strServiceName)
        {
            try
            {
                var serviceExists = ServiceController.GetServices().Any(s => s.ServiceName == strServiceName);
                return Convert.ToBoolean(serviceExists);
            }
            catch (Exception)
            {
                return false;
            }
        }
        private Tuple<bool, string> IsServiceRunning(string strServiceName)
        {
            try
            {
                ServiceController sc = new ServiceController(strServiceName);
                string ServiceStatus = string.Empty;
                switch (sc.Status)
                {
                    case ServiceControllerStatus.Running:
                        ServiceStatus = "Running";
                        break;
                    case ServiceControllerStatus.Stopped:
                        ServiceStatus = "Stopped";
                        break;
                    case ServiceControllerStatus.Paused:
                        ServiceStatus = "Paused";
                        break;
                    case ServiceControllerStatus.StopPending:
                        ServiceStatus = "Stopping";
                        break;
                    case ServiceControllerStatus.StartPending:
                        ServiceStatus = "Starting";
                        break;
                    default:
                        ServiceStatus = "Changing";
                        break;
                }

                if (!string.IsNullOrEmpty(ServiceStatus)
                    && (ServiceStatus == "Running" || ServiceStatus == "Starting"))
                {
                    return new Tuple<bool, string>(true, ServiceStatus);
                }
                else
                    return new Tuple<bool, string>(false, ServiceStatus);
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string>(false, ex.Message);
            }
        }
        private void EnableControls()
        {
            //code to check the service is install or not 
            //if install disable the install service button else enable 
            //if install enable the uninstall service button else disable
            bool lInstalled = IsServiceInstalled("KockpitStudioService");
            buttonInstallService.Enabled = !lInstalled;
            buttonUninstallService.Enabled = lInstalled;

            //code to check the service is start or not
            //if service start then enable the stop service button else disable
            //if service stop then enable the start service button else disable
            Tuple<bool, string> tplRunningStatus = IsServiceRunning("KockpitStudioService");
            buttonStartService.Enabled = !tplRunningStatus.Item1 && lInstalled;
            buttonStopService.Enabled = tplRunningStatus.Item1;

            //Code to change the status color
            lblServiceStatus.ForeColor = (!tplRunningStatus.Item1 || !lInstalled)
                ? Color.Red
                : (tplRunningStatus.Item2 == "Paused" || tplRunningStatus.Item2 == "Stopping"
                || tplRunningStatus.Item2 == "Stopping")
                ? Color.Orange
                : Color.Green;
            //Code to change the status text
            lblServiceStatus.Text = !lInstalled
                ? "Service Not Installed, To install click on the Install Service button."
                : (!tplRunningStatus.Item1)
                ? "Service Not Started, To start click on the Start Service button."
                : "Service Status : " + tplRunningStatus.Item2;
        }
        private string ReadXML()
        {
            //code to read the xml
            String configFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "KockpitStudioTaskSchedulerServices.xml");
            String xmlString = String.Empty;
            try
            {
                xmlString = System.IO.File.ReadAllText(configFile);
                return xmlString;
            }
            catch (Exception)
            {
                return "";
            }
        }
        #endregion

        #region [Clicks]
        private void buttonCreateTrigger_Click(object sender, EventArgs e)
        {
            if(ValidateEntry())
                CreateSchedulerItem();
        }
        private void buttonReset_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("All the background scheduled task will deleted. " + Environment.NewLine + " Are you sure wants to reset ?",
                "Reset", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                ResetScheduler();
            }
        }
        private void buttonInstallService_Click(object sender, EventArgs e)
        {
            InstallService();
            EnableControls();
        }
        private void buttonUninstallService_Click(object sender, EventArgs e)
        {
            UninstallService();
            EnableControls();
        }
        private void buttonStartService_Click(object sender, EventArgs e)
        {
            StartService();
            EnableControls();
        }
        private void buttonStopService_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox
                .Show("All the background running jobs will terminated. " + Environment.NewLine + " Are you sure want to stop the service ?",
                            "Stop Service", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                StopService();
                EnableControls();
            }
        }
        private void TaskSchedulerForm_Shown(object sender, EventArgs e)
        {
            EnableControls();
            ImportCollectionFromXML();
            textBoxlabelOneTimeOnlyTag.Text = @"spark-submit D:\Workspace\Stage1\DataIngestion.py "+Environment.NewLine+ @"spark - submit D:\Workspace\Stage2\AR.py"+Environment.NewLine+ @"spark - submit D:\Workspace\Stage3\AP.py"+Environment.NewLine+"...";
            textBoxlabelOneTimeOnlyTag.ForeColor = Color.Gray;
        }
        private void dgvItems_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            //paint info
            if (e.ColumnIndex == 3)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                var w = Properties.Resources.info.Width;
                var h = Properties.Resources.info.Height;
                var x = e.CellBounds.Left + (e.CellBounds.Width - w) / 2;
                var y = e.CellBounds.Top + (e.CellBounds.Height - h) / 2;

                e.Graphics.DrawImage(Properties.Resources.info, new Rectangle(x, y, w, h));
                e.Handled = true;
            }

            //paint delete
            if (e.ColumnIndex == 4)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                var w = Properties.Resources.clear.Width;
                var h = Properties.Resources.clear.Height;
                var x = e.CellBounds.Left + (e.CellBounds.Width - w) / 2;
                var y = e.CellBounds.Top + (e.CellBounds.Height - h) / 2;

                e.Graphics.DrawImage(Properties.Resources.clear, new Rectangle(x, y, w, h));
                e.Handled = true;
            }
        }
        private void dgvItems_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;
            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                //TODO - Button Clicked - Execute Code Here
                DataGridViewButtonCell cell = (DataGridViewButtonCell)dgvItems.CurrentCell;
                string currentTagName = dgvItems.Rows[e.RowIndex].Cells[0].Value.ToString().Trim();
                if (cell.ColumnIndex == 3)
                {
                    var dataSource = _taskScheduler.TriggerItems.Cast<TriggerItem>()
                                    .Where(c => (string)c.TagName == currentTagName).FirstOrDefault();
                    if (dataSource != null)
                    {
                        ShowAllTriggerDates(dataSource);
                    }
                }
                else if (cell.ColumnIndex == 4)
                {
                    if (IsServiceRunning("KockpitStudioService").Item1)
                    {
                        DialogResult dialogResult = MessageBox.Show("All the background running jobs will terminated. " + Environment.NewLine + " Are you sure want to delete the job ?",
                            "Delete Job", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.Yes)
                        {
                            //remove
                            if (_taskScheduler.TriggerItems != null && _taskScheduler.TriggerItems.Count > 0)
                            {
                                var dataSource = _taskScheduler.TriggerItems.Cast<TriggerItem>()
                                    .Where(c => (string)c.TagName == currentTagName).FirstOrDefault();
                                if (dataSource != null)
                                {
                                    _taskScheduler.TriggerItems.Remove(dataSource);
                                    UpdateTaskList();

                                    SaveAsServiceConfig();
                                    Cursor.Current = Cursors.WaitCursor;
                                    KockpitStudioServiceAssistant.StopService();
                                    KockpitStudioServiceAssistant.StartService();
                                    EnableControls();
                                }
                            }
                        }
                    }
                    else
                    {
                        //remove
                        if (_taskScheduler.TriggerItems != null && _taskScheduler.TriggerItems.Count > 0)
                        {
                            var dataSource = _taskScheduler.TriggerItems.Cast<TriggerItem>()
                                .Where(c => (string)c.TagName == currentTagName).FirstOrDefault();
                            if (dataSource != null)
                            {
                                _taskScheduler.TriggerItems.Remove(dataSource);
                                UpdateTaskList();
                                SaveConfig();
                            }
                        }
                    }
                }
            }
        }
        #endregion


        private void textBoxlabelOneTimeOnlyTag_Enter(object sender, EventArgs e)
        {
            if (textBoxlabelOneTimeOnlyTag.Text == @"spark-submit D:\Workspace\Stage1\DataIngestion.py " + Environment.NewLine + @"spark - submit D:\Workspace\Stage2\AR.py" + Environment.NewLine + @"spark - submit D:\Workspace\Stage3\AP.py" + Environment.NewLine + "...")
            {
                textBoxlabelOneTimeOnlyTag.Text = "";
                textBoxlabelOneTimeOnlyTag.ForeColor = System.Drawing.SystemColors.WindowText;
            }
        }

        private void textBoxlabelOneTimeOnlyTag_Leave(object sender, EventArgs e)
        {
            if (textBoxlabelOneTimeOnlyTag.Text == "")
            {
                textBoxlabelOneTimeOnlyTag.Text = @"spark-submit D:\Workspace\Stage1\DataIngestion.py " + Environment.NewLine + @"spark - submit D:\Workspace\Stage2\AR.py" + Environment.NewLine + @"spark - submit D:\Workspace\Stage3\AP.py" + Environment.NewLine + "...";
                textBoxlabelOneTimeOnlyTag.ForeColor = Color.Gray;
            }
        }
    }
}
