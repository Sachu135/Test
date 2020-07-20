using Renci.SshNet;
using Renci.SshNet.Sftp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UIFunctionality.Common;
using UIFunctionality.POCO;

namespace UIFunctionality
{
    public class SSHManager : IDisposable
    {
        StudioConfig studioConfig;
        SftpClient sftpClient;
        SshClient sshClient;
        public SSHManager()
        {
            
        }

        public SSHManager(StudioConfig sc)
        {
            studioConfig = sc;
        }

        public void DisposeConnection()
        {
            sftpClient = null;
            sshClient = null;
        }
        public static List<DirectoryOrFile> GetProjectDirectoryList(string path)
        {
            return new List<DirectoryOrFile>();
            /*
            var t = new Task<List<DirectoryOrFile>>(() =>
            {
                List<DirectoryOrFile> filesList = new List<DirectoryOrFile>();
                using (SshClient client = new SshClient("13.67.128.66", 22, "root1", "Root1@123456"))
                {
                    client.KeepAliveInterval = TimeSpan.FromSeconds(2);
                    client.Connect();

                    if (!client.IsConnected)
                        throw new Exception("Failed to connect ssh");

                    //var retcode = client.RunCommand("rm -rf /firmware/loaded");

                    //using (ScpClient scpClient = new ScpClient(client.ConnectionInfo))
                    //{
                    //    scpClient.Connect();

                    //    if (!scpClient.IsConnected)
                    //        throw new Exception("Failed to connect scp");

                    //    scpClient.Upload(new FileInfo(firmware_file), "/firmware/" + Path.GetFileName(firmware_file));
                    //}

                    using (SftpClient sftpClient = new SftpClient(client.ConnectionInfo))
                    {
                        sftpClient.Connect();
                        sftpClient.ChangeDirectory(path);
                        DirectoryOrFile dirOrFileObj = null;
                        ListDirectory(sftpClient, sftpClient.WorkingDirectory, ref filesList, ref dirOrFileObj);
                        sftpClient.Disconnect();
                    }

                    return filesList;
                }
            });
            t.Start();
            return t.Result;
            */
        }

        public static void TestConnection(string url, string userName, string password)
        {
            using (SshClient client = new SshClient(url, 22, userName, password))
            {
                client.KeepAliveInterval = TimeSpan.FromSeconds(2);
                client.Connect();

                if (!client.IsConnected)
                    throw new Exception("Failed to connect ssh");
            }
        }

        public static string ReadFileContent(string url, string userName, string password, string filePath)
        {
            using (SftpClient sftp = new SftpClient(url, userName, password))
            {
                sftp.KeepAliveInterval = TimeSpan.FromSeconds(2);
                sftp.Connect();

                if (!sftp.IsConnected)
                    throw new Exception("Failed to connect ssh");

                using (StreamReader sr = sftp.OpenText(filePath))
                {
                    var ct = sr.ReadToEnd();
                    sr.Close();
                    sr.Dispose();
                    sftp.Disconnect();
                    sftp.Dispose();
                    return ct;
                }
                
            }
        }

        public Stream ReadFileStream(string url, string userName, string password, string filePath)
        {
            using (SftpClient sftp = new SftpClient(url, userName, password))
            {
                sftp.KeepAliveInterval = TimeSpan.FromSeconds(2);
                sftp.Connect();

                if (!sftp.IsConnected)
                    throw new Exception("Failed to connect ssh");

                return new MemoryStream(sftp.ReadAllBytes(filePath)); 
            }
        }

        public static void WriteFileContent(string url, string userName, string password, string filePath, string content)
        {
            using (SftpClient sftp = new SftpClient(url, userName,  password))
            {
                sftp.KeepAliveInterval = TimeSpan.FromSeconds(2);
                sftp.Connect();
                if (!sftp.IsConnected)
                    throw new Exception("Failed to connect ssh");

                using (var ms = new SSHManager().GenerateStreamFromString(content))
                {
                    sftp.BufferSize = (uint)ms.Length; // bypass Payload error large files
                    sftp.UploadFile(ms, filePath);
                }
            }
        }

        private Stream GenerateStreamFromString(string s)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(s);

            var stream = new MemoryStream();
            stream.Write(bytes, 0, bytes.Length);
            stream.Position = 0;
            return stream;
        }

        public void WriteFileContentMethod(string url, string userName, string password, string filePath, string content)
        {
            using (sftpClient = new SftpClient(url, userName, password))
            {
                sftpClient.KeepAliveInterval = TimeSpan.FromSeconds(2);
                sftpClient.Connect();
                if (!sftpClient.IsConnected)
                    throw new Exception("Failed to connect ssh");

                byte[] bytes = Encoding.ASCII.GetBytes(content);
                sftpClient.DeleteFile(filePath);
                sftpClient.WriteAllBytes(filePath, bytes);

                //using (var ms = GenerateStreamFromString(content))
                //{
                //    sftpClient.BufferSize = (uint)ms.Length; // bypass Payload error large files
                //    sftpClient.UploadFile(ms, filePath, true);
                //}

                //sftpClient.UploadFile()
                //sftpClient.WriteAllText(filePath, content);
            }
        }

        public void CreateDirectory(string url, string userName, string password, string dirPath)
        {
            using (sftpClient = new SftpClient(url, userName, password))
            {
                sftpClient.KeepAliveInterval = TimeSpan.FromSeconds(2);
                sftpClient.Connect();
                if (!sftpClient.IsConnected)
                    throw new Exception("Failed to connect ssh");
                sftpClient.CreateDirectory(dirPath);
            }
        }

        public void WriteFileBytesContentMethod(string url, string userName, string password, string filePath, byte[] content)
        {
            using (sftpClient = new SftpClient(url, userName, password))
            {
                sftpClient.KeepAliveInterval = TimeSpan.FromSeconds(2);
                sftpClient.Connect();
                if (!sftpClient.IsConnected)
                    throw new Exception("Failed to connect ssh");
                sftpClient.WriteAllBytes(filePath, content);
            }
        }

        public void RenameFile(string url, string userName, string password, string oldName, string newName)
        {
            using (sftpClient = new SftpClient(url, userName, password))
            {
                sftpClient.KeepAliveInterval = TimeSpan.FromSeconds(2);
                sftpClient.Connect();
                if (!sftpClient.IsConnected)
                    throw new Exception("Failed to connect ssh");
                sftpClient.RenameFile(oldName, newName);
            }
        }

        public void RemoveFile(string url, string userName, string password, string filePath)
        {
            using (sftpClient = new SftpClient(url, userName, password))
            {
                sftpClient.KeepAliveInterval = TimeSpan.FromSeconds(2);
                sftpClient.Connect();
                if (!sftpClient.IsConnected)
                    throw new Exception("Failed to connect ssh");
                sftpClient.Delete(filePath);
            }
        }

        private void DeleteDirectory(SftpClient client, string path)
        {
            foreach (SftpFile file in client.ListDirectory(path))
            {
                if ((file.Name != ".") && (file.Name != ".."))
                {
                    if (file.IsDirectory)
                    {
                        DeleteDirectory(client, file.FullName);
                    }
                    else
                    {
                        client.DeleteFile(file.FullName);
                    }
                }
            }

            client.DeleteDirectory(path);
        }

        public void RemoveDirectory(string url, string userName, string password, string filePath)
        {
            using (sftpClient = new SftpClient(url, userName, password))
            {

                sftpClient.KeepAliveInterval = TimeSpan.FromSeconds(2);
                sftpClient.Connect();
                if (!sftpClient.IsConnected)
                    throw new Exception("Failed to connect ssh");
                DeleteDirectory(sftpClient, filePath);
            }
        }
        public static void ExecuteCommandOnConsole(string url, string userName, string password, string platform, string filePath, RichTextBox txtConsoleLog, Action opCompleted)
        {
            List<DirectoryOrFile> filesList = new List<DirectoryOrFile>();
            using (SshClient client = new SshClient(url, 22, userName, password))
            {
                client.KeepAliveInterval = TimeSpan.FromSeconds(2);
                client.Connect();

                if (!client.IsConnected)
                    throw new Exception("Failed to connect ssh");

                var st = client.CreateShellStream("bash", 80, 24, 800, 600, 1024 * 8);

                // wait for bash prompt
                while (!st.DataAvailable)
                    System.Threading.Thread.Sleep(200);

                //st.WriteLine("echo '####KockpitStudioStart####'; python Kockpit/Stage1/p1.py; echo '####KockpitStudioEnd####'; exit;");
                st.WriteLine("echo '####KockpitStudioStart####';");
                st.WriteLine(string.Format("{0} {1};", platform, filePath));
                st.WriteLine("echo '####KockpitStudioEnd####';");
                //st.WriteLine("exit;");
                //st.WriteLine("python Kockpit/Stage1/p1.py; exit;");
                st.Flush();

                StringBuilder output = new StringBuilder();

                bool loggingStart = false;
                while (client.IsConnected)
                {
                    var line = st.ReadLine();
                    //Debug.WriteLine(line);
                    //Debug.WriteLine("--------------------------------------");
                    output.Append(line);

                    //var ot = output.ToString();
                    if (line.Equals("####KockpitStudioStart####"))
                    {
                        loggingStart = true;
                        continue;
                    }
                    if (line.Contains("####KockpitStudioEnd####"))
                    {
                        opCompleted();
                        break;
                    }
                    if (loggingStart) //loggingStart && loggingEnd
                    {
                        txtConsoleLog.PerformSafely(() =>
                        {
                            txtConsoleLog.AppendText(line);
                            txtConsoleLog.AppendText(Environment.NewLine);
                            //txtConsoleLog.AppendText(line, Color.Lime);
                            txtConsoleLog.SelectionStart = txtConsoleLog.Text.Length;
                            txtConsoleLog.ScrollToCaret();
                        });
                    }

                    System.Threading.Thread.Sleep(100);

                    if (line.Contains("logout"))
                    {
                        opCompleted();
                        //client.Disconnect();
                        break;
                    }
                }
            }
        }


        public void ExecuteCommandOnConsoleMethod(string url, string userName, string password, string platform, string filePath, RichTextBox txtConsoleLog, Action opCompleted)
        {
            List<DirectoryOrFile> filesList = new List<DirectoryOrFile>();
            using (sshClient = new SshClient(url, 22, userName, password))
            {
                sshClient.KeepAliveInterval = TimeSpan.FromSeconds(2);
                sshClient.Connect();

                if (!sshClient.IsConnected)
                    throw new Exception("Failed to connect ssh");

                var st = sshClient.CreateShellStream("bash", 80, 24, 800, 600, 1024 * 8);

                // wait for bash prompt
                while (!st.DataAvailable)
                    System.Threading.Thread.Sleep(200);

                //st.WriteLine("echo '####KockpitStudioStart####'; python Kockpit/Stage1/p1.py; echo '####KockpitStudioEnd####'; exit;");
                st.WriteLine("echo '####KockpitStudioStart####';");
                st.WriteLine(string.Format("{0} {1};", platform, filePath));
                st.WriteLine("echo '####KockpitStudioEnd####';");
                //st.WriteLine("exit;");
                //st.WriteLine("python Kockpit/Stage1/p1.py; exit;");
                st.Flush();

                StringBuilder output = new StringBuilder();

                bool loggingStart = false;
                while (sshClient.IsConnected)
                {
                    var line = st.ReadLine();
                    //Debug.WriteLine(line);
                    //Debug.WriteLine("--------------------------------------");
                    output.Append(line);

                    //var ot = output.ToString();
                    if (line.Equals("####KockpitStudioStart####"))
                    {
                        loggingStart = true;
                        continue;
                    }
                    if (line.Contains("####KockpitStudioEnd####"))
                    {
                        opCompleted();
                        break;
                    }
                    if (loggingStart) //loggingStart && loggingEnd
                    {
                        txtConsoleLog.PerformSafely(() =>
                        {
                            txtConsoleLog.AppendText(line);
                            txtConsoleLog.AppendText(Environment.NewLine);
                            //txtConsoleLog.AppendText(line, Color.Lime);
                            txtConsoleLog.SelectionStart = txtConsoleLog.Text.Length;
                            txtConsoleLog.ScrollToCaret();
                        });
                    }

                    System.Threading.Thread.Sleep(100);

                    if (line.Contains("logout"))
                    {
                        opCompleted();
                        //client.Disconnect();
                        break;
                    }
                }
            }
        }


        public static string ListDirectory(string dirPath)
        {
            var t = new Task<string>(() =>
            {
                List<DirectoryOrFile> filesList = new List<DirectoryOrFile>();
                using (SshClient client = new SshClient("13.67.128.66", 22, "root1", "Root1@123456"))
                {
                    client.KeepAliveInterval = TimeSpan.FromSeconds(2);
                    client.Connect();

                    if (!client.IsConnected)
                        throw new Exception("Failed to connect ssh");

                    SshCommand sc = client.CreateCommand(string.Format("cd {0}; ls -R", dirPath));
                    sc.Execute();
                    string output = sc.Result;
                    return output;
                }
            });
            t.Start();
            return t.Result;
        }
        static void ListDirectory(SftpClient client, String dirName, ref List<DirectoryOrFile> files, ref DirectoryOrFile currentDir)
        {
            if (dirName[dirName.Length - 1] == '.')
                return;
            var allDirs = client.ListDirectory(dirName).ToList();
            foreach (var entry in allDirs)
            {
                var isNewItem = (currentDir == null); ;
                if (entry.IsDirectory)
                {
                    if (entry.FullName.Split('/').Last() == "." || entry.FullName.Split('/').Last() == "..")
                        continue;
                    
                    var newObj = new DirectoryOrFile()
                    {
                        Name = entry.FullName.Split('/').Last(),
                        FullPath = entry.FullName,
                        IsDirectory = true,
                        files = new List<DirectoryOrFile>(),
                        FileType = eFileType.Dir
                    };
                    if (isNewItem)
                    {
                        files.Add(newObj);
                    }
                    else
                    {
                        currentDir.files.Add(newObj);
                    }
                    ListDirectory(client, entry.FullName, ref files, ref newObj);
                }
                else
                {
                    //files.Add(entry.FullName);
                    var ext = Path.GetExtension(entry.FullName).ToLower();
                    eFileType eFType;
                    switch (ext)
                    {
                        case "py":
                            eFType = eFileType.Python;
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
                            eFType = eFileType.Python;
                            break;
                        default:
                            eFType = eFileType.UnKnown;
                            break;

                    }
                    var newObj = new DirectoryOrFile()
                    {
                        Name = entry.FullName.Split('/').Last(),
                        FullPath = entry.FullName,
                        FileType = eFType,
                        IsDirectory = false
                    };
                    if (isNewItem)
                    {
                        files.Add(newObj);
                    }
                    else
                    {
                        currentDir.files.Add(newObj);
                    }
                }
            }
        }

        public void Dispose()
        {
            if (sftpClient != null)
            {
                sftpClient.Dispose();
            }
            if (sshClient != null)
            {
                sshClient.Dispose();
            }
        }
    }
}
