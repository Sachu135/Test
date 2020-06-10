using DockSample.lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
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
            config = new StudioConfig(AppDomain.CurrentDomain.BaseDirectory);
            
        }

        private async void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Task t = new Task(() =>
            {
                //throw new NotImplementedException();
                // here you can have column reference by using e.ColumnIndex
                //DataGridViewImageCell cell = (DataGridViewImageCell)
                //dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (e.ColumnIndex == 3)
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
                    var studioConfig = config.SaveAndLoadConfig();

                    loader.PerformSafely(() =>
                    {
                        loader.Hide();
                        loader.Close();
                    });
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
                        var dataSource = config.databaseConnections.Select(c => new { ConnName = c.ConnName, DbType = c.DbType, DbName = c.DbName }).ToList();
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
    }
}
