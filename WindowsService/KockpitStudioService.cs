using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using UIFunctionality.Common;

namespace WindowsService
{
    public class KockpitStudioService : ServiceBase
    {
        TaskScheduler _taskScheduler;
        private Boolean _shutdownFlag;

        public KockpitStudioService()
        {
            this.ServiceName = "KockpitStudioService";
            this.EventLog.Log = "Application";

            writeInfoLogEntry("KockpitStudioService()");
           
            this.CanPauseAndContinue = true;
            this.CanShutdown = true;
            this.CanStop = true;

            _shutdownFlag = false;
        }

        private void RunService()
        {
            writeInfoLogEntry("Call DoWork");
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += DoWork;
            worker.RunWorkerAsync();
        }

        private void DoWork(object sender, DoWorkEventArgs e)
        {
            _taskScheduler = new TaskScheduler();
            writeInfoLogEntry("Call loadItems");
            loadItems();
            _taskScheduler.Enabled = true;

            bool shutdown;            
            do
            {
                lock (_taskScheduler)
                {
                    shutdown = _shutdownFlag;
                }
                System.Threading.Thread.Sleep(100);
            } while (!shutdown);
        }

        private void ShutdownService()
        {
            lock (_taskScheduler)
            {
                _shutdownFlag = true;
            }
        }

        private void writeInfoLogEntry(String Message)
        {
            EventLog.WriteEntry(Message, EventLogEntryType.Information);
            //Code to write log
            CreateLog.EventLog(Message);
        }

        private void writeErrorLogEntry(String Message)
        {
            EventLog.WriteEntry(Message, EventLogEntryType.Error);
            //Code to write log
            CreateLog.ErrorLog(Message);
        }

        private void loadItems()
        {
            String configFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"KockpitStudioTaskSchedulerServices.xml");
            if (!File.Exists(configFile))
            {
                writeErrorLogEntry("Config file not found: "+ configFile);
                return;
            }

            String xmlString = String.Empty;
            try
            {
                xmlString = System.IO.File.ReadAllText(configFile);
            }
            catch (Exception ex)
            {
                writeErrorLogEntry("Can't read config file: " + configFile + ": " + ex.Message);
                return;
            }

            try
            {
                TaskScheduler.TriggerItemCollection items = TaskScheduler.TriggerItemCollection.FromXML(xmlString);
                _taskScheduler.TriggerItems.AddRange(items, new TaskScheduler.TriggerItem.OnTriggerEventHandler(OnTrigger));
                writeInfoLogEntry("Trigger items loaded: " + items.Count.ToString());
            }
            catch (Exception ex)
            {
                writeErrorLogEntry("Can't parse config file: " + configFile + ": " + ex.Message);
                return;
            }
        }

        void OnTrigger(object sender, TaskScheduler.OnTriggerEventArgs e)
        {
            try
            {
                string[] liTags = e.Item.Tag.ToString()
                    .Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                if (liTags != null && liTags.Length > 0)
                {
                    foreach (string s in liTags)
                    {
                        //Process.Start(s);
                        //System.Diagnostics.Process process = new System.Diagnostics.Process();
                        //System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                        //startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
                        //startInfo.FileName = "cmd.exe";
                        //startInfo.Arguments = string.Format(@"/c {0}", s); //@"/c python D:\test.py";
                        //process.StartInfo = startInfo;
                        //process.Start();
                        //process.WaitForExit();
                        //process.Close();

                        //Task t = new Task(() => {
                        string strErrorMsg = string.Empty;
                        var processInfo = new ProcessStartInfo("cmd.exe", @"/c " + s + "");
                        processInfo.CreateNoWindow = true;
                        processInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        processInfo.UseShellExecute = false;
                        processInfo.RedirectStandardError = true;
                        processInfo.RedirectStandardOutput = true;
                        processInfo.Verb = "runas";
                        var process = Process.Start(processInfo);
                        //process.OutputDataReceived += (object sender1, DataReceivedEventArgs e1) =>
                        //{
                        //    if (e1.Data != null)
                        //    {}
                        //};
                        //process.BeginOutputReadLine();
                        process.ErrorDataReceived += (object sender1, DataReceivedEventArgs e1) =>
                        {
                            if (e1.Data != null)
                                strErrorMsg += e1.Data + Environment.NewLine;
                        };
                        process.BeginErrorReadLine();
                        process.WaitForExit();
                        process.Close();
                        //code to send mail if error
                        if (!string.IsNullOrEmpty(strErrorMsg))
                        {
                            string sMailOutput = "";
                            string sMailSubject = string.Format("ETL Job error in KockpitStudio, Job Name: {0}", e.Item.TagName);
                            string sMailBody = string.Format("Job Name: <h3>{0}</h3> <br/> Error : <h4>{1}</h4>", e.Item.TagName, strErrorMsg);
                            if (Mail.Send(sMailSubject, sMailBody, out sMailOutput))
                                writeInfoLogEntry("OnMailSent: Tag: " + e.Item.Tag.ToString() + Environment.NewLine + "Status: Mail sent successfully..");
                            else
                                writeErrorLogEntry("OnMailSent: Tag: " + e.Item.Tag.ToString() + Environment.NewLine + "Status: " + sMailOutput);
                        }
                        //});
                        //t.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                writeErrorLogEntry("OnTrigger: Tag: " + e.Item.Tag.ToString() + Environment.NewLine + "Error: " + ex.Message);
            }
            if (e.Item.GetNextTriggerDateTime() != DateTime.MaxValue)
                writeInfoLogEntry("OnTrigger: Tag: " + e.Item.Tag.ToString() + Environment.NewLine + "Next trigger: " + e.Item.GetNextTriggerDateTime().ToString());
            else
                writeInfoLogEntry("OnTrigger: Tag: " + e.Item.Tag.ToString() + Environment.NewLine + "Next trigger: Never");
        }

        protected override void OnStart(string[] args)
        {
            writeInfoLogEntry("OnStart");
            RunService();
            base.OnStart(args);
        }

        protected override void OnStop()
        {
            writeInfoLogEntry("OnStop");
            ShutdownService();
            base.OnStop();
        }

        protected override void OnPause()
        {
            writeInfoLogEntry("OnPause");
            _taskScheduler.Enabled = false;
            base.OnPause();
        }

        protected override void OnContinue()
        {
            writeInfoLogEntry("OnContinue");
            loadItems(); // refresh config on continue
            _taskScheduler.Enabled = true;
            base.OnContinue();
        }
    }
}
