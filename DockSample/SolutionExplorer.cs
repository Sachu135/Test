using DockSample.Controls;
using DockSample.lib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using UIFunctionality;
using UIFunctionality.Common;
using UIFunctionality.POCO;
using WeifenLuo.WinFormsUI.Docking;

namespace DockSample
{
    public partial class SolutionExplorer : DockContent// Form// 
    {
        StudioConfig studioConfig;
        MainForm mainFrm;
        public SolutionExplorer()
        {
            InitializeComponent();
        }

        public SolutionExplorer(StudioConfig sc, MainForm frm)
        {
            studioConfig = sc;
            mainFrm = frm;
            InitializeComponent();
        }

        protected override void OnRightToLeftLayoutChanged(EventArgs e)
        {
            treeView1.RightToLeftLayout = RightToLeftLayout;
        }

        private async void SolutionExplorer_Load(object sender, EventArgs e)
        {
            Task t = new Task(() =>
            {
                //var loader = new UCLoader();
                //loader.Dock = DockStyle.Top;
                //this.PerformSafely(() =>
                //{
                //    this.Controls.Add(loader);
                //    treeView2.Visible = false;
                //});
                var filesList = studioConfig.projectInfo.DirectoryInfo.files;

                var treeNodes = new List<TreeNode>();
                foreach (var fileOrDir in filesList.OrderBy(c => c.Name))
                {
                    if (fileOrDir.IsDirectory)
                    {
                        var treeNode = new TreeNode() { Text = fileOrDir.Name,  ToolTipText = fileOrDir.FullPath, Tag = fileOrDir };
                        treeNodes.Add(treeNode);
                        AddNodes(treeNode, fileOrDir.files);
                    }
                    else 
                    {
                        var treeNode = new TreeNode() { Text = fileOrDir.Name, ToolTipText = fileOrDir.FullPath, Tag = fileOrDir }; 
                        treeNodes.Add(treeNode);
                    }
                }

                treeView2.PerformSafely(() =>
                {
                    //this.Controls.Remove(loader);
                    System.Windows.Forms.TreeNode rootTreeNode = new System.Windows.Forms.TreeNode() { Text= studioConfig.projectInfo.DirectoryInfo.FullPath, ToolTipText = studioConfig.projectInfo.DirectoryInfo.FullPath };
                    rootTreeNode.Nodes.AddRange(treeNodes.ToArray());
                    rootTreeNode.ExpandAll();
                    treeView2.Nodes.Add(rootTreeNode);
                    treeView2.Dock = DockStyle.Fill;
                    treeView2.Visible = true;
                });
                
            });
            t.Start();
            //await t;
        }

        void AddNodes(TreeNode treeNode, List<SFTPEntities.DirectoryOrFile> files)
        {
            if (files.Count > 0)
            {
                foreach (var fl in files)
                {
                    if (!fl.IsDirectory)
                    {
                        treeNode.Nodes.Add(new TreeNode() { Text = fl.Name, ToolTipText = fl.FullPath });
                    }
                    else
                    {
                        var newDir = new TreeNode() { Text = fl.Name, ToolTipText = fl.FullPath };
                        treeNode.Nodes.Add(newDir);
                        AddNodes(newDir, fl.files);
                    }
                }
            }
        }

        private void treeView2_Click(object sender, EventArgs e)
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
                            if (mainFrm.FindDocumentWithPath(info.Node.ToolTipText) != null)
                            {
                                MessageBox.Show("The document: " + info.Node.ToolTipText + " has already opened!");
                            }
                            else 
                            {
                                var ext = Path.GetExtension(info.Node.Text).ToLower().Replace(".", string.Empty);
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

                                if (eFType == SFTPEntities.eFileType.Csv
                                    || eFType == SFTPEntities.eFileType.Json
                                    || eFType == SFTPEntities.eFileType.Python
                                    || eFType == SFTPEntities.eFileType.Text
                                    || eFType == SFTPEntities.eFileType.Xml)
                                {
                                    var fileContent = SSHManager.ReadFileContent(studioConfig.sSHClientInfo.IPAddress, studioConfig.sSHClientInfo.UserName, studioConfig.sSHClientInfo.Password, info.Node.ToolTipText);
                                    mainFrm.CreateDocumentAndShow(fileContent, info.Node.Text, info.Node.ToolTipText, eFType);
                                    //MessageBox.Show(fileContent);
                                }
                                else if (eFType == SFTPEntities.eFileType.Dir)
                                {
                                    //Dont do anything
                                }
                                else
                                {
                                    MessageBox.Show("File format not supported!");
                                }
                            }
                        }
                        
                        // MessageBox.Show(info.Node.Text, info.Node.ToolTipText);
                    }
                    loader.PerformSafely(() =>
                    {
                        loader.Hide();
                        loader.Close();
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                
            }).Start();
            loader.ShowDialog(this);
        }
    }
}