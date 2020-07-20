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

        public Action<string> SaveCliked;

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

        public void TextControlSelectAll()
        {
            textBox1.SelectAll();
        }
        public void ResetControl()
        {
            textBox1.Text = string.Empty;
            panel1.Visible = false;
            label2.Text = string.Empty;
        }
        private async void button2_Click(object sender, EventArgs e)
        {
            Task t = new Task(() =>
            {
                this.PerformSafely(() =>
                {
                    textBox1.Text = string.Empty;
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
            else
            {
                SaveCliked(textBox1.Text.Trim());
            }
        }

        private async void AddFile_Load(object sender, EventArgs e)
        {
            panel1.Visible = false;
            label2.Text = string.Empty;
        }
    }
}
