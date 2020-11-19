using CustomControls;
using DockSample.Controls;
using DockSample.lib;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ubiety.Dns.Core;
using WeifenLuo.WinFormsUI.Docking;

namespace DockSample
{
    public partial class DummyTaskList : DockContent //ToolWindow
    {
        TaskConfig _taskConfig;
        bool lAlreadySet = false;
        private string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "todo.json");
        AddTask addtaskForm = new AddTask();
        UCLoaderForm loader = new UCLoaderForm();

        public DummyTaskList()
        {
            InitializeComponent();
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)(((((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft)
                        | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight)
                        | WeifenLuo.WinFormsUI.Docking.DockAreas.DockTop)
                        | WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom)));
            this.HideOnClose = true;
            this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockTopAutoHide;
            this.TabText = "Task List";


            addtaskForm.SaveClicked = (Dictionary<string,string> response, int _mode) =>
            {
                //new Task(() =>
                //{
                //    this.PerformSafely(() =>
                //    {
                //        loader.ShowDialog(this);
                //    });
                //}).Start();

                new Task(() =>
                {
                    try
                    {
                        //this.PerformSafely(() =>
                        //{
                        //    loader.Hide();
                        //});

                        if (IsAlreadyExists(response, _mode))
                        {
                            addtaskForm.InfoMessage = "Already exists";
                            addtaskForm.SetPanelVisible(true);
                        }
                        else
                        {
                            if (AddTaskList(response, _mode))
                            {
                                addtaskForm.PerformSafely(() => { addtaskForm.Hide(); });
                                if (_mode == 1)
                                {
                                    this.Alert("New task list added successfully, to add the tasks please click on 'Add Task' button", Form_Alert.enmType.Success);
                                    //code to bind the drop down and selected it
                                    cmbTaskList.ComboBox.PerformSafely(() =>
                                    {
                                        _taskConfig = GetTaskListFromFile();
                                        if (_taskConfig != null)
                                        {
                                            BindComboBox();
                                            cmbTaskList.ComboBox.SelectedValue = response["list_name"];
                                            //cmbTaskList_SelectedIndexChanged(null, null);
                                        }
                                    });
                                }
                                else
                                {
                                    this.Alert("New task added successfully", Form_Alert.enmType.Success);
                                    cmbTaskList_SelectedIndexChanged(null, null);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        this.Alert(ex.Message, Form_Alert.enmType.Error);
                        //this.PerformSafely(() =>
                        //{
                        //    loader.Hide();
                        //});
                    }
                }).Start();
            };
        }

        enum mode
        {
            hide,
            unhide
        }

        bool IsAlreadyExists(Dictionary<string, string> diTaskListName, int mode)
        {
            var taskList = GetTaskListFromFile();
            if (diTaskListName.Count > 0 && taskList !=null)
            {
                if (mode == 1)
                {
                    var data = taskList.task_list.Where(m => m.list_name == diTaskListName["list_name"].ToString()).Select(m => m.list_name).ToList();
                    if(data!=null && data.Count > 0)
                    {
                        return true;
                    }
                }
                else if (mode == 2)
                {
                    string selectedVal = string.Empty;
                    cmbTaskList.ComboBox.PerformSafely(() =>
                    {
                        selectedVal = cmbTaskList.ComboBox.SelectedValue.ToString();
                    });
                    var data = taskList.task_list.Where(m => m.list_name == selectedVal.ToString()).Select(m => m.tasks).ToList();
                    if (data != null && data.Count > 0)
                    {
                        foreach (var item in data[0])
                        {
                            if (item.title == diTaskListName["title"])
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        public void Alert(string msg, Form_Alert.enmType type)
        {
            Task t = new Task(() => {
                this.PerformSafely(() => {
                    Form_Alert frm = new Form_Alert();
                    frm.BringToFront();
                    frm.showAlert(msg, type, this);
                });
            });
            t.Start();
        }

        public TaskConfig GetTaskListFromFile()
        {
            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, string.Empty);
            }

            var text = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<TaskConfig>(text);
        }

        private void HidePanel(mode _mode)
        {
            panel3.PerformSafely(() =>
            {
                int pnl2X = panel2.Location.X;
                int pnl2Y = panel2.Location.Y;

                int pnl3X = panel3.Location.X;
                int pnl3Y = panel3.Location.Y;

                if (_mode == mode.hide)
                {
                    panel3.Location = panel2.Location;
                    panel3.Height += panel2.Height;

                    cmbTaskList.ComboBox.PerformSafely(() => {
                        cmbTaskList.ComboBox.SelectedValue = "-1";
                    });

                    this.PerformSafely(() => {
                        btnDeleteTaskList.Visible = false;
                    });
                }
                else
                {
                    if (!lAlreadySet)
                    {
                        panel3.Location = new Point(0, pnl3Y + 25);
                        panel3.Height -= panel2.Height;
                    }
                }
            });
        }

        void BindGrid(string selectedVal)
        {
            dgvList.PerformSafely(() =>
            {
                dgvList.DataSource = null;
                dgvList.Rows.Clear();
                dgvList.Columns.Clear();
            });

            DataTable tData = new DataTable();
            tData.Columns.Add("Task", typeof(string));
            tData.Columns.Add("Description", typeof(string));
            tData.Columns.Add("CreateDate", typeof(string));
            tData.Columns.Add("DueDate", typeof(DateTime));
            tData.Columns.Add("completed", typeof(bool));

            var data = (from s in _taskConfig.task_list where s.list_name == selectedVal select s.tasks).ToList();
            if (data != null && data.Count > 0)
            {
                foreach (var item in data[0])
                {
                    DataRow dr = tData.NewRow();
                    dr[0] = item.title;
                    dr[1] = item.description;
                    dr[2] = item.creation_date;
                    dr[3] = item.due_date;
                    dr[4] = item.completed;
                    tData.Rows.Add(dr);
                }

                dgvList.PerformSafely(() =>
                {
                    DataView dv = tData.DefaultView;
                    dv.Sort = "DueDate asc";
                    DataTable sortedDT = dv.ToTable();

                    dgvList.DataSource = tData;

                    dgvList.Columns[4].Visible = false;

                    DataGridViewImageColumn comCol = new DataGridViewImageColumn();
                    Image img1 = Properties.Resources.markascomplete;
                    comCol.Image = img1;
                    dgvList.Columns.Add(comCol);
                    comCol.HeaderText = "Complete";
                    comCol.Name = "Complete";
                    comCol.ToolTipText = "Complete";
                    comCol.ImageLayout = DataGridViewImageCellLayout.Zoom;

                    DataGridViewImageColumn delCol = new DataGridViewImageColumn();
                    Image img2 = Properties.Resources.delete;
                    delCol.Image = img2;
                    dgvList.Columns.Add(delCol);
                    delCol.HeaderText = "Delete";
                    delCol.Name = "Delete";
                    delCol.ToolTipText = "Delete";
                    delCol.ImageLayout = DataGridViewImageCellLayout.Zoom;

                    //dgvList.Columns[0].Width = 300;
                    //dgvList.Columns[1].Width = 700;
                    //dgvList.Columns[2].Width = 100;
                    //dgvList.Columns[3].Width = 100;
                    //dgvList.Columns[5].Width = 70;
                    //dgvList.Columns[6].Width = 70;


                    for (int i = 0; i < dgvList.Rows.Count; i++)
                    {
                        dgvList.Rows[i].Cells[0].ReadOnly = true;
                        dgvList.Rows[i].Cells[1].ReadOnly = true;
                        dgvList.Rows[i].Cells[2].ReadOnly = true;
                        dgvList.Rows[i].Cells[3].ReadOnly = true;
                        dgvList.Rows[i].Cells[4].ReadOnly = true;

                        bool isCompleted = Convert.ToBoolean(dgvList.Rows[i].Cells["completed"].Value.ToString().Trim());
                        DateTime dtDueDate = Convert.ToDateTime(dgvList.Rows[i].Cells["DueDate"].Value.ToString().Trim());
                        if (isCompleted)
                        {
                            dgvList.Rows[i].Cells[0].Style.BackColor =
                            dgvList.Rows[i].Cells[1].Style.BackColor =
                            dgvList.Rows[i].Cells[2].Style.BackColor =
                            dgvList.Rows[i].Cells[3].Style.BackColor =
                            dgvList.Rows[i].Cells[4].Style.BackColor =
                            dgvList.Rows[i].Cells[5].Style.BackColor =
                            dgvList.Rows[i].Cells[6].Style.BackColor = Color.FromArgb(204, 255, 204);
                            ((DataGridViewImageCell)dgvList.Rows[i].Cells[5]).Value = Properties.Resources.markcompleted;
                            ((DataGridViewImageCell)dgvList.Rows[i].Cells[5]).ToolTipText = "Reopen";
                        }
                        else
                        {
                            ((DataGridViewImageCell)dgvList.Rows[i].Cells[5]).Value = Properties.Resources.markascomplete;
                            ((DataGridViewImageCell)dgvList.Rows[i].Cells[5]).ToolTipText = "Mark as complete";
                        }

                        if (dtDueDate.Date < DateTime.Now.Date && !isCompleted)
                        {
                            dgvList.Rows[i].Cells[0].Style.BackColor =
                            dgvList.Rows[i].Cells[1].Style.BackColor =
                            dgvList.Rows[i].Cells[2].Style.BackColor =
                            dgvList.Rows[i].Cells[3].Style.BackColor =
                            dgvList.Rows[i].Cells[4].Style.BackColor =
                            dgvList.Rows[i].Cells[5].Style.BackColor =
                            dgvList.Rows[i].Cells[6].Style.BackColor = Color.FromArgb(255, 204, 204);
                            ((DataGridViewImageCell)dgvList.Rows[i].Cells[5]).Value = Properties.Resources.markascomplete;
                            ((DataGridViewImageCell)dgvList.Rows[i].Cells[5]).ToolTipText = "Mark as complete";
                        }

                        ((DataGridViewImageCell)dgvList.Rows[i].Cells[6]).ToolTipText = "Delete task";
                    }

                    if(dgvList.Rows!=null && dgvList.Rows.Count > 0)
                    {
                        if (Convert.ToBoolean(dgvList.Rows[0].Cells[4].Value) == true)
                            dgvList.DefaultCellStyle.SelectionBackColor = Color.FromArgb(204, 255, 204);
                        else
                            dgvList.DefaultCellStyle.SelectionBackColor = Color.Transparent;

                        dgvList.Focus();
                        dgvList.Rows[0].Selected = true;
                    }
                    else
                    {
                        dgvList.DefaultCellStyle.SelectionBackColor = Color.Transparent;
                    }
                    
                });
            }
        }

        void BindComboBox()
        {
            cmbTaskList.ComboBox.PerformSafely(() => {

                cmbTaskList.ComboBox.DataSource = null;

                DataTable tCombo = new DataTable();
                tCombo.Columns.Add("TextField", typeof(string));
                tCombo.Columns.Add("ValueField", typeof(string));

                DataRow drDefault = tCombo.NewRow();
                drDefault["TextField"] = "--select--";
                drDefault["ValueField"] = "-1";
                tCombo.Rows.Add(drDefault);

                //var ds = _taskConfig.task_list.Select(m => m.list_name + "(" + m.creation_date + "-" + m.due_date + ")").ToList();
                var ds = _taskConfig.task_list.ToList();
                if (ds != null && ds.Count > 0)
                {
                    foreach (var item in ds)
                    {
                        DataRow dr = tCombo.NewRow();
                        dr["TextField"] = item.list_name + " (" + Convert.ToDateTime(item.creation_date).ToString("dd/MM/yyyy") + "-" + Convert.ToDateTime(item.due_date).ToString("dd/MM/yyyy") + ")";
                        dr["ValueField"] = item.list_name;
                        tCombo.Rows.Add(dr);
                    }
                }
                
                cmbTaskList.ComboBox.ValueMember = "ValueField";
                cmbTaskList.ComboBox.DisplayMember = "TextField";
                cmbTaskList.ComboBox.DataSource = tCombo;
                cmbTaskList.ComboBox.SelectedIndex = 0;
            });
        }

        bool CompleteTask(string strTaskName, string strTaskmode)
        {
            if (!string.IsNullOrEmpty(strTaskName))
            {
                TaskConfig newTaskConfig = new TaskConfig();

                string selectedVal = cmbTaskList.ComboBox.SelectedValue.ToString();
                List<TaskList> updatedtaskList = new List<TaskList>();
                foreach(var item in _taskConfig.task_list)
                {
                    TaskList taskList = new TaskList();
                    taskList.list_name = item.list_name;
                    taskList.creation_date = item.creation_date;
                    taskList.due_date = item.due_date;

                    if (item.list_name == selectedVal)
                    {
                        List<Tasks> listtasks = new List<Tasks>();
                        foreach (var subitem in item.tasks)
                        {
                            if(subitem.title == strTaskName)
                            {
                                if(strTaskmode.ToLower() == "reopen")
                                    subitem.completed = false;
                                else
                                    subitem.completed = true;
                            }
                            listtasks.Add(subitem);
                        }
                        taskList.tasks = listtasks;
                    }
                    else
                    {
                        taskList.tasks = item.tasks;
                    }

                    updatedtaskList.Add(taskList);
                }

                newTaskConfig.task_list = updatedtaskList;

                // serialize JSON to a string and then write string to a file
                File.WriteAllText(filePath, JsonConvert.SerializeObject(newTaskConfig));
                BindGrid(selectedVal);
                _taskConfig = GetTaskListFromFile();
                cmbTaskList_SelectedIndexChanged(null, null);
                return true;
            }
            else
                return false;
        }

        bool DeleteTask(string strTaskName)
        {
            if (!string.IsNullOrEmpty(strTaskName))
            {
                TaskConfig newTaskConfig = new TaskConfig();

                string selectedVal = cmbTaskList.ComboBox.SelectedValue.ToString();
                List<TaskList> updatedtaskList = new List<TaskList>();
                foreach (var item in _taskConfig.task_list)
                {
                    TaskList taskList = new TaskList();
                    taskList.list_name = item.list_name;
                    taskList.creation_date = item.creation_date;
                    taskList.due_date = item.due_date;

                    if (item.list_name == selectedVal)
                    {
                        List<Tasks> listtasks = new List<Tasks>();
                        foreach (var subitem in item.tasks)
                        {
                            if (subitem.title != strTaskName)
                                listtasks.Add(subitem);
                            else
                                continue;
                        }
                        taskList.tasks = listtasks;
                    }
                    else
                    {
                        taskList.tasks = item.tasks;
                    }

                    updatedtaskList.Add(taskList);
                }

                newTaskConfig.task_list = updatedtaskList;

                // serialize JSON to a string and then write string to a file
                File.WriteAllText(filePath, JsonConvert.SerializeObject(newTaskConfig));
                BindGrid(selectedVal);
                _taskConfig = GetTaskListFromFile();
                cmbTaskList_SelectedIndexChanged(null, null);
                return true;
            }
            else
                return false;
        }

        bool AddTaskList(Dictionary<string,string> diTaskListName, int mode)
        {
            if (diTaskListName.Count > 0)
            {
                TaskConfig newTaskConfig = new TaskConfig();
                List<TaskList> updatedtaskList = new List<TaskList>();

                if (mode == 1)
                {
                    if(_taskConfig!=null)
                        updatedtaskList = _taskConfig.task_list;

                    TaskList taskList = new TaskList();
                    taskList.list_name = diTaskListName["list_name"];
                    taskList.creation_date = diTaskListName["creation_date"];
                    taskList.due_date = diTaskListName["due_date"];
                    taskList.tasks = new List<Tasks>();
                    updatedtaskList.Add(taskList);
                }
                else if(mode == 2)
                {
                    cmbTaskList.ComboBox.PerformSafely(() => {
                        string selectedVal = cmbTaskList.ComboBox.SelectedValue.ToString();
                        foreach (var item in _taskConfig.task_list)
                        {
                            TaskList taskList = new TaskList();
                            taskList.list_name = item.list_name;
                            taskList.creation_date = item.creation_date;
                            taskList.due_date = item.due_date;
                            if (item.list_name == selectedVal)
                            {
                                List<Tasks> listtasks = new List<Tasks>();
                                foreach (var subitem in item.tasks)
                                    listtasks.Add(subitem);

                                Tasks newAddedTask = new Tasks()
                                {
                                    title = diTaskListName["title"],
                                    description = diTaskListName["description"],
                                    creation_date = diTaskListName["creation_date"],
                                    due_date = diTaskListName["due_date"],
                                    completed = false
                                };

                                listtasks.Add(newAddedTask);
                                taskList.tasks = listtasks;
                            }
                            else
                            {
                                taskList.tasks = item.tasks;
                            }

                            updatedtaskList.Add(taskList);
                        }
                    });
                }

                newTaskConfig.task_list = updatedtaskList;

                File.WriteAllText(filePath, JsonConvert.SerializeObject(newTaskConfig));
                //BindGrid(selectedVal);
                _taskConfig = GetTaskListFromFile();
                //cmbTaskList_SelectedIndexChanged(null, null);
                return true;
            }
            else
                return false;
        }

        bool DeleteTaskList(string strTaskListName)
        {
            if (!string.IsNullOrEmpty(strTaskListName))
            {
                TaskConfig newTaskConfig = new TaskConfig();

                List<TaskList> updatedtaskList = new List<TaskList>();
                foreach (var item in _taskConfig.task_list)
                {
                    if (item.list_name == strTaskListName)
                    {
                        continue;
                    }
                    else
                    {
                        TaskList taskList = new TaskList();
                        taskList.list_name = item.list_name;
                        taskList.creation_date = item.creation_date;
                        taskList.due_date = item.due_date;
                        taskList.tasks = item.tasks;
                        updatedtaskList.Add(taskList);
                    }
                }

                newTaskConfig.task_list = updatedtaskList;

                // serialize JSON to a string and then write string to a file
                File.WriteAllText(filePath, JsonConvert.SerializeObject(newTaskConfig));
                _taskConfig = GetTaskListFromFile();
                BindComboBox();
                cmbTaskList_SelectedIndexChanged(null, null);
                return true;
            }
            else
                return false;
        }

        private void cmbTaskList_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            new Task(() =>
            {
                int index = -1;
                this.PerformSafely(() =>
                {
                    index = cmbTaskList.SelectedIndex;
                });

                if (index > 0)
                {
                    //new Task(() =>
                    //{
                    //    this.PerformSafely(() =>
                    //    {
                    //        loader.ShowDialog(this);
                    //    });
                    //}).Start();

                    this.PerformSafely(() => {
                        btnDeleteTaskList.Visible = true;
                    });

                    HidePanel(mode.unhide);
                    lAlreadySet = true;

                    string selectedVal = string.Empty;
                    this.PerformSafely(() =>
                    {
                        selectedVal = cmbTaskList.ComboBox.SelectedValue.ToString();
                        index = cmbTaskList.SelectedIndex;
                    });

                    BindGrid(selectedVal);

                    ////new Task(() =>
                    ////{
                    ////    //code to bind the list view
                    ////    //_taskConfig

                    ////    BindGrid(selectedVal);

                    ////    //this.PerformSafely(() =>
                    ////    //{
                    ////    //    loader.Hide();
                    ////    //});
                    ////}).Start();
                }
                else
                {
                    this.PerformSafely(() => {
                        btnDeleteTaskList.Visible = false;
                    });

                    dgvList.PerformSafely(() =>
                    {
                        dgvList.DataSource = null;
                        dgvList.Rows.Clear();
                        dgvList.Columns.Clear();
                    });

                    lAlreadySet = false;
                    HidePanel(mode.hide);
                }
            }).Start();
        }

        private void DummyTaskList_Shown(object sender, System.EventArgs e)
        {
            HidePanel(mode.hide);

            _taskConfig = GetTaskListFromFile();
            if(_taskConfig != null)
            {
                //var ds = _taskConfig.task_list.Select(m => m.list_name + "(" + m.creation_date + "-" + m.due_date + ")").ToList();
                //ds.Insert(0, "-Select-");
                //cmbTaskList.ComboBox.DataSource = ds;
                BindComboBox();
            }
        }

        private void dgvList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //var senderGrid = (DataGridView)sender;
                //if (senderGrid.Columns[e.ColumnIndex] is DataGridViewImageCell &&
                //    e.RowIndex >= 0)
                //{
                //    //TODO - Button Clicked - Execute Code Here
                //    DataGridViewImageCell cell = (DataGridViewImageCell)dgvList.CurrentCell;
                //    string currentTaskName = dgvList.Rows[e.RowIndex].Cells[0].Value.ToString().Trim();

                //    if (cell.ColumnIndex == 5)
                //    {
                //        //code to complete the selected task
                //        if (CompleteTask(currentTaskName))
                //        {
                //            Image img = Properties.Resources.delete;
                //            cell.ImageLayout = DataGridViewImageCellLayout.Zoom;
                //            cell.Value = "Reopen";
                //            cell.Style.BackColor = Color.FromArgb(204, 255, 204);
                //            this.Alert("Task Completed", Form_Alert.enmType.Success);
                //        }
                //    }
                //    else if (cell.ColumnIndex == 6)
                //    {
                //        DialogResult dialogResult = MessageBox.Show("Are you sure want to delete the task ?",
                //            "Delete Task", MessageBoxButtons.YesNo);
                //        if (dialogResult == DialogResult.Yes)
                //        {
                //            //code to delete the selected task from list
                //            if (DeleteTask(currentTaskName))
                //            {
                //                this.Alert("Task Deleted", Form_Alert.enmType.Success);
                //            }
                //        }
                //    }
                //}

                var senderGrid = (DataGridView)sender;
                if (senderGrid.CurrentCell is DataGridViewImageCell &&
                    e.RowIndex >= 0)
                {
                    //TODO - Button Clicked - Execute Code Here
                    DataGridViewImageCell cell = (DataGridViewImageCell)dgvList.CurrentCell;
                    string currentTaskName = dgvList.Rows[e.RowIndex].Cells[0].Value.ToString().Trim();

                    if (cell.ColumnIndex == 5)
                    {
                        string s = cell.ToolTipText;
                        //code to complete the selected task
                        if (CompleteTask(currentTaskName, s))
                        {
                            if(s.ToLower() == "reopen")
                                this.Alert("Task Reopened", Form_Alert.enmType.Success);
                            else
                                this.Alert("Task Completed", Form_Alert.enmType.Success);
                        }
                    }
                    else if (cell.ColumnIndex == 6)
                    {
                        DialogResult dialogResult = MessageBox.Show("Are you sure want to delete the task ?",
                            "Delete Task", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.Yes)
                        {
                            //code to delete the selected task from list
                            if (DeleteTask(currentTaskName))
                            {
                                this.Alert("Task Deleted", Form_Alert.enmType.Success);
                            }
                        }
                    }

                    this.Refresh();
                }
            }
            catch (Exception)
            {
            }
        }

        private void btnAddNewTaskList_Click(object sender, EventArgs e)
        {
            //code to add new task list with blank entries
            Task t = new Task(() =>
            {
                try
                {
                    addtaskForm.PerformSafely(() =>
                    {
                        addtaskForm.mode = 1;
                    });
                    this.PerformSafely(() =>
                    {
                        addtaskForm.ShowDialog(this);
                    });
                }
                catch (Exception ex)
                {
                    this.Alert(ex.Message, Form_Alert.enmType.Error);
                }
            });
            t.Start();
        }

        private void btnAddNewTask_Click(object sender, EventArgs e)
        {
            //code to add the task
            Task t = new Task(() =>
            {
                try
                {
                    addtaskForm.PerformSafely(() =>
                    {
                        addtaskForm.mode = 2;
                    });
                    this.PerformSafely(() =>
                    {
                        addtaskForm.ShowDialog(this);
                    });
                }
                catch (Exception ex)
                {
                    this.Alert(ex.Message, Form_Alert.enmType.Error);
                }
            });
            t.Start();
        }

        private void btnDeleteTaskList_Click(object sender, EventArgs e)
        {
            new Task(() =>
            {
                int index = -1;
                this.PerformSafely(() =>
                {
                    index = cmbTaskList.SelectedIndex;
                });

                if (index > 0)
                {
                    string selectedVal = string.Empty;
                    this.PerformSafely(() =>
                    {
                        selectedVal = cmbTaskList.ComboBox.SelectedValue.ToString();
                    });
                    DialogResult dialogResult = MessageBox.Show("All the task will also deleted, Are you sure want to delete the task list ?",
                           "Delete Task", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        if (DeleteTaskList(selectedVal))
                        {
                            this.Alert("Task list deleted", Form_Alert.enmType.Success);
                            HidePanel(mode.hide);
                        }
                    }
                }
            }).Start();
        }

        private void dgvList_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                var rowsCount = dgvList.SelectedRows.Count;
                if (rowsCount == 0 || rowsCount > 1) return;

                var row = dgvList.SelectedRows[0];
                if (row == null) return;

                if (!string.IsNullOrEmpty(row.Cells[4].Value.ToString()))
                {
                    if (Convert.ToBoolean(row.Cells[4].Value))
                        dgvList.DefaultCellStyle.SelectionBackColor = Color.FromArgb(204, 255, 204);
                    else
                        dgvList.DefaultCellStyle.SelectionBackColor = Color.Transparent;
                }
                else
                {
                    dgvList.DefaultCellStyle.SelectionBackColor = Color.Transparent;
                }
            }
            catch (Exception)
            {
            }
        }

    }
}