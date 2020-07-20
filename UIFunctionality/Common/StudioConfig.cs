﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace UIFunctionality.Common
{
    public class SSHClientInfo
    {
        public string IPAddress { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public SSHClientInfo()
        { }

    }
    public class ProjectInfo
    {
        public string ProjectName { get; set; }
        public string ProjectPath { get; set; }
        public string KockpitServiceUrl { get; set; }
        public SFTPEntities.DirectoryOrFile DirectoryInfo { get; set; }
        public ProjectInfo()
        { }
    }
    public class TerminalInfo
    {
        public string Url { get; set; }
        public TerminalInfo()
        { }
    }

    public class OtherServices
    {
        public string AirflowService { get; set; }
        public string HealthCheckService { get; set; }
        public OtherServices()
        { }
    }

    public class DatabaseConnectionInfo
    {
        public string ConnName { get; set; }
        public string DbType { get; set; }
        public string ServerName { get; set; }
        public string DbName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public DatabaseConnectionInfo()
        { }
    }
    public class StudioConfig
    {
        public string AppPath { get; set; }
        private string configFileName = "config.json";
        private string servicePath = "api/Directory";
        public SSHClientInfo sSHClientInfo { get; set; }
        public ProjectInfo projectInfo { get; set; }

        public OtherServices otherServices = new OtherServices();

        public List<ProjectInfo> projectInfoList { get; set; }
        public TerminalInfo terminalInfo { get; set; }

        public List<DatabaseConnectionInfo> databaseConnections { get; set; }
        public StudioConfig(string appPath)
        {
            this.AppPath = appPath;
        }

        public StudioConfig SaveAndLoadConfig()
        {
            //Validate SSh client
            SSHManager.TestConnection(sSHClientInfo.IPAddress, sSHClientInfo.UserName, sSHClientInfo.Password);

            foreach(var proInfo in this.projectInfoList)
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(proInfo.KockpitServiceUrl);
                // Add an Accept header for JSON format.  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var path = string.Join("/", servicePath, "GetDirectoryList") + "?dirInfo=" + HttpUtility.UrlEncode(proInfo.ProjectPath);
                HttpResponseMessage response = client.GetAsync(path).Result;  // Blocking call!  
                if (response.IsSuccessStatusCode)
                {
                    var output = response.Content.ReadAsStringAsync().Result;
                    proInfo.DirectoryInfo = JsonConvert.DeserializeObject<SFTPEntities.DirectoryOrFile>(output);
                }
                else
                {
                    throw new Exception("Service url not working.");
                }
            }
            //Check Service
            

            //Check Terminal working

            WebRequest webRequest = WebRequest.Create(this.terminalInfo.Url);
            WebResponse webResponse;
            try
            {
                webResponse = webRequest.GetResponse();
            }
            catch //If exception thrown then couldn't get response from address
            {
                throw new Exception("Terminal Url not working.");
            }


            var filePath = Path.Combine(AppPath, configFileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            File.WriteAllText(filePath, JsonConvert.SerializeObject(this));

            return GetStudioConfigFromFile();
        }

        public StudioConfig OverrdieProjectInfo(string proName)
        {
            var proInfo = this.projectInfoList.FirstOrDefault(c => c.ProjectName.Equals(proName));

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(proInfo.KockpitServiceUrl);
            // Add an Accept header for JSON format.  
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var path = string.Join("/", servicePath, "GetDirectoryList") + "?dirInfo=" + HttpUtility.UrlEncode(proInfo.ProjectPath);
            HttpResponseMessage response = client.GetAsync(path).Result;  // Blocking call!  
            if (response.IsSuccessStatusCode)
            {
                var output = response.Content.ReadAsStringAsync().Result;
                proInfo.DirectoryInfo = JsonConvert.DeserializeObject<SFTPEntities.DirectoryOrFile>(output);
            }
            else
            {
                throw new Exception("Service url not working.");
            }

            var filePath = Path.Combine(AppPath, configFileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            File.WriteAllText(filePath, JsonConvert.SerializeObject(this));

            return GetStudioConfigFromFile();
        }

        public void SaveConfigWithoutValidate()
        {
            var filePath = Path.Combine(AppPath, configFileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            File.WriteAllText(filePath, JsonConvert.SerializeObject(this));
        }

        public StudioConfig GetStudioConfigFromFile()
        {
            var filePath = Path.Combine(AppPath, configFileName);
            var text = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<StudioConfig>(text);
        }

    }
}

