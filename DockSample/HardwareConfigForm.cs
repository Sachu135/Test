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

        private DataTable tData = new DataTable("tData");

        public HardwareConfigForm(string windowServer)
        {
            InitializeComponent();
            _windowServer = windowServer;

            tData.Columns.Add("Install", typeof(bool));
            tData.Columns.Add("Package", typeof(string));
            tData.Columns.Add("Version", typeof(string));
            tData.Columns.Add("Status", typeof(string));
        }

        private void HardwareConfigForm_Shown(object sender, EventArgs e)
        {
            panel1.PerformSafely(() => {
                panel1.Visible = false;
            });
            tab.PerformSafely(() =>
            {
                tab.TabPages.Remove(tabEnvironment);
                tab.TabPages.Remove(tabLogs);
            });
            panel2.PerformSafely(() => {
                panel2.Dock = DockStyle.Fill;
            });

            btnNext.PerformSafely(() => {
                btnNext.Enabled = false;
            });

            CheckForPreRequisite();
        }

        private void CheckForPreRequisite()
        {
            ///code to check the packages to install 
            ///if installed the remove the check of checkbox and if not then add the checkbox
            ///

            var section = (PackageValuesSection)ConfigurationManager.GetSection("WindowsPackages");
            var applications = (from object value in section.Values
                                select (PackageElement)value)
                                .ToList();
            if (applications != null)
            {
                List<PackageElement> liPackages = applications;

                Task t1 = new Task(() =>
                {
                    var oldValue = CheckForEnvironmentVariables();
                    List<string> liPaths = oldValue.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries).ToList();

                    var windowsFolderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Windows);
                    var windowsDrive = windowsFolderPath.Substring(0, windowsFolderPath.IndexOf(System.IO.Path.VolumeSeparatorChar));
                    var windowsDriveInfo = new System.IO.DriveInfo(windowsDrive);

                    List<PackageElement> liInstallationPackages = new List<PackageElement>();
                    using (RunspaceInvoke invoker = new RunspaceInvoke())
                    {
                        invoker.Invoke("Set-ExecutionPolicy Unrestricted -Scope CurrentUser");
                    }

                    #region [Check for All Packages]
                    int vExitCode = 0;
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
                            strListPackages += (e.Data + Environment.NewLine);
                        }
                    };
                    processValidate.BeginOutputReadLine();
                    processValidate.ErrorDataReceived += (object sender, DataReceivedEventArgs e) =>
                    {
                        if (e.Data != null)
                        {
                            vExitCode = 1;
                        }
                    };
                    processValidate.BeginErrorReadLine();
                    processValidate.WaitForExit();
                    processValidate.Close();
                    #endregion
                    if (vExitCode == 0)
                    {
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
                                        else
                                        {
                                            if (Directory.Exists(Path.Combine(windowsDriveInfo + "KockpitStudio", "Spark")))
                                            {
                                                var name = item.EnvVariable;
                                                var scope = EnvironmentVariableTarget.Machine;
                                                var spark_var = Environment.GetEnvironmentVariable(name, scope);
                                                if (spark_var != null)
                                                    lfound = true;
                                                else
                                                    lfound = false;
                                            }
                                            else
                                                lfound = false;
                                        }
                                    }
                                }
                                else if (item.Name.ToLower().Trim() == "hadoop")
                                {
                                    if (Directory.Exists(Path.Combine(windowsDriveInfo + "KockpitStudio", "WinUtil")))
                                    {
                                        var name = item.EnvVariable;
                                        var scope = EnvironmentVariableTarget.Machine;
                                        var hadoop_var = Environment.GetEnvironmentVariable(name, scope);
                                        if (hadoop_var != null)
                                            lfound = true;
                                        else
                                            lfound = false;
                                    }
                                    else
                                        lfound = false;
                                }
                                else if (item.Name.ToLower().Trim() == "jupyter")
                                {
                                    var output = ProcessExecReturnOutput("jupyter --version");
                                    if (output.Count > 0)
                                    {
                                        ///jupyter lab
                                        //foreach(var outputitem in output)
                                        //{
                                        //    var vJLab = outputitem.Split(' ')[0] + outputitem.Split(' ')[1];
                                        //    if (vJLab == "jupyterlab")
                                        //    {
                                        //        lfound = true;
                                        //        break;
                                        //    }
                                        //}

                                        ///jupyter notebook
                                        foreach (var outputitem in output)
                                        {
                                            if(outputitem.Split(' ')[0].ToLower().Trim().Contains("jupyter-notebook"))
                                            {
                                                lfound = true;
                                                break;
                                            }
                                        }
                                    }
                                    else
                                        lfound = false;
                                }
                                else
                                {
                                    foreach (string s in lines)
                                    {
                                        if (s.Contains(item.Package))
                                        {
                                            lfound = true;
                                            //if (item.Name.ToLower().Trim() == "python")
                                            //{
                                            //    bool lPython3found = false;
                                            //    foreach (var path in liPaths)
                                            //    {
                                            //        if (path.Contains(@"\Python3") && !path.Contains(@"\Python36"))
                                            //        {
                                            //            lPython3found = true;
                                            //            break;
                                            //        }
                                            //    }
                                            //    if (lPython3found)
                                            //    {

                                            //        lfound = false;

                                            //        oldValue = CheckForEnvironmentVariables();
                                            //        liPaths = oldValue.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                                            //    }
                                            //    else
                                            //        SetText(item.Package + " already installed");
                                            //}
                                            break;
                                        }
                                    }
                                }

                                if (!lfound)
                                    liInstallationPackages.Add(item);
                            }
                        }
                        else
                            liInstallationPackages = applications;

                        dgvCheckPreRequisite.PerformSafely(() =>
                        {
                            for (int i = 0; i < dgvCheckPreRequisite.Rows.Count; i++)
                            {
                                dgvCheckPreRequisite.Rows[i].Cells[1].ReadOnly = true;
                                dgvCheckPreRequisite.Rows[i].Cells[2].ReadOnly = true;
                                dgvCheckPreRequisite.Rows[i].Cells[3].ReadOnly = true;

                                bool lPackageInstalled = true;
                                foreach (var subitem in liInstallationPackages)
                                {
                                    if (subitem.Name == dgvCheckPreRequisite.Rows[i].Cells["Package"].Value.ToString().Trim())
                                    {
                                        lPackageInstalled = false;
                                        break;
                                    }
                                }

                                if (lPackageInstalled)
                                {
                                    dgvCheckPreRequisite.Rows[i].Cells["Install"].Value = true;
                                    dgvCheckPreRequisite.Rows[i].Cells["Install"].ReadOnly = true;
                                    dgvCheckPreRequisite.Rows[i].Cells["Status"].Value = "Already Exist";
                                    dgvCheckPreRequisite.Rows[i].Cells["Status"].Style.ForeColor = Color.Green;
                                }
                                else
                                {
                                    dgvCheckPreRequisite.Rows[i].Cells["Install"].Value = false;
                                    dgvCheckPreRequisite.Rows[i].Cells["Install"].ReadOnly = false;
                                    dgvCheckPreRequisite.Rows[i].Cells["Status"].Value = "Not Exist";
                                    dgvCheckPreRequisite.Rows[i].Cells["Status"].Style.ForeColor = Color.Red;
                                }
                            }
                        });
                    }

                    btnNext.PerformSafely(() =>
                    {
                        btnNext.Enabled = true;
                    });
                });
                t1.Start();

                Task t = new Task(() =>
                {
                    dgvCheckPreRequisite.PerformSafely(() =>
                    {
                        foreach (var item in liPackages)
                        {
                            DataRow dr = tData.NewRow();
                            dr["Package"] = item.Name;
                            dr["Version"] = item.Version;
                            dr["Install"] = false;
                            dr["Status"] = "Checking..";
                            tData.Rows.Add(dr);
                        }

                        if (tData != null && tData.Rows.Count > 0)
                        {
                            dgvCheckPreRequisite.DataSource = tData;
                            dgvCheckPreRequisite.AutoGenerateColumns = false;

                            dgvCheckPreRequisite.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                            dgvCheckPreRequisite.ColumnHeadersHeight = 25;
                            dgvCheckPreRequisite.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
                            dgvCheckPreRequisite.EnableHeadersVisualStyles = false;
                            dgvCheckPreRequisite.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
                            dgvCheckPreRequisite.RowHeadersVisible = false;
                            dgvCheckPreRequisite.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
                            {
                                BackColor = Color.SteelBlue,
                                ForeColor = Color.White,
                            };
                        }
                    });
                });
                t.Start();
            }
        }

        private void SetConfiguration(List<PackageElement> liInstallationPackages)
        {
            //Task to install the dependencies
            Task t = new Task(() =>
            {
                int vExitCode = 0;

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

                #region [Uninstall the external python packages]
                UninstallPyton(true);
                #endregion

                #region [Install and Set Environment Path]
                if (liInstallationPackages != null && liInstallationPackages.Count > 0)
                {
                    string strSparkEnvVarName = string.Empty;
                    string strHadoopEnvVarName = string.Empty;

                    bool lSparkExistsForInstallation = false;
                    bool lHadoopExistsForInstallation = false;
                    bool ljupyterExists = false;

                    //Stage 3
                    //check packages installed or not

                    SetText("-----------Installing Required Packages------------");
                    StringBuilder sb = new StringBuilder();
                    foreach (var item in liInstallationPackages)
                    {
                        if (item.Name.ToLower() == "spark")
                        {
                            lSparkExistsForInstallation = true;
                            strSparkEnvVarName = item.EnvVariable;
                            continue;
                        }

                        if (item.Name.ToLower() == "hadoop")
                        {
                            lHadoopExistsForInstallation = true;
                            strHadoopEnvVarName = item.EnvVariable;
                            continue;
                        }

                        if (item.Name.ToLower() == "jupyter")
                        {
                            ljupyterExists = true;
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
                        {
                            if (e.Data.ToLower().Contains("progress:"))
                            {
                                //code to override the last line of richtextbox
                                richTextBox2.PerformSafely(() =>
                                {
                                    int totalLines = richTextBox2.Lines.Length;
                                    string lastLine = richTextBox2.Lines[totalLines - 2];
                                    if (lastLine.ToLower().Contains("progress:"))
                                        richTextBox2.Text.Replace(lastLine, e.Data);
                                    else
                                        SetText(e.Data);
                                });
                            }
                            else
                                SetText(e.Data);
                        }
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

                        ///code to check if environment variable not set for python then set it
                        if (Directory.Exists(windowsDriveInfo + @"Python36"))
                        {
                            //code to create the copy of python.exe => python3.exe
                            if (!File.Exists(Path.Combine(windowsDriveInfo + @"Python36", "python3.exe")) &&
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

                        if (lSparkExistsForInstallation)
                        {
                            _KockPitDirectory = windowsDriveInfo + "KockpitStudio";
                            if (!Directory.Exists(_KockPitDirectory))
                                Directory.CreateDirectory(_KockPitDirectory);

                            if (Directory.Exists(_KockPitDirectory))
                            {
                                _SparkDir = Path.Combine(_KockPitDirectory, "Spark");
                                _ETLJobsDir = Path.Combine(_KockPitDirectory, "ETLJobs");

                                //Directory for Spark
                                if (!Directory.Exists(_SparkDir))
                                    Directory.CreateDirectory(_SparkDir);

                                //Directory for ETLJobs
                                if (!Directory.Exists(_ETLJobsDir))
                                    Directory.CreateDirectory(_ETLJobsDir);

                                string newValue = "";
                                if (Directory.Exists(_SparkDir))
                                {
                                    SetText("Extracting spark");
                                    //code to pull the zip from resources and unzip it.
                                    using (Stream stream = new MemoryStream(Resources.spark))
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
                                                reader.WriteEntryToDirectory(_SparkDir, opt);
                                            }
                                        }
                                    }
                                    SetText("Extraction Completed");
                                    //Environment Set For Spark
                                    SetEnv(strSparkEnvVarName, _SparkDir + "\\spark-2.4.6-bin-hadoop2.7");
                                    SetText("Environment Variable Path is set for " + strSparkEnvVarName + "");

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

                                //set Combine Path
                                if (!string.IsNullOrEmpty(newValue))
                                {
                                    Environment.SetEnvironmentVariable("Path", newValue, EnvironmentVariableTarget.Machine);
                                }
                            }
                        }

                        if (lHadoopExistsForInstallation)
                        {
                            _KockPitDirectory = windowsDriveInfo + "KockpitStudio";
                            if (!Directory.Exists(_KockPitDirectory))
                                Directory.CreateDirectory(_KockPitDirectory);

                            if (Directory.Exists(_KockPitDirectory))
                            {
                                _WinUtilDir = Path.Combine(_KockPitDirectory, "WinUtil");
                                _ETLJobsDir = Path.Combine(_KockPitDirectory, "ETLJobs");

                                //Directory for Winutils
                                if (!Directory.Exists(_WinUtilDir))
                                    Directory.CreateDirectory(_WinUtilDir);

                                //Directory for ETLJobs
                                if (!Directory.Exists(_ETLJobsDir))
                                    Directory.CreateDirectory(_ETLJobsDir);

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
                                    //Environment Set For Hadoop
                                    SetEnv(strHadoopEnvVarName, _WinUtilDir + "\\hadoop-3.0.0");
                                    SetText("Environment Variable Path is set for " + strHadoopEnvVarName + "");
                                }
                            }
                        }

                        if (ljupyterExists)
                        {
                            string cmd = string.Format(@"/c {0} & {1}", "python3 -m pip install -upgrade pip -y", "pip install jupyterlab -y");
                            var processInfoNtop = new ProcessStartInfo("cmd.exe", cmd);
                            processInfoNtop.CreateNoWindow = true;
                            processInfoNtop.WindowStyle = ProcessWindowStyle.Hidden;
                            processInfoNtop.UseShellExecute = false;
                            processInfoNtop.RedirectStandardError = true;
                            processInfoNtop.RedirectStandardOutput = true;
                            processInfoNtop.Verb = "runas";
                            var processNtop = Process.Start(processInfoNtop);
                            processNtop.Exited += (object sender, EventArgs e) =>
                            {
                                vExitCode = processInstall.ExitCode;
                            };
                            processNtop.OutputDataReceived += (object sender, DataReceivedEventArgs e) =>
                            {
                                var d = e.Data;
                                if (e.Data != null)
                                {
                                    SetText(e.Data);
                                }
                            };
                            processNtop.BeginOutputReadLine();
                            processNtop.ErrorDataReceived += (object sender, DataReceivedEventArgs e) =>
                            {
                                var d = e.Data;
                                if (e.Data != null)
                                {
                                    SetText("Error: " + e.Data);
                                    vExitCode = 1;
                                }
                            };
                            processNtop.BeginErrorReadLine();
                            processNtop.WaitForExit();
                            processNtop.Close();
                        }

                        DataTable tData = new DataTable();
                        tData.Columns.Add("Package", typeof(string));
                        tData.Columns.Add("Variable", typeof(string));
                        tData.Columns.Add("Path", typeof(string));
                        foreach (var item in liInstallationPackages)
                        {
                            if (item.IsEnvPathRequired && item.Name != "spark" && item.Name != "hadoop")
                            {
                                DataRow dr = tData.NewRow();
                                dr["Package"] = item.Package.ToString().Trim();
                                dr["Variable"] = item.EnvVariable.ToString().Trim();
                                dr["Path"] = "";
                                tData.Rows.Add(dr);
                            }
                        }


                        SetText("-----------Packages Installation Finished------------");
                        SetText("-----------Click on Proceed Button-------------------");

                        dgvData.PerformSafely(() =>
                        {
                            dgvData.DataSource = null;
                            dgvData.Rows.Clear();
                            dgvData.Columns.Clear();
                            if (tData != null && tData.Rows.Count > 0)
                            {
                                tab.PerformSafely(() =>
                                {
                                    tab.TabPages.Add(tabEnvironment);
                                });

                                dgvData.DataSource = tData;
                                dgvData.AutoGenerateColumns = false;
                                dgvData.Columns["Package"].ReadOnly = true;
                                dgvData.Columns["Variable"].ReadOnly = true;

                                btnSetEnvVar.PerformSafely(() =>
                                {
                                    btnSetEnvVar.Enabled = true;
                                });

                                tab.PerformSafely(() =>
                                {
                                    tab.SelectedTab = tabEnvironment;
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
            });
            t.Start();
        }

        //public void DownloadCompleted(string FilePath, string directoryPath)
        //{
        //    //code to unzip the targz file 
        //    if(File.Exists(FilePath))
        //    {
        //        using (Stream stream = File.OpenRead(FilePath))
        //        {
        //            var reader = ReaderFactory.Open(stream);
        //            while (reader.MoveToNextEntry())
        //            {
        //                if (!reader.Entry.IsDirectory)
        //                {
        //                    ExtractionOptions opt = new ExtractionOptions
        //                    {
        //                        ExtractFullPath = true,
        //                        Overwrite = true
        //                    };
        //                    reader.WriteEntryToDirectory(directoryPath, opt);
        //                }
        //            }
        //        }
        //    }
        //}


        #region [Events]
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
        private void btnNext_Click(object sender, EventArgs e)
        {
            //code to loop the datagrid view and check 
            ///if all installed then exit
            ///if not installed then go to installation part.
            ///exit
            ///
            List<PackageElement> listPackages = new List<PackageElement>();
            var section = (PackageValuesSection)ConfigurationManager.GetSection("WindowsPackages");
            var applications = (from object value in section.Values
                                select (PackageElement)value)
                                .ToList();

            dgvCheckPreRequisite.PerformSafely(() => {
                if (dgvCheckPreRequisite != null && dgvCheckPreRequisite.Rows.Count > 0)
                {
                    for (int i = 0; i < dgvCheckPreRequisite.Rows.Count; i++)
                    {
                        if (dgvCheckPreRequisite.Rows[i].Cells[3].Value.ToString().ToLower() == "not exist")
                        {
                            if (Convert.ToBoolean(dgvCheckPreRequisite.Rows[i].Cells[0].Value.ToString()) == true)
                            {
                                var v_item = applications.Where(m => m.Name == dgvCheckPreRequisite.Rows[i].Cells[1].Value.ToString()).SingleOrDefault();
                                listPackages.Add(v_item);
                            }
                        }
                    }
                }
            });

            if (listPackages.Count > 0)
            {
                panel1.PerformSafely(() => {
                    panel1.Visible = true;
                });
                panel2.PerformSafely(() => {
                    panel2.Dock = DockStyle.None;
                });
                tab.PerformSafely(() =>
                {
                    tab.TabPages.Add(tabLogs);
                    tab.TabPages.Remove(tabPrerequisite);
                    tab.Dock = DockStyle.None;
                });

                SetConfiguration(listPackages);
            }
            else
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
        #endregion

        #region [Methods]
        private void SetText(string strMsg)
        {
            richTextBox2.PerformSafely(() =>
            {
                richTextBox2.AppendText(strMsg);
                richTextBox2.AppendText(Environment.NewLine);
                richTextBox2.SelectionStart = richTextBox2.Text.Length;
                //richTextBox2.SelectedText = strMsg + Environment.NewLine;
                richTextBox2.ScrollToCaret();

                ConfigLog.WindowCreate(_windowServer, strMsg);
            });
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
        public List<string> ProcessExecReturnOutput(string s)
        {
            var processInfoValidate = new ProcessStartInfo("cmd.exe", @"/c " + s + "");
            processInfoValidate.CreateNoWindow = true;
            processInfoValidate.WindowStyle = ProcessWindowStyle.Hidden;
            processInfoValidate.UseShellExecute = false;
            processInfoValidate.RedirectStandardError = true;
            processInfoValidate.RedirectStandardOutput = true;
            processInfoValidate.Verb = "runas";
            var processValidate = Process.Start(processInfoValidate);
            List<string> outputlist = new List<string>();
            processValidate.OutputDataReceived += (object sender1, DataReceivedEventArgs e1) =>
            {
                if (e1.Data != null)
                {
                    outputlist.Add(e1.Data.Trim());
                }
            };
            processValidate.BeginOutputReadLine();
            processValidate.ErrorDataReceived += (object sender1, DataReceivedEventArgs e1) =>
            {
                if (e1.Data != null)
                {
                    outputlist.Add(e1.Data.Trim());
                }
            };
            processValidate.BeginErrorReadLine();
            processValidate.WaitForExit();
            processValidate.Close();
            return outputlist;
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

                DialogResult dialogResult = MessageBox.Show("Already a different python version is installed.." + Environment.NewLine + " Do you want to uninstall it?",
                    "Python Found", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
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
                            //UninstallPyton(true);
                        }
                        else
                            UninstallPyton(false);
                    }
                }
                else
                {
                    return;
                }
            }
            else
            {
                UninstallPyton(false);
            }
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
        #endregion
    }
}
