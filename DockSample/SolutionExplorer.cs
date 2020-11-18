using CustomControls;
using DockSample.Controls;
using DockSample.lib;
using SFTPEntities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UIFunctionality;
using UIFunctionality.Common;
using unvell.ReoGrid.Editor;
using WeifenLuo.WinFormsUI.Docking;

namespace DockSample
{
    public partial class SolutionExplorer : DockContent //Form
    {
        StudioConfig studioConfig;
        MainForm mainFrm;
        int currentSelectedIndex = -1;
        UCLoaderForm loader = new UCLoaderForm();
        SFTPEntities.eFileType newFileExtension = new SFTPEntities.eFileType();
        AddFile addFileForm = new AddFile();
        MoveFile moveFileForm = new MoveFile();
        ProjectInfo CurrentProj;
        GitCommit gitCommitForm;
        GitPush gitPushForm;

        public SolutionExplorer()
        {
            InitializeComponent();
        }

        class FileOrDirRenameInfo
        {
            public string OldName { get; set; }
            public string Extension { get; set; }
            public string RootPath { get; set; }
        }
        enum eOperation
        {
            NewFile,
            RenameFile,
            NewDirectory,
            RenameDirectory,
            CreateCopyFile,
        }

        eOperation op;
        FileOrDirRenameInfo fileOrDirRenameInfo;
        public SolutionExplorer(StudioConfig sc, MainForm frm):this()
        {
            studioConfig = sc;
            mainFrm = frm;
            this.CloseButton = false;
            this.CloseButtonVisible = false;

            UIFunctionality.Common.Utility.fitFormToScreen(this, 768, 1360);

            treeView2.BackColor = System.Drawing.Color.FromArgb(243, 243, 243);

            mainFrm.OnOpenMenuChanged += MainFrm_OnOpenMenuChanged;

            //imageList2.Images.Add(Properties.Resources.f_pyico);
            //imageList2.Images.Add(Properties.Resources.f_sol);
            //imageList2.Images.Add(Properties.Resources.f_py);
            //imageList2.Images.Add(Properties.Resources.f_excel);
            //imageList2.Images.Add(Properties.Resources.f_json);
            //imageList2.Images.Add(Properties.Resources.f_xml);
            //imageList2.Images.Add(Properties.Resources.f_text);
            //imageList2.Images.Add(Properties.Resources.f_other);
            //imageList2.Images.Add(Properties.Resources.f_dir);

            imageList2.Images.Add(Properties.Resources.git_commited1);
            imageList2.Images.Add(Properties.Resources.git_pendingcommit1);
            imageList2.Images.Add(Properties.Resources.git_new1);

            //treeView2.ImageIndex = 0;

            addFileForm.SaveCliked = (string fileName, string copytoPath) =>
            {
                    new Task(()=> 
                    {
                        this.PerformSafely(() =>
                        {
                            loader.ShowDialog(this);
                        });
                    }).Start();
                    
                    new Task(() =>
                    {
                        try
                        {
                            var selectedNodePath = string.Empty;
                            treeView2.PerformSafely(() => {
                                selectedNodePath = treeView2.SelectedNode.ToolTipText;
                            });
                            var sshManager = new SSHManager();
                            if (op == eOperation.NewFile)
                            {
                                var ext = string.Empty;

                                switch (newFileExtension)
                                {
                                    case SFTPEntities.eFileType.Python:
                                        ext = ".py";
                                        break;
                                    case SFTPEntities.eFileType.Text:
                                        ext = ".txt";
                                        break;
                                    case SFTPEntities.eFileType.Xml:
                                        ext = ".xml";
                                        break;
                                    case SFTPEntities.eFileType.Json:
                                        ext = ".json";
                                        break;
                                    case SFTPEntities.eFileType.Csv:
                                        ext = ".csv";
                                        break;
                                    case SFTPEntities.eFileType.Excel:
                                        ext = ".xlsx";
                                        break;

                                }
                                
                                var fileNameWithExt = (fileName + ext);

                                if (IsFileExists(selectedNodePath, fileNameWithExt))
                                {
                                    addFileForm.InfoMessage = "File already exists";

                                    this.PerformSafely(() =>
                                    {
                                        loader.Hide();
                                    });
                                    addFileForm.SetPanelVisible(true);
                                }
                                else
                                {
                                    addFileForm.PerformSafely(() => { addFileForm.Hide(); });

                                    var fullPath = string.Empty;
                                    treeView2.PerformSafely(() => 
                                    {
                                        if (CurrentProj.IsWindows)
                                        {
                                            fullPath = (treeView2.SelectedNode.ToolTipText + "\\" + fileNameWithExt);
                                        }
                                        else
                                        {
                                            fullPath = (treeView2.SelectedNode.ToolTipText + "/" + fileNameWithExt);
                                        }
                                    });
                                    
                                    sshManager.WriteFileBytesContentMethod(mainFrm.CurrentProj.sSHClientInfo.IPAddress, mainFrm.CurrentProj.sSHClientInfo.UserName, mainFrm.CurrentProj.sSHClientInfo.Password, fullPath, new byte[] { }, CurrentProj.IsWindows);
                                    var selectedProj = CurrentProj.ProjectName;
                                    studioConfig = studioConfig.OverrdieProjectInfo(selectedProj);
                                    FillTreeView(selectedProj);
                                    
                                    if (newFileExtension == SFTPEntities.eFileType.Json
                                                    || newFileExtension == SFTPEntities.eFileType.Python
                                                    || newFileExtension == SFTPEntities.eFileType.Text
                                                    || newFileExtension == SFTPEntities.eFileType.Xml)
                                    {
                                        mainFrm.CreateDocumentAndShow(string.Empty, fileNameWithExt, fullPath, newFileExtension, CurrentProj.IsWindows);
                                        //MessageBox.Show(fileContent);
                                    }
                                    this.PerformSafely(() =>
                                    {
                                        loader.Hide();
                                    });
                                }
                            }
                            else if (op == eOperation.NewDirectory)
                            {
                                var dirPath = (selectedNodePath + (CurrentProj.IsWindows ? "\\" : "/") + fileName);
                                if (IsDirectoryExists(dirPath))
                                {
                                    addFileForm.InfoMessage = "Directory already exists";

                                    this.PerformSafely(() =>
                                    {
                                        loader.Hide();
                                    });
                                    addFileForm.SetPanelVisible(true);
                                }
                                else
                                {
                                    addFileForm.PerformSafely(() => { addFileForm.Hide(); });

                                    sshManager.CreateDirectory(CurrentProj.sSHClientInfo.IPAddress, CurrentProj.sSHClientInfo.UserName, CurrentProj.sSHClientInfo.Password, dirPath, CurrentProj.IsWindows);
                                    var selectedProj = CurrentProj.ProjectName;
                                    studioConfig = studioConfig.OverrdieProjectInfo(selectedProj);
                                    FillTreeView(selectedProj);
                                    this.PerformSafely(() =>
                                    {
                                        loader.Hide();
                                    });
                                }
                            }
                            else if (op == eOperation.RenameFile)
                            {
                                var fileNameWithExt = fileName.Trim() + fileOrDirRenameInfo.Extension;
                                var oldNameFullPath = (CurrentProj.IsWindows ? fileOrDirRenameInfo.RootPath.Replace("/", "\\") : fileOrDirRenameInfo.RootPath) + (CurrentProj.IsWindows ? "\\" : "/") + fileOrDirRenameInfo.OldName + fileOrDirRenameInfo.Extension; 
                                if (fileOrDirRenameInfo.OldName.Equals(fileName.Trim()))
                                {
                                    this.PerformSafely(() =>
                                    {
                                        loader.Hide();
                                    });
                                    addFileForm.PerformSafely(() => { addFileForm.Hide(); });
                                }
                                else if (IsFileExists((CurrentProj.IsWindows ? fileOrDirRenameInfo.RootPath.Replace("/", "\\") : fileOrDirRenameInfo.RootPath), fileNameWithExt))
                                {
                                    addFileForm.InfoMessage = "File already exists with this name";

                                    this.PerformSafely(() =>
                                    {
                                        loader.Hide();
                                    });
                                    addFileForm.SetPanelVisible(true);
                                }
                                else
                                {
                                    addFileForm.PerformSafely(() => { addFileForm.Hide(); });

                                    var newFileNameFullPath = (fileOrDirRenameInfo.RootPath + "/" + fileNameWithExt);
                                    //Rename file
                                    sshManager.RenameFile(CurrentProj.sSHClientInfo.IPAddress, CurrentProj.sSHClientInfo.UserName, CurrentProj.sSHClientInfo.Password, oldNameFullPath, newFileNameFullPath, CurrentProj.IsWindows);
                                    var selectedProj = CurrentProj.ProjectName;
                                    studioConfig = studioConfig.OverrdieProjectInfo(selectedProj);
                                    FillTreeView(selectedProj);

                                    var tabDoc = mainFrm.FindDocumentWithPath(oldNameFullPath);
                                    ChangeDocumentheader(tabDoc, newFileNameFullPath, fileNameWithExt);
                                    this.PerformSafely(() =>
                                    {
                                        loader.Hide();
                                    });
                                    
                                    //addFileForm.PerformSafely(() => {
                                    //    addFileForm.Hide();
                                    //});
                                }
                            }
                            else if (op == eOperation.CreateCopyFile)
                            {
                                var fileNameWithExt = fileName.Trim() + fileOrDirRenameInfo.Extension;
                                var oldNameFullPath = (CurrentProj.IsWindows ? fileOrDirRenameInfo.RootPath.Replace("/", "\\") : fileOrDirRenameInfo.RootPath) + (CurrentProj.IsWindows ? "\\" : "/") + fileOrDirRenameInfo.OldName + fileOrDirRenameInfo.Extension;
                                if (IsFileExists((CurrentProj.IsWindows ? copytoPath.Replace("/", "\\") : copytoPath), fileNameWithExt))
                                {
                                    addFileForm.InfoMessage = "File already exists with this name";

                                    this.PerformSafely(() =>
                                    {
                                        loader.Hide();
                                    });
                                    addFileForm.SetPanelVisible(true);
                                }
                                else
                                {
                                    addFileForm.PerformSafely(() => { addFileForm.Hide(); });

                                    var newFileNameFullPath = (copytoPath + "/" + fileNameWithExt);
                                    //Create Copy file
                                    sshManager.CreateCopyFile(CurrentProj.sSHClientInfo.IPAddress, CurrentProj.sSHClientInfo.UserName, CurrentProj.sSHClientInfo.Password, oldNameFullPath, newFileNameFullPath, CurrentProj.IsWindows);
                                    var selectedProj = CurrentProj.ProjectName;
                                    studioConfig = studioConfig.OverrdieProjectInfo(selectedProj);
                                    FillTreeView(selectedProj);

                                    this.PerformSafely(() =>
                                    {
                                        loader.Hide();
                                    });

                                    //addFileForm.PerformSafely(() => {
                                    //    addFileForm.Hide();
                                    //});
                                }
                                //if (IsFileExists((CurrentProj.IsWindows ? fileOrDirRenameInfo.RootPath.Replace("/", "\\") : fileOrDirRenameInfo.RootPath), fileNameWithExt))
                                //{
                                //    addFileForm.InfoMessage = "File already exists with this name";

                                //    this.PerformSafely(() =>
                                //    {
                                //        loader.Hide();
                                //    });
                                //    addFileForm.SetPanelVisible(true);
                                //}
                                //else
                                //{
                                //    addFileForm.PerformSafely(() => { addFileForm.Hide(); });

                                //    var newFileNameFullPath = (fileOrDirRenameInfo.RootPath + "/" + fileNameWithExt);
                                //    //Create Copy file
                                //    sshManager.CreateCopyFile(CurrentProj.sSHClientInfo.IPAddress, CurrentProj.sSHClientInfo.UserName, CurrentProj.sSHClientInfo.Password, oldNameFullPath, newFileNameFullPath, CurrentProj.IsWindows);
                                //    var selectedProj = CurrentProj.ProjectName;
                                //    studioConfig = studioConfig.OverrdieProjectInfo(selectedProj);
                                //    FillTreeView(selectedProj);

                                //    this.PerformSafely(() =>
                                //    {
                                //        loader.Hide();
                                //    });

                                //    //addFileForm.PerformSafely(() => {
                                //    //    addFileForm.Hide();
                                //    //});
                                //}
                            }
                            else if (op == eOperation.RenameDirectory)
                            {
                                var newDirName = fileName.Trim();

                                var oldDirFullPath = (CurrentProj.IsWindows ? fileOrDirRenameInfo.RootPath.Replace("/", "\\") : fileOrDirRenameInfo.RootPath) + (CurrentProj.IsWindows ? "\\" : "/") + fileOrDirRenameInfo.OldName;
                                var newDirFullPath = (CurrentProj.IsWindows ? fileOrDirRenameInfo.RootPath.Replace("/", "\\") : fileOrDirRenameInfo.RootPath) + (CurrentProj.IsWindows ? "\\" : "/") + newDirName;
                                if (fileOrDirRenameInfo.OldName.Equals(newDirName))
                                {
                                    this.PerformSafely(() =>
                                    {
                                        loader.Hide();
                                    });
                                    addFileForm.PerformSafely(() => { addFileForm.Hide(); });
                                }
                                else if (IsDirectoryExists(newDirFullPath))
                                {
                                    addFileForm.InfoMessage = "Directory already exists with this name";

                                    this.PerformSafely(() =>
                                    {
                                        loader.Hide();
                                    });
                                    addFileForm.SetPanelVisible(true);
                                }
                                else
                                {
                                    addFileForm.PerformSafely(() => { addFileForm.Hide(); });
                                    //Rename dir
                                    sshManager.RenameDir(CurrentProj.sSHClientInfo.IPAddress, CurrentProj.sSHClientInfo.UserName, CurrentProj.sSHClientInfo.Password, oldDirFullPath, newDirFullPath, CurrentProj.IsWindows);
                                    var selectedProj = CurrentProj.ProjectName;
                                    studioConfig = studioConfig.OverrdieProjectInfo(selectedProj);
                                    FillTreeView(selectedProj);

                                    var oldDirFullPathWithSlashSufix = oldDirFullPath + "/";
                                    var tabDocs = mainFrm.FindAllDocumentsInDirectory(oldDirFullPathWithSlashSufix);
                                    foreach(var tabDoc in tabDocs)
                                    {
                                        if (tabDoc != null)
                                        {
                                            if (tabDoc is DummyDoc)
                                            {
                                                var tabDocCodeFile = (DummyDoc)tabDoc;
                                                tabDocCodeFile.PerformSafely(() =>
                                                {
                                                    tabDocCodeFile.FullPath = tabDocCodeFile.FullPath.Replace(oldDirFullPath, newDirFullPath);
                                                    tabDocCodeFile.ToolTipText = tabDocCodeFile.ToolTipText.Replace(oldDirFullPath, newDirFullPath);
                                                });
                                            }
                                            else if (tabDoc is ReoGridEditor)
                                            {
                                                var tabDocExcelFile = (ReoGridEditor)tabDoc;
                                                tabDocExcelFile.PerformSafely(() =>
                                                {
                                                    tabDocExcelFile.CurrentFilePath = tabDocExcelFile.CurrentFilePath.Replace(oldDirFullPath, newDirFullPath);
                                                    tabDocExcelFile.ToolTipText = tabDocExcelFile.ToolTipText.Replace(oldDirFullPath, newDirFullPath);
                                                });
                                            }
                                        }
                                    }
                                    
                                    this.PerformSafely(() =>
                                    {
                                        loader.Hide();
                                    });
                                }
                            }

                            RefreshContent();
                        }
                        catch (Exception ex)
                        {
                            //MessageBox.Show(ex.Message, "Error!");
                            this.Alert(ex.Message, Form_Alert.enmType.Error);

                            this.PerformSafely(() =>
                            {
                                loader.Hide();
                            });
                        }
                    }).Start();
            };

            moveFileForm.SaveCliked = (string newFilePath) =>
            {
                new Task(() =>
                {
                    this.PerformSafely(() =>
                    {
                        loader.ShowDialog(this);
                    });
                }).Start();

                new Task(() =>
                {
                    try
                    {
                        var selectedNodePath = string.Empty;
                        string selectedProj = CurrentProj.ProjectName;
                        treeView2.PerformSafely(() =>
                        {
                            selectedNodePath = treeView2.SelectedNode.ToolTipText;
                        });
                        var parentDirectory = Path.GetPathRoot(selectedNodePath);

                        var oldFilePath = CurrentProj.IsWindows ? System.IO.Path.GetDirectoryName(selectedNodePath) : System.IO.Path.GetDirectoryName(selectedNodePath).Replace("\\", "/");
                        var fileNameWithExt = System.IO.Path.GetFileName(selectedNodePath);

                        var oldFileNameFullPath = (oldFilePath + (CurrentProj.IsWindows ? "\\" : "/") + fileNameWithExt);
                        var newFileNameFullPath = (newFilePath + (CurrentProj.IsWindows ? "\\" : "/") + fileNameWithExt);

                        if (IsFileExists(newFilePath, fileNameWithExt))
                        {
                            moveFileForm.SetError("File already exist in same folder");
                        }
                        else
                        {
                            SSHManager sshManager = new SSHManager();
                            //Move file
                            sshManager.RenameFile(CurrentProj.sSHClientInfo.IPAddress, CurrentProj.sSHClientInfo.UserName, CurrentProj.sSHClientInfo.Password, oldFileNameFullPath, newFileNameFullPath, CurrentProj.IsWindows);

                            studioConfig = studioConfig.OverrdieProjectInfo(selectedProj);
                            FillTreeView(selectedProj);

                            var tabDoc = mainFrm.FindDocumentWithPath(oldFileNameFullPath);
                            ChangeDocumentheader(tabDoc, newFileNameFullPath, fileNameWithExt);

                            ////
                            moveFileForm.ResetControl();

                            moveFileForm.PerformSafely(() =>
                            {
                                moveFileForm.Hide();
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        moveFileForm.ResetControl();
                        //MessageBox.Show(ex.Message, "Error!");
                        this.Alert(ex.Message, Form_Alert.enmType.Error);
                    }
                    finally
                    {
                        this.PerformSafely(() =>
                        {
                            loader.Hide();
                        });
                    }
                }).Start();
            };

        }

        public void Alert(string msg, Form_Alert.enmType type)
        {
            Task t = new Task(() => {
                this.PerformSafely(() => {
                    Form_Alert frm = new Form_Alert();
                    frm.BringToFront();
                    frm.showAlert(msg, type, this);
                });
            });
            t.Start();
        }

        private void MainFrm_OnOpenMenuChanged(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = comboBox1.FindStringExact(sender.ToString());
            comboBox1_SelectedIndexChanged(sender, e);
        }

        private void ChangeDocumentheader(IDockContent tabDoc, string newFileNameFullPath, string fileNameWithExt)
        {
            if (tabDoc != null)
            {
                if (tabDoc is DummyDoc)
                {
                    var tabDocCodeFile = (DummyDoc)tabDoc;
                    tabDocCodeFile.PerformSafely(() =>
                    {
                        tabDocCodeFile.FileName = fileNameWithExt;
                        tabDocCodeFile.TabText = fileNameWithExt;

                        tabDocCodeFile.FullPath = newFileNameFullPath;
                        tabDocCodeFile.ToolTipText = newFileNameFullPath;
                    });


                    if (tabDocCodeFile.outputWindow != null)
                    {
                        tabDocCodeFile.outputWindow.PerformSafely(() =>
                        {
                            tabDocCodeFile.outputWindow.TabText = (fileNameWithExt + " " + "Output");
                        });

                    }
                }
                else if (tabDoc is ReoGridEditor)
                {
                    var tabDocExcelFile = (ReoGridEditor)tabDoc;
                    tabDocExcelFile.PerformSafely(() =>
                    {
                        tabDocExcelFile.CurrentFilePath = newFileNameFullPath;
                        tabDocExcelFile.Text = fileNameWithExt;

                        tabDocExcelFile.ToolTipText = newFileNameFullPath;
                    });
                }
            }
        }
        protected override void OnRightToLeftLayoutChanged(EventArgs e)
        {
            treeView2.RightToLeftLayout = RightToLeftLayout;
        }

        private async void SolutionExplorer_Load(object sender, EventArgs e)
        {
            Task t = new Task(() =>
            {
                comboBox1.PerformSafely(() =>
                {
                    comboBox1.DataSource = studioConfig.projectInfoList.Select(c => c.ProjectName).ToList();
                    currentSelectedIndex = comboBox1.SelectedIndex;
                    comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;

                });

                FillTreeView(comboBox1.Items[0].ToString());
            });
            t.Start();
            //await t;


        }

        void AddNodes(TreeNode treeNode, List<SFTPEntities.DirectoryOrFile> files, bool showFiles = true, 
            Tuple<bool,bool,Dictionary<string, string>> tuple = null, string dirName = "")
        {
            if (files.Count > 0)
            {
                foreach (var fl in files)
                {
                    var imageIndex = 0;

                    SFTPEntities.eFileType eFType = GetFileType(fl.FullPath);
                    switch (eFType)
                    {
                        case eFileType.Python:
                            imageIndex = 1;
                            break;
                        case eFileType.Excel:
                        case eFileType.Csv:
                            imageIndex = 2;
                            break;
                        case eFileType.Json:
                            imageIndex = 3;
                            break;
                        case eFileType.Xml:
                            imageIndex = 4;
                            break;
                        case eFileType.Text:
                            imageIndex = 5;
                            break;
                        case eFileType.UnKnown:
                            imageIndex = 6;
                            break;

                    }
                    if (!fl.IsDirectory)
                    {
                        if (showFiles)
                        {
                            TreeNode tr = new TreeNode();
                            if (tuple != null)
                            {
                                if (tuple.Item1 == false)
                                {
                                    treeView2.PerformSafely(() => {
                                        treeView2.ImageList = null;
                                    });
                                    tr = new TreeNode() { Text = fl.Name, ToolTipText = fl.FullPath, Tag = false };
                                }
                                else
                                {
                                    treeView2.PerformSafely(() => {
                                        treeView2.ImageList = imageList2;
                                    });
                                    if (tuple.Item2 == true)
                                    {
                                        //code to check the file
                                        if (tuple.Item3.Keys.Contains(dirName +"/"+ fl.Name))
                                        {
                                            string strValue = tuple.Item3[dirName + "/" + fl.Name];
                                            switch (strValue)
                                            {
                                                case "NEW":
                                                    tr = new TreeNode() { Text = fl.Name, ToolTipText = fl.FullPath, Tag = false, ImageIndex = 2, SelectedImageIndex = 2 };
                                                    break;
                                                case "UPDATE":
                                                    tr = new TreeNode() { Text = fl.Name, ToolTipText = fl.FullPath, Tag = false, ImageIndex = 1, SelectedImageIndex = 1 };
                                                    break;
                                                case "DELETE":
                                                    break;
                                            }
                                        }
                                        else
                                            tr = new TreeNode() { Text = fl.Name, ToolTipText = fl.FullPath, Tag = false, ImageIndex = 0, SelectedImageIndex = 0 };
                                    }
                                    else
                                    {
                                        tr = new TreeNode() { Text = fl.Name, ToolTipText = fl.FullPath, Tag = false, ImageIndex = 0, SelectedImageIndex = 0 };
                                    }
                                }
                            }
                            else
                            {
                                tr = new TreeNode() { Text = fl.Name, ToolTipText = fl.FullPath, Tag = false };
                            }
                            tr.ContextMenuStrip = cmsFile;

                            //treeNode.ImageIndex = imageIndex;
                            treeNode.Nodes.Add(tr);
                        }
                    }
                    else
                    {
                        TreeNode tr = new TreeNode() { Text = fl.Name, ToolTipText = fl.FullPath, Tag = true };
                        if(showFiles)
                        {
                            tr.ContextMenuStrip = cmsDirectory;
                        }
                        //treeNode.ImageIndex = 7;
                        treeNode.Nodes.Add(tr);
                        AddNodes(tr, fl.files, showFiles, tuple, fl.Name);
                    }
                }
            }
        }

        void TraverseDirsToFindFile(DirectoryOrFile dirRoot, string pathToMatch, string fileNameToFound, ref bool isFileFound)
        {
            if (dirRoot.FullPath.Equals(pathToMatch))
            {
                if (dirRoot.files.Where(c => !c.IsDirectory).Count(c => c.Name.Equals(fileNameToFound)) > 0)
                {
                    isFileFound = true;
                }
            }
            else
            {
                foreach (var dir in dirRoot.files.Where(c => c.IsDirectory))
                {
                    if (pathToMatch.Equals(dir.FullPath))
                    {
                        if (dir.files.Where(c => !c.IsDirectory).Count(c => c.Name.Equals(fileNameToFound)) > 0)
                        {
                            isFileFound = true;
                        }
                    }
                    else
                    {
                        TraverseDirsToFindFile(dir, pathToMatch, fileNameToFound, ref isFileFound);
                    }
                }
            }
            
        }

        void TraverseDirsToFindDirectory(DirectoryOrFile dirRoot, string pathToMatch, ref bool isDirFound)
        {
            foreach (var dir in dirRoot.files.Where(c => c.IsDirectory))
            {
                if (pathToMatch.Equals(dir.FullPath))
                {
                    isDirFound = true;
                }
                else
                {
                    TraverseDirsToFindDirectory(dir, pathToMatch, ref isDirFound);
                }
            }
        }

        bool IsFileExists(string path, string fileName)
        {
            var selectedProj = CurrentProj.ProjectName;
            var isFileFound = false;
            var proj = studioConfig.projectInfoList.First(c => c.ProjectName == selectedProj).DirectoryInfo;

            TraverseDirsToFindFile(proj, path, fileName, ref isFileFound);

            return isFileFound;
        }

        bool IsDirectoryExists(string dirPath)
        {
            var selectedProj = CurrentProj.ProjectName;
            var isDirFound = false;

            var proj = studioConfig.projectInfoList.First(c => c.ProjectName == selectedProj).DirectoryInfo;
            TraverseDirsToFindDirectory(proj, dirPath, ref isDirFound);
            return isDirFound;
        }

        void FillTreeView(string proName)
        {
            CurrentProj = studioConfig.projectInfoList.First(c => c.ProjectName == proName);

            GitRepositoryManager gitRepositoryManager = new GitRepositoryManager(CurrentProj.GitUsername,
                            CurrentProj.GitPassword,
                            CurrentProj.GitRepoURL,
                            CurrentProj.ProjectPath);
            var hasUnCommit = gitRepositoryManager.HasUncommittedChanges();

            var filesList = CurrentProj.DirectoryInfo.files;
            var treeNodes = new List<TreeNode>();
            foreach (var fileOrDir in filesList.OrderBy(c => c.Name))
            {
                if (fileOrDir.IsDirectory)
                {
                    var treeNode = new TreeNode() { Text = fileOrDir.Name, ToolTipText = fileOrDir.FullPath, Tag = false };
                    treeNode.ContextMenuStrip = cmsDirectory;
                    //treeNode.NodeFont = new Font(treeView2.Font.FontFamily, 9, FontStyle.Regular);
                    treeNodes.Add(treeNode);
                    //treeNode.ImageIndex = 7;
                    AddNodes(treeNode, fileOrDir.files, true, hasUnCommit, fileOrDir.Name);
                }
                else
                {
                    TreeNode treeNode = new TreeNode();
                    if (hasUnCommit.Item1 == false)
                    {
                        treeView2.PerformSafely(() => {
                            treeView2.ImageList = null;
                        });
                        treeNode = new TreeNode() { Text = fileOrDir.Name, ToolTipText = fileOrDir.FullPath, Tag = false };
                    }
                    else
                    {
                        treeView2.PerformSafely(() => {
                            treeView2.ImageList = imageList2;
                        });
                        if (hasUnCommit.Item2 == true)
                        {
                            //code to check the file
                            if(hasUnCommit.Item3.Keys.Contains(fileOrDir.Name))
                            {
                                string strValue = hasUnCommit.Item3[fileOrDir.Name];
                                switch (strValue)
                                {
                                    case "NEW":
                                        treeNode = new TreeNode() { Text = fileOrDir.Name, ToolTipText = fileOrDir.FullPath, Tag = false, ImageIndex = 2, SelectedImageIndex = 2 };
                                        break;
                                    case "UPDATE":
                                        treeNode = new TreeNode() { Text = fileOrDir.Name, ToolTipText = fileOrDir.FullPath, Tag = false, ImageIndex = 1, SelectedImageIndex = 1 };
                                        break;
                                    case "DELETE":
                                        break;
                                }
                            }
                            else
                                treeNode = new TreeNode() { Text = fileOrDir.Name, ToolTipText = fileOrDir.FullPath, Tag = false, ImageIndex = 0, SelectedImageIndex = 0 };
                        }
                        else
                        {
                            treeNode = new TreeNode() { Text = fileOrDir.Name, ToolTipText = fileOrDir.FullPath, Tag = false, ImageIndex = 0, SelectedImageIndex = 0 };
                        }
                    }

                    //var treeNode = new TreeNode() { Text = fileOrDir.Name, ToolTipText = fileOrDir.FullPath, Tag = false };
                    treeNode.ContextMenuStrip = cmsFile;
                    ////treeNode.NodeFont = new Font(treeView2.Font.FontFamily, 9, FontStyle.Regular);
                    treeNodes.Add(treeNode);
                }
            }

            treeView2.PerformSafely(() =>
            {
                //this.Controls.Remove(loader);
                System.Windows.Forms.TreeNode rootTreeNode = new System.Windows.Forms.TreeNode() { Text = CurrentProj.DirectoryInfo.FullPath, ToolTipText = CurrentProj.DirectoryInfo.FullPath };
                treeView2.Nodes.Clear();
                rootTreeNode.Nodes.AddRange(treeNodes.ToArray());
                rootTreeNode.ExpandAll();
                rootTreeNode.ContextMenuStrip = cmsRootPath;
                //rootTreeNode.NodeFont = new Font(treeView2.Font.FontFamily, 10, FontStyle.Regular);
                treeView2.Nodes.Add(rootTreeNode);
                //treeView2.Dock = DockStyle.Fill;
                treeView2.Visible = true;
            });

            mainFrm.CurrentProj = CurrentProj;
            mainFrm.EnableDisableControls();
        }

        void FillTreeViewForMoveFile(string proName)
        {        
            var listproj = studioConfig.projectInfoList;
            if(listproj!=null && listproj.Count > 0)
            {
                //------------------------------------------------------------------------------------
                System.Windows.Forms.TreeView rootTreeNode = new System.Windows.Forms.TreeView();
                foreach (var proj in listproj)
                {
                    var filesList = proj.DirectoryInfo.files;
                    var treeNodes = new List<TreeNode>();
                    foreach (var fileOrDir in filesList.OrderBy(c => c.Name))
                    {
                        if (fileOrDir.IsDirectory)
                        {
                            var treeNode = new TreeNode() { Text = fileOrDir.Name, ToolTipText = fileOrDir.FullPath, Tag = true };
                            treeNodes.Add(treeNode);
                            AddNodes(treeNode, fileOrDir.files, false);
                        }
                    }

                    System.Windows.Forms.TreeNode subrootTreeNode = new System.Windows.Forms.TreeNode() { Text = proj.ProjectName + " (" + proj.DirectoryInfo.FullPath + ")", ToolTipText = proj.DirectoryInfo.FullPath };
                    subrootTreeNode.Nodes.AddRange(treeNodes.ToArray());
                    rootTreeNode.Nodes.Add(subrootTreeNode);
                }

                rootTreeNode.ExpandAll();
                moveFileForm.ResetNode(rootTreeNode);
            }


            //var proj = studioConfig.projectInfoList.First(c => c.ProjectName == proName).DirectoryInfo;
            //var filesList = proj.files;

            //var treeNodes = new List<TreeNode>();
            //foreach (var fileOrDir in filesList.OrderBy(c => c.Name))
            //{
            //    if (fileOrDir.IsDirectory)
            //    {
            //        var treeNode = new TreeNode() { Text = fileOrDir.Name, ToolTipText = fileOrDir.FullPath, Tag = true };
            //        treeNodes.Add(treeNode);
            //        AddNodes(treeNode, fileOrDir.files, false);
            //    }
            //}

            //System.Windows.Forms.TreeNode rootTreeNode = new System.Windows.Forms.TreeNode() { Text = proj.FullPath, ToolTipText = proj.FullPath };
            //rootTreeNode.Nodes.AddRange(treeNodes.ToArray());
            //rootTreeNode.ExpandAll();
            //moveFileForm.ResetNode(rootTreeNode);
        }

        void FillTreeViewForCopyFile()
        {
            var listproj = studioConfig.projectInfoList;
            if (listproj != null && listproj.Count > 0)
            {
                //------------------------------------------------------------------------------------
                System.Windows.Forms.TreeView rootTreeNode = new System.Windows.Forms.TreeView();
                foreach (var proj in listproj)
                {
                    var filesList = proj.DirectoryInfo.files;
                    var treeNodes = new List<TreeNode>();
                    foreach (var fileOrDir in filesList.OrderBy(c => c.Name))
                    {
                        if (fileOrDir.IsDirectory)
                        {
                            var treeNode = new TreeNode() { Text = fileOrDir.Name, ToolTipText = fileOrDir.FullPath, Tag = true };
                            treeNodes.Add(treeNode);
                            AddNodes(treeNode, fileOrDir.files, false);
                        }
                    }

                    System.Windows.Forms.TreeNode subrootTreeNode = new System.Windows.Forms.TreeNode() { Text = proj.ProjectName + " (" + proj.DirectoryInfo.FullPath +")", ToolTipText = proj.DirectoryInfo.FullPath };
                    subrootTreeNode.Nodes.AddRange(treeNodes.ToArray());
                    rootTreeNode.Nodes.Add(subrootTreeNode);
                }

                rootTreeNode.ExpandAll();
                addFileForm.ResetNode(rootTreeNode);
            }
        }

        private SFTPEntities.eFileType GetFileType(string nodeText)
        {
            var ext = Path.GetExtension(nodeText).ToLower().Replace(".", string.Empty);
            SFTPEntities.eFileType eFType;
            switch (ext)
            {
                case "py":
                    eFType = SFTPEntities.eFileType.Python;
                    break;
                case "txt":
                    eFType = SFTPEntities.eFileType.Text;
                    break;
                case "xml":
                    eFType = SFTPEntities.eFileType.Xml;
                    break;
                case "json":
                    eFType = SFTPEntities.eFileType.Json;
                    break;
                case "csv":
                    eFType = SFTPEntities.eFileType.Csv;
                    break;
                case "xls":
                case "xlsx":
                    eFType = SFTPEntities.eFileType.Excel;
                    break;
                default:
                    eFType = SFTPEntities.eFileType.UnKnown;
                    break;
            }
            return eFType;
        }
        private async void treeView2_Click(object sender, EventArgs e)
        {
            var loader = new UCLoaderForm();
            
            new Task(() => {
                try
                {
                    TreeViewHitTestInfo info = null;
                    treeView2.PerformSafely(() =>
                    {
                        info = treeView2.HitTest(treeView2.PointToClient(Cursor.Position));
                    });
                    
                    if (info != null)
                    {
                        if (info.Node != null)
                        {
                            var nodeTag = info.Node.Tag is null ? true : (bool)info.Node.Tag;

                            if (mainFrm.FindDocumentWithPath(info.Node.ToolTipText) != null)
                            {
                                //MessageBox.Show("The document: " + info.Node.ToolTipText + " has already opened!");
                                this.Alert("The document: " + info.Node.ToolTipText + " is already opened!", Form_Alert.enmType.Info);
                            }
                            else if (nodeTag == true)
                            {
                                //Do nothing
                            }
                            else
                            {
                                SFTPEntities.eFileType eFType = GetFileType(info.Node.Text);
                                
                                if (eFType == SFTPEntities.eFileType.Json
                                    || eFType == SFTPEntities.eFileType.Python
                                    || eFType == SFTPEntities.eFileType.Text
                                    || eFType == SFTPEntities.eFileType.Xml)
                                {
                                    var fileContent = SSHManager.ReadFileContent(CurrentProj.sSHClientInfo.IPAddress, CurrentProj.sSHClientInfo.UserName, CurrentProj.sSHClientInfo.Password, info.Node.ToolTipText, CurrentProj.IsWindows);
                                    mainFrm.CreateDocumentAndShow(fileContent, info.Node.Text, info.Node.ToolTipText, eFType, CurrentProj.IsWindows);
                                    //MessageBox.Show(fileContent);
                                }
                                else if (eFType == SFTPEntities.eFileType.Csv || eFType == SFTPEntities.eFileType.Excel)
                                {
                                    mainFrm.OpenExcelCsvDoc(info.Node.Text, info.Node.ToolTipText, eFType);
                                }
                                else if (eFType == SFTPEntities.eFileType.Dir)
                                {
                                    //Dont do anything
                                }
                                else
                                {
                                    //MessageBox.Show("File format not supported!");
                                    this.Alert("File format not supported!", Form_Alert.enmType.Info);
                                }
                            }
                        }
                        
                        // MessageBox.Show(info.Node.Text, info.Node.ToolTipText);
                    }
                    loader.PerformSafely(() =>
                    {
                        loader.Hide();
                        //loader.Close();
                    });
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message);
                    this.Alert(ex.Message, Form_Alert.enmType.Error);
                }
                
            }).Start();
            loader.ShowDialog(this);
        }

        private async void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CurrentProj.ProjectName.Trim() == comboBox1.SelectedItem.ToString().Trim())
            {
                //MessageBox.Show("The same workspace is already in use..", "Already Opened", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Alert("The same workspace is already in use..", Form_Alert.enmType.Info);
                return;
            }

            if (mainFrm.HasDocuments())
            {
                if (MessageBox.Show(this, "Operation will close all opened documents, Are you sure?",
                                string.Empty, MessageBoxButtons.OKCancel)
                                == System.Windows.Forms.DialogResult.OK)
                {
                    mainFrm.CloseAllDocumentsExceptSolutionExplorer();
                    FillTreeView(comboBox1.SelectedItem.ToString());
                }
                else
                {
                    comboBox1.SelectedIndexChanged -= comboBox1_SelectedIndexChanged;
                    comboBox1.SelectedIndex = currentSelectedIndex;
                    comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
                }
            }
            else
            {
                FillTreeView(comboBox1.SelectedItem.ToString());
            }
            currentSelectedIndex = comboBox1.SelectedIndex;
        }

        public void RefreshContent()
        {
            Task t = new Task(() =>
            {
                var selectedProj = CurrentProj.ProjectName;
                try
                {
                    //Thread.Sleep(2000);
                    studioConfig = studioConfig.OverrdieProjectInfo(selectedProj);
                    FillTreeView(selectedProj);
                    this.PerformSafely(() =>
                    {
                        loader.Hide();
                        //loader.Close();
                    });
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message);
                    this.Alert(ex.Message, Form_Alert.enmType.Error);
                }

            });
            t.Start();
        }
        private async void SolutionExplorer_Activated(object sender, EventArgs e)
        {

        }

        private async void cmsRootPath_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            Task mnTsk = new Task(() =>
            {
            try
            {
                var topNodePath = string.Empty;
                treeView2.PerformSafely(() =>
                {
                    topNodePath = treeView2.TopNode.ToolTipText;
                });

                    switch (e.ClickedItem.Text)
                    {
                        case "Refresh":
                            RefreshContent();
                            this.PerformSafely(() =>
                            {
                                loader.ShowDialog(this);
                            });
                            break;
                        case "Upload File":
                            UploadFile();
                            break;
                        case "New Directory":
                            CreateDirectory();
                            break;
                        case "Commit":
                            this.PerformSafely(() =>
                            {
                                gitCommitForm = new GitCommit(this.CurrentProj, topNodePath);
                                if (gitCommitForm.ShowDialog(this) == DialogResult.OK)
                                {
                                    this.Alert("Workspace Committed successfull..", Form_Alert.enmType.Success);
                                    FillTreeView(CurrentProj.ProjectName);
                                }
                            });
                            break;
                        case "Push to Git":
                            this.PerformSafely(() =>
                            {
                                gitPushForm = new GitPush(this.CurrentProj, topNodePath, "push");
                                if (gitPushForm.ShowDialog(this) == DialogResult.OK)
                                {
                                    this.Alert("Workspace pushed to Git Successfully!", Form_Alert.enmType.Success);
                                    FillTreeView(CurrentProj.ProjectName);
                                }
                            });
                            break;
                        case "Commit && Push":
                            this.PerformSafely(() =>
                            {
                                gitCommitForm = new GitCommit(this.CurrentProj, topNodePath);
                                if (gitCommitForm.ShowDialog(this) == DialogResult.OK)
                                {
                                    gitPushForm = new GitPush(this.CurrentProj, topNodePath, "push");
                                    if (gitPushForm.ShowDialog(this) == DialogResult.OK)
                                    {
                                        FillTreeView(CurrentProj.ProjectName);
                                    }
                                }
                            });
                            break;
                        case "Create &Repository":
                            this.PerformSafely(() =>
                            {
                                //GitRepositoryManager gitRepositoryManager = new GitRepositoryManager();
                            });
                            break;
                        case "Pull from Git":
                            this.PerformSafely(() =>
                            {
                                gitPushForm = new GitPush(this.CurrentProj, topNodePath, "pull");
                                if (gitPushForm.ShowDialog(this) == DialogResult.OK)
                                {
                                    this.Alert("Workspace pulled from Git Successfully!", Form_Alert.enmType.Success);
                                    RefreshContent();
                                }
                            });
                            break;
                    }
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message, "Error!");
                    this.Alert(ex.Message, Form_Alert.enmType.Error);
                }
            });
            mnTsk.Start();
        }

        private async void ToolStripMenuItem2_DropDownItemClicked(object sender, System.Windows.Forms.ToolStripItemClickedEventArgs e)
        {
            CreateFile(e);
        }

        private void UploadFile()
        {
            new Task(() =>
            {
                try
                {
                    using (OpenFileDialog ofd = new OpenFileDialog())
                    {
                        ofd.Filter = "Files|*.py;*.json;*.xml;*.txt;*.xlsx;*.csv;";

                        this.PerformSafely(() =>
                        {
                            if (ofd.ShowDialog(this) == DialogResult.OK)
                            {
                                //OpenFileDialog.SafeFileName
                                var content = File.ReadAllBytes(ofd.FileName);
                                var sshManager = new SSHManager();
                                var path = string.Empty;
                                if (CurrentProj.IsWindows)
                                {
                                    path = (treeView2.SelectedNode.ToolTipText + "\\" + ofd.SafeFileName);
                                }
                                else
                                {
                                    path = (treeView2.SelectedNode.ToolTipText + "/" + ofd.SafeFileName);
                                }
                                
                                sshManager.WriteFileBytesContentMethod(CurrentProj.sSHClientInfo.IPAddress, CurrentProj.sSHClientInfo.UserName, CurrentProj.sSHClientInfo.Password, path, content, CurrentProj.IsWindows);
                            }
                        });
                    }
                    var selectedProj = CurrentProj.ProjectName;
                    studioConfig = studioConfig.OverrdieProjectInfo(selectedProj);
                    FillTreeView(selectedProj);
                    this.PerformSafely(() =>
                    {
                        loader.Hide();
                        //loader.Close();
                    });
                }
                catch (Exception ex)
                {
                    this.PerformSafely(() =>
                    {
                        loader.Hide();
                        //loader.Close();
                    });
                    //MessageBox.Show(ex.Message);
                    this.Alert(ex.Message, Form_Alert.enmType.Error);
                }
            }).Start();
            this.PerformSafely(() =>
            {
                loader.ShowDialog(this);
            });
        }

        private void CreateDirectory()
        {
            new Task(() =>
            {
                try
                {
                    op = eOperation.NewDirectory;
                    addFileForm.PerformSafely(() =>
                    {
                        addFileForm.Header = "Add New Directory";
                        addFileForm.ResetControl();
                        addFileForm.Size = new Size(addFileForm.Width, addFileForm.Height - addFileForm.SplitContainerControl.Panel2.Height);
                        addFileForm.SplitContainerControl.Panel2Collapsed = true;
                        addFileForm.Mode = "Add New Directory";
                    });
                    this.PerformSafely(() =>
                    {
                        loader.Hide();
                    });
                    this.PerformSafely(() =>
                    {
                        addFileForm.ShowDialog(this);
                    });
                }
                catch (Exception ex)
                {
                    this.PerformSafely(() =>
                    {
                        loader.Hide();
                        //loader.Close();
                    });
                    //MessageBox.Show(ex.Message);
                    this.Alert(ex.Message, Form_Alert.enmType.Error);
                }
            }).Start();
            this.PerformSafely(() =>
            {
                loader.ShowDialog(this);
            });
        }
        private async void cmsDirectory_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            Task t;
            switch (e.ClickedItem.Text)
            {
                case "Upload File":
                    UploadFile();
                    break;
                case "Create Directory":
                    CreateDirectory();
                    break;
                case "Rename Directory":
                    t = new Task(() =>
                    {
                        try
                        {
                            op = eOperation.RenameDirectory;
                            var selectedNodePath = string.Empty;
                            treeView2.PerformSafely(() => {
                                selectedNodePath = treeView2.SelectedNode.ToolTipText;
                            });
                            fileOrDirRenameInfo = new FileOrDirRenameInfo()
                            {
                                OldName = System.IO.Path.GetFileNameWithoutExtension(selectedNodePath),
                                Extension = string.Empty,
                                RootPath = System.IO.Path.GetDirectoryName(selectedNodePath).Replace("\\", "/")
                            };
                            addFileForm.PerformSafely(() =>
                            {
                                addFileForm.Header = "Rename Directory";
                                addFileForm.ResetControl();
                                addFileForm.TextControl = System.IO.Path.GetFileName(selectedNodePath);
                                addFileForm.TextControlSelectAll();
                                addFileForm.Size = new Size(addFileForm.Width, addFileForm.Height - addFileForm.SplitContainerControl.Panel2.Height);
                                addFileForm.SplitContainerControl.Panel2Collapsed = true;
                                addFileForm.Mode = e.ClickedItem.Text;
                            });
                            this.PerformSafely(() =>
                            {
                                loader.Hide();
                            });
                            this.PerformSafely(() =>
                            {
                                addFileForm.ShowDialog(this);
                            });
                        }
                        catch (Exception ex)
                        {
                            this.PerformSafely(() =>
                            {
                                loader.Hide();
                                //loader.Close();
                            });
                            //MessageBox.Show(ex.Message);
                            this.Alert(ex.Message, Form_Alert.enmType.Error);

                        }
                    });
                    t.Start();
                    this.PerformSafely(() =>
                    {
                        loader.ShowDialog(this);
                    });
                    break;
                case "Remove Directory":
                    new Task(() =>
                    {
                        System.Windows.Forms.DialogResult allowToDelete = DialogResult.Cancel;
                        this.PerformSafely(() =>
                        {
                            allowToDelete = MessageBox.Show(this, "Directory included files if exists, will be removed from server, Are you sure?",
                                    string.Empty, MessageBoxButtons.OKCancel);
                        });
                        if (allowToDelete == System.Windows.Forms.DialogResult.OK)
                        {
                            new Task(() =>
                            {
                                this.PerformSafely(() =>
                                {
                                    loader.ShowDialog(this);
                                });
                            }).Start();

                            var sshManager = new SSHManager();
                            var selectedNodePath = string.Empty;
                            treeView2.PerformSafely(() => {
                                selectedNodePath = treeView2.SelectedNode.ToolTipText;
                            });
                            sshManager.RemoveDirectory(CurrentProj.sSHClientInfo.IPAddress, CurrentProj.sSHClientInfo.UserName, CurrentProj.sSHClientInfo.Password, selectedNodePath, CurrentProj.IsWindows);
                            var selectedProj = CurrentProj.ProjectName;
                            studioConfig = studioConfig.OverrdieProjectInfo(selectedProj);
                            FillTreeView(selectedProj);
                            var tabDocs = mainFrm.FindAllDocumentsInDirectory(selectedNodePath);
                            foreach (var tabDoc in tabDocs)
                            {
                                if (tabDoc != null)
                                {
                                    if (tabDoc is DummyDoc)
                                    {
                                        var tabDocCodeFile = (DummyDoc)tabDoc;
                                        tabDocCodeFile.PerformSafely(() =>
                                        {
                                            tabDocCodeFile.Close();
                                        });
                                        if (tabDocCodeFile.outputWindow != null)
                                        {
                                            tabDocCodeFile.outputWindow.PerformSafely(() =>
                                            {
                                                tabDocCodeFile.outputWindow.Close();
                                            });
                                        }
                                    }
                                    else if (tabDoc is ReoGridEditor)
                                    {
                                        var tabDocExcelFile = (ReoGridEditor)tabDoc;
                                        tabDocExcelFile.PerformSafely(() => 
                                        {
                                            tabDocExcelFile.Close();
                                        });
                                    }
                                }
                            }
                            this.PerformSafely(() =>
                            {
                                loader.Hide();
                            });
                        }
                    }).Start();
                    
                    break;
            }
        }

        private async void newFileToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            op = eOperation.NewFile;
            CreateFile(e);
        }

        void CreateFile(ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Text)
            {
                case "Python":
                    newFileExtension = SFTPEntities.eFileType.Python;
                    addFileForm.Header = "Add New file (.py)";
                    break;
                case "Json":
                    newFileExtension = SFTPEntities.eFileType.Json;
                    addFileForm.Header = "Add New file (.json)";
                    break;
                case "Xml":
                    newFileExtension = SFTPEntities.eFileType.Xml;
                    addFileForm.Header = "Add New file (.xml)";
                    break;
                case "Text":
                    newFileExtension = SFTPEntities.eFileType.Text;
                    addFileForm.Header = "Add New file (.txt)";
                    break;
                case "Excel":
                    newFileExtension = SFTPEntities.eFileType.Excel;
                    addFileForm.Header = "Add New file (.xlsx)";
                    break;
                case "CSV":
                    newFileExtension = SFTPEntities.eFileType.Csv;
                    addFileForm.Header = "Add New file (.csv)";
                    break;
            }
            addFileForm.ResetControl();
            addFileForm.Size = new Size(addFileForm.Width, addFileForm.Height - addFileForm.SplitContainerControl.Panel2.Height);
            addFileForm.SplitContainerControl.Panel2Collapsed = true;
            addFileForm.Mode = "New File";
            addFileForm.ShowDialog(this);
        }

                   
        private void DownloadFile(string selectedNodePath)
        {
            new Task(() =>
            {
                try
                {
                    using (SaveFileDialog sf = new SaveFileDialog()) 
                    {
                        this.PerformSafely(() => {
                            sf.Title = "Save As";
                            sf.RestoreDirectory = true;
                            sf.CheckFileExists = false;
                            sf.CheckPathExists = true;
                            sf.FileName = System.IO.Path.GetFileNameWithoutExtension(selectedNodePath);
                            sf.DefaultExt = System.IO.Path.GetExtension(selectedNodePath);
                            sf.AddExtension = true;
                            if (sf.ShowDialog() == DialogResult.OK)
                            {
                                var fileContent = SSHManager.ReadFileContent(CurrentProj.sSHClientInfo.IPAddress,
                                                    CurrentProj.sSHClientInfo.UserName,
                                                    CurrentProj.sSHClientInfo.Password,
                                                    selectedNodePath, CurrentProj.IsWindows);

                                using (var stream = new FileStream(sf.FileName,
                                    FileMode.Create,
                                    FileAccess.Write,
                                    FileShare.Write))
                                {
                                    var bytes = Encoding.UTF8.GetBytes(fileContent);
                                    stream.Write(bytes, 0, bytes.Length);
                                }
                            }
                        });
                    }

                    var selectedProj = CurrentProj.ProjectName;
                    studioConfig = studioConfig.OverrdieProjectInfo(selectedProj);
                    FillTreeView(selectedProj);
                    this.PerformSafely(() =>
                    {
                        loader.Hide();
                        //loader.Close();
                    });
                }
                catch (Exception ex)
                {
                    this.PerformSafely(() =>
                    {
                        loader.Hide();
                        //loader.Close();
                    });
                    //MessageBox.Show(ex.Message);
                    this.Alert(ex.Message, Form_Alert.enmType.Error);

                }
            }).Start();
            this.PerformSafely(() =>
            {
                loader.ShowDialog(this);
            });
        }
        private async void cmsFile_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            Task mnTsk = new Task(() => 
            {
                try
                {
                    var sshManager = new SSHManager();
                    var selectedNodePath = string.Empty;
                    var topNodePath = string.Empty;
                    var selectedProj = string.Empty;
                    treeView2.PerformSafely(() => {
                        selectedNodePath = treeView2.SelectedNode.ToolTipText;
                        topNodePath = treeView2.TopNode.ToolTipText;
                    });
                    switch (e.ClickedItem.Text)
                    {
                        case "Delete":
                            System.Windows.Forms.DialogResult allowToDelete = DialogResult.Cancel;
                            this.PerformSafely(() =>
                            {
                                allowToDelete = MessageBox.Show(this, "File will be deleted from server, Are you sure?",
                                        string.Empty, MessageBoxButtons.OKCancel);
                            });
                            if (allowToDelete == System.Windows.Forms.DialogResult.OK)
                            {
                                new Task(() =>
                                {
                                    this.PerformSafely(() =>
                                    {
                                        loader.ShowDialog(this);
                                    });
                                }).Start();

                                Task t1 = new Task(() =>
                                {
                                    sshManager.RemoveFile(CurrentProj.sSHClientInfo.IPAddress, CurrentProj.sSHClientInfo.UserName, CurrentProj.sSHClientInfo.Password, selectedNodePath, CurrentProj.IsWindows);
                                    selectedProj = CurrentProj.ProjectName;
                                    
                                    studioConfig = studioConfig.OverrdieProjectInfo(selectedProj);
                                    FillTreeView(selectedProj);
                                    mainFrm.CloseDocumentTab(selectedNodePath);
                                    this.PerformSafely(() =>
                                    {
                                        loader.Hide();
                                    });
                                });
                                t1.Start();
                            }
                            break;
                        case "Rename":
                            op = eOperation.RenameFile;

                            fileOrDirRenameInfo = new FileOrDirRenameInfo()
                            {
                                OldName = System.IO.Path.GetFileNameWithoutExtension(selectedNodePath),
                                Extension = System.IO.Path.GetExtension(selectedNodePath),
                                RootPath = System.IO.Path.GetDirectoryName(selectedNodePath).Replace("\\", "/")
                            };
                           
                            addFileForm.PerformSafely(() =>
                            {
                                addFileForm.Header = "Rename File (" + System.IO.Path.GetExtension(selectedNodePath) + ")";
                                addFileForm.ResetControl();
                                addFileForm.TextControl = System.IO.Path.GetFileNameWithoutExtension(selectedNodePath);
                                addFileForm.TextControlSelectAll();
                                addFileForm.Size = new Size(addFileForm.Width, addFileForm.Height - addFileForm.SplitContainerControl.Panel2.Height);
                                addFileForm.SplitContainerControl.Panel2Collapsed = true;
                                addFileForm.Mode = e.ClickedItem.Text;
                            });
                            this.PerformSafely(() =>
                            {
                                loader.Hide();
                            });
                            this.PerformSafely(() =>
                            {
                                addFileForm.ShowDialog(this);
                            });
                            break;
                        case "Create Copy":
                            op = eOperation.CreateCopyFile;

                            fileOrDirRenameInfo = new FileOrDirRenameInfo()
                            {
                                OldName = System.IO.Path.GetFileNameWithoutExtension(selectedNodePath),
                                Extension = System.IO.Path.GetExtension(selectedNodePath),
                                RootPath = System.IO.Path.GetDirectoryName(selectedNodePath).Replace("\\", "/")
                            };
                            addFileForm.ResetControl();
                            addFileForm.PerformSafely(() =>
                            {
                                addFileForm.Header = "Create Copy File (" + System.IO.Path.GetExtension(selectedNodePath) + ")";
                                addFileForm.ResetControl();
                                addFileForm.TextControl = System.IO.Path.GetFileNameWithoutExtension(selectedNodePath) + "_Copy";
                                addFileForm.TextControlSelectAll();
                                if(addFileForm.SplitContainerControl.Panel2Collapsed == true)
                                {
                                    addFileForm.SplitContainerControl.Panel2Collapsed = false;
                                    addFileForm.Size = new Size(addFileForm.Width, addFileForm.Height + addFileForm.SplitContainerControl.Panel2.Height);
                                }
                                addFileForm.Mode = e.ClickedItem.Text;
                            });
                            addFileForm.PerformSafely(() =>
                            {
                                FillTreeViewForCopyFile();
                            });
                            this.PerformSafely(() =>
                            {
                                loader.Hide();
                            });
                            this.PerformSafely(() =>
                            {
                                addFileForm.ShowDialog(this);
                            });
                            break;
                        case "Move":
                            selectedProj = CurrentProj.ProjectName;
                            moveFileForm.ResetControl();

                            moveFileForm.PerformSafely(() =>
                            {
                                FillTreeViewForMoveFile(selectedProj);
                            });
                            this.PerformSafely(() =>
                            {
                                loader.Hide();
                            });
                            this.PerformSafely(() =>
                            {
                                moveFileForm.ShowDialog(this);
                            });
                            break;
                        case "Download":
                            //code to download the selected file into local
                            DownloadFile(selectedNodePath);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message, "Error!");
                    this.Alert(ex.Message, Form_Alert.enmType.Error);

                }
            });
            mnTsk.Start();
        }

        private void treeView2_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            treeView2.SelectedNode = e.Node;
        }
    }
}