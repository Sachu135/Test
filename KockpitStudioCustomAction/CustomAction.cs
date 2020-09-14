using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Deployment.WindowsInstaller;

namespace KockpitStudioCustomAction
{
    public class CustomActions
    {
        [CustomAction]
        public static ActionResult CustomAction1(Session session)
        {
            //session.Log("Begin CustomAction1");

            WindowsPrincipal pricipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            bool hasAdministrativeRight = pricipal.IsInRole(WindowsBuiltInRole.Administrator);

            if (!hasAdministrativeRight)
            {
                if (MessageBox.Show("This installer requires administrator privileges.\r\n\r\nDo you want to attempt to restart it with administrator privileges?", "Administrator Privileges Required", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    ProcessStartInfo processInfo = new ProcessStartInfo();
                    processInfo.Verb = "runas";
                    processInfo.FileName = "msiexec";
                    processInfo.Arguments = "/i " + session["OriginalDatabase"];
                    try
                    {
                        Process.Start(processInfo);
                    }
                    catch (Win32Exception)
                    {
                        //Do nothing. Probably the user canceled the UAC window
                        return ActionResult.UserExit;
                    }
                }
                return ActionResult.UserExit;
            }
            else
            {
                return ActionResult.Success;
            }

            //frmInstall oForm = new frmInstall();
            //if (oForm.ShowDialog() == DialogResult.Cancel)
            //    return ActionResult.UserExit;

            //return ActionResult.Success;
        }

        [CustomAction]
        public static ActionResult CustomAction2(Session session)
        {
            //session.Log("Begin CustomAction1");

            //frmInstall oForm = new frmInstall();
            //if (oForm.ShowDialog() == DialogResult.Cancel)
            //    return ActionResult.UserExit;
            //return ActionResult.Success;

            int vExitCode = 0;
            #region [Check for Choco]
            //Stage 1
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
            };
            processChoco.ErrorDataReceived += (object sender, DataReceivedEventArgs e) =>
            {
                if (e.Data != null)
                {
                    vExitCode = 1;
                }
            };
            processChoco.BeginErrorReadLine();
            processChoco.WaitForExit();
            vExitCode = processChoco.ExitCode;
            processChoco.Close();

            if (vExitCode != 0)
                return ActionResult.UserExit;
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
                    bool lChocoPathExists = false;
                    foreach (var path in liPaths)
                    {
                        if (path.Contains(@"\ProgramData\chocolatey\bin"))
                        {
                            lChocoPathExists = true;
                            break;
                        }
                    }

                    if (!lChocoPathExists)
                    {
                        oldValue = oldValue + ";" + windowsDriveInfo + @"ProgramData\chocolatey\bin";
                        Environment.SetEnvironmentVariable("Path", oldValue, EnvironmentVariableTarget.Machine);
                    }
                    return ActionResult.Success;
                }
                else
                    return ActionResult.UserExit;
            }
            #endregion
        }

        private static string CheckForEnvironmentVariables()
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
    }
}
