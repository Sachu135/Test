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
    public partial class AddFile : Form
    {
        public AddFile()
        {
            InitializeComponent();
        }

        public Action<string, string> SaveCliked;

        public string Header 
        {
            get { return this.Text; }
            set { this.Text = value; }
        }

        public bool IsVisible
        {
            get { return this.IsVisible; }
        }

        public void SetPanelVisible(bool visible)
        {
            panel1.PerformSafely(() => {
                panel1.Visible = visible;
            });
        }
        public string InfoMessage
        {
            get { return this.label2.Text; }
            set {
                
                label2.PerformSafely(() => {
                    label2.Text = value;
                });
                
            }
        }

        public string TextControl
        {
            get { return this.textBox1.Text; }
            set
            {
                textBox1.PerformSafely(() => {
                    textBox1.Text = value;
                });

            }
        }

        public TreeView TreeViewControl
        {
            get { return this.treeView1; }
        }

        public SplitContainer SplitContainerControl
        {
            get { return this.splitContainer1; }
        }

        public string _Mode { get; set; }
        public string Mode
        {
            get { return _Mode; }
            set { _Mode = value; }
        }

        public void TextControlSelectAll()
        {
            textBox1.SelectAll();
        }
        public void ResetControl()
        {
            textBox1.Text = string.Empty;
            panel1.Visible = false;
            label2.Text = string.Empty;
            treeView1.PerformSafely(() =>
            {
                treeView1.Nodes.Clear();
            });
        }
        private async void button2_Click(object sender, EventArgs e)
        {
            Task t = new Task(() =>
            {
                this.PerformSafely(() =>
                {
                    textBox1.Text = string.Empty;
                    treeView1.Nodes.Clear();
                    this.Hide();
                });
            });
            t.Start();
            
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            label2.Text = string.Empty;
            panel1.Visible = false;
            if (textBox1.Text.Trim().Length == 0 || textBox1.Text.Contains("."))
            {
                panel1.Visible = true;
                label2.Text = "Invalid file name (or invalid character (.) found).";
            }
            else if (treeView1.SelectedNode == null && _Mode == "Create Copy")
            {
                panel1.Visible = true;
                label2.Text = "Please select a directory to move in.";
            }
            else
            {
                SaveCliked(textBox1.Text.Trim(), (_Mode == "Create Copy") ? treeView1.SelectedNode.ToolTipText : string.Empty);
            }
        }

        private async void AddFile_Load(object sender, EventArgs e)
        {
            panel1.Visible = false;
            label2.Text = string.Empty;
        }

        public void ResetNode(TreeView rootTreeNode)
        {
            treeView1.PerformSafely(() =>
            {
                foreach (TreeNode node in rootTreeNode.Nodes)
                {
                    treeView1.Nodes.Add((TreeNode)node.Clone());
                }
            });
        }
    }
}
