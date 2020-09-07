using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Security.Principal;
using System.Text;
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

            frmInstall oForm = new frmInstall();
            if (oForm.ShowDialog() == DialogResult.Cancel)
                return ActionResult.UserExit;

            return ActionResult.Success;
        }
    }
}
