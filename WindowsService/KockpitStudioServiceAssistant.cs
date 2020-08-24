using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace WindowsService
{

    [RunInstaller(true)]
    public sealed class KockpitStudioServiceInstallerProcess : ServiceProcessInstaller
    {
        public KockpitStudioServiceInstallerProcess()
        {
            this.Account = ServiceAccount.NetworkService;
        }

        private string AppendPathParameter(string path, string parameter)
        {
            path += " " + parameter;
            return path;
        }

        protected override void OnBeforeInstall(System.Collections.IDictionary savedState)
        {
            Context.Parameters["assemblypath"] = AppendPathParameter(Context.Parameters["assemblypath"], " \" --service\"");
            base.OnBeforeInstall(savedState);
        }

        private void InitializeComponent()
        {
            // 
            // KockpitStudioServiceInstallerProcess
            // 
            this.Account = System.ServiceProcess.ServiceAccount.LocalService;

        }
    }

    [RunInstaller(true)]
    public sealed class KockpitStudioServiceInstaller : ServiceInstaller
    {
        public KockpitStudioServiceInstaller()
        {
            this.ServiceName = "KockpitStudioService";
            this.DisplayName = "Kockpit Studio";
            this.Description = "Kockpit Studio service";
            this.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
        }
    }

    //public class KockpitStudioServiceAssistant
    //{
    //    /// <summary>
    //    /// Install the new service
    //    /// </summary>
    //    public static void Install()
    //    {
    //        using (AssemblyInstaller inst = new AssemblyInstaller(typeof(Program).Assembly, null))
    //        {
    //            IDictionary state = new Hashtable();
    //            inst.UseNewContext = true;
    //            try
    //            {
    //                inst.Install(state);
    //                inst.Commit(state);
    //            }
    //            catch (Exception ex)
    //            {
    //                try
    //                {
    //                    inst.Rollback(state);
    //                }
    //                catch { }
    //                throw;
    //            }
    //        }
    //    }

    //    /// <summary>
    //    /// Uninstall the service
    //    /// </summary>
    //    public static void Uninstall()
    //    {
    //        using (AssemblyInstaller inst = new AssemblyInstaller(typeof(Program).Assembly, null))
    //        {
    //            IDictionary state = new Hashtable();
    //            inst.UseNewContext = true;
    //            try
    //            {
    //                inst.Uninstall(state);
    //            }
    //            catch
    //            {
    //                try
    //                {
    //                    inst.Rollback(state);
    //                }
    //                catch { }
    //                throw;
    //            }
    //        }
    //    }


    //    /// <summary>
    //    /// Start task scheduler service
    //    /// </summary>
    //    public static void StartService()
    //    {
    //        ServiceController service = new ServiceController("KockpitStudioService");
    //        TimeSpan timeout = TimeSpan.FromMilliseconds(5000);

    //        service.Start();
    //        service.WaitForStatus(ServiceControllerStatus.Running, timeout);
    //    }

    //    /// <summary>
    //    /// Stop task scheduler service
    //    /// </summary>
    //    public static void StopService()
    //    {
    //        ServiceController service = new ServiceController("KockpitStudioService");
    //        TimeSpan timeout = TimeSpan.FromMilliseconds(5000);

    //        service.Stop();
    //        service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
    //    }
    //}
}
