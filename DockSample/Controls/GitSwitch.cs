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
    public partial class GitSwitch : Form
    {
        GitRepositoryManager gitRepositoryManager;
        ProjectInfo _CurrentProj;
        string _strSelectedNodePath;
        UCLoaderForm loader = new UCLoaderForm();

        public GitSwitch(ProjectInfo CurrentProj, string strSelectedNodePath)
        {
            _CurrentProj = CurrentProj;
            _strSelectedNodePath = strSelectedNodePath;
            InitializeComponent();

            gitRepositoryManager = new GitRepositoryManager();
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

        private void GitSwitch_Load(object sender, EventArgs e)
        {
            BindLocalBranches();
            BindBranchName();
        }

        List<string> GetLocalBranchesList()
        {
            //code to get the commit message
            var listBranches = gitRepositoryManager.GetLocalBranches(_strSelectedNodePath);
            return listBranches.ToList();
        }

        void BindLocalBranches()
        {
            var data = GetLocalBranchesList();
            var headBranch = gitRepositoryManager.GetAssociateBranch(_strSelectedNodePath);
            if (data != null && data.Count > 0)
            {
                cmbLocalBranches.PerformSafely(() => {
                    cmbLocalBranches.DataSource = data;
                    cmbLocalBranches.SelectedText = headBranch;
                });
            }
        }

        void BindBranchName()
        {
            var vCurrentBranch = gitRepositoryManager.GetAssociateBranch(_strSelectedNodePath);

            lblCurrentBranch.PerformSafely(() => {
                lblCurrentBranch.Text = vCurrentBranch;
            });
        }

        private void btnSwitch_Click(object sender, EventArgs e)
        {
            new Task(() =>
            {
                try
                {
                    string strSelectedBranch = string.Empty;
                    cmbLocalBranches.PerformSafely(() => {
                        strSelectedBranch = cmbLocalBranches.SelectedValue.ToString().Trim();
                    });

                    if (string.IsNullOrEmpty(strSelectedBranch))
                    {
                        cmbLocalBranches.PerformSafely(() => {
                            strSelectedBranch = cmbLocalBranches.SelectedValue.ToString().Trim();
                        });
                    }

                    //code to get the commit message
                    gitRepositoryManager = new GitRepositoryManager(_CurrentProj.GitUsername,
                    _CurrentProj.GitPassword,
                    _CurrentProj.GitRepoURL,
                    _strSelectedNodePath);

                    var response = gitRepositoryManager.SwitchBranch(strSelectedBranch);
                    if(response == true)
                    {
                        this.PerformSafely(() =>
                        {
                            this.DialogResult = DialogResult.OK;
                            loader.Hide();
                            this.Hide();
                        });
                    }
                    else
                    {
                        this.PerformSafely(() =>
                        {
                            this.DialogResult = DialogResult.Cancel;
                            loader.Hide();
                            this.Hide();
                        });
                    }
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            Task t = new Task(() =>
            {
                this.PerformSafely(() =>
                {
                    this.Hide();
                });
            });
            t.Start();
        }
    }
}
