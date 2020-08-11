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

namespace DockSample.Controls
{
    public partial class MoveFile : Form
    {
        public MoveFile()
        {
            InitializeComponent();
        }

        public Action<string> SaveCliked;

        public TreeView CtrlTreeView
        {
            get { return treeView1; }
        }


        public void ResetNode(TreeNode rootTreeNode)
        {
            treeView1.PerformSafely(() =>
            {
                //treeView1.Nodes.Clear();
                treeView1.Nodes.Add(rootTreeNode);
            });
        }

        public string Header
        {
            get { return this.Text; }
            set { this.Text = value; }
        }

        public void ResetControl()
        {
            label2.PerformSafely(() =>
            {
                label2.Text = string.Empty;
            });
            panel1.PerformSafely(() => 
            {
                panel1.Visible = false;
            });
            treeView1.PerformSafely(() =>
            {
                treeView1.Nodes.Clear();
            });
        }

        public void SetError(string msg)
        {
            panel1.PerformSafely(() =>
            {
                panel1.Visible = true;
            });
            label2.PerformSafely(() =>
            {
                label2.Text = msg;
            });
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            this.PerformSafely(() =>
            {
                treeView1.Nodes.Clear();
                this.Hide();
            });
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            if (treeView1.SelectedNode == null)
            {
                panel1.Visible = true;
                label2.Text = "Please select a directory to move in.";
            }
            else
            {
                SaveCliked(treeView1.SelectedNode.ToolTipText);
            }
        }

        private async void MoveFile_FormClosing(object sender, FormClosingEventArgs e)
        {
            ResetControl();
        }
    }
}
