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
using UIFunctionality;
using UIFunctionality.Common;

namespace DockSample
{
    public partial class LinuxConfigForm : Form
    {
        private string strWebServiceDir = "/etc/KockpitStudio/WebService";
        private ProjectInfo _projectInfo;
        SSHManager sshManager = new SSHManager();

        public LinuxConfigForm(ProjectInfo projectInfo)
        {
            _projectInfo = projectInfo;
            InitializeComponent();
        }

        #region [Create Directory]
        private bool CreateDirectory()
        {
            try
            {
                string dirPath = "/etc/KockpitStudio";

                if (!sshManager.DirectoryOrFileExists(_projectInfo.sSHClientInfo.IPAddress,
                    _projectInfo.sSHClientInfo.UserName,
                    _projectInfo.sSHClientInfo.Password, dirPath))
                {
                    sshManager.CreateDirectory(_projectInfo.sSHClientInfo.IPAddress,
                    _projectInfo.sSHClientInfo.UserName,
                    _projectInfo.sSHClientInfo.Password, dirPath, false);

                    sshManager.CreateDirectory(_projectInfo.sSHClientInfo.IPAddress,
                        _projectInfo.sSHClientInfo.UserName,
                        _projectInfo.sSHClientInfo.Password, strWebServiceDir, false);
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
                    string[] filePaths = Directory.GetFiles(_ResourcePath);
                    if (filePaths != null && filePaths.Count() > 0)
                    {
                        foreach (var item in filePaths)
                        {
                            Task t = new Task(() =>
                            {
                                string contents = File.ReadAllText(item);
                                var filePath = strWebServiceDir + "//" + Path.GetFileName(item);
                                sshManager.WriteFileContent(_projectInfo.sSHClientInfo.IPAddress,
                                    _projectInfo.sSHClientInfo.UserName,
                                    _projectInfo.sSHClientInfo.Password, filePath, contents);
                            });
                            t.Start();
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
                    //sudo apt update; sudo apt install snapd; sudo snap install dotnet-runtime - 31; sudo snap install dotnet-sdk--classic; sudo snap alias dotnet-sdk.dotnet dotnet
                    //string strCmd = @"sudo apt update; sudo apt install snapd; sudo snap install dotnet-runtime - 31; sudo snap install dotnet-sdk--classic; sudo snap alias dotnet-sdk.dotnet dotnet";
                    string strCmd = @"sudo apt update";
                    
                    sshManager.ExecuteCommandOnConsole(_projectInfo.sSHClientInfo.IPAddress,
                        _projectInfo.sSHClientInfo.UserName,
                        _projectInfo.sSHClientInfo.Password,
                        strCmd,
                        () => {
                            this.PerformSafely(() =>
                            {
                                this.DialogResult = DialogResult.OK;
                                this.Close();
                            });
                        });
                });
                t.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.PerformSafely(() =>
                {
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                });
            }
        }
        #endregion

        private void LinuxConfigForm_Shown(object sender, EventArgs e)
        {
            //code to create the directory
            //code to run the scripts
            if (CreateDirectory())
            {
                //code to copy the KockpitWebServices Files
                CopyServices();
                RunScript();
            }
            else
            {
                this.PerformSafely(() =>
                {
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                });
            }
        }
    }
}
