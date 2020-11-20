using CustomControls;
using DockSample.lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UIFunctionality.Common;

namespace DockSample.Controls
{
    public partial class GitCommit : Form
    {
        GitRepositoryManager gitRepositoryManager;
        ProjectInfo _CurrentProj;
        string _strSelectedNodePath;
        UCLoaderForm loader = new UCLoaderForm();

        public GitCommit(ProjectInfo CurrentProj, string strSelectedNodePath)
        {
            _CurrentProj = CurrentProj;
            _strSelectedNodePath = strSelectedNodePath;
            InitializeComponent();
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            Task t = new Task(() =>
            {
                this.PerformSafely(() =>
                {
                    txtCommitMessage.Text = string.Empty;
                    this.Hide();
                });
            });
            t.Start();
        }

        private void btnCommit_Click(object sender, EventArgs e)
        {
            if (txtCommitMessage.Text.Trim().Length == 0)
            {
                this.Alert("Please enter commit message..", Form_Alert.enmType.Error);
            }
            else if (chkNewBranch.Checked && txtBranchName.Text.Trim().Length == 0)
            {
                this.Alert("Please enter branch name..", Form_Alert.enmType.Error);
            }
            else if (chkNewBranch.Checked && GetBranchesList().Contains(txtBranchName.Text.Trim()))
            {
                this.Alert("Branch is already exists.", Form_Alert.enmType.Info);
            }
            else
            {
                new Task(() =>
                {
                    try
                    {
                        //code to get the commit message
                        gitRepositoryManager = new GitRepositoryManager(_CurrentProj.GitUsername,
                            _CurrentProj.GitPassword,
                            _CurrentProj.GitRepoURL,
                            _strSelectedNodePath);
                        if (chkNewBranch.Checked)
                        {
                            gitRepositoryManager.CreateNewLocalBranch(txtBranchName.Text.Trim());
                        }
                        
                        //gitRepositoryManager.CommitAllChanges(txtCommitMessage.Text.Trim(), _strSelectedNodePath, _CurrentProj.GitEmail);
                        gitRepositoryManager.CommitToBranch(txtCommitMessage.Text.Trim(), _strSelectedNodePath, txtBranchName.Text.Trim(), _CurrentProj.GitEmail);

                        txtCommitMessage.PerformSafely(() => {
                            txtCommitMessage.Text = string.Empty;
                        });
                        BindBranchName();
                        this.PerformSafely(() =>
                        {
                            this.DialogResult = DialogResult.OK;
                            loader.Hide();
                            this.Hide();
                        });
                    }
                    catch (Exception ex)
                    {
                        this.Alert(ex.Message, Form_Alert.enmType.Error);
                        txtCommitMessage.PerformSafely(() => {
                            txtCommitMessage.Text = string.Empty;
                        });
                        this.PerformSafely(() =>
                        {
                            loader.Hide();
                            //this.Hide();
                        });
                    }
                }).Start();
                this.PerformSafely(() =>
                {
                    loader.ShowDialog(this);
                });
            }
        }

        private void GitCommit_Load(object sender, EventArgs e)
        {
            //code to check the local branch name
            BindBranchName();
        }

        void BindBranchName()
        {
            GitRepositoryManager gitRepositoryManager = new GitRepositoryManager();
            var vCurrentBranch = gitRepositoryManager.GetAssociateBranch(_strSelectedNodePath);

            txtBranchName.PerformSafely(() => {
                txtBranchName.Text = vCurrentBranch;
            });
        }

        List<string> GetBranchesList()
        {
            //code to get the commit message
            gitRepositoryManager = new GitRepositoryManager();
            var listBranches = gitRepositoryManager.GetLocalBranches(_strSelectedNodePath);

            return listBranches.ToList();
        }

        //private void btnSaveNewBranch_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (txtBranch.Text.Trim().Length == 0)
        //        {
        //            this.Alert("Please mention the branch name", Form_Alert.enmType.Info);
        //        }
        //        else if (GetBranchesList().Contains(txtBranch.Text.Trim()))
        //        {
        //            this.Alert("Branch is already exists.", Form_Alert.enmType.Info);
        //        }
        //        else
        //        {
        //            new Task(() =>
        //            {
        //                //code to create the new branch
        //                gitRepositoryManager = new GitRepositoryManager(_CurrentProj.GitUsername,
        //                        _CurrentProj.GitPassword,
        //                        _CurrentProj.GitRepoURL,
        //                        _strSelectedNodePath);
        //                string strNewBranch = string.Empty;
        //                txtBranch.PerformSafely(() => {
        //                    strNewBranch = txtBranch.Text.Trim();
        //                });
        //                if (gitRepositoryManager.CreateNewBranch(strNewBranch))
        //                {
        //                    //code to bind the label
        //                    ResetAddNewBranch();
        //                }

        //                this.PerformSafely(() =>
        //                {
        //                    loader.Hide();
        //                });
        //            }).Start();
        //            this.PerformSafely(() =>
        //            {
        //                loader.ShowDialog(this);
        //            });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        this.Alert(ex.Message, Form_Alert.enmType.Error);
        //    }
        //}

        private void chkNewBranch_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                chkNewBranch.PerformSafely(() => {
                    var vIsChecked = chkNewBranch.Checked;
                    txtBranchName.Enabled = vIsChecked;

                    if (!vIsChecked)
                        BindBranchName();
                    else
                    {
                        txtBranchName.Text = string.Empty;
                        txtBranchName.Focus();
                    }
                });
            }
            catch (Exception)
            {
            }
        }
    }
}
