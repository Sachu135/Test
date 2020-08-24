using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration.Install;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace UIFunctionality.Common
{
    public class KockpitStudioServiceAssistant
    {
        /// <summary>
        /// Install the new service
        /// </summary>
        public static void Install(Assembly assembly)
        {
            using (AssemblyInstaller inst = new AssemblyInstaller(assembly, null))
            {
                IDictionary state = new Hashtable();
                inst.UseNewContext = true;
                try
                {
                    inst.Install(state);
                    inst.Commit(state);
                }
                catch (Exception ex)
                {
                    try
                    {
                        inst.Rollback(state);
                    }
                    catch { }
                    throw;
                }
            }
        }

        /// <summary>
        /// Uninstall the service
        /// </summary>
        public static void Uninstall(Assembly assembly)
        {
            using (AssemblyInstaller inst = new AssemblyInstaller(assembly, null))
            {
                IDictionary state = new Hashtable();
                inst.UseNewContext = true;
                try
                {
                    inst.Uninstall(state);
                }
                catch
                {
                    try
                    {
                        inst.Rollback(state);
                    }
                    catch { }
                    throw;
                }
            }
        }


        /// <summary>
        /// Start task scheduler service
        /// </summary>
        public static void StartService()
        {
            ServiceController service = new ServiceController("KockpitStudioService");
            TimeSpan timeout = TimeSpan.FromMilliseconds(5000);

            service.Start();
            service.WaitForStatus(ServiceControllerStatus.Running, timeout);
        }

        /// <summary>
        /// Stop task scheduler service
        /// </summary>
        public static void StopService()
        {
            ServiceController service = new ServiceController("KockpitStudioService");
            TimeSpan timeout = TimeSpan.FromMilliseconds(5000);

            service.Stop();
            service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
        }
    }
}
