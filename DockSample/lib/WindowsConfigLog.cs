using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DockSample.lib
{
    public static class WindowsConfigLog
    {
        private static string sLogFormat; // string variable for LogFormat
        private static string sTime;  // string variable for Error Time

        /// <summary>
        /// method to create a event log
        /// </summary>
        /// <param name="strDescriptions">Log Description</param>
        public static void Create(string strWinServer, string strDescriptions)
        {
            try
            {
                if (!Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Logs\Windows\WindowsConfig\")))
                    Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Logs\Windows\WindowsConfig\"));

                sLogFormat = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss.fff");  // DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString();
                string sYear = DateTime.Now.Year.ToString();
                string sMonth = DateTime.Now.Month.ToString();
                string sDay = DateTime.Now.Day.ToString();
                sTime = sDay + "_" + sMonth + "_" + sYear;

                string sPathName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Logs\Windows\WindowsConfig\");
                if (System.IO.Directory.Exists(sPathName))
                {
                    StreamWriter sw = new StreamWriter(sPathName + sTime, true);
                    sw.WriteLine(strWinServer + " (" + sLogFormat + ") " + strDescriptions);
                    sw.Flush();
                    sw.Close();
                }
            }
            catch { }
        }

    }
}
