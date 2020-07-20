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
        public ConfigurationForm()
        {
            InitializeComponent();
        }

        public ConfigurationForm(string msg, Action loadComp)
        {
            InitializeComponent();
            lblMsg.Text = msg;
            loadCompleted = loadComp;
            this.Activated += ConfigurationForm_Activated;
            
            dataGridView1.CellClick += DataGridView1_CellClick;
            dataGridView2.CellClick += DataGridView2_CellClick;
            config = new StudioConfig(AppDomain.CurrentDomain.BaseDirectory);
            
        }

        private async void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Task t = new Task(() =>
            {
                if (e.ColumnIndex == 4)
                {
                    dataGridView1.PerformSafely(() => {
                        var connName = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                        var selectedConn = config.databaseConnections.First(c => c.ConnName == connName);

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
                if (e.ColumnIndex == 2)
                {
                    dataGridView2.PerformSafely(() => {
                        var connName = dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString();
                        var selectedConn = config.projectInfoList.First(c => c.ProjectName == connName);

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

                loader.PerformSafely(() =>
                {
                    loader.Hide();
                    loader.Close();
                });
            });
            t.Start();
            loader.ShowDialog(this);
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
                    if (textBox1.Text.Trim().Length == 0)
                    {
                        MessageBox.Show("IPAddress not filled");
                        HideLoader();
                        return;
                    }
                    if (textBox2.Text.Trim().Length == 0)
                    {
                        MessageBox.Show("User name not filled");
                        HideLoader();
                        return;
                    }
                    if (textBox3.Text.Trim().Length == 0)
                    {
                        MessageBox.Show("Password not filled");
                        HideLoader();
                        return;
                    }
                    if (textBox5.Text.Trim().Length == 0)
                    {
                        MessageBox.Show("Service Url not filled");
                        HideLoader();
                        return;
                    }

                    if (textBox4.Text.Trim().Length == 0)
                    {
                        MessageBox.Show("Terminal url not filled");
                        HideLoader();
                        return;
                    }

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

                    config.sSHClientInfo = new SSHClientInfo()
                    {
                        IPAddress = textBox1.Text.Trim(),
                        UserName = textBox2.Text.Trim(),
                        Password = textBox3.Text.Trim()
                    };
                    config.projectInfo = new ProjectInfo()
                    {
                        ProjectPath = textBox6.Text.Trim(),
                        KockpitServiceUrl = textBox5.Text.Trim()
                    };
                    config.terminalInfo = new TerminalInfo()
                    {
                        Url = textBox4.Text.Trim()
                    };

                    config.projectInfoList.ForEach(c =>
                    {
                        c.KockpitServiceUrl = config.projectInfo.KockpitServiceUrl;
                    });

                    config.otherServices.AirflowService = txtAirflow.Text.Trim();
                    config.otherServices.HealthCheckService = txtHealthCheck.Text.Trim();

                    var studioConfig = config.SaveAndLoadConfig();

                    HideLoader();
                    this.PerformSafely(() =>
                    {
                        
                        MainForm frm = new MainForm(studioConfig, ()=> {
                            this.Hide();
                            this.Close();
                        });
                        frm.ShowDialog();
                        
                    });
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
        private void button2_Click(object sender, EventArgs e)
        {
            loader.Close();
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
                        var dataSource = config.databaseConnections.Select(c => new { ConnName = c.ConnName, DbType = c.DbType, Server = c.ServerName, DbName = c.DbName }).ToList();
                        dataGridView1.DataSource = dataSource;

                        DataGridViewLinkColumn Deletelink = new DataGridViewLinkColumn();
                        Deletelink.UseColumnTextForLinkValue = true;
                        Deletelink.HeaderText = "Delete Action";
                        Deletelink.DataPropertyName = "lnkColumn";
                        Deletelink.LinkBehavior = LinkBehavior.SystemDefault;
                        Deletelink.Text = "Delete";
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
                        var dataSource = config.projectInfoList.Select(c => new { ProjectName = c.ProjectName, Location = c.ProjectPath}).ToList();
                        dataGridView2.DataSource = dataSource;

                        DataGridViewLinkColumn Deletelink = new DataGridViewLinkColumn();
                        Deletelink.UseColumnTextForLinkValue = true;
                        Deletelink.HeaderText = "Delete Action";
                        Deletelink.DataPropertyName = "lnkColumn";
                        Deletelink.LinkBehavior = LinkBehavior.SystemDefault;
                        Deletelink.Text = "Delete";
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
            Task t = new Task(() =>
            {
                try
                {
                    //var configData = config.GetStudioConfigFromFile();

                    if (config.projectInfoList == null)
                    {
                        config.projectInfoList = new List<ProjectInfo>();
                    }

                    if (config.projectInfoList.Any(c => c.ProjectName.ToLower().Equals(txtProName.Text.ToLower())))
                    {
                        MessageBox.Show("Project already exists!");
                    }
                    else
                    {

                        config.projectInfoList.Add(new ProjectInfo()
                        {
                            ProjectName = txtProName.Text.Trim(),
                            ProjectPath = txtProLocation.Text.Trim()
                        });

                        txtProName.PerformSafely(() =>
                        {
                            txtProName.Text = string.Empty;
                        });
                        txtProLocation.PerformSafely(() =>
                        {
                            txtProLocation.Text = string.Empty;
                        });
                       
                        ShowProjects();
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
    }
}
