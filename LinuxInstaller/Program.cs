using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace LinuxInstallers
{
    class Program
    {
        /*
         {
            "commands": "sudo apt-get update; sudo apt-get install htop[NEXT]ls -a[NEXT]####UpdateDotnetEnvVariable####[NEXT]####HostWebService####[NEXT]####CreateHealthCheck####",
            "installerPath": "/etc/KockpitStudio/Packages/Installer/"
        }
         */

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

            for (int i = 0; i < commands.Count(); i++)
            {
                var cmd = commands[i];
                Console.WriteLine(Environment.NewLine);
                Console.WriteLine(string.Format("########## Step {0} started ##########", (i + 1)));
                Console.WriteLine(cmd);
                if (cmd.StartsWith("####"))
                {
                    var fnc = cmd.Replace("####", string.Empty);
                    var obj = new Program();
                    obj.GetType().GetMethod(fnc).Invoke(obj, null);
                }
                else
                {
                    //ExecuteProcess(i, cmd);
                    //ExecuteProcessManually(i, cmd);
                    
                    Task t = ExecuteProcessManually(i, cmd);
                    t.Wait();
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

        static async Task ExecuteProcessManually(int step, string command, bool isOutputRequired = false)
        {
            var outputString = string.Empty;
            try
            {
                //using (var process = Process.Start(new ProcessStartInfo { FileName = "/bin/bash", RedirectStandardInput = true, RedirectStandardOutput = true }))
                //{
                //    process.StandardInput.WriteLine("yes | sudo ambari-server setup");
                //    //process.StandardInput.WriteLine("exit");
                //    process.WaitForExit();
                //}

                try
                {
                    //"-c \" " + command + " \""

                    var exitCode = await ProcessExecute.StartProcess(
                        "/bin/bash",
                        "-c \" " + command.Trim() + " \""
                        );
                    Console.WriteLine($"Process Exited with Exit Code {exitCode}!");
                }
                catch (TaskCanceledException)
                {
                    Console.WriteLine("Process Timed Out!");
                }

                //ProcessExecute.StartProcess("/bin/bash", "-c \" " + command + " \"");

                //ProcessStartInfo psi = new ProcessStartInfo();
                //psi.FileName = "/bin/bash";
                //psi.RedirectStandardInput = true;
                //psi.RedirectStandardOutput = false;
                //psi.RedirectStandardError = false;
                //psi.Arguments = "-c \" " + command + " \"";
                //psi.UseShellExecute = false;
                //psi.CreateNoWindow = true;

                //using (Process process = new Process())
                //{
                //    process.StartInfo = psi;
                //    process.Start();
                //    process.StandardInput.WriteLine("y");
                //    process.StandardInput.WriteLine("root");
                //    process.StandardInput.WriteLine("1");
                //    process.StandardInput.WriteLine("y");
                //    process.StandardInput.WriteLine("y");
                //    process.StandardInput.WriteLine("n");
                //    process.WaitForExit();
                //}

                /////-------------------------------------------------------------------------
                ///
                #region []
                //var processInfoChoco = new ProcessStartInfo();
                //processInfoChoco.FileName = "/bin/bash";
                //processInfoChoco.Arguments = "-c \" " + command + " \"";
                //processInfoChoco.CreateNoWindow = true;
                //processInfoChoco.WindowStyle = ProcessWindowStyle.Hidden;
                //processInfoChoco.UseShellExecute = false;
                //processInfoChoco.RedirectStandardError = true;
                //processInfoChoco.RedirectStandardOutput = true;
                //processInfoChoco.RedirectStandardInput = false;
                //processInfoChoco.Verb = "runas";
                //var processChoco = Process.Start(processInfoChoco);
                //processChoco.Exited += (object sender, EventArgs e) =>
                //{
                //};
                //processChoco.OutputDataReceived += (object sender, DataReceivedEventArgs e) =>
                //{
                //    if (e.Data != null)
                //    {
                //        if (isOutputRequired)
                //        {
                //            outputString += e.Data;
                //        }
                //        else
                //        {
                //            Console.Write(e.Data);
                //            Console.Write((e.Data + Environment.NewLine));
                //        }

                //    }
                //};
                //processChoco.BeginOutputReadLine();
                //processChoco.ErrorDataReceived += (object sender, DataReceivedEventArgs e) =>
                //{
                //    if (e.Data != null)
                //    {
                //        Console.Write(("Error: " + e.Data + Environment.NewLine));
                //    }
                //};
                //processChoco.BeginErrorReadLine();
                //processChoco.WaitForExit();
                //processChoco.Close();
                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine(("Error: " + ex.Message));
            }
            //return outputString;
        }

        static string ExecuteProcess(int step, string command, bool isOutputRequired = false)
        {
            var outputString = string.Empty;
            try
            {
                //Console.WriteLine("ls -a");
                //System.Threading.Tasks.Task t = new System.Threading.Tasks.Task(() =>
                //{
                //    System.Threading.Thread.Sleep(2000);
                //    Console.WriteLine("enter");
                //});
                //t.Start();
                //Console.ReadKey(true);
                //Console.WriteLine("enter pressed");
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
                        if (isOutputRequired)
                        {
                            outputString += e.Data;
                        }
                        else
                        {
                            Console.Write((e.Data + Environment.NewLine));
                        }

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
            return outputString;
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
                        //sw.WriteLine("stderr_logfile=/var/log/KockpitWebService.err.log");
                        //sw.WriteLine("stdout_logfile=/var/log/KockpitWebService.out.log");
                        //sw.WriteLine("environment=ASPNETCORE_ENVIRONMENT=Production");
                        //sw.WriteLine("user=www-data");
                        //sw.WriteLine("stopsignal=INT");
                    }
                    Console.WriteLine(("Success: File created on" + path));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(("Error: " + ex.Message));
            }
        }

        public void UpdateDotnetEnvVariable()
        {
            try
            {
                //Console.WriteLine("UpdateDotnetEnvVariable");
                //var pathString = ExecuteProcess(0, "echo $PATH;", true);

                //Console.WriteLine(File.ReadAllText(@"/etc/environment"));
                var pathString = File.ReadAllText(@"/etc/environment");
                //Console.WriteLine(pathString);
                if (!pathString.Contains(":/snap/bin"))
                {

                    //get the string between char ""
                    Match match = Regex.Match(pathString, "\".*?\"");
                    string sPaths = match.ToString().TrimStart('"').TrimEnd('"');
                    if (!string.IsNullOrEmpty(sPaths))
                    {
                        sPaths += ":/snap/bin";
                        sPaths = "PATH=\"" + sPaths + "\"";

                        File.WriteAllText(@"/etc/environment", sPaths);
                        //Console.WriteLine("Path Set: " + sPaths);
                    }

                    //pathString += ":/snap/bin";
                    //pathString = "PATH=\"" + pathString + "\"";
                    //File.WriteAllText(@"/etc/environment", pathString);
                }
                ExecuteProcess(0, "source /etc/environment;");
                //pathString = ExecuteProcess(0, "echo $PATH;", true);
                pathString = File.ReadAllText(@"/etc/environment");
                Console.WriteLine(pathString);
            }
            catch (Exception ex)
            {
                Console.WriteLine(("Error: " + ex.Message));
            }
        }


        public void UpdateHostName()
        {
            try
            {
                string hostString = File.ReadAllText(@"/etc/hosts");
                //Console.WriteLine(hostString);
                if (!string.IsNullOrEmpty(hostString))
                {
                    if (hostString.Contains("127.0.0.1"))
                    {
                        List<string> listNewRows = new List<string>();
                        List<string> listRows = hostString.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).ToList();
                        if (listRows != null && listRows.Count > 0)
                        {
                            foreach (string s in listRows)
                            {
                                string strRow = s;
                                if (strRow.Contains("127.0.0.1"))
                                {
                                    string sa = strRow.Replace("127.0.0.1", "").Trim();
                                    strRow = strRow.Replace(sa, "hadoopmaster");
                                }
                                listNewRows.Add(strRow);
                            }

                            string strNewHostString = string.Join(Environment.NewLine, listNewRows);
                            File.WriteAllText(@"/etc/hosts", strNewHostString);
                            Console.WriteLine("/etc/hosts file updated");
                        }

                        ExecuteProcess(0, "sudo hostnamectl set-hostname hadoopmaster");
                        Console.WriteLine("hostname updated");
                    }
                }

                hostString = File.ReadAllText(@"/etc/hosts");
                //Console.WriteLine(hostString);
            }
            catch (Exception ex)
            {
                Console.WriteLine(("Error: " + ex.Message));
            }
        }

        public void UpdateHostConfig()
        {
            try
            {
                string strhba = File.ReadAllText(@"/etc/postgresql/10/main/pg_hba.conf");
                if (!string.IsNullOrEmpty(strhba))
                {
                    List<string> listRows = strhba.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).ToList();
                    if (listRows != null && listRows.Count > 0)
                    {
                        bool lfound = false;
                        //check for if line is not exists then append it.
                        foreach (string s in listRows)
                        {
                            string s1 = s.Replace(" ", string.Empty).Trim();
                            if (s1.Contains("hostallall0.0.0.0/0md5"))
                            {
                                lfound = true;
                                break;
                            }
                        }

                        if (!lfound)
                        {
                            Console.WriteLine(lfound);
                            listRows.Add("host    all     all     0.0.0.0/0       md5");
                            string strNewHostString = string.Join(Environment.NewLine, listRows);
                            File.WriteAllText(@"/etc/postgresql/10/main/pg_hba.conf", strNewHostString);
                            Console.WriteLine("/etc/postgresql/10/main/pg_hba.conf file updated");
                        }
                        else
                        {
                            Console.WriteLine("/etc/postgresql/10/main/pg_hba.conf already modified");
                        }
                    }
                }


                string strpostgresql = File.ReadAllText(@"/etc/postgresql/10/main/postgresql.conf");
                if (!string.IsNullOrEmpty(strpostgresql))
                {
                    List<string> listNewRows = new List<string>();
                    List<string> listRows = strpostgresql.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).ToList();
                    if (listRows != null && listRows.Count > 0)
                    {
                        foreach (string s in listRows)
                        {
                            string strRow = s;
                            if (strRow.Contains("listen_addresses"))
                            {
                                ///if(strRow[0].ToString().Contains("#"))
                                strRow = strRow.Replace(strRow.ToString(), "listen_addresses = '*'");

                                //string pattern = @"listen_addresses = '(.+?)'";
                                //RegexOptions options = RegexOptions.Multiline;
                                //Match m = Regex.Match(strRow, pattern, options);
                                //if (m.Success && m.Groups.Count > 0)
                                //{
                                //    strRow = strRow.Replace(m.Groups[1].ToString(), "*");
                                //}
                            }
                            listNewRows.Add(strRow);
                        }

                        string strNewHostString = string.Join(Environment.NewLine, listNewRows);
                        File.WriteAllText(@"/etc/postgresql/10/main/postgresql.conf", strNewHostString);
                        Console.WriteLine("/etc/postgresql/10/main/postgresql.conf file updated");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(("Error: " + ex.Message));
            }
        }

        public void GenerateSSHKey()
        {
            // Console.WriteLine("Calling function");
            // Console.WriteLine("ls -a" + Environment.NewLine);
            try
            {
                //ExecuteProcess(0, "ssh-keygen -y");
                string isSSHDirectoryExists = ExecuteProcess(0, "if test -d ~/.ssh; then echo 'true'; else echo 'false'; fi ", true);
                Console.WriteLine(isSSHDirectoryExists);
                if (!string.IsNullOrEmpty(isSSHDirectoryExists))
                {
                    if (isSSHDirectoryExists.ToLower().Trim() == "false")
                    {
                        ExecuteProcess(0, "mkdir -p ~/.ssh");
                        Console.WriteLine("Press enter to generate keys if ask");
                        ExecuteProcess(0, "sudo ssh-keygen");
                    }

                    isSSHDirectoryExists = ExecuteProcess(0, "if test -d ~/.ssh; then echo 'true'; else echo 'false'; fi ", true);
                    if (isSSHDirectoryExists.ToLower().Trim() == "true")
                    {
                        ExecuteProcess(0, "cat ~/.ssh/id_rsa.pub > ~/.ssh/authorized_keys");

                        //code to sudo nano /etc/ssh/sshd_config
                        string sshd_config = File.ReadAllText(@"/etc/ssh/sshd_config");
                        // Console.WriteLine(sshd_config);
                        if (!string.IsNullOrEmpty(sshd_config))
                        {
                            List<string> listNewRows = new List<string>();
                            List<string> listRows = sshd_config.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).ToList();
                            if (listRows != null && listRows.Count > 0)
                            {
                                foreach (string s in listRows)
                                {
                                    string strRow = s;
                                    if (strRow.Contains("PubkeyAuthentication"))
                                    {
                                        if (strRow[0].ToString().Contains("#"))
                                            strRow = strRow.Replace(strRow[0].ToString(), string.Empty);
                                    }

                                    if (strRow.Contains("AuthorizedKeysFile"))
                                    {
                                        if (strRow[0].ToString().Contains("#"))
                                            strRow = strRow.Replace(strRow[0].ToString(), string.Empty);
                                    }

                                    listNewRows.Add(strRow);
                                }

                                string new_sshd_config = string.Join(Environment.NewLine, listNewRows);
                                File.WriteAllText(@"/etc/ssh/sshd_config", new_sshd_config);
                                Console.WriteLine("/etc/ssh/sshd_config file updated");
                            }
                        }

                        ExecuteProcess(0, "sudo systemctl restart ssh");
                        Console.WriteLine("ssh restarted");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                throw;
            }

        }
    }

    public static class ProcessExecute
    {
        public static async Task<int> StartProcess(
           string filename,
           string arguments,
           string workingDirectory = null,
           int? timeout = null,
           TextWriter outputTextWriter = null,
           TextWriter errorTextWriter = null)
        {
            using (var process = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    CreateNoWindow = true,
                    Arguments = arguments,
                    FileName = filename,
                    RedirectStandardOutput = outputTextWriter != null,
                    RedirectStandardError = errorTextWriter != null,
                    UseShellExecute = false,
                    WorkingDirectory = workingDirectory
                }
            })
            {
                var cancellationTokenSource = timeout.HasValue ?
                    new CancellationTokenSource(timeout.Value) :
                    new CancellationTokenSource();

                process.Start();

                var tasks = new List<Task>(3) { process.WaitForExitAsync(cancellationTokenSource.Token) };
                if (outputTextWriter != null)
                {
                    tasks.Add(ReadAsync(
                        x =>
                        {
                            process.OutputDataReceived += x;
                            process.BeginOutputReadLine();
                        },
                        x => process.OutputDataReceived -= x,
                        outputTextWriter,
                        cancellationTokenSource.Token));
                }

                if (errorTextWriter != null)
                {
                    tasks.Add(ReadAsync(
                        x =>
                        {
                            process.ErrorDataReceived += x;
                            process.BeginErrorReadLine();
                        },
                        x => process.ErrorDataReceived -= x,
                        errorTextWriter,
                        cancellationTokenSource.Token));
                }

                await Task.WhenAll(tasks);
                return process.ExitCode;
            }
        }

        /// <summary>
        /// Waits asynchronously for the process to exit.
        /// </summary>
        /// <param name="process">The process to wait for cancellation.</param>
        /// <param name="cancellationToken">A cancellation token. If invoked, the task will return
        /// immediately as cancelled.</param>
        /// <returns>A Task representing waiting for the process to end.</returns>
        public static Task WaitForExitAsync(
            this Process process,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            process.EnableRaisingEvents = true;

            var taskCompletionSource = new TaskCompletionSource<object>();

            EventHandler handler = null;
            handler = (sender, args) =>
            {
                process.Exited -= handler;
                taskCompletionSource.TrySetResult(null);
            };
            process.Exited += handler;

            if (cancellationToken != default(CancellationToken))
            {
                cancellationToken.Register(
                    () =>
                    {
                        process.Exited -= handler;
                        taskCompletionSource.TrySetCanceled();
                    });
            }

            return taskCompletionSource.Task;
        }

        /// <summary>
        /// Reads the data from the specified data recieved event and writes it to the
        /// <paramref name="textWriter"/>.
        /// </summary>
        /// <param name="addHandler">Adds the event handler.</param>
        /// <param name="removeHandler">Removes the event handler.</param>
        /// <param name="textWriter">The text writer.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static Task ReadAsync(
            this Action<DataReceivedEventHandler> addHandler,
            Action<DataReceivedEventHandler> removeHandler,
            TextWriter textWriter,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var taskCompletionSource = new TaskCompletionSource<object>();

            DataReceivedEventHandler handler = null;
            handler = new DataReceivedEventHandler(
                (sender, e) =>
                {
                    if (e.Data == null)
                    {
                        removeHandler(handler);
                        taskCompletionSource.TrySetResult(null);
                    }
                    else
                    {
                        textWriter.WriteLine(e.Data);
                    }
                });

            addHandler(handler);

            if (cancellationToken != default(CancellationToken))
            {
                cancellationToken.Register(
                    () =>
                    {
                        removeHandler(handler);
                        taskCompletionSource.TrySetCanceled();
                    });
            }

            return taskCompletionSource.Task;
        }

    }
}