using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KockpitStudioCustomAction
{
    public partial class frmInstall : Form
    {
        public frmInstall()
        {
            InitializeComponent();
            Application.EnableVisualStyles();
            this.TopMost = true;
        }

        public void PerformSafely(Control target, Action action)
        {
            if (target.InvokeRequired)
            {
                target.Invoke(action);
            }
            else
            {
                action();
            }
        }

        private void SetText(string strMsg)
        {
            this.PerformSafely(richTextBox1, () => {
                richTextBox1.SelectionStart = richTextBox1.Text.Length;
                richTextBox1.SelectedText = strMsg + Environment.NewLine;
                richTextBox1.ScrollToCaret();
            });
        }

        private string CheckForEnvironmentVariables()
        {
            var name = "Path";
            var scope = EnvironmentVariableTarget.Machine; // or User
            var oldValue = Environment.GetEnvironmentVariable(name, scope);
            List<string> liPaths = oldValue.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries).ToList();

            string newValue = "";
            foreach (string s in liPaths)
            {
                string sPath = s.Replace(@"\\", @"\");
                if (Directory.Exists(sPath))
                    newValue += sPath + ";";
            }
            Environment.SetEnvironmentVariable(name, newValue, scope);
            return newValue;
        }

        private void btnNext_Click(object sender1, EventArgs e1)
        {
            this.DialogResult = DialogResult.Yes;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void frmInstall_Shown(object sender1, EventArgs e1)
        {
            Task t = new Task(() =>
            {
                int vExitCode = -1;
                #region [Check for Choco]
                //Stage 1
                SetText("Checking Prerequisites...");
                StringBuilder sbChoco = new StringBuilder();
                sbChoco.AppendLine(@"Set-ExecutionPolicy Bypass -Scope Process -Force");
                sbChoco.AppendLine(@"$testchoco = powershell choco -v");
                sbChoco.AppendLine(@"If(!($testchoco)) {");
                sbChoco.AppendLine(@"    Invoke-Expression((New-Object System.Net.WebClient).DownloadString('https://chocolatey.org/install.ps1'))");
                sbChoco.AppendLine(@"}");
                var processInfoChoco = new ProcessStartInfo("powershell.exe", @"& {" + sbChoco.ToString() + "}");
                processInfoChoco.CreateNoWindow = true;
                processInfoChoco.WindowStyle = ProcessWindowStyle.Hidden;
                processInfoChoco.UseShellExecute = false;
                processInfoChoco.RedirectStandardError = true;
                processInfoChoco.RedirectStandardOutput = true;
                processInfoChoco.Verb = "runas";
                var processChoco = Process.Start(processInfoChoco);
                processChoco.Exited += (object sender, EventArgs e) =>
                {
                    vExitCode = processChoco.ExitCode;
                    SetText("-----------Process Choco Checking/Installation Finished------------");
                };
                processChoco.OutputDataReceived += (object sender, DataReceivedEventArgs e) =>
                {
                    if (e.Data != null)
                    {
                        SetText(e.Data);
                    }
                };
                processChoco.BeginOutputReadLine();
                processChoco.ErrorDataReceived += (object sender, DataReceivedEventArgs e) =>
                {
                    if (e.Data != null)
                    {
                        vExitCode = 1;
                        SetText("Error: " + e.Data);
                    }
                };
                processChoco.BeginErrorReadLine();
                processChoco.WaitForExit();
                vExitCode = processChoco.ExitCode;
                processChoco.Close();

                if(vExitCode != 0)
                {
                    MessageBox.Show("Installation Failed");
                    this.DialogResult = DialogResult.Cancel;
                    this.PerformSafely(pictureBox1, () =>
                    {
                        pictureBox1.Visible = false;
                    });
                    this.PerformSafely(richTextBox1, () => 
                    {
                        richTextBox1.Dock = DockStyle.Fill;
                    });
                }
                else
                {
                    var oldValue = CheckForEnvironmentVariables();
                    List<string> liPaths = oldValue.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries).ToList();

                    var windowsFolderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Windows);
                    var windowsDrive = windowsFolderPath.Substring(0, windowsFolderPath.IndexOf(System.IO.Path.VolumeSeparatorChar));
                    var windowsDriveInfo = new System.IO.DriveInfo(windowsDrive);

                    ///code to check if environment variable not set for choco then set it
                    if (Directory.Exists(windowsDriveInfo + @"ProgramData\chocolatey\bin"))
                    {
                        SetText("Checking for Choco Environment Variable...");
                        bool lChocoPathExists = false;
                        foreach (var path in liPaths)
                        {
                            if (path.Contains(@"\ProgramData\chocolatey\bin"))
                            {
                                SetText("Choco Environment Variable exists.");
                                SetText("------Finished---------");
                                SetText("------Click on Next button---------");
                                lChocoPathExists = true;
                                break;
                            }
                        }

                        if (!lChocoPathExists)
                        {
                            SetText("Choco Environment Variable not exists.");
                            oldValue = oldValue + ";" + windowsDriveInfo + @"ProgramData\chocolatey\bin";
                            Environment.SetEnvironmentVariable("Path", oldValue, EnvironmentVariableTarget.Machine);
                            SetText("Setting the Choco Environment variable.");
                            SetText("------Finished---------");
                            SetText("------Click on Next button---------");
                        }

                        this.PerformSafely(btnNext, () =>
                        {
                            btnNext.Enabled = true;
                        });
                        this.PerformSafely(pictureBox1, () =>
                        {
                            pictureBox1.Visible = false;
                        });
                        this.PerformSafely(richTextBox1, () =>
                        {
                            richTextBox1.Dock = DockStyle.Fill;
                        });
                    }
                    else
                    {
                        MessageBox.Show("Installation Failed");
                        this.DialogResult = DialogResult.Cancel;
                        this.PerformSafely(pictureBox1, () =>
                        {
                            pictureBox1.Visible = false;
                        });
                        this.PerformSafely(richTextBox1, () =>
                        {
                            richTextBox1.Dock = DockStyle.Fill;
                        });
                    }
                }
                
                #endregion
            });
            t.Start();
        }
    }
}
