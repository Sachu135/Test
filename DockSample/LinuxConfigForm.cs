using CefSharp.WinForms;
using CustomControls;
using DockSample.lib;
using Renci.SshNet.Security;
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
using UIFunctionality;
using UIFunctionality.Common;

namespace DockSample
{
    public partial class LinuxConfigForm : Form
    {
        private string strPackagesDir = "/etc/KockpitStudio/Packages";
        private string strKockpitDir = "/etc/KockpitStudio";
        
        private ProjectInfo _projectInfo;
        SSHManager sshManager = new SSHManager();

        private string Url;
        private ChromiumWebBrowser browser;
        private UCLoaderForm loader;

        public LinuxConfigForm(ProjectInfo projectInfo)
        {
            _projectInfo = projectInfo;
            InitializeComponent();
            loader = new UCLoaderForm();
            Url = string.Format("http://{0}:5001/", _projectInfo.sSHClientInfo.IPAddress);
            browser = new ChromiumWebBrowser(Url);
        }

        #region [Create Directory]
        private bool CreateDirectory(string dirPath)
        {
            try
            {
                if (!sshManager.DirectoryOrFileExists(_projectInfo.sSHClientInfo.IPAddress,
                    _projectInfo.sSHClientInfo.UserName,
                    _projectInfo.sSHClientInfo.Password, dirPath))
                {
                    sshManager.CreateDirectory(_projectInfo.sSHClientInfo.IPAddress,
                    _projectInfo.sSHClientInfo.UserName,
                    _projectInfo.sSHClientInfo.Password, dirPath, false);
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        #endregion

        #region [Copy the files]
        void CopyServices()
        {
            try
            {
                //code to get the files and copy to server
                string _ResourcePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Resources\Linux\");
                if (Directory.Exists(_ResourcePath))
                {
                    string[] folderPaths = Directory.GetDirectories(_ResourcePath);
                    if (folderPaths != null && folderPaths.Count() > 0)
                    {
                        foreach(var item in folderPaths)
                        {
                            var folderName = strPackagesDir + "/" + Path.GetFileName(item);

                            if (CreateDirectory(folderName))
                            {
                                string[] filePaths = Directory.GetFiles(item);

                                if (filePaths != null && filePaths.Count() > 0)
                                {
                                    richTextBox1.PerformSafely(() => {
                                        richTextBox1.AppendText(string.Format("Copying {0} files to server...", filePaths.Count()));
                                        richTextBox1.AppendText(Environment.NewLine);
                                        richTextBox1.AppendText(Environment.NewLine);
                                    });
                                    List<Task> ts = new List<Task>();
                                    for (int i = 0; i < filePaths.Count(); i++)
                                    {
                                        var _idx = i + 1;
                                        var _flPath = filePaths[i];
                                        Action<int, string> action = (idx, flPath) =>
                                        {
                                            richTextBox1.PerformSafely(() =>
                                            {
                                                richTextBox1.AppendText(string.Format("File:{0} [{1}] start copying...", idx, Path.GetFileName(flPath)));
                                                richTextBox1.AppendText(Environment.NewLine);
                                            });
                                            string contents = File.ReadAllText(flPath);
                                            var filePath = folderName + "/" + Path.GetFileName(flPath);
                                            sshManager.WriteFileContent(_projectInfo.sSHClientInfo.IPAddress,
                                                _projectInfo.sSHClientInfo.UserName,
                                                _projectInfo.sSHClientInfo.Password, filePath, contents);
                                            richTextBox1.PerformSafely(() =>
                                            {
                                                richTextBox1.AppendText(string.Format("File:{0} [{1}] copied successfully...", idx, Path.GetFileName(flPath)));
                                                richTextBox1.AppendText(Environment.NewLine);
                                            });
                                        };

                                        Task t = new Task(() => action(_idx, _flPath));
                                        t.Start();
                                        ts.Add(t);
                                    }
                                    Task.WaitAll(ts.ToArray());
                                    richTextBox1.PerformSafely(() =>
                                    {
                                        richTextBox1.AppendText(string.Format("All Files copied successfully..."));
                                        richTextBox1.AppendText(Environment.NewLine);
                                    });
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region [Run Scripts]
        void RunScript()
        {
            try
            {
                Task t = new Task(() => {

                    Dictionary<string, string> diCmds = new Dictionary<string, string>();
                    diCmds.Add(@"sudo apt update;sudo apt install snapd;sudo snap install dotnet-runtime-31;sudo snap install dotnet-sdk --classic;sudo snap alias dotnet-sdk.dotnet dotnet;", "####Kockpit Signal Abort####");
                    diCmds.Add(@"sudo apt-get install supervisor -y;", "task2");
                    diCmds.Add(@"sudo add-apt-repository main; sudo add-apt-repository universe; sudo add-apt-repository restricted; sudo add-apt-repository multiverse; mkdir /etc/ttyd; sudo apt-get install build-essential cmake git libjson-c-dev libwebsockets-dev -y; git clone https://github.com/tsl0922/ttyd.git; cd ttyd; mkdir build; cd build; cmake ..; sudo make; sudo make install;", "task3");
                    diCmds.Add(@"sudo echo -e '[program:ttyd]\ncommand=ttyd -p 5001 bash\nautostart=true\nautorestart=true' > /etc/supervisor/conf.d/ttyd.conf;", "####Kockpit Signal Abort####");
                    diCmds.Add(@"sudo echo -e '[program:KockpitLinuxInstallation]\ncommand=ttyd -p 5002 dotnet /etc/KockpitStudio/Packages/Installer/LinuxInstaller.dll\nautostart=true\nautorestart=true' > /etc/supervisor/conf.d/KockpitLinuxInstallation.conf;", "####Kockpit Signal Abort####");
                    diCmds.Add(@"sudo service supervisor stop;sudo service supervisor start;", "####Kockpit Signal Abort####");

                    //List<string> strCmds = new List<string>();
                    //strCmds.Add(@"sudo apt update;sudo apt install snapd;sudo snap install dotnet-runtime-31;sudo snap install dotnet-sdk --classic;sudo snap alias dotnet-sdk.dotnet dotnet;");
                    //strCmds.Add(@"sudo apt-get install supervisor -y;");
                    //strCmds.Add(@"sudo add-apt-repository main; sudo add-apt-repository universe; sudo add-apt-repository restricted; sudo add-apt-repository multiverse; mkdir /etc/ttyd; sudo apt-get install build-essential cmake git libjson-c-dev libwebsockets-dev -y; git clone https://github.com/tsl0922/ttyd.git; cd ttyd; mkdir build; cd build; cmake ..; sudo make; sudo make install;");
                    //strCmds.Add(@"sudo echo -e '[program:ttyd]\ncommand=ttyd -p 5001 bash\nautostart=true\nautorestart=true' > /etc/supervisor/conf.d/ttyd.conf;");
                    //strCmds.Add(@"sudo echo -e '[program:KockpitLinuxInstallation]\ncommand=ttyd -p 5002 dotnet /etc/KockpitStudio/Packages/Installer/LinuxInstaller.dll\nautostart=true\nautorestart=true' > /etc/supervisor/conf.d/KockpitLinuxInstallation.conf;");
                    //strCmds.Add(@"sudo service supervisor stop;sudo service supervisor start;");

                    int nTaskIndex = 0;

                    lblTaskStatus.PerformSafely(() => {
                        lblTaskStatus.Text = string.Format("Tasks completed: {0} (out of {1})", nTaskIndex, diCmds.Count());
                    });
                    foreach (var cmd in diCmds)
                    {
                        CancellationTokenSource tokenSource = new CancellationTokenSource();
                        CancellationToken token = tokenSource.Token;
                        Task t1 = Task.Run(() =>
                        {
                            nTaskIndex += 1;
                            sshManager.ExecuteTeminalCommand(_projectInfo.sSHClientInfo.IPAddress,
                           _projectInfo.sSHClientInfo.UserName,
                           _projectInfo.sSHClientInfo.Password,
                           cmd, richTextBox1,
                           () =>
                           {
                               tokenSource.Cancel();
                           });
                            while (!token.IsCancellationRequested)
                            {
                                Thread.Sleep(1000);
                            }
                            if (token.IsCancellationRequested)
                            {
                                //var d = 0;
                                richTextBox1.PerformSafely(() => {
                                    richTextBox1.AppendText(string.Format("Tasks completed: {0} (out of {1})", nTaskIndex, diCmds.Count()));
                                    richTextBox1.AppendText(Environment.NewLine + "---------------------------------------------");
                                    richTextBox1.AppendText(Environment.NewLine);
                                });

                                lblTaskStatus.PerformSafely(() => {
                                    lblTaskStatus.Text = string.Format("Tasks completed: {0} (out of {1})", nTaskIndex, diCmds.Count());
                                });

                                if(nTaskIndex == diCmds.Count())
                                {
                                    MessageBox.Show("System initialization has been completed. " + Environment.NewLine + " Now, it will proceed for installation of essential services and packages.",
                                        "Success",
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                                    BrowseTerminal();
                                }
                            }
                        }, token);
                        t1.Wait();
                    }
                });
                t.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //this.PerformSafely(() =>
                //{
                //    this.DialogResult = DialogResult.Cancel;
                //    this.Close();
                //});
            }
        }
        #endregion

        #region [Browse Terminal]
        void BrowseTerminal()
        {
            new Task(() =>
            {
                this.PerformSafely(() =>
                {
                    loader.ShowDialog(this);
                });
            }).Start();

            new Task(() =>
            {
                this.PerformSafely(() =>
                {
                    progressBar1.Visible = false;
                    label1.Visible = false;
                    lblTaskStatus.Visible = false;
                    richTextBox1.Visible = false;
                });

                pnlBrowse.PerformSafely(() =>
                {
                    browser.FrameLoadStart += Browser_FrameLoadStart;
                    browser.FrameLoadEnd += Browser_FrameLoadEnd;
                    pnlBrowse.Controls.Add(browser);
                    pnlBrowse.Dock = DockStyle.Fill;
                    pnlBrowse.Focus();
                });
            }).Start();
        }

        private async void Browser_FrameLoadEnd(object sender, CefSharp.FrameLoadEndEventArgs e)
        {
            new Task(() =>
            {
                loader.PerformSafely(() =>
                {
                    loader.Hide();
                });
            }).Start();
        }
        private async void Browser_FrameLoadStart(object sender, CefSharp.FrameLoadStartEventArgs e)
        {
            new Task(() =>
            {
                this.PerformSafely(() =>
                {
                    if (!loader.Visible)
                    {
                        loader.ShowDialog(this);
                    }
                });
            }).Start();
        }
        #endregion

        private void LinuxConfigForm_Shown(object sender, EventArgs e)
        {
            //code to create the directory
            //code to run the scripts
            Task t = new Task(() => 
            {
                if (CreateDirectory(strKockpitDir))
                {
                    if (CreateDirectory(strPackagesDir))
                    {
                        //code to copy the KockpitWebServices Files
                        CopyServices();
                        RunScript();
                    }
                }
                else
                {
                    this.PerformSafely(() =>
                    {
                        this.DialogResult = DialogResult.Cancel;
                        this.Close();
                    });
                }
            });
            t.Start();
        }
    }
}
