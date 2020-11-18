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
    public partial class GitPush : Form
    {
        GitRepositoryManager gitRepositoryManager;
        ProjectInfo _CurrentProj;
        string _strSelectedNodePath;
        string _pushOrPull;
        UCLoaderForm loader = new UCLoaderForm();

        public GitPush(ProjectInfo CurrentProj, string strSelectedNodePath, string strPushOrPull)
        {
            _CurrentProj = CurrentProj;
            _strSelectedNodePath = strSelectedNodePath;
            _pushOrPull = strPushOrPull;
            InitializeComponent();

            btnPush.PerformSafely(() => {
                if (_pushOrPull.ToLower() == "push")
                {
                    btnPush.Text = "Push";
                }
                else
                    btnPush.Text = "Pull";
            });

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

        private void btnPush_Click(object sender, EventArgs e)
        {
            if (cmbBranches.SelectedValue.ToString().Trim().Length == 0)
            {
                this.Alert("Please select branch..", Form_Alert.enmType.Info);
            }
            else
            {
                new Task(() =>
                {
                    try
                    {
                        #region [Push&Pull]
                        string strSelectedBranch = string.Empty;
                        cmbBranches.PerformSafely(() => {
                            strSelectedBranch = cmbBranches.SelectedValue.ToString().Trim();
                        });

                        //code to get the commit message
                        gitRepositoryManager = new GitRepositoryManager(_CurrentProj.GitUsername,
                        _CurrentProj.GitPassword,
                        _CurrentProj.GitRepoURL,
                        _strSelectedNodePath);
                        if (_pushOrPull.ToLower() == "push")
                            gitRepositoryManager.PushCommits(strSelectedBranch, _strSelectedNodePath, _CurrentProj.GitEmail);
                        else
                        {
                            gitRepositoryManager.Pull(strSelectedBranch, _strSelectedNodePath, _CurrentProj.GitEmail);
                        }

                        this.PerformSafely(() =>
                        {
                            this.DialogResult = DialogResult.OK;
                            loader.Hide();
                            this.Hide();
                        });
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        this.Alert(ex.Message, Form_Alert.enmType.Error);
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            Task t = new Task(() =>
            {
                this.PerformSafely(() =>
                {
                    //txtCommitMessage.Text = string.Empty;
                    this.Hide();
                });
            });
            t.Start();
        }

        private void GitPush_Load(object sender, EventArgs e)
        {
            Task t = new Task(() =>
            {
                BindBranches();
            });
            t.Start();
        }

        List<string> GetBranchesList()
        {
            //code to get the commit message
            gitRepositoryManager = new GitRepositoryManager(_CurrentProj.GitUsername,
                _CurrentProj.GitPassword,
                _CurrentProj.GitRepoURL,
                _strSelectedNodePath);
            var listBranches = gitRepositoryManager.GetBranches();

            return listBranches.ToList();
        }

        void BindBranches()
        {
            if (GetBranchesList() != null && GetBranchesList().Count > 0)
            {
                cmbBranches.PerformSafely(() => {
                    cmbBranches.DataSource = GetBranchesList();
                });
            }
        }

        private void tsBtnAddNewBranch_Click(object sender, EventArgs e)
        {
            //code to view pnlNewBranch
            pnlNewBranch.PerformSafely(() =>
            {
                pnlNewBranch.Visible = true;
            });
        }

        private void btnSaveNewBranch_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtBranch.Text.Trim().Length == 0)
                {
                    this.Alert("Please mention the branch name", Form_Alert.enmType.Info);
                }
                else if (GetBranchesList().Contains(txtBranch.Text.Trim()))
                {
                    this.Alert("Branch is already exists.", Form_Alert.enmType.Info);
                }
                else
                {
                    new Task(() =>
                    {
                        //code to create the new branch
                        gitRepositoryManager = new GitRepositoryManager(_CurrentProj.GitUsername,
                                _CurrentProj.GitPassword,
                                _CurrentProj.GitRepoURL,
                                _strSelectedNodePath);
                        string strNewBranch = string.Empty;
                        txtBranch.PerformSafely(() => {
                            strNewBranch = txtBranch.Text.Trim();
                        });
                        if (gitRepositoryManager.CreateNewBranch(strNewBranch))
                        {
                            cmbBranches.PerformSafely(() => {
                                BindBranches();
                                cmbBranches.SelectedItem = strNewBranch;
                            });
                            ResetAddNewBranch();
                        }

                        this.PerformSafely(() =>
                        {
                            loader.Hide();
                        });
                    }).Start();
                    this.PerformSafely(() =>
                    {
                        loader.ShowDialog(this);
                    });
                }
            }
            catch (Exception ex)
            {
                this.Alert(ex.Message, Form_Alert.enmType.Error);
            }
        }

        void ResetAddNewBranch()
        {
            pnlNewBranch.PerformSafely(() =>
            {
                pnlNewBranch.Visible = false;
            });

            txtBranch.PerformSafely(() => {
                txtBranch.Text = string.Empty;
            });
        }

        private void btnCancelAddNewBranch_Click(object sender, EventArgs e)
        {
            ResetAddNewBranch();
        }
    }
}
