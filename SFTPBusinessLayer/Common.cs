using SFTPEntities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SFTPBusinessLayer
{
    public class Common
    {
        public static DirectoryOrFile GetDirectoryFiles(string dirPath)
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

        static void FullDirList(DirectoryInfo dir, string searchPattern, ref DirectoryOrFile parentDir)
        {

            var allFiles = dir.GetFiles(searchPattern).OrderBy(c => c.Name).ToList();
            var allDirs = dir.GetDirectories().OrderBy(c => c.Name).ToList();
            foreach (DirectoryInfo d in allDirs)
            {
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

            // process each directory
            // If I have been able to see the files in the directory I should also be able 
            // to look at its directories so I dont think I should place this in a try catch block


        }
        /*
        public static void GetDirectoryFiles()
        {
            string rootDirPath = "/home/root1/Kockpit";
            string ot = UIFunctionality.SSHManager.ListDirectory(rootDirPath);
            var lines = ot.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).ToList();

            UIFunctionality.POCO.DirectoryOrFile rootDir = new UIFunctionality.POCO.DirectoryOrFile()
            {
                FullPath = rootDirPath,
                Name = rootDirPath.Split('/').Last(),
                FileType = UIFunctionality.POCO.eFileType.Dir,
                files = new List<UIFunctionality.POCO.DirectoryOrFile>()
            };

            List<UIFunctionality.POCO.DirectoryOrFile> allNodes = new List<UIFunctionality.POCO.DirectoryOrFile>();
            if (lines.Count() > 0)
            {
                var allDirs = lines.Where(c => c.Contains("/")).Select(c => new { Text = c, Level = c.Split('/').Count() - 1 }).ToList();
                var maxLevel = allDirs.Max(c => c.Level);

                foreach (var dir in allDirs)
                {
                    var dirFullName = dir.Text.Replace(":", string.Empty).Replace(".", rootDirPath);
                    var dirColl = dirFullName.Split('/').ToList();
                    var parentFullPath = String.Join("/", dirColl.Take(dirColl.Count - 1).ToArray());
                    var dirName = dir.Text.Split('/').Last().Replace(":", string.Empty);
                    var newDir = new UIFunctionality.POCO.DirectoryOrFile()
                    {
                        NodeLevel = dir.Level,
                        Name = dirName,
                        FullPath = dirFullName,
                        ParentFullPath = parentFullPath,
                        IsDirectory = true,
                        FileType = UIFunctionality.POCO.eFileType.Dir,
                        files = new List<UIFunctionality.POCO.DirectoryOrFile>()
                    };
                    allNodes.Add(newDir);
                }

                var level = 1;
                var dirpath = string.Empty;
                for (int counter = 0; counter < lines.Count(); counter++)
                {
                    string text = lines[counter];

                    if (counter == 0 || string.IsNullOrEmpty(text))
                    {
                        continue;
                    }

                    if (text.Contains("/"))
                    {
                        level = text.Count(c => c.Equals('/')) + 1;
                        dirpath = text.Replace("./", string.Empty).Replace(":", string.Empty);

                        continue;
                    }

                    if (allNodes.Exists(c => (c.Name == text && c.NodeLevel == level)))
                    {
                        continue;
                    }
                    else
                    {
                        var ext = Path.GetExtension(text).ToLower().Replace(".", string.Empty);
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
                                eFType = eFileType.Python;
                                break;
                            default:
                                eFType = eFileType.UnKnown;
                                break;
                        }
                        var newFile = new UIFunctionality.POCO.DirectoryOrFile()
                        {
                            NodeLevel = level,
                            Name = text,
                            FullPath = string.IsNullOrEmpty(dirpath) ? string.Join("/", rootDirPath, text) : string.Join("/", rootDirPath, dirpath, text),
                            IsDirectory = false,
                            FileType = eFType,
                            files = new List<UIFunctionality.POCO.DirectoryOrFile>()
                        };
                        allNodes.Add(newFile);
                    }
                }
            }

            foreach (var obj in allNodes)
            {
                if (obj.NodeLevel == 1)
                {
                    var clonedObj = DeepClone.Obj<DirectoryOrFile>(obj);
                    rootDir.files.Add(clonedObj);
                }
                else
                {

                }
            }
        }
        */
    }
}
