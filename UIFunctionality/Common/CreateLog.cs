using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIFunctionality.Common
{
    public static class CreateLog
    {
        private static string sLogFormat; // string variable for LogFormat
        private static string sTime;  // string variable for Error Time

        /// <summary>
        /// method to create a event log
        /// </summary>
        /// <param name="strDescriptions">Log Description</param>
        public static void EventLog(string strDescriptions)
        {
            try
            {
                if (!Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Logs\Windows\TaskScheduler\EventLog\")))
                    Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Logs\Windows\TaskScheduler\EventLog\"));
               
                sLogFormat = DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ==> ";
                string sYear = DateTime.Now.Year.ToString();
                string sMonth = DateTime.Now.Month.ToString();
                string sDay = DateTime.Now.Day.ToString();
                sTime = sDay + "_" + sMonth + "_" + sYear;

                string sPathName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Logs\Windows\TaskScheduler\EventLog\");
                if (System.IO.Directory.Exists(sPathName))
                {
                    StreamWriter sw = new StreamWriter(sPathName + sTime, true);
                    sw.WriteLine(sLogFormat + strDescriptions);
                    sw.Flush();
                    sw.Close();
                }
            }
            catch { }
        }

        /// <summary>
        /// method to create a error log
        /// </summary>
        /// <param name="strDescriptions">Log Description</param>
        public static void ErrorLog(string strDescriptions)
        {
            try
            {
                if (!Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Logs\Windows\TaskScheduler\EventLog\")))
                    Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Logs\Windows\TaskScheduler\ErrorLog\"));

                sLogFormat = DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ==> ";
                string sYear = DateTime.Now.Year.ToString();
                string sMonth = DateTime.Now.Month.ToString();
                string sDay = DateTime.Now.Day.ToString();
                sTime = sDay + "_" + sMonth + "_" + sYear;

                string sPathName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Logs\Windows\TaskScheduler\ErrorLog\");
                if (System.IO.Directory.Exists(sPathName))
                {
                    StreamWriter sw = new StreamWriter(sPathName + sTime, true);
                    sw.WriteLine(sLogFormat + strDescriptions);
                    sw.Flush();
                    sw.Close();
                }
            }
            catch { }
        }
    }
}
