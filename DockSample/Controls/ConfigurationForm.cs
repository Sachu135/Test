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
            groupBox7.Enabled = groupBox8.Enabled = groupBox9.Enabled = false;
            if (config.IsConfigExist())
            {
                config = config.GetStudioConfigFromFile();
                ShowProjects();
                ShowConnections();
            }
        }

        private async void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Task t = new Task(() =>
            {
                if (dataGridView1.CurrentCell is DataGridViewLinkCell)
                {
                    DataGridViewLinkCell cell = (DataGridViewLinkCell)dataGridView1.CurrentCell;
                    dataGridView1.PerformSafely(() => {
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
                    loader.Close();
                });
            });
            t.Start();
            loader.ShowDialog(this);
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
                                txtExplorerServiceUrl.PerformSafely(() =>
                                {
                                    txtExplorerServiceUrl.Text = selectedConn.KockpitServiceUrl;
                                });
                                txtTerminalUrl.PerformSafely(() =>
                                {
                                    txtTerminalUrl.Text = selectedConn.terminalInfo.Url;
                                });

                                txtAirflow.PerformSafely(() =>
                                {
                                    txtAirflow.Text = selectedConn.otherServices.AirflowService;
                                });
                                txtHealthCheck.PerformSafely(() =>
                                {
                                    txtHealthCheck.Text = selectedConn.otherServices.HealthCheckService;
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
                    throw;
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
                        });
                        txtConnPassword.PerformSafely(() =>
                        {
                            txtConnPassword.Text = string.Empty;
                        });

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
            if (cbConnDbType.SelectedItem.ToString() == "Json" || cbConnDbType.SelectedItem.ToString() == "Xml" || cbConnDbType.SelectedItem.ToString() == "Streaming Data" || cbConnDbType.SelectedItem.ToString() == "Cloud Storage")
            {
                txtDbName.Text = txtConnUserName.Text = txtConnPassword.Text = string.Empty;
                txtDbName.Enabled = txtConnUserName.Enabled = txtConnPassword.Enabled = false;
            }
            else
            {
                txtDbName.Enabled = txtConnUserName.Enabled = txtConnPassword.Enabled = true;
            }

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

                    if (string.IsNullOrEmpty(txtProName.Text.Trim()))
                    {
                        MessageBox.Show("Project name cannot be blank!");
                    }
                    else if (string.IsNullOrEmpty(serverType))
                    {
                        MessageBox.Show("Please select Server type!");
                    }
                    else if (findIndex > -1 && findIndex != editIndex)
                    {
                        MessageBox.Show("Project already exists!");
                    }
                    else
                    {
                        if (txtProLocation.Text.Trim().Length == 0)
                        {
                            MessageBox.Show("Location path not filled");
                            HideLoader();
                            return;
                        }

                        ProjectInfo proInfo = new ProjectInfo();
                        proInfo.sSHClientInfo = new SSHClientInfo();
                        proInfo.ProjectName = txtProName.Text.Trim();
                        if (serverType.Equals("Linux"))
                        {
                            if (txtExplorerServiceUrl.Text.Trim().Length == 0)
                            {
                                MessageBox.Show("Service Url not filled");
                                HideLoader();
                                return;
                            }

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
                            

                            if (txtTerminalUrl.Text.Trim().Length == 0)
                            {
                                MessageBox.Show("Terminal url not filled");
                                HideLoader();
                                return;
                            }

                            proInfo.sSHClientInfo = new SSHClientInfo()
                            {
                                IPAddress = txtSshIP.Text.Trim(),
                                UserName = txtSshUser.Text.Trim(),
                                Password = txtSshPass.Text.Trim()
                            };
                            proInfo.terminalInfo = new TerminalInfo()
                            {
                                Url = txtTerminalUrl.Text.Trim()
                            };

                            proInfo.KockpitServiceUrl = txtExplorerServiceUrl.Text.Trim();
                            proInfo.otherServices.AirflowService = txtAirflow.Text;
                            proInfo.otherServices.HealthCheckService = txtHealthCheck.Text;
                        }

                        proInfo.ProjectPath = txtProLocation.Text.Trim();

                        proInfo.IsWindows = serverType == "Windows" ? true : false;

                        if (editIndex > -1)
                        {
                            config.projectInfoList.RemoveAt(editIndex);
                        }

                        if (proInfo.IsWindows && editIndex == -1)
                        {
                            using (HardwareConfigForm form = new HardwareConfigForm())
                            {
                                this.PerformSafely(() =>
                                {
                                    form.ShowDialog(this);
                                });
                            }
                        }
                        editIndex = -1;
                        config.projectInfoList.Add(proInfo);
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
            txtExplorerServiceUrl.PerformSafely(() =>
            {
                txtExplorerServiceUrl.Text = string.Empty;
            });
            txtTerminalUrl.PerformSafely(() =>
            {
                txtTerminalUrl.Text = string.Empty;
            });
            txtAirflow.PerformSafely(() =>
            {
                txtAirflow.Text = string.Empty;
            });
            txtHealthCheck.PerformSafely(() =>
            {
                txtHealthCheck.Text = string.Empty;
            });
        }
        private async void button4_Click(object sender, EventArgs e)
        {
            Task t = new Task(() =>
            {
                try
                {
                    Thread.Sleep(2000);
                    loader.PerformSafely(() =>
                    {
                        loader.Hide();
                        loader.Close();
                    });
                    MessageBox.Show("Connection tested Successfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            });
            t.Start();

            loader.ShowDialog(this);
        }

        private async void txtConnServerName_TextChanged(object sender, EventArgs e)
        {
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
                groupBox7.Enabled = groupBox8.Enabled = groupBox9.Enabled = false;
            }
            else if (cmbServerType.SelectedIndex == 0)
            {
                groupBox7.Enabled = true;
                groupBox8.Enabled = groupBox9.Enabled = lblServiceUrl.Enabled = txtExplorerServiceUrl.Enabled = false;
            }
            else if (cmbServerType.SelectedIndex == 1)
            {
                groupBox7.Enabled = true;
                lblServiceUrl.Enabled = txtExplorerServiceUrl.Enabled = groupBox8.Enabled = groupBox9.Enabled = true;
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
                groupBox7.Enabled = groupBox8.Enabled = groupBox9.Enabled = false;
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
    }
}
