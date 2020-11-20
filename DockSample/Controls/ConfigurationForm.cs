using CustomControls;
using DockSample.lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UIFunctionality.Common;

namespace DockSample.Controls
{
    public partial class ConfigurationForm : Form
    {
        StudioConfig config;
        UCLoaderForm loader;
        Action loadCompleted;
        int editIndex = -1;
        int editDbIndex = -1;
        //bool closingByProgram = false;
        public ConfigurationForm()
        {
            InitializeComponent();
        }

        private class ProjectInfoGridInfo
        {
            public ProjectInfoGridInfo() { }
            public string ProjectName { get; set; }
            public string Location { get; set; }
        }

        private class DatabaseConnectionInfoGridInfo
        {
            public DatabaseConnectionInfoGridInfo() { }
            public string ConnName { get; set; }
            public string DbType { get; set; }
            public string Server { get; set; }
            public string DbName { get; set; }
        }
        public ConfigurationForm(string msg, Action loadComp)
        {
            InitializeComponent();
            lblMsg.Text = msg;
            loadCompleted = loadComp;
            this.Activated += ConfigurationForm_Activated;

            //dataGridView1.AutoGenerateColumns = false;
            //dataGridView2.AutoGenerateColumns = false;
            dataGridView1.CellContentClick += DataGridView1_CellClick;
            dataGridView2.CellContentClick += DataGridView2_CellClick;
            config = new StudioConfig(AppDomain.CurrentDomain.BaseDirectory);
            groupBox7.Enabled = groupBox8.Enabled = groupBox1.Enabled = false;
            if (config.IsConfigExist())
            {
                config = config.GetStudioConfigFromFile();
                ShowProjects();
                ShowConnections();
            }
        }

        private async void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            ////Task t = new Task(() =>
            ////{
            ////    if (dataGridView1.CurrentCell is DataGridViewLinkCell)
            ////    {
            ////        DataGridViewLinkCell cell = (DataGridViewLinkCell)dataGridView1.CurrentCell;
            ////        dataGridView1.PerformSafely(() => {
            ////            var gridRow = (DatabaseConnectionInfoGridInfo)dataGridView1.Rows[e.RowIndex].DataBoundItem;
            ////            DatabaseConnectionInfo selectedConn = config.databaseConnections.First(c => c.ConnName.ToLower().ToString() == gridRow.ConnName.ToLower().ToString());

            ////            var confirmResult = MessageBox.Show("Are you sure to delete connection ??",
            ////                        "Confirm Delete!!",
            ////                        MessageBoxButtons.YesNo);
            ////            if (confirmResult == DialogResult.Yes)
            ////            {
            ////                config.databaseConnections.Remove(selectedConn);
            ////                ShowConnections();
            ////            }
            ////            else
            ////            {
            ////                // If 'No', do something here.
            ////            }
            ////        });
            ////    }

            ////    loader.PerformSafely(() =>
            ////    {
            ////        loader.Hide();
            ////        //loader.Close();
            ////    });
            ////});
            ////t.Start();
            ////loader.ShowDialog(this);


            if (dataGridView1.CurrentCell is DataGridViewLinkCell)
            {
                Task t = new Task(() =>
                {
                    DataGridViewLinkCell cell = (DataGridViewLinkCell)dataGridView1.CurrentCell;
                    if (cell.Value == "Edit")
                    {
                        dataGridView1.PerformSafely(() =>
                        {
                            var gridRow = (DatabaseConnectionInfoGridInfo)dataGridView1.Rows[e.RowIndex].DataBoundItem;
                            DatabaseConnectionInfo selectedConn = config.databaseConnections.First(c => c.ConnName.ToLower().ToString() == gridRow.ConnName.ToLower().ToString());

                            txtConnKey.PerformSafely(() => {
                                txtConnKey.Text = selectedConn.ConnName;
                            });

                            cbConnDbType.PerformSafely(() =>
                            {
                                cbConnDbType.SelectedItem = selectedConn.DbType;
                            });

                            txtConnServerName.PerformSafely(() =>
                            {
                                txtConnServerName.Text = selectedConn.ServerName;
                            });

                            txtDbName.PerformSafely(() => {
                                txtDbName.Text = selectedConn.DbName;

                                if (selectedConn.DbType.ToString() == "Json"
                                || selectedConn.DbType.ToString() == "Xml"
                                || selectedConn.DbType.ToString() == "Streaming Data"
                                || selectedConn.DbType.ToString() == "Cloud Storage")
                                {
                                    txtDbName.Enabled = false;
                                }
                            });

                            txtConnUserName.PerformSafely(() => {
                                txtConnUserName.Text = selectedConn.UserName;
                                if (selectedConn.DbType == "Sql Server")
                                {
                                    if (string.IsNullOrEmpty(selectedConn.UserName) && string.IsNullOrEmpty(selectedConn.Password))
                                        txtConnUserName.Enabled = false;
                                    else
                                        txtConnUserName.Enabled = true;
                                }
                                else if (selectedConn.DbType.ToString() == "Json"
                                || selectedConn.DbType.ToString() == "Xml"
                                || selectedConn.DbType.ToString() == "Streaming Data"
                                || selectedConn.DbType.ToString() == "Cloud Storage")
                                {
                                    txtConnUserName.Enabled = false;
                                }
                                else
                                {
                                    txtConnUserName.Enabled = true;
                                }
                            });

                            txtConnPassword.PerformSafely(() => {
                                txtConnPassword.Text = selectedConn.Password;
                                if (selectedConn.DbType == "Sql Server")
                                {
                                    if (string.IsNullOrEmpty(selectedConn.UserName) && string.IsNullOrEmpty(selectedConn.Password))
                                        txtConnPassword.Enabled = false;
                                    else
                                        txtConnPassword.Enabled = true;
                                }
                                else if (selectedConn.DbType.ToString() == "Json" 
                                || selectedConn.DbType.ToString() == "Xml" 
                                || selectedConn.DbType.ToString() == "Streaming Data" 
                                || selectedConn.DbType.ToString() == "Cloud Storage")
                                {
                                    txtConnPassword.Enabled = false;
                                }
                                else
                                {
                                    txtConnPassword.Enabled = true;
                                }
                            });

                            cmbServerMode.PerformSafely(() =>
                            {
                                cmbServerMode.Visible = (selectedConn.DbType == "Sql Server");
                                if (string.IsNullOrEmpty(selectedConn.UserName) && string.IsNullOrEmpty(selectedConn.Password))
                                    cmbServerMode.SelectedIndex = 0;
                                else
                                    cmbServerMode.SelectedIndex = 1;
                            });

                            editDbIndex = config.databaseConnections.FindIndex(c => c.ConnName.ToLower() == selectedConn.ConnName.ToLower());

                        });

                        btnSaveConnection.PerformSafely(() =>
                        {
                            btnSaveConnection.Visible = false;
                        });
                        button7.PerformSafely(() =>
                        {
                            button7.Visible = true;
                        });
                        button8.PerformSafely(() =>
                        {
                            button8.Visible = true;
                        });
                    }
                    else if (cell.Value == "Delete")
                    {
                        dataGridView1.PerformSafely(() =>
                        {
                            var gridRow = (DatabaseConnectionInfoGridInfo)dataGridView1.Rows[e.RowIndex].DataBoundItem;
                            DatabaseConnectionInfo selectedConn = config.databaseConnections.First(c => c.ConnName.ToLower().ToString() == gridRow.ConnName.ToLower().ToString());

                            var confirmResult = MessageBox.Show("Are you sure to delete connection ??",
                                        "Confirm Delete!!",
                                        MessageBoxButtons.YesNo);
                            if (confirmResult == DialogResult.Yes)
                            {
                                config.databaseConnections.Remove(selectedConn);
                                ShowConnections();
                            }
                            else
                            {
                                // If 'No', do something here.
                            }
                        });
                    }
                    

                    loader.PerformSafely(() =>
                    {
                        loader.Hide();
                        //loader.Close();
                    });
                });
                t.Start();
                loader.ShowDialog(this);
            }
        }

        private async void DataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Task t = new Task(() =>
            {
                if (dataGridView2.CurrentCell is DataGridViewLinkCell)
                {
                    DataGridViewLinkCell cell = (DataGridViewLinkCell)dataGridView2.CurrentCell;
                    if (cell.Value == "Edit")
                    {
                        dataGridView2.PerformSafely(() => {

                            var gridRow = dataGridView2.Rows[e.RowIndex].DataBoundItem as ProjectInfoGridInfo;
                            var selectedConn = config.projectInfoList.First(c => c.ProjectName.Trim().ToLower() == gridRow.ProjectName.Trim().ToLower());
                            //cmbServerType.SelectedIndexChanged -= cmbServerType_SelectedIndexChanged;
                            cmbServerType.SelectedIndex = selectedConn.IsWindows ? 0 : 1;
                            ServerTypeIndexChanges();
                            //cmbServerType.SelectedIndexChanged += cmbServerType_SelectedIndexChanged;
                            cmbServerType.Enabled = false;
                            ClearProjControls();

                            txtProName.PerformSafely(() =>
                            {
                                txtProName.Text = selectedConn.ProjectName;
                            });
                            txtProLocation.PerformSafely(() =>
                            {
                                txtProLocation.Text = selectedConn.ProjectPath;
                            });
                            txtGitUrl.PerformSafely(() => {
                                txtGitUrl.Text = selectedConn.GitRepoURL;
                            });
                            txtGitUsername.PerformSafely(() => {
                                txtGitUsername.Text = selectedConn.GitUsername;
                            });
                            txtGitUrl.PerformSafely(() => {
                                txtGitPassword.Text = selectedConn.GitPassword;
                            });
                            txtGitEmail.PerformSafely(() => {
                                txtGitEmail.Text = selectedConn.GitEmail;
                            });

                            if (!selectedConn.IsWindows)
                            {
                                txtSshIP.PerformSafely(() =>
                                {
                                    txtSshIP.Text = selectedConn.sSHClientInfo.IPAddress;
                                });
                                txtSshUser.PerformSafely(() =>
                                {
                                    txtSshUser.Text = selectedConn.sSHClientInfo.UserName;
                                });
                                txtSshPass.PerformSafely(() =>
                                {
                                    txtSshPass.Text = selectedConn.sSHClientInfo.Password;
                                });
                                
                            }
                            dataGridView2.Enabled = false;
                            editIndex = config.projectInfoList.FindIndex(c => c.ProjectName.ToLower() == selectedConn.ProjectName.ToLower());
                            //ValidateProject(editIndex);
                            //
                            //cmbServerType.SelectedIndex = -1;
                        });
                        button3.PerformSafely(() =>
                        {
                            button3.Visible = false;
                        });
                        button5.PerformSafely(() =>
                        {
                            button5.Visible = true;
                        });
                        button6.PerformSafely(() =>
                        {
                            button6.Visible = true;
                        });
                    }
                    else if (cell.Value == "Delete")
                    {
                        dataGridView2.PerformSafely(() => {
                            var gridRow = dataGridView2.Rows[e.RowIndex].DataBoundItem as ProjectInfoGridInfo;
                            var selectedConn = config.projectInfoList.First(c => c.ProjectName.Trim().ToLower() == gridRow.ProjectName.Trim().ToLower());

                            var confirmResult = MessageBox.Show("Are you sure to delete Project Info ??",
                                        "Confirm Delete!!",
                                        MessageBoxButtons.YesNo);
                            if (confirmResult == DialogResult.Yes)
                            {
                                config.projectInfoList.Remove(selectedConn);
                                ShowProjects();
                            }
                            else
                            {
                                // If 'No', do something here.
                            }
                        });
                    }
                }
                

                //this.PerformSafely(() =>
                //{
                //    loader.Hide();
                //    loader.Close();
                //});
            });
            t.Start();
            //loader.ShowDialog(this);
        }

        private void ConfigurationForm_Activated(object sender, EventArgs e)
        {
            if (loadCompleted != null)
            {
                loadCompleted();
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ConfigurationForm_Load(object sender, EventArgs e)
        {
            loader = new UCLoaderForm();
            ShowConnections();


            //txtProName.Text = "Linux Project";
            //txtProLocation.Text = "/home/";
            //txtSshIP.Text = "192.168.0.104";
            //txtSshUser.Text = "root";
            //txtSshPass.Text = "root";
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            Task t = new Task(() =>
            {
                try
                {
                    if (config.projectInfoList == null)
                    {
                        MessageBox.Show("Add atleast 1 project information");
                        HideLoader();
                        return;
                    }
                    if (config.projectInfoList.Count == 0)
                    {
                        MessageBox.Show("Add atleast 1 project information");
                        HideLoader();
                        return;
                    }

                    var studioConfig = config.SaveAndLoadConfig();

                    HideLoader();
                    Task t1 = new Task(() =>
                    {
                        this.PerformSafely(() =>
                        {
                            MainForm frm = new MainForm(studioConfig, () => {
                                this.FormClosing -= ConfigurationForm_FormClosing;
                                this.Hide();
                                this.Close();
                            });
                            frm.ShowDialog();
                        });
                        
                    });
                    t1.Start();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error:" + ex.Message);
                    HideLoader();
                }
                
            });
            t.Start();
            loader.ShowDialog(this);
        }

        private void HideLoader()
        {
            loader.PerformSafely(() =>
            {
                loader.Hide();
                loader.Close();
            });
        }
        private async void button2_Click(object sender, EventArgs e)
        {
            Task t1 = new Task(() =>
            {
                if (config.IsConfigExist())
                {
                    config = config.GetStudioConfigFromFile();
                    this.PerformSafely(() =>
                    {
                        MainForm frm = new MainForm(config, () =>
                        {
                        
                            this.FormClosing -= ConfigurationForm_FormClosing;
                            this.Hide();
                            this.Close();
                        });
                        frm.ShowDialog();
                    });
                }
                else
                {
                    Environment.Exit(0);
                }
                
            });
            t1.Start();
        }

        private async void btnSaveConnection_Click(object sender, EventArgs e)
        {
            Task t = new Task(() =>
            {
                try
                {
                    //var configData = config.GetStudioConfigFromFile();

                    if (config.databaseConnections == null)
                    {
                        config.databaseConnections = new List<DatabaseConnectionInfo>();
                    }

                    if (config.databaseConnections.Any(c => c.ConnName.ToLower().Equals(txtConnKey.Text.ToLower())))
                    {
                        MessageBox.Show("Connection exists!");
                    }
                    else
                    {
                        cbConnDbType.PerformSafely(() =>
                        {
                            //var ddd = cbConnDbType.SelectedValue.ToString();
                        });

                        cbConnDbType.PerformSafely(() =>
                        {
                            config.databaseConnections.Add(new DatabaseConnectionInfo()
                            {
                                ConnName = txtConnKey.Text.Trim(),
                                DbType = cbConnDbType.SelectedItem.ToString().Trim(),
                                ServerName = txtConnServerName.Text.Trim(),
                                DbName = txtDbName.Text.Trim(),
                                UserName = txtConnUserName.Text.Trim(),
                                Password = txtConnPassword.Text.Trim()
                            });
                        });

                        ClearDatabaseControls();

                        //config.SaveConfigWithoutValidate();
                        ShowConnections();
                    }

                    loader.PerformSafely(() =>
                    {
                        loader.Hide();
                        loader.Close();
                    });
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            });
            t.Start();
            
            loader.ShowDialog(this);
        }

        void ShowConnections()
        {
            dataGridView1.PerformSafely(() =>
            {
                dataGridView1.DataSource = null;
                dataGridView1.Rows.Clear();
                dataGridView1.Columns.Clear();

                if (config.databaseConnections != null)
                {
                    if (config.databaseConnections.Count > 0)
                    {
                        var dataSource = config.databaseConnections.Select(c => new DatabaseConnectionInfoGridInfo { ConnName = c.ConnName, DbType = c.DbType, Server = c.ServerName, DbName = c.DbName }).ToList();
                        dataGridView1.DataSource = dataSource;

                        DataGridViewLinkColumn Editlink = new DataGridViewLinkColumn();
                        Editlink.UseColumnTextForLinkValue = true;
                        Editlink.HeaderText = "Edit Action";
                        Editlink.DataPropertyName = "lnkColumn";
                        Editlink.LinkBehavior = LinkBehavior.SystemDefault;
                        Editlink.Text = "Edit";
                        Editlink.Tag = "Edit";
                        dataGridView1.Columns.Add(Editlink);

                        DataGridViewLinkColumn Deletelink = new DataGridViewLinkColumn();
                        Deletelink.UseColumnTextForLinkValue = true;
                        Deletelink.HeaderText = "Delete Action";
                        Deletelink.DataPropertyName = "lnkColumn";
                        Deletelink.LinkBehavior = LinkBehavior.SystemDefault;
                        Deletelink.Text = "Delete";
                        Deletelink.Tag = "Delete";
                        dataGridView1.Columns.Add(Deletelink);
                    }
                }
            });

        }

        void ShowProjects()
        {
            dataGridView1.PerformSafely(() =>
            {
                dataGridView2.DataSource = null;
                dataGridView2.Rows.Clear();
                dataGridView2.Columns.Clear();

                if (config.projectInfoList != null)
                {
                    if (config.projectInfoList.Count > 0)
                    {
                        var dataSource = config.projectInfoList.Select(c => new ProjectInfoGridInfo { ProjectName = c.ProjectName, Location = c.ProjectPath}).ToList();
                        dataGridView2.DataSource = dataSource;

                        //DataGridViewColumn ProjectName = new DataGridViewColumn();
                        //ProjectName.HeaderText = "Project Name";
                        //ProjectName.DataPropertyName = "ProjectName";
                        //dataGridView2.Columns.Add(ProjectName);

                        DataGridViewLinkColumn EditLink = new DataGridViewLinkColumn();
                        EditLink.UseColumnTextForLinkValue = true;
                        EditLink.HeaderText = "Edit Action";
                        EditLink.DataPropertyName = "lnkEditColumn";
                        EditLink.LinkBehavior = LinkBehavior.SystemDefault;
                        EditLink.Text = "Edit";
                        EditLink.Tag = "Edit";
                        dataGridView2.Columns.Add(EditLink);

                        DataGridViewLinkColumn Deletelink = new DataGridViewLinkColumn();
                        Deletelink.UseColumnTextForLinkValue = true;
                        Deletelink.HeaderText = "Delete Action";
                        Deletelink.DataPropertyName = "lnkColumn";
                        Deletelink.LinkBehavior = LinkBehavior.SystemDefault;
                        Deletelink.Text = "Delete";
                        Deletelink.Tag = "Delete";
                        dataGridView2.Columns.Add(Deletelink);
                    }
                }
            });
        }

        private async void cbConnDbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cbConnDbType.SelectedIndex == -1)
            {
                return;
            }

            if (cbConnDbType.SelectedItem.ToString() == "Json" || cbConnDbType.SelectedItem.ToString() == "Xml" || cbConnDbType.SelectedItem.ToString() == "Streaming Data" || cbConnDbType.SelectedItem.ToString() == "Cloud Storage")
            {
                txtDbName.Text = txtConnUserName.Text = txtConnPassword.Text = string.Empty;
                txtDbName.Enabled = txtConnUserName.Enabled = txtConnPassword.Enabled = false;
            }
            else
            {
                txtDbName.Enabled = txtConnUserName.Enabled = txtConnPassword.Enabled = true;
            }

            lblAuthentication.Visible = cmbServerMode.Visible = (cbConnDbType.SelectedItem.ToString() == "Sql Server");

            if (cbConnDbType.SelectedItem.ToString() == "Cloud Storage")
            {
                label7.Text = "Provider :";
            }
            else
            {
                label7.Text = "Database :";
            }
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            ValidateProject();
        }

        void ValidateProject()
        {
            Task t = new Task(() =>
            {
                try
                {
                    if (config.projectInfoList == null)
                    {
                        config.projectInfoList = new List<ProjectInfo>();
                    }
                    var serverType = string.Empty;
                    cmbServerType.PerformSafely(() => {
                        serverType = cmbServerType.SelectedIndex >= 0 ? cmbServerType.SelectedItem.ToString() : string.Empty;
                    });

                    var findIndex = config.projectInfoList.FindIndex(c => c.ProjectName.ToLower().Equals(txtProName.Text.Trim().ToLower()));
                    var LocfindIndex = config.projectInfoList.FindIndex(c => c.DirectoryInfo.FullPath.ToLower().Equals(txtProLocation.Text.Trim().ToLower()));

                    if (string.IsNullOrEmpty(txtProName.Text.Trim()))
                    {
                        MessageBox.Show("Workspace name cannot be blank!");
                    }
                    else if (string.IsNullOrEmpty(serverType))
                    {
                        MessageBox.Show("Please select Server type!");
                    }
                    else if (findIndex > -1 && findIndex != editIndex)
                    {
                        MessageBox.Show("Project already exists!");
                    }
                    else if (LocfindIndex > -1 && LocfindIndex != editIndex)
                    {
                        MessageBox.Show("Project location already exists!");
                    }
                    //else if (!string.IsNullOrEmpty(txtGitUrl.Text) || !string.IsNullOrEmpty(txtGitUsername.Text) || !string.IsNullOrEmpty(txtGitPassword.Text))
                    //{
                    //    //GitRepositoryManager gitRepositoryManager = new GitRepositoryManager();
                    //    //bool lretval = gitRepositoryManager.IsValidRepoURL(txtGitUrl.Text.Trim());
                    //}
                    else
                    {
                        if (txtProLocation.Text.Trim().Length == 0)
                        {
                            MessageBox.Show("Location path not filled");
                            HideLoader();
                            return;
                        }

                        if(txtProLocation.Text.Trim().Length != 0)
                        {
                            //code to check the path exists or not
                            if(!Directory.Exists(txtProLocation.Text.Trim()) && serverType == "Windows")
                            {
                                MessageBox.Show("Location path does not exists..");
                                HideLoader();
                                return;
                            }
                        }

                        ProjectInfo proInfo = new ProjectInfo();
                        proInfo.sSHClientInfo = new SSHClientInfo();
                        proInfo.ProjectName = txtProName.Text.Trim();
                        proInfo.GitRepoURL = txtGitUrl.Text.Trim();
                        proInfo.GitUsername = txtGitUsername.Text.Trim();
                        proInfo.GitPassword = txtGitPassword.Text.Trim();
                        proInfo.GitEmail = txtGitEmail.Text.Trim();

                        if (serverType.Equals("Linux"))
                        {
                            if (txtSshIP.Text.Trim().Length == 0)
                            {
                                MessageBox.Show("IP Address not filled");
                                HideLoader();
                                return;
                            }
                            if (txtSshUser.Text.Trim().Length == 0)
                            {
                                MessageBox.Show("User name not filled");
                                HideLoader();
                                return;
                            }
                            if (txtSshPass.Text.Trim().Length == 0)
                            {
                                MessageBox.Show("Password not filled");
                                HideLoader();
                                return;
                            }

                            proInfo.sSHClientInfo = new SSHClientInfo()
                            {
                                IPAddress = txtSshIP.Text.Trim(),
                                UserName = txtSshUser.Text.Trim(),
                                Password = txtSshPass.Text.Trim()
                            };
                        }

                        proInfo.ProjectPath = txtProLocation.Text.Trim();

                        proInfo.IsWindows = serverType == "Windows" ? true : false;

                        if (editIndex > -1)
                        {
                            config.projectInfoList.RemoveAt(editIndex);
                        }

                        if (proInfo.IsWindows)
                        {
                            if(editIndex == -1)
                            {
                                using (HardwareConfigForm form = new HardwareConfigForm(proInfo.ProjectName))
                                {
                                    this.PerformSafely(() =>
                                    {
                                        DialogResult dresult = form.ShowDialog(this);
                                        if (dresult == DialogResult.OK)
                                        {
                                            editIndex = -1;
                                            config.projectInfoList.Add(proInfo);
                                        }
                                    });
                                }
                            }
                            else
                            {
                                editIndex = -1;
                                config.projectInfoList.Add(proInfo);
                            }
                        }
                        if(!proInfo.IsWindows)
                        {
                            string strServiceUrl = string.Format("http://{0}:5000/", proInfo.sSHClientInfo.IPAddress);
                            string strTerminalUrl = string.Format("http://{0}:5001/", proInfo.sSHClientInfo.IPAddress);
                            string strAirflowUrl = string.Format("http://{0}:5002/", proInfo.sSHClientInfo.IPAddress);
                            string strHealthCheckUrl = string.Format("http://{0}:5003/", proInfo.sSHClientInfo.IPAddress);
                            string strClusterSetupUrl = string.Format("http://{0}:8080/", proInfo.sSHClientInfo.IPAddress);
                            if (editIndex == -1)
                            {
                                using (LinuxConfigForm form = new LinuxConfigForm(proInfo))
                                {
                                    this.PerformSafely(() =>
                                    {
                                        DialogResult dresult = form.ShowDialog(this);
                                        if (dresult == DialogResult.OK)
                                        {
                                            editIndex = -1;
                                            proInfo.KockpitServiceUrl = strServiceUrl;
                                            proInfo.terminalInfo = new TerminalInfo() { Url = strTerminalUrl };
                                            proInfo.otherServices.AirflowService = strAirflowUrl;
                                            proInfo.otherServices.HealthCheckService = strHealthCheckUrl;
                                            proInfo.otherServices.ClusterSetupService = strClusterSetupUrl;
                                            config.projectInfoList.Add(proInfo);
                                        }
                                    });
                                }
                            }
                            else
                            {
                                proInfo.KockpitServiceUrl = strServiceUrl;
                                proInfo.terminalInfo = new TerminalInfo() { Url = strTerminalUrl };
                                proInfo.otherServices.AirflowService = strAirflowUrl;
                                proInfo.otherServices.HealthCheckService = strHealthCheckUrl;
                                proInfo.otherServices.ClusterSetupService = strClusterSetupUrl;
                                config.projectInfoList.Add(proInfo);
                            }
                        }

                        ShowProjects();
                        button6_Click(null, null);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            });
            t.Start();
        }
        void ClearProjControls()
        {
            txtProName.PerformSafely(() =>
            {
                txtProName.Text = string.Empty;
            });
            txtProLocation.PerformSafely(() =>
            {
                txtProLocation.Text = string.Empty;
            });
            txtSshIP.PerformSafely(() =>
            {
                txtSshIP.Text = string.Empty;
            });
            txtSshUser.PerformSafely(() =>
            {
                txtSshUser.Text = string.Empty;
            });
            txtSshPass.PerformSafely(() =>
            {
                txtSshPass.Text = string.Empty;
            });

            txtGitUrl.PerformSafely(() => {
                txtGitUrl.Text = string.Empty;
            });
            txtGitUsername.PerformSafely(() => {
                txtGitUsername.Text = string.Empty;
            });
            txtGitPassword.PerformSafely(() => {
                txtGitPassword.Text = string.Empty;
            });
            txtGitEmail.PerformSafely(() => {
                txtGitEmail.Text = string.Empty;
            });


        }

        void ClearDatabaseControls()
        {
            editDbIndex = -1;
            cbConnDbType.PerformSafely(() => {
                cbConnDbType.SelectedIndex = -1;
            });
            txtConnKey.PerformSafely(() =>
            {
                txtConnKey.Text = string.Empty;
            });
            txtConnServerName.PerformSafely(() =>
            {
                txtConnServerName.Text = string.Empty;
            });
            txtDbName.PerformSafely(() =>
            {
                txtDbName.Text = string.Empty;
            });
            txtConnUserName.PerformSafely(() =>
            {
                txtConnUserName.Text = string.Empty;
                txtConnUserName.Enabled = true;
            });
            txtConnPassword.PerformSafely(() =>
            {
                txtConnPassword.Text = string.Empty;
                txtConnPassword.Enabled = true;
            });
            cmbServerMode.PerformSafely(() => {
                cmbServerMode.Visible = false;
            });
            lblAuthentication.PerformSafely(() => {
                lblAuthentication.Visible = false;
            });

            btnSaveConnection.PerformSafely(() =>
            {
                btnSaveConnection.Visible = true;
            });
            button7.PerformSafely(() =>
            {
                button7.Visible = false;
            });
            button8.PerformSafely(() =>
            {
                button8.Visible = false;
            });

        }
        private async void button4_Click(object sender, EventArgs e)
        {
            Task t = new Task(() =>
            {
                try
                {
                    //Thread.Sleep(2000);
                    cbConnDbType.PerformSafely(() =>
                    {
                        IClientFactory clientFactory;
                        string connectString, connectDescription;

                        if (cbConnDbType.SelectedItem.ToString().Trim().Equals("Sql Server")) //SqlServer
                        {
                            clientFactory = new SqlFactory();
                            if (!string.IsNullOrEmpty(txtConnUserName.Text.Trim()) && !string.IsNullOrEmpty(txtConnPassword.Text.Trim()))
                                connectString = "Data Source=" + txtConnServerName.Text.Trim() + ";Initial Catalog=" + txtDbName.Text.Trim() + "; User ID=" + txtConnUserName.Text.Trim() + ";Password=" + txtConnPassword.Text.Trim();
                            else
                                connectString = "Data Source=" + txtConnServerName.Text.Trim() + ";Initial Catalog=" + txtDbName.Text.Trim() + "; Integrated Security=True";

                            connectDescription = txtConnServerName.Text.Trim() + " (" + txtConnUserName.Text.Trim() + ")";
                        }
                        else if (cbConnDbType.SelectedItem.ToString().Trim().Equals("Postgres")) //Postgres
                        {
                            clientFactory = new PostgresFactory();
                            connectString = "Host=" + txtConnServerName.Text.Trim() + ";Port=5432;Database=" + txtDbName.Text.Trim() + ";Username=" + txtConnUserName.Text.Trim() + ";Password=" + txtConnPassword.Text.Trim() + ";Integrated Security=False;;SSL Mode=Require;TrustServerCertificate=true;";
                            connectDescription = txtConnServerName.Text.Trim() + " (" + txtConnUserName.Text.Trim() + ")";
                        }
                        else  //MySql
                        {
                            clientFactory = new MySqlFactory();
                            connectString = "server=" + txtConnServerName.Text.Trim() + ";port=3306;database=" + txtDbName.Text.Trim() + ";uid=" + txtConnUserName.Text.Trim() + ";password=" + txtConnPassword.Text.Trim();
                            connectDescription = txtConnServerName.Text.Trim() + " (" + txtConnUserName.Text.Trim() + ")";
                        }

                        DbClient dbClient = new DbClient(clientFactory, connectString, connectDescription);
                        bool success = dbClient.Connect();
                        if (success)
                            MessageBox.Show("Connection tested Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else
                            MessageBox.Show("Connection failed", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        loader.PerformSafely(() =>
                        {
                            loader.Hide();
                            loader.Close();
                        });
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    loader.PerformSafely(() =>
                    {
                        loader.Hide();
                        loader.Close();
                    });
                }

            });
            t.Start();

            loader.ShowDialog(this);
        }

        private async void txtConnServerName_TextChanged(object sender, EventArgs e)
        {
            if (cbConnDbType.SelectedIndex == -1)
                return;

            if (cbConnDbType.SelectedItem.ToString() == "Cloud Storage")
            {
                if (txtConnServerName.Text.Contains("googleapis"))
                {
                    txtDbName.Text = "Google Cloud";
                }
                else
                {
                    txtDbName.Text = string.Empty;
                }
            }
            

        }

        private void ConfigurationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!config.IsConfigExist())
            {
                Environment.Exit(0);
            }
        }

        private async void txtProName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.ToString() == "(" || e.KeyChar.ToString() == ")")
            {
                e.Handled = true;
            }
        }


        void ServerTypeIndexChanges()
        {
            if (cmbServerType.SelectedIndex == -1)
            {
                groupBox7.Enabled = groupBox8.Enabled = groupBox1.Enabled = false;
                btnDefault.Enabled = false;
                btnBrowse.Enabled = false;
                txtGitUrl.Enabled = false;
            }
            else if (cmbServerType.SelectedIndex == 0)
            {
                groupBox7.Enabled = true;
                groupBox1.Enabled = true;
                groupBox8.Enabled = false;
                btnDefault.Enabled = true;
                btnBrowse.Enabled = true;
                txtGitUrl.Enabled = true;
            }
            else if (cmbServerType.SelectedIndex == 1)
            {
                groupBox7.Enabled = true;
                groupBox1.Enabled = false;
                groupBox8.Enabled = true;
                btnDefault.Enabled = false;
                btnBrowse.Enabled = false;
                txtGitUrl.Enabled = false;
            }
        }
        private async void cmbServerType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ServerTypeIndexChanges();
        }

        private async void button5_Click(object sender, EventArgs e)
        {
            ValidateProject();
        }

        private async void button6_Click(object sender, EventArgs e)
        {
            ClearProjControls();
            editIndex = -1;
            cmbServerType.PerformSafely(() => {
                cmbServerType.SelectedIndex = -1;
                groupBox7.Enabled = groupBox8.Enabled = groupBox1.Enabled = false;
            });
            button3.PerformSafely(() =>
            {
                button3.Visible = true;
            });
            button5.PerformSafely(() =>
            {
                button5.Visible = false;
            });
            button6.PerformSafely(() =>
            {
                button6.Visible = false;
            });
            cmbServerType.PerformSafely(() =>
            {
                cmbServerType.Enabled = true;
            });
            dataGridView2.PerformSafely(() =>
            {
                dataGridView2.Enabled = true;
            });
        }

        private void btnDefault_Click(object sender, EventArgs e)
        {
            try
            {
                //get the type of server
                string serverType = string.Empty;
                cmbServerType.PerformSafely(() => {
                    serverType = cmbServerType.SelectedIndex >= 0 
                    ? cmbServerType.SelectedItem.ToString() : string.Empty;
                });

                if (!string.IsNullOrEmpty(serverType))
                {
                    if (serverType == "Windows")
                    {
                        ///case when windows
                        ///on drop down change of server type if windows then enable the default button
                        ///on default button click set the Location as : C:\KockpitStudio\ETLJobs
                        ///after click on Add New button create the directory inside KockputStudio name as ETLJobs
                        ///
                        var windowsFolderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Windows);
                        var windowsDrive = windowsFolderPath.Substring(0, windowsFolderPath.IndexOf(System.IO.Path.VolumeSeparatorChar));
                        var windowsDriveInfo = new System.IO.DriveInfo(windowsDrive);
                        //code to create the directory for KockpitStudio
                        string _KockPitDirectory = windowsDriveInfo + "KockpitStudio";
                        string _ETLJobsDir = Path.Combine(_KockPitDirectory, "ETLJobs");

                        if (!Directory.Exists(_KockPitDirectory))
                            Directory.CreateDirectory(_KockPitDirectory);

                        //Directory for ETLJobs
                        if (!Directory.Exists(_ETLJobsDir))
                            Directory.CreateDirectory(_ETLJobsDir);

                        txtProLocation.Text = _ETLJobsDir;
                    }

                    if(serverType == "Linux")
                    {
                        ///case when linux
                        ///after filled the ssh client info then enable the default button
                        ///on default click get the username
                        ///set the location as : /home/username/KockpitStudio
                        ///after click on Add New button show the confirmation message of OK and Cancel button. 
                        string strUsername = txtSshUser.Text.Trim();
                        if (!string.IsNullOrEmpty(strUsername))
                            txtProLocation.Text = string.Format("/home/{0}/KockpitStudio", strUsername);
                        else
                            MessageBox.Show("Username cannot be blank..");
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        private void enableSetDefault()
        {
            //code to enable the set default button
            btnDefault.Enabled = (!string.IsNullOrEmpty(txtSshIP.Text.Trim())
                && !string.IsNullOrEmpty(txtSshUser.Text.Trim())
                && !string.IsNullOrEmpty(txtSshPass.Text.Trim())) ? true : false;
        }

        private void txtSshPass_Validated(object sender, EventArgs e)
        {
            enableSetDefault();
        }

        private void txtSshUser_Validated(object sender, EventArgs e)
        {
            enableSetDefault();
        }

        private void txtSshIP_Validated(object sender, EventArgs e)
        {
            enableSetDefault();
        }


        private void btnBrowse_Click(object sender, EventArgs e)
        {
            //code to browse for the location if server type  is windows
            //get the type of server
            string serverType = string.Empty;
            cmbServerType.PerformSafely(() => {
                serverType = cmbServerType.SelectedIndex >= 0
                ? cmbServerType.SelectedItem.ToString() : string.Empty;
            });

            if (!string.IsNullOrEmpty(serverType) && serverType == "Windows")
            {
                string folderPath = "";
                FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    folderPath = folderBrowserDialog1.SelectedPath;
                }

                txtProLocation.Text = folderPath;
            }
        }

        private void cmbServerMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cmbServerMode.SelectedIndex == 0)
            {
                txtConnUserName.Enabled = txtConnPassword.Enabled = false;
                txtConnUserName.Text = txtConnPassword.Text = string.Empty;
            }
            else
            {
                txtConnUserName.Enabled = txtConnPassword.Enabled = true;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Task t = new Task(() =>
            {
                try
                {
                    if (config.databaseConnections == null)
                    {
                        config.databaseConnections = new List<DatabaseConnectionInfo>();
                    }

                    var findIndex = config.databaseConnections.FindIndex(c => c.ConnName.ToLower().Equals(txtConnKey.Text.Trim().ToLower()));
                    if (findIndex > -1 && findIndex != editDbIndex)
                    {
                        MessageBox.Show("Connection already exists!");
                    }
                    else
                    {
                        if (editDbIndex > -1)
                        {
                            config.databaseConnections.RemoveAt(editDbIndex);
                        }

                        cbConnDbType.PerformSafely(() =>
                        {
                            config.databaseConnections.Add(new DatabaseConnectionInfo()
                            {
                                ConnName = txtConnKey.Text.Trim(),
                                DbType = cbConnDbType.SelectedItem.ToString().Trim(),
                                ServerName = txtConnServerName.Text.Trim(),
                                DbName = txtDbName.Text.Trim(),
                                UserName = txtConnUserName.Text.Trim(),
                                Password = txtConnPassword.Text.Trim()
                            });
                        });

                        ClearDatabaseControls();

                        //config.SaveConfigWithoutValidate();
                        ShowConnections();
                    }

                    loader.PerformSafely(() =>
                    {
                        loader.Hide();
                        loader.Close();
                    });

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            });
            t.Start();

            loader.ShowDialog(this);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ClearDatabaseControls();
        }
    }
}
