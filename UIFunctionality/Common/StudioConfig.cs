using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SFTPEntities;
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
        public bool IsWindows { get; set; }
        public SSHClientInfo sSHClientInfo { get; set; }
        public TerminalInfo terminalInfo { get; set; }
        public OtherServices otherServices = new OtherServices();
        public SFTPEntities.DirectoryOrFile DirectoryInfo { get; set; }


        //git properties
        public string GitRepoURL { get; set; }
        public string GitUsername { get; set; }
        public string GitPassword { get; set; }
        public string GitEmail { get; set; }
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
        public string ClusterSetupService { get; set; }
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
        //public SSHClientInfo sSHClientInfo { get; set; }
        public List<ProjectInfo> projectInfoList { get; set; }
        //public TerminalInfo terminalInfo { get; set; }

        public List<DatabaseConnectionInfo> databaseConnections { get; set; }
        public StudioConfig(string appPath)
        {
            this.AppPath = appPath;
        }


        private DirectoryOrFile GetDirectoryFiles(string dirPath)
        {
            string st = dirPath;

            DirectoryInfo di = new DirectoryInfo(st);

            DirectoryOrFile rootDir = new DirectoryOrFile()
            {
                FullPath = di.FullName,
                IsDirectory = true,
                NodeLevel = 0,
                Name = di.Name,
                FileType = eFileType.Dir,
                files = new List<DirectoryOrFile>()
            };

            FullDirList(di, "*", ref rootDir);
            return rootDir;
        }

        private void FullDirList(DirectoryInfo dir, string searchPattern, ref DirectoryOrFile parentDir)
        {

            var allFiles = dir.GetFiles(searchPattern).OrderBy(c => c.Name).ToList();
            var allDirs = dir.GetDirectories().OrderBy(c => c.Name).ToList();
            foreach (DirectoryInfo d in allDirs)
            {
                if (d.FullName.Contains(".git"))
                {
                    if ((d.Attributes & FileAttributes.Hidden) != 0)
                        continue;
                }

                DirectoryOrFile newDir = new DirectoryOrFile()
                {
                    FullPath = d.FullName,
                    IsDirectory = true,
                    NodeLevel = parentDir.NodeLevel + 1,
                    Name = d.Name,
                    FileType = eFileType.Dir,
                    files = new List<DirectoryOrFile>()
                };
                parentDir.files.Add(newDir);
                FullDirList(d, searchPattern, ref newDir);
            }
            try
            {
                foreach (FileInfo f in allFiles)
                {
                    var ext = Path.GetExtension(f.Name).ToLower().Replace(".", string.Empty);
                    eFileType eFType;
                    switch (ext)
                    {
                        case "py":
                            eFType = eFileType.Python;
                            break;
                        case "txt":
                            eFType = eFileType.Text;
                            break;
                        case "xml":
                            eFType = eFileType.Xml;
                            break;
                        case "json":
                            eFType = eFileType.Json;
                            break;
                        case "csv":
                            eFType = eFileType.Csv;
                            break;
                        case "xls":
                        case "xlsx":
                            eFType = eFileType.Excel;
                            break;
                        default:
                            eFType = eFileType.UnKnown;
                            break;
                    }
                    var newFile = new DirectoryOrFile()
                    {
                        NodeLevel = parentDir.NodeLevel + 1,
                        Name = f.Name,
                        FullPath = f.FullName,
                        IsDirectory = false,
                        FileType = eFType,
                        files = new List<DirectoryOrFile>()
                    };
                    parentDir.files.Add(newFile);
                }
            }
            catch
            {
                //Console.WriteLine("Directory {0}  \n could not be accessed!!!!", dir.FullName);
                return;  // We alredy got an error trying to access dir so dont try to access it again
            }
        }

        public StudioConfig SaveAndLoadConfig()
        {
            //Validate SSh client

            foreach(var proInfo in this.projectInfoList)
            {
                if (!proInfo.IsWindows)
                {
                    SSHManager.TestConnection(proInfo.sSHClientInfo.IPAddress, proInfo.sSHClientInfo.UserName, proInfo.sSHClientInfo.Password);

                    WebRequest webRequest = WebRequest.Create(proInfo.terminalInfo.Url);
                    WebResponse webResponse;
                    try
                    {
                        webResponse = webRequest.GetResponse();
                    }
                    catch //If exception thrown then couldn't get response from address
                    {
                        throw new Exception("Terminal Url not working.");
                    }

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
                else
                {
                    var ot = GetDirectoryFiles(proInfo.ProjectPath);
                    var otStr = JsonConvert.SerializeObject(ot);
                    proInfo.DirectoryInfo = JsonConvert.DeserializeObject<SFTPEntities.DirectoryOrFile>(otStr);
                }
                
            }

            //Check Terminal working

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

            if (proInfo.IsWindows)
            {
                var ot = GetDirectoryFiles(proInfo.ProjectPath);
                var otStr = JsonConvert.SerializeObject(ot);
                proInfo.DirectoryInfo = JsonConvert.DeserializeObject<SFTPEntities.DirectoryOrFile>(otStr);
            }
            else
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

        public bool IsConfigExist()
        {
            var filePath = Path.Combine(AppPath, configFileName);
            return File.Exists(filePath);
        }
    }
}

