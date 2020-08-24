using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.ServiceProcess;
using System.Windows.Forms;
using UIFunctionality.Common;
using WindowsService;

namespace DockSample
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        //static void Main()
        //{
        //    Application.EnableVisualStyles();
        //    Application.SetCompatibleTextRenderingDefault(false);
        //    Application.Run(new AppStarterForm()); //MainForm
        //}
        static int Main(string[] args)
        {
            bool install = false, uninstall = false, start = false, stop = false, service = false;
            bool runConfiguration = true;
            try
            {
                foreach (string arg in args)
                {
                    switch (arg)
                    {
                        case "-i":
                        case "--install":
                            install = true; break;
                        case "-u":
                        case "--uninstall":
                            uninstall = true; break;
                        case "-s":
                        case "--start":
                            start = true; break;
                        case "-t":
                        case "--stop":
                            stop = true; break;
                        case "--service":
                            service = true; break;
                        default:
                            //ShowHelp();
                            return 0;
                    }
                }

                if (uninstall)
                {
                    runConfiguration = false;
                    var asmPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WindowsService.exe");
                    KockpitStudioServiceAssistant.Uninstall(Assembly.LoadFrom(asmPath));
                }

                if (install)
                {
                    runConfiguration = false;
                    var asmPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WindowsService.exe");
                    KockpitStudioServiceAssistant.Install(Assembly.LoadFrom(asmPath));
                }

                if (start)
                {
                    runConfiguration = false;
                    KockpitStudioServiceAssistant.StartService();
                }

                if (stop)
                {
                    runConfiguration = false;
                    KockpitStudioServiceAssistant.StopService();
                }

                if (service)
                {
                    runConfiguration = false;
                    ServiceBase[] services = { new KockpitStudioService() };
                    ServiceBase.Run(services);
                }

                if (runConfiguration)
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new AppStarterForm()); //MainForm
                }

                return 0;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

    }
}