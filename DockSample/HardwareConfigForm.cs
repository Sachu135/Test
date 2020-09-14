using DockSample.lib;
using DockSample.Properties;
using Microsoft.Win32;
using SharpCompress.Common;
using SharpCompress.Readers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Management.Automation;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using UIFunctionality.Common;

namespace DockSample
{
    public partial class HardwareConfigForm : Form
    {
        private string _KockPitDirectory;
        private string _SparkDir;
        private string _WinUtilDir;
        private string _ETLJobsDir;


        public string _windowServer;

        public HardwareConfigForm(string windowServer)
        {
            InitializeComponent();
            _windowServer = windowServer;
        }

        private void HardwareConfigForm_Shown(object sender, EventArgs e)
        {
            tabControl1.PerformSafely(() =>
            {
                tabControl1.TabPages.Remove(tabPage1);
            });
            SetConfiguration();
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

        public Dictionary<string, Tuple<string, string>> checkInstalled(string c_name)
        {
            Dictionary<string, Tuple<string, string>> dicItems = new Dictionary<string, Tuple<string, string>>();
            try
            {
                string displayName;
                string registryKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
                RegistryKey key = Registry.LocalMachine.OpenSubKey(registryKey);
                if (key != null)
                {
                    foreach (RegistryKey subkey in key.GetSubKeyNames().Select(keyName => key.OpenSubKey(keyName)))
                    {
                        displayName = subkey.GetValue("DisplayName") as string;
                        if (displayName != null && displayName.Contains(c_name))
                        {
                            string testText = subkey.GetValue("UninstallString").ToString().Replace("/I", "/x");
                            int startindex = testText.IndexOf('{');
                            int Endindex = testText.IndexOf('}');
                            string outputstring = testText.Substring(startindex + 1, Endindex - startindex - 1);

                            dicItems.Add(outputstring, new Tuple<string, string>(displayName, testText));
                        }
                    }
                    key.Close();
                }

                registryKey = @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall";
                key = Registry.LocalMachine.OpenSubKey(registryKey);
                if (key != null)
                {
                    foreach (RegistryKey subkey in key.GetSubKeyNames().Select(keyName => key.OpenSubKey(keyName)))
                    {
                        displayName = subkey.GetValue("DisplayName") as string;
                        if (displayName != null && displayName.Contains(c_name))
                        {
                            string testText = subkey.GetValue("UninstallString").ToString().Replace("/I", "/x");
                            int startindex = testText.IndexOf('{');
                            int Endindex = testText.IndexOf('}');
                            string outputstring = testText.Substring(startindex + 1, Endindex - startindex - 1);

                            if (!dicItems.Keys.Contains(outputstring))
                                dicItems.Add(outputstring, new Tuple<string, string>(displayName, testText));
                        }
                    }
                    key.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return dicItems;
        }
        public void ProcessExec(string s)
        {
            var processInfo = new ProcessStartInfo("cmd.exe", @"/c " + s + "");
            processInfo.CreateNoWindow = true;
            processInfo.WindowStyle = ProcessWindowStyle.Hidden;
            processInfo.UseShellExecute = false;
            processInfo.RedirectStandardError = true;
            processInfo.RedirectStandardOutput = true;
            processInfo.Verb = "runas";
            var process = Process.Start(processInfo);
            process.WaitForExit();
            process.Close();
        }
        private void UninstallPyton(bool lExecute)
        {
            if (!lExecute)
                return;

            SetText("Checking for external python installation exists..");
            Dictionary<string, Tuple<string, string>> result = checkInstalled("Python");
            if (result != null && result.Count > 0)
            {
                //code to check the python version
                //if not python version is 3.6.8
                //uninstall

                List<string> listPyVersion = new List<string>();
                foreach (var item in result)
                {
                    string pattern = @"Python \d.\d.\d";
                    string input = item.Value.Item1;
                    RegexOptions options = RegexOptions.Multiline;

                    foreach (Match m in Regex.Matches(input, pattern, options))
                    {
                        listPyVersion.Add(m.Value);
                    }
                }

                listPyVersion = listPyVersion.Distinct().ToList();
                if (listPyVersion.Count > 0)
                {
                    bool lPython368found = false;

                    //check for require version
                    foreach (string s in listPyVersion)
                    {
                        if (s.ToLower().Contains("3.6.8"))
                        {
                            lPython368found = true;
                            break;
                        }
                    }

                    //check for python2
                    foreach (string s in listPyVersion)
                    {
                        Regex regex = new Regex(@"2.\d.\d");
                        Match match = regex.Match(s);
                        if (match.Success)
                        {
                            lPython368found = true;
                            break;
                        }
                    }

                    if (!lPython368found)
                    {
                        SetText("Already Python exists..");
                        SetText("Please ensure to uninstall it.");
                        foreach (var item in result)
                        {
                            if (item.Value.Item1.ToLower().Contains("bootstrap"))
                            {
                                SetText("Uninstalling " + item.Value.Item1);
                                ProcessExec(item.Value.Item2);
                                continue;
                            }

                            if (item.Value.Item1.ToLower().Contains("tcl"))
                            {
                                SetText("Uninstalling " + item.Value.Item1);
                                ProcessExec(item.Value.Item2);
                                continue;
                            }
                        }

                        Dictionary<string, Tuple<string, string>> result1 = checkInstalled("Python");
                        foreach (var item in result1)
                        {
                            SetText("Uninstalling " + item.Value.Item1);
                            ProcessExec(item.Value.Item2);
                        }

                        UninstallPyton(true);
                    }
                    else
                        UninstallPyton(false);
                }
            }
            else
            {
                UninstallPyton(false);
            }
        }

        private void SetConfiguration()
        {
            //Task to install the dependencies
            Task t = new Task(() =>
            {
                int vExitCode = 0;

                var section = (PackageValuesSection)ConfigurationManager.GetSection("WindowsPackages");
                var applications = (from object value in section.Values
                                    select (PackageElement)value)
                                    .ToList();

                //to check the paths if not exists the remove it
                //CheckForEnvironmentVariables();

                var oldValue = CheckForEnvironmentVariables();
                List<string> liPaths = oldValue.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries).ToList();

                var windowsFolderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Windows);
                var windowsDrive = windowsFolderPath.Substring(0, windowsFolderPath.IndexOf(System.IO.Path.VolumeSeparatorChar));
                var windowsDriveInfo = new System.IO.DriveInfo(windowsDrive);

                ///steps:
                ///1. check choco is install or not if not then install
                ///2. check packages installed or not if not then install and set env variables
                ///3. setup env variables
                ///

                using (RunspaceInvoke invoker = new RunspaceInvoke())
                {
                    invoker.Invoke("Set-ExecutionPolicy Unrestricted -Scope CurrentUser");
                }

                #region [Check for Choco]
                //Stage 1
                //SetText("Checking Prerequisites...");
                //StringBuilder sbChoco = new StringBuilder();
                //sbChoco.AppendLine(@"Set-ExecutionPolicy Bypass -Scope Process -Force");
                //sbChoco.AppendLine(@"$testchoco = powershell choco -v");
                //sbChoco.AppendLine(@"If(!($testchoco)) {");
                //sbChoco.AppendLine(@"    Invoke-Expression((New-Object System.Net.WebClient).DownloadString('https://chocolatey.org/install.ps1'))");
                //sbChoco.AppendLine(@"}");
                //var processInfoChoco = new ProcessStartInfo("powershell.exe", @"& {" + sbChoco.ToString() + "}");
                //processInfoChoco.CreateNoWindow = true;
                //processInfoChoco.WindowStyle = ProcessWindowStyle.Hidden;
                //processInfoChoco.UseShellExecute = false;
                //processInfoChoco.RedirectStandardError = true;
                //processInfoChoco.RedirectStandardOutput = true;
                //processInfoChoco.Verb = "runas";
                //var processChoco = Process.Start(processInfoChoco);
                //processChoco.Exited += (object sender, EventArgs e) =>
                //{
                //    vExitCode = processChoco.ExitCode;
                //    SetText("-----------Process Choco Checking/Installation Finished------------");
                //};
                //processChoco.OutputDataReceived += (object sender, DataReceivedEventArgs e) =>
                //{
                //    if (e.Data != null)
                //    {
                //        SetText(e.Data);
                //    }
                //};
                //processChoco.BeginOutputReadLine();
                //processChoco.ErrorDataReceived += (object sender, DataReceivedEventArgs e) =>
                //{
                //    if (e.Data != null)
                //    {
                //        SetText("Error: " + e.Data);
                //    }
                //};
                //processChoco.BeginErrorReadLine();
                //processChoco.WaitForExit();
                //vExitCode = processChoco.ExitCode;
                //processChoco.Close();
                #endregion

                //if (vExitCode != 0)
                //{
                //    btnProceed.Tag = (string)"0";
                //    btnProceed.PerformSafely(() =>
                //    {
                //        btnProceed.Enabled = true;
                //    });
                //    panel1.PerformSafely(() =>
                //    {
                //        panel1.Visible = false;
                //    });
                //    panel2.PerformSafely(() =>
                //    {
                //        panel2.Dock = DockStyle.Fill;
                //    });
                //}
                //else
                //{
                /////code to check if environment variable not set for choco then set it
                //if (Directory.Exists(windowsDriveInfo + @"ProgramData\chocolatey\bin"))
                //{
                //    SetText("Checking for Choco Environment Variable...");
                //    bool lChocoPathExists = false;
                //    foreach (var path in liPaths)
                //    {
                //        if (path.Contains(@"\ProgramData\chocolatey\bin"))
                //        {
                //            SetText("Choco Environment Variable exists.");
                //            lChocoPathExists = true;
                //            break;
                //        }
                //    }

                //    if (!lChocoPathExists)
                //    {
                //        SetText("Choco Environment Variable not exists.");
                //        oldValue = oldValue + ";" + windowsDriveInfo + @"ProgramData\chocolatey\bin";
                //        Environment.SetEnvironmentVariable("Path", oldValue, EnvironmentVariableTarget.Machine);
                //        SetText("Setting the Choco Environment variable.");
                //    }
                //}

                #region [Check for All Packages]
                //Stage 2
                //check packages installed or not
                SetText("Checking Prerequisites...");
                var processInfoValidate = new ProcessStartInfo("powershell.exe", @"& {choco list --localonly}");
                processInfoValidate.CreateNoWindow = true;
                processInfoValidate.WindowStyle = ProcessWindowStyle.Hidden;
                processInfoValidate.UseShellExecute = false;
                processInfoValidate.RedirectStandardError = true;
                processInfoValidate.RedirectStandardOutput = true;
                processInfoValidate.Verb = "runas";
                var processValidate = Process.Start(processInfoValidate);
                string strListPackages = string.Empty;
                processValidate.OutputDataReceived += (object sender, DataReceivedEventArgs e) =>
                {
                    if (e.Data != null)
                    {
                        this.PerformSafely(() =>
                        {
                            strListPackages += (e.Data + Environment.NewLine);
                        });
                    }
                };
                processValidate.BeginOutputReadLine();
                processValidate.ErrorDataReceived += (object sender, DataReceivedEventArgs e) =>
                {
                    if (e.Data != null)
                    {
                        vExitCode = 1;
                        SetText("Error: " + e.Data);
                    }
                };
                processValidate.BeginErrorReadLine();
                processValidate.WaitForExit();
                processValidate.Close();
                #endregion

                if (vExitCode != 0)
                {
                    SetText("---Finished with Errors----");
                    SetText("---Click on Proceed Button---");
                    btnProceed.Tag = (string)"0";
                    btnProceed.PerformSafely(() =>
                    {
                        btnProceed.Enabled = true;
                    });
                    panel1.PerformSafely(() =>
                    {
                        panel1.Visible = false;
                    });
                    panel2.PerformSafely(() =>
                    {
                        panel2.Dock = DockStyle.Fill;
                    });
                }
                else
                {
                    #region [Uninstall the external python packages]
                    UninstallPyton(true);
                    #endregion

                    #region [Install and Set Environment Path]
                    List<PackageElement> liInstallationPackages = new List<PackageElement>();
                    if (!string.IsNullOrEmpty(strListPackages))
                    {
                        //check the packages if exists then remove from the applications list otherwise continue
                        List<string> lines = strListPackages.Split(
                                                new[] { "\r\n", "\r", "\n" },
                                                StringSplitOptions.RemoveEmptyEntries).ToList();

                        foreach (var item in applications)
                        {
                            bool lfound = false;
                            if (item.Name.ToLower().Trim() == "spark")
                            {
                                //Check for pyspark is installed or not
                                var processInfoValidateSpark = new ProcessStartInfo("powershell.exe", @"pyspark --version");
                                processInfoValidateSpark.CreateNoWindow = true;
                                processInfoValidateSpark.WindowStyle = ProcessWindowStyle.Hidden;
                                processInfoValidateSpark.UseShellExecute = false;
                                processInfoValidateSpark.RedirectStandardError = true;
                                processInfoValidateSpark.RedirectStandardOutput = true;
                                processInfoValidateSpark.Verb = "runas";
                                var processValidateSpark = Process.Start(processInfoValidateSpark);
                                string strOutput = string.Empty;
                                processValidateSpark.OutputDataReceived += (object sender, DataReceivedEventArgs e) =>
                                {
                                    if (e.Data != null)
                                    {
                                        strOutput += (e.Data + Environment.NewLine);
                                    }
                                };
                                processValidateSpark.BeginOutputReadLine();
                                processValidateSpark.ErrorDataReceived += (object sender, DataReceivedEventArgs e) =>
                                {
                                    if (e.Data != null)
                                    {
                                        strOutput += (e.Data + Environment.NewLine);
                                    }
                                };
                                processValidateSpark.BeginErrorReadLine();
                                processValidateSpark.WaitForExit();
                                processValidateSpark.Close();
                                if (!string.IsNullOrEmpty(strOutput))
                                {
                                    if (strOutput.Contains("Welcome to"))
                                        lfound = true;
                                }
                            }

                            if (lfound)
                                break;

                            foreach (string s in lines)
                            {
                                if (s.Contains(item.Package))
                                {
                                    lfound = true;
                                    if (item.Name.ToLower().Trim() == "python")
                                    {
                                        bool lPython3found = false;
                                        foreach (var path in liPaths)
                                        {
                                            if (path.Contains(@"\Python3") && !path.Contains(@"\Python36"))
                                            {
                                                lPython3found = true;
                                                break;
                                            }
                                        }

                                        if (lPython3found)
                                        {
                                            //code to remove the python
                                            StringBuilder sbPyUninstall = new StringBuilder();
                                            sbPyUninstall.AppendLine(@"cuninst python -y");
                                            sbPyUninstall.AppendLine(@"cuninst python3 -y");
                                            sbPyUninstall.AppendLine(@"refreshenv");
                                            var processInfoUnPython = new ProcessStartInfo("powershell.exe", @"& {" + sbPyUninstall.ToString() + "}");
                                            processInfoUnPython.CreateNoWindow = true;
                                            processInfoUnPython.WindowStyle = ProcessWindowStyle.Hidden;
                                            processInfoUnPython.UseShellExecute = false;
                                            processInfoUnPython.RedirectStandardError = true;
                                            processInfoUnPython.RedirectStandardOutput = true;
                                            processInfoUnPython.Verb = "runas";
                                            var processUnPython = Process.Start(processInfoUnPython);
                                            processUnPython.WaitForExit();
                                            processUnPython.Close();
                                            lfound = false;

                                            oldValue = CheckForEnvironmentVariables();
                                            liPaths = oldValue.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                                        }
                                        else
                                            SetText(item.Package + " already installed");
                                    }
                                    else
                                        SetText(item.Package + " already installed");
                                    break;
                                }
                            }

                            if (!lfound)
                                liInstallationPackages.Add(item);
                        }
                    }
                    else
                    {
                        liInstallationPackages = applications;
                    }

                    if (liInstallationPackages != null && liInstallationPackages.Count > 0)
                    {
                        bool lSparkExistsForInstallation = false;
                        //Stage 3
                        //check packages installed or not

                        SetText("-----------Installing Required Packages------------");
                        StringBuilder sb = new StringBuilder();
                        foreach (var item in liInstallationPackages)
                        {
                            if (item.Name.ToLower() == "spark")
                            {
                                lSparkExistsForInstallation = true;
                                continue;
                            }

                            if (!string.IsNullOrEmpty(item.Version))
                                sb.AppendLine(@"choco install " + item.Package + " --version=" + item.Version + " -y");
                            else
                                sb.AppendLine(@"choco install " + item.Package + " -y");
                        }

                        var processInfoInstall = new ProcessStartInfo("powershell.exe", @"& {" + sb.ToString() + "}");
                        processInfoInstall.CreateNoWindow = true;
                        processInfoInstall.WindowStyle = ProcessWindowStyle.Hidden;
                        processInfoInstall.UseShellExecute = false;
                        processInfoInstall.RedirectStandardError = true;
                        processInfoInstall.RedirectStandardOutput = true;
                        processInfoInstall.Verb = "runas";
                        var processInstall = Process.Start(processInfoInstall);
                        processInstall.Exited += (object sender, EventArgs e) =>
                        {
                            vExitCode = processInstall.ExitCode;
                        };
                        processInstall.OutputDataReceived += (object sender, DataReceivedEventArgs e) =>
                        {
                            var d = e.Data;
                            if (e.Data != null)
                                SetText(e.Data);
                        };
                        processInstall.BeginOutputReadLine();
                        processInstall.ErrorDataReceived += (object sender, DataReceivedEventArgs e) =>
                        {
                            var d = e.Data;
                            if (e.Data != null)
                            {
                                SetText("Error: " + e.Data);
                                vExitCode = 1;
                            }
                        };
                        processInstall.BeginErrorReadLine();
                        processInstall.WaitForExit();
                        //vExitCode = processInstall.ExitCode;
                        processInstall.Close();

                        if (vExitCode != 0)
                        {
                            SetText("---Finished with Errors----");
                            SetText("---Click on Proceed Button---");
                            btnProceed.Tag = (string)"0";
                            btnProceed.PerformSafely(() =>
                            {
                                btnProceed.Enabled = true;
                            });
                            panel1.PerformSafely(() =>
                            {
                                panel1.Visible = false;
                            });
                            panel2.PerformSafely(() =>
                            {
                                panel2.Dock = DockStyle.Fill;
                            });
                        }
                        else
                        {
                            SetText("-----------Packages Installation Finished------------");
                            ///code to check if environment variable not set for python then set it
                            if (Directory.Exists(windowsDriveInfo + @"Python36"))
                            {
                                //code to create the copy of python.exe => python3.exe
                                if(!File.Exists(Path.Combine(windowsDriveInfo + @"Python36", "python3.exe")) &&
                                    File.Exists(Path.Combine(windowsDriveInfo + @"Python36", "python.exe")))
                                {
                                    File.Copy(Path.Combine(windowsDriveInfo + @"Python36", "python.exe"),
                                        Path.Combine(windowsDriveInfo + @"Python36", "python3.exe"));
                                }

                                SetEnv("PYSPARK_PYTHON", Path.Combine(windowsDriveInfo + @"Python36", "python3.exe"));
                                SetText("Checking for Python Environment Variable...");
                                bool lPythonPathExists = false;
                                foreach (var path in liPaths)
                                {
                                    if (path.Contains(@"\Python36"))
                                    {
                                        SetText("Python Environment Variable exists.");
                                        lPythonPathExists = true;
                                        break;
                                    }
                                }
                                if (!lPythonPathExists)
                                {
                                    SetText("Python Environment Variable not exists.");
                                    oldValue = oldValue + ";" + windowsDriveInfo + @"\Python36\Scripts\" + ";" + windowsDriveInfo + @"\Python36";
                                    Environment.SetEnvironmentVariable("Path", oldValue, EnvironmentVariableTarget.Machine);
                                    SetText("Python Environment Variable set.");
                                }
                            }

                            //code to
                            //1.https://spark.apache.org/downloads.html (2.4.6) (through c# code in c:\KockpitStudio\Spark\spark-2.4.7-bin-hadoop2.7
                            //2.Set spark home through C# code 'SPARK_HOME', 'c:\KockpitStudio\Spark\spark-2.4.7-bin-hadoop2.7'
                            //3.c:\KockpitStudio\Spark\Winutil
                            //4.Set Hadoop Home path
                            if (lSparkExistsForInstallation)
                            {
                                //code to create the directory for KockpitStudio
                                _KockPitDirectory = windowsDriveInfo + "KockpitStudio";
                                if (!Directory.Exists(_KockPitDirectory))
                                    Directory.CreateDirectory(_KockPitDirectory);

                                if (Directory.Exists(_KockPitDirectory))
                                {
                                    _SparkDir = Path.Combine(_KockPitDirectory, "Spark");
                                    _WinUtilDir = Path.Combine(_KockPitDirectory, "WinUtil");
                                    _ETLJobsDir = Path.Combine(_KockPitDirectory, "ETLJobs");

                                    //Directory for Spark
                                    if (!Directory.Exists(_SparkDir))
                                        Directory.CreateDirectory(_SparkDir);

                                    //Directory for Winutils
                                    if (!Directory.Exists(_WinUtilDir))
                                        Directory.CreateDirectory(_WinUtilDir);

                                    //Directory for ETLJobs
                                    if (!Directory.Exists(_ETLJobsDir))
                                        Directory.CreateDirectory(_ETLJobsDir);

                                    string newValue = "";
                                    if (Directory.Exists(_SparkDir))
                                    {
                                        //Code to download the file from URL and unzip it
                                        using (WebClient myWebClient = new WebClient())
                                        {
                                            if (!File.Exists(_SparkDir + "\\spark.tgz"))
                                            {
                                                SetText("Downloading Spark From https://downloads.apache.org/spark/spark-2.4.6/spark-2.4.6-bin-hadoop2.7.tgz");
                                                myWebClient.DownloadFile(new Uri("https://downloads.apache.org/spark/spark-2.4.6/spark-2.4.6-bin-hadoop2.7.tgz"), _SparkDir + "\\spark.tgz");
                                                SetText("Download Completed");
                                            }
                                            SetText("Extracting spark..");
                                            DownloadCompleted(_SparkDir + "\\spark.tgz", _SparkDir);
                                            SetText("Extraction Completed");

                                            //Environment Set For Spark
                                            SetEnv("SPARK_HOME", _SparkDir + "\\spark-2.4.6-bin-hadoop2.7");
                                            SetText("Environment Variable Path is set for SPARK_HOME");

                                            bool lSparkPathExists = false;
                                            foreach (var path in liPaths)
                                            {
                                                if (path.Contains(@"\spark-2.4.6-bin-hadoop2.7\bin"))
                                                {
                                                    lSparkPathExists = true;
                                                    break;
                                                }
                                            }

                                            if (!lSparkPathExists)
                                                newValue = oldValue + ";" + _SparkDir + "\\spark-2.4.6-bin-hadoop2.7\\bin;";
                                        }
                                    }

                                    if (Directory.Exists(_WinUtilDir))
                                    {
                                        SetText("Extracting winutils.exe");
                                        //code to pull the zip from resources and unzip it.
                                        using (Stream stream = new MemoryStream(Resources.winutils))
                                        {
                                            var reader = ReaderFactory.Open(stream);
                                            while (reader.MoveToNextEntry())
                                            {
                                                if (!reader.Entry.IsDirectory)
                                                {
                                                    ExtractionOptions opt = new ExtractionOptions
                                                    {
                                                        ExtractFullPath = true,
                                                        Overwrite = true
                                                    };
                                                    reader.WriteEntryToDirectory(_WinUtilDir, opt);
                                                }
                                            }
                                        }
                                        SetText("Extraction Completed");

                                        //Environment Set For Spark
                                        SetEnv("HADOOP_HOME", _WinUtilDir + "\\hadoop-3.0.0");
                                        SetText("Environment Variable Path is set for HADOOP_HOME");
                                        //newValue += ";" + _WinUtilDir + "\\hadoop-3.0.0\\bin;";
                                    }

                                    //set Combine Path
                                    if (!string.IsNullOrEmpty(newValue))
                                    {
                                        Environment.SetEnvironmentVariable("Path", newValue, EnvironmentVariableTarget.Machine);
                                    }

                                    SetText("--Finished--");
                                    SetText("--Click on Proceed Button--");
                                }
                            }
                            else
                            {
                                SetText("Spark already installed");
                                SetText("Hadoop already installed");
                                SetText("--Finished--");
                                SetText("--Click on Proceed Button--");
                            }

                            DataTable tData = new DataTable();
                            tData.Columns.Add("Package", typeof(string));
                            tData.Columns.Add("Variable", typeof(string));
                            tData.Columns.Add("Path", typeof(string));
                            foreach (var item in liInstallationPackages)
                            {
                                if (item.IsEnvPathRequired && item.Name != "spark")
                                {
                                    DataRow dr = tData.NewRow();
                                    dr["Package"] = item.Package.ToString().Trim();
                                    dr["Variable"] = item.EnvVariable.ToString().Trim();
                                    dr["Path"] = "";
                                    tData.Rows.Add(dr);
                                }
                            }

                            dgvData.PerformSafely(() =>
                            {
                                dgvData.DataSource = null;
                                dgvData.Rows.Clear();
                                dgvData.Columns.Clear();
                                if (tData != null && tData.Rows.Count > 0)
                                {
                                    tabControl1.PerformSafely(() =>
                                    {
                                        tabControl1.TabPages.Add(tabPage1);
                                    });

                                    dgvData.DataSource = tData;
                                    dgvData.AutoGenerateColumns = false;
                                    dgvData.Columns["Package"].ReadOnly = true;
                                    dgvData.Columns["Variable"].ReadOnly = true;

                                    btnSetEnvVar.PerformSafely(() =>
                                    {
                                        btnSetEnvVar.Enabled = true;
                                    });

                                    tabControl1.PerformSafely(() =>
                                    {
                                        tabControl1.SelectedTab = tabPage1;
                                    });
                                }
                                else
                                {
                                    btnProceed.Tag = (string)"1";
                                    btnProceed.PerformSafely(() =>
                                    {
                                        btnProceed.Enabled = true;
                                    });
                                }
                            });

                            panel1.PerformSafely(() =>
                            {
                                panel1.Visible = false;
                            });
                            panel2.PerformSafely(() =>
                            {
                                panel2.Dock = DockStyle.Fill;
                            });
                        }
                    }
                    else
                    {
                        SetText("Spark already installed");
                        SetText("Hadoop already installed");
                        SetText("--Finished--");
                        SetText("--Click on Proceed Button--");
                        btnProceed.Tag = (string)"1";
                        btnProceed.PerformSafely(() =>
                        {
                            btnProceed.Enabled = true;
                        });
                        panel1.PerformSafely(() =>
                        {
                            panel1.Visible = false;
                        });
                        panel2.PerformSafely(() =>
                        {
                            panel2.Dock = DockStyle.Fill;
                        });
                    }
                    #endregion
                }
                //}
            });
            t.Start();
        }
        public void DownloadCompleted(string FilePath, string directoryPath)
        {
            //code to unzip the targz file 
            if(File.Exists(FilePath))
            {
                using (Stream stream = File.OpenRead(FilePath))
                {
                    var reader = ReaderFactory.Open(stream);
                    while (reader.MoveToNextEntry())
                    {
                        if (!reader.Entry.IsDirectory)
                        {
                            ExtractionOptions opt = new ExtractionOptions
                            {
                                ExtractFullPath = true,
                                Overwrite = true
                            };
                            reader.WriteEntryToDirectory(directoryPath, opt);
                        }
                    }
                }
            }
        }
        public void SetEnv(string name, string value)
        {
            Environment.SetEnvironmentVariable(name, value, EnvironmentVariableTarget.Machine);
        }
        private bool ValidateEntry()
        {
            if (dgvData != null && dgvData.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvData.Rows)
                {
                    if (string.IsNullOrEmpty(row.Cells["Path"].Value.ToString().Trim()))
                    {
                        MessageBox.Show("Please mention path for " + row.Cells["Package"].Value.ToString().Trim());
                        return false;
                    }

                    if (!Directory.Exists(row.Cells["Path"].Value.ToString().Trim()))
                    {
                        MessageBox.Show("Directory not exists for " + row.Cells["Package"].Value.ToString().Trim());
                        return false;
                    }
                }
            }
            else
                return false;

            return true;
        }

        private void btnSetEnvVar_Click(object sender1, EventArgs e1)
        {
            if (ValidateEntry())
            {
                panel1.PerformSafely(() =>
                {
                    panel1.Visible = true;
                });
                panel2.PerformSafely(() =>
                {
                    panel2.Dock = DockStyle.None;
                });

                foreach (DataGridViewRow row in dgvData.Rows)
                {
                    string strPackage = row.Cells["Package"].Value.ToString().Trim();
                    string strVariable = row.Cells["Variable"].Value.ToString().Trim();
                    string strPackagePath = row.Cells["Path"].Value.ToString().Trim();
                    Task t = new Task(() =>
                    {
                        using (RunspaceInvoke invoker = new RunspaceInvoke())
                        {
                            invoker.Invoke("Set-ExecutionPolicy Unrestricted -Scope CurrentUser");
                        }

                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine(string.Format("[System.Environment]::SetEnvironmentVariable('{0}', {1}, 'Machine')", strVariable, strPackagePath));

                        var processInfo = new ProcessStartInfo("powershell.exe", @"& {" + sb.ToString() + "}");
                        processInfo.CreateNoWindow = true;
                        processInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        processInfo.UseShellExecute = false;
                        processInfo.RedirectStandardError = true;
                        processInfo.RedirectStandardOutput = true;
                        processInfo.Verb = "runas";
                        var process = Process.Start(processInfo);
                        process.Exited += (object sender, EventArgs e) =>
                        {
                            SetText("-----------Finished------------");
                        };
                        process.OutputDataReceived += (object sender, DataReceivedEventArgs e) =>
                        {
                            var d = e.Data;
                            if (e.Data != null)
                            {
                                SetText(e.Data);
                            }
                        };
                        process.BeginOutputReadLine();
                        process.ErrorDataReceived += (object sender, DataReceivedEventArgs e) =>
                        {
                            var d = e.Data;
                            if (e.Data != null)
                            {
                                SetText("Error: " + e.Data);
                            }
                        };
                        process.BeginErrorReadLine();
                        process.WaitForExit();
                        process.Close();

                        panel1.PerformSafely(() =>
                        {
                            panel1.Visible = false;
                        });
                        panel2.PerformSafely(() =>
                        {
                            panel2.Dock = DockStyle.Fill;
                        });

                        btnProceed.Tag = (string)"1";
                        btnProceed.PerformSafely(() => {
                            btnProceed.Enabled = true;
                        });
                    });
                    t.Start();
                }
            }
        }

        private void btnProceed_Click(object sender, EventArgs e)
        {
            this.PerformSafely(() =>
            {
                if ((string)btnProceed.Tag != "1")
                    this.DialogResult = DialogResult.Cancel;
                else
                    this.DialogResult = DialogResult.OK;
                this.Close();
            });
        }

        private void SetText(string strMsg)
        {
            richTextBox2.PerformSafely(() =>
            {
                richTextBox2.AppendText(strMsg);
                richTextBox2.AppendText(Environment.NewLine);
                richTextBox2.SelectionStart = richTextBox2.Text.Length;
                //richTextBox2.SelectedText = strMsg + Environment.NewLine;
                richTextBox2.ScrollToCaret();

                WindowsConfigLog.Create(_windowServer, strMsg);
            });
        }
    }
}
