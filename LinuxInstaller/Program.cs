using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace LinuxInstallers
{
    class Program
    {
        //"commands": "sudo apt-get update; sudo apt-get install htop[NEXT]ls -a[NEXT]ifconfig[NEXT]####CreateWebTerminal####[NEXT]####CreateHealthCheck####"
        static void Main(string[] args)
        {
            var _appStngPath = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build()["installerPath"].ToString();

            var builder = new ConfigurationBuilder()
                //.SetBasePath(Directory.GetCurrentDirectory())
                .SetBasePath(_appStngPath)
                .AddJsonFile("appsettings.json")
                .Build();
            var commands = builder["commands"].ToString().Split("[NEXT]").ToList();

            for(int i=0; i < commands.Count(); i++)
            {
                var cmd = commands[i];
                Console.WriteLine(string.Format("########## Step {0} started ##########", (i + 1)));
                if (cmd.StartsWith("####"))
                {
                    var fnc = cmd.Replace("####", string.Empty);
                    var obj = new Program();
                    obj.GetType().GetMethod(fnc).Invoke(obj, null);
                }
                else
                {
                    ExecuteProcess(i, cmd);
                }
                Console.WriteLine(string.Format("########## Step {0} completed ##########", (i + 1)));
            }

            exitProgram();
        }

        static void exitProgram()
        {
            Console.WriteLine("Type quit and press [Enter] to exit!");
            var input = Console.ReadLine();
            if (input.ToLower().Trim().Equals("quit"))
            {
                return;
            }
            else 
            {
                exitProgram();
            }
        }
        static void ExecuteProcess(int step, string command)
        {
            try
            {
                var processInfoChoco = new ProcessStartInfo();
                processInfoChoco.FileName = "/bin/bash";
                processInfoChoco.Arguments = "-c \" " + command + " \"";
                processInfoChoco.CreateNoWindow = true;
                processInfoChoco.WindowStyle = ProcessWindowStyle.Hidden;
                processInfoChoco.UseShellExecute = false;
                processInfoChoco.RedirectStandardError = true;
                processInfoChoco.RedirectStandardOutput = true;
                processInfoChoco.Verb = "runas";
                var processChoco = Process.Start(processInfoChoco);
                processChoco.Exited += (object sender, EventArgs e) =>
                {
                };
                processChoco.OutputDataReceived += (object sender, DataReceivedEventArgs e) =>
                {
                    if (e.Data != null)
                    {
                        Console.Write((e.Data + Environment.NewLine));
                    }
                };
                processChoco.BeginOutputReadLine();
                processChoco.ErrorDataReceived += (object sender, DataReceivedEventArgs e) =>
                {
                    if (e.Data != null)
                    {
                        Console.Write(("Error: " + e.Data + Environment.NewLine));
                    }
                };
                processChoco.BeginErrorReadLine();
                processChoco.WaitForExit();
                processChoco.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(("Error: " + ex.Message));
            }
        }

        public void CreateWebTerminal()
        {
            try
            {
                string path = @"/etc/supervisor/conf.d/ttyd.conf";
                if (!File.Exists(path))
                {
                    // Create a file to write to.
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        sw.WriteLine("[program:ttyd]");
                        sw.WriteLine("command=ttyd -p 5001 bash");
                        sw.WriteLine("autostart=true");
                        sw.WriteLine("autorestart=true");
                    }
                    Console.WriteLine(("Success: File created on" + path));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(("Error: " + ex.Message));
            }
        }

        public void CreateHealthCheck()
        {
            try
            {
                string path = @"/etc/supervisor/conf.d/healthcheck.conf";
                if (!File.Exists(path))
                {
                    // Create a file to write to.
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        sw.WriteLine("[program:healthcheck]");
                        sw.WriteLine("command=ttyd -p 5003 htop");
                        sw.WriteLine("autostart=true");
                        sw.WriteLine("autorestart=true");
                    }
                    Console.WriteLine(("Success: File created on" + path));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(("Error: " + ex.Message));
            }
        }

        public void HostWebService()
        {
            try
            {
                string path = @"/etc/supervisor/conf.d/KockpitWebService.conf";
                if (!File.Exists(path))
                {
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        sw.WriteLine("[program:KockpitWebService]");
                        sw.WriteLine("command=dotnet /etc/KockpitStudio/Packages/WebService/KockpitWebService.dll");
                        sw.WriteLine("directory=/etc/KockpitStudio/Packages/");
                        sw.WriteLine("autostart=true");
                        sw.WriteLine("autorestart=true");
                        sw.WriteLine("stderr_logfile=/var/log/KockpitWebService.err.log");
                        sw.WriteLine("stdout_logfile=/var/log/KockpitWebService.out.log");
                        sw.WriteLine("environment=ASPNETCORE_ENVIRONMENT=Production");
                        sw.WriteLine("user=www-data");
                        sw.WriteLine("stopsignal=INT");
                    }
                    Console.WriteLine(("Success: File created on" + path));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(("Error: " + ex.Message));
            }
        }
    }
}
