using DockSample.lib;
using DockSample.Properties;
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

namespace DockSample
{
    public partial class HardwareConfigForm : Form
    {
        private string _KockPitDirectory;
        private string _SparkDir;
        private string _WinUtilDir;

        public HardwareConfigForm()
        {
            InitializeComponent();
        }

        private void HardwareConfigForm_Shown(object sender, EventArgs e)
        {
            SetConfiguration();
        }

        private async void SetConfiguration()
        {
            var section = (PackageValuesSection)ConfigurationManager.GetSection("WindowsPackages");
            var applications = (from object value in section.Values
                                select (PackageElement)value)
                                .ToList();

            //Task to install the dependencies
            Task t = new Task(() =>
            {
                using (RunspaceInvoke invoker = new RunspaceInvoke())
                {
                    invoker.Invoke("Set-ExecutionPolicy Unrestricted -Scope CurrentUser");
                }

                ///steps:
                ///1. check choco is install or not if not then install
                ///2. check packages installed or not if not then install and set env variables
                ///3. setup env variables

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
                    this.PerformSafely(() =>
                    {
                        richTextBox2.Text += ("-----------Choco Installed Finished------------" + Environment.NewLine);
                        richTextBox2.ScrollToCaret();
                    });
                };
                processChoco.OutputDataReceived += (object sender, DataReceivedEventArgs e) =>
                {
                    if (e.Data != null)
                    {
                        richTextBox2.PerformSafely(() =>
                        {
                            richTextBox2.Text += (e.Data + Environment.NewLine);
                            richTextBox2.SelectionStart = richTextBox2.Text.Length;
                            richTextBox2.ScrollToCaret();
                        });
                    }
                };
                processChoco.BeginOutputReadLine();
                processChoco.ErrorDataReceived += (object sender, DataReceivedEventArgs e) =>
                {
                    if (e.Data != null)
                    {
                        richTextBox2.PerformSafely(() =>
                        {
                            richTextBox2.Text += ("Error: " + e.Data + Environment.NewLine);
                            richTextBox2.SelectionStart = richTextBox2.Text.Length;
                            richTextBox2.ScrollToCaret();
                        });

                    }
                };
                processChoco.BeginErrorReadLine();
                processChoco.WaitForExit();
                processChoco.Close();

                //Stage 2
                //check packages installed or not
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
                processValidate.WaitForExit();
                processValidate.Close();

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
                                if (!strOutput.Contains("is not recognized as an internal or external command"))
                                    lfound = true;
                            }
                        }

                        foreach (string s in lines)
                        {
                            if (s.Contains(item.Package))
                            {
                                lfound = true;
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
                    bool lSparkExists = false;
                    //Stage 3
                    //check packages installed or not

                    StringBuilder sb = new StringBuilder();
                    foreach (var item in liInstallationPackages)
                    {
                        if (item.Name == "spark")
                        {
                            lSparkExists = true;
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
                        this.PerformSafely(() =>
                        {
                            richTextBox2.Text += ("-----------Finished------------" + Environment.NewLine);
                            richTextBox2.ScrollToCaret();
                        });
                    };
                    processInstall.OutputDataReceived += (object sender, DataReceivedEventArgs e) =>
                    {
                        var d = e.Data;
                        if (e.Data != null)
                        {
                            richTextBox2.PerformSafely(() =>
                            {
                                richTextBox2.Text += (e.Data + Environment.NewLine);
                                richTextBox2.SelectionStart = richTextBox2.Text.Length;
                                richTextBox2.ScrollToCaret();
                            });
                        }
                    };
                    processInstall.BeginOutputReadLine();
                    processInstall.ErrorDataReceived += (object sender, DataReceivedEventArgs e) =>
                    {
                        var d = e.Data;
                        if (e.Data != null)
                        {
                            richTextBox2.PerformSafely(() =>
                            {
                                richTextBox2.Text += ("Error: " + e.Data + Environment.NewLine);
                                richTextBox2.SelectionStart = richTextBox2.Text.Length;
                                richTextBox2.ScrollToCaret();
                            });

                        }
                    };
                    processInstall.BeginErrorReadLine();
                    processInstall.WaitForExit();
                    processInstall.Close();


                    //code to
                    //1.https://spark.apache.org/downloads.html (2.4.6) (through c# code in c:\KockpitStudio\Spark\spark-2.4.7-bin-hadoop2.7
                    //2.Set spark home through C# code 'SPARK_HOME', 'c:\KockpitStudio\Spark\spark-2.4.7-bin-hadoop2.7'
                    //3.c:\KockpitStudio\Spark\Winutil
                    //4.Set Hadoop Home path
                    if (lSparkExists)
                    {
                        var windowsFolderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Windows);
                        var windowsDrive = windowsFolderPath.Substring(0, windowsFolderPath.IndexOf(System.IO.Path.VolumeSeparatorChar));
                        var windowsDriveInfo = new System.IO.DriveInfo(windowsDrive);

                        //code to create the directory for KockpitStudio
                        _KockPitDirectory = windowsDriveInfo + "KockpitStudio\\";
                        if (!Directory.Exists(_KockPitDirectory))
                            Directory.CreateDirectory(_KockPitDirectory);

                        if (Directory.Exists(_KockPitDirectory))
                        {
                            _SparkDir = _KockPitDirectory + "\\Spark";
                            _WinUtilDir = _KockPitDirectory + "\\WinUtil";

                            //Directory for Spark
                            if (!Directory.Exists(_SparkDir))
                                Directory.CreateDirectory(_SparkDir);

                            //Directory for Winutils
                            if (!Directory.Exists(_WinUtilDir))
                                Directory.CreateDirectory(_WinUtilDir);


                            var name = "Path";
                            var scope = EnvironmentVariableTarget.Machine; // or User
                            var oldValue = Environment.GetEnvironmentVariable(name, scope);
                            string newValue = "";
                            if (Directory.Exists(_SparkDir))
                            {
                                //Code to download the file from URL and unzip it
                                using (WebClient myWebClient = new WebClient())
                                {
                                    if(!File.Exists(_SparkDir + "\\spark.tgz"))
                                    {
                                        richTextBox2.PerformSafely(() =>
                                        {
                                            richTextBox2.Text += ("Downloading Spark From https://downloads.apache.org/spark/spark-3.0.0/spark-3.0.0-bin-hadoop2.7.tgz" + Environment.NewLine);
                                            richTextBox2.SelectionStart = richTextBox2.Text.Length;
                                            richTextBox2.ScrollToCaret();
                                        });
                                        myWebClient.DownloadFile(new Uri("https://downloads.apache.org/spark/spark-3.0.0/spark-3.0.0-bin-hadoop2.7.tgz"), _SparkDir + "\\spark.tgz");
                                        richTextBox2.PerformSafely(() =>
                                        {
                                            richTextBox2.Text += ("Download Completed" + Environment.NewLine + "Extracting..");
                                            richTextBox2.SelectionStart = richTextBox2.Text.Length;
                                            richTextBox2.ScrollToCaret();
                                        });
                                    }
                                    DownloadCompleted(_SparkDir + "\\spark.tgz", _SparkDir);
                                    richTextBox2.PerformSafely(() =>
                                    {
                                        richTextBox2.Text += ("Extraction Completed" + Environment.NewLine);
                                        richTextBox2.SelectionStart = richTextBox2.Text.Length;
                                        richTextBox2.ScrollToCaret();
                                    });

                                    //Environment Set For Spark
                                    SetEnv("SPARK_HOME", _SparkDir + "\\spark-3.0.0-bin-hadoop2.7");
                                    newValue = oldValue + ";" + _SparkDir + "\\spark-3.0.0-bin-hadoop2.7\\bin;";
                                }
                            }

                            if (Directory.Exists(_WinUtilDir))
                            {
                                richTextBox2.PerformSafely(() =>
                                {
                                    richTextBox2.Text += ("Extracting winutils.exe" + Environment.NewLine);
                                    richTextBox2.SelectionStart = richTextBox2.Text.Length;
                                    richTextBox2.ScrollToCaret();
                                });
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
                                richTextBox2.PerformSafely(() =>
                                {
                                    richTextBox2.Text += ("Extraction Completed" + Environment.NewLine);
                                    richTextBox2.SelectionStart = richTextBox2.Text.Length;
                                    richTextBox2.ScrollToCaret();
                                });

                                //Environment Set For Spark
                                SetEnv("HADOOP_HOME", _WinUtilDir + "\\hadoop-3.0.0\\bin");
                                newValue += ";" + _WinUtilDir + "\\hadoop-3.0.0\\bin;";
                            }

                            //set Combine Path
                            if (!string.IsNullOrEmpty(newValue))
                            {
                                Environment.SetEnvironmentVariable(name, newValue, scope);
                            }
                        }
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
                else
                {
                    this.PerformSafely(() =>
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    });
                }
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
                            this.PerformSafely(() =>
                            {
                                richTextBox2.Text += ("-----------Finished------------" + Environment.NewLine);
                            });
                        };
                        process.OutputDataReceived += (object sender, DataReceivedEventArgs e) =>
                        {
                            var d = e.Data;
                            if (e.Data != null)
                            {
                                richTextBox2.PerformSafely(() =>
                                {
                                    richTextBox2.Text += (e.Data + Environment.NewLine);
                                    richTextBox2.SelectionStart = richTextBox2.Text.Length;
                                    richTextBox2.ScrollToCaret();
                                });
                            }
                        };
                        process.BeginOutputReadLine();
                        process.ErrorDataReceived += (object sender, DataReceivedEventArgs e) =>
                        {
                            var d = e.Data;
                            if (e.Data != null)
                            {
                                richTextBox2.PerformSafely(() =>
                                {
                                    richTextBox2.Text += ("Error: " + e.Data + Environment.NewLine);
                                    richTextBox2.SelectionStart = richTextBox2.Text.Length;
                                    richTextBox2.ScrollToCaret();
                                });

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
                this.DialogResult = DialogResult.OK;
                this.Close();
            });
        }
    }
}
