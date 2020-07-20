using DockSample.lib;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using UIFunctionality;
using UIFunctionality.Common;

namespace DockSample
{
    public partial class DummyOutputWindow : ToolWindow
    {
        public MainForm mainFrm { get; set; }
        StudioConfig studioConfig;
        public RichTextBox OutputTextControl { get; set; }
        public DummyOutputWindow()
        {
            InitializeComponent();
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)(((((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft)
                        | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight)
                        | WeifenLuo.WinFormsUI.Docking.DockAreas.DockTop)
                        | WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom)));
            this.HideOnClose = true;
            this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockBottomAutoHide;
            //this.TabText = "Output";
            //richTextBox1.Enabled = false;
            richTextBox1.ForeColor = System.Drawing.Color.FromArgb(0, 0, 255);
            studioConfig = new StudioConfig(AppDomain.CurrentDomain.BaseDirectory).GetStudioConfigFromFile();
            this.CloseButton = false;

            OutputTextControl = richTextBox1;
            this.richTextBox1.ContextMenuStrip = contextMenuStrip1;
            contextMenuStrip1.Enabled = false;
            contextMenuStrip1.ItemClicked += ContextMenuStrip1_ItemClicked;
        }

        private async void ContextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            mainFrm.OpenExcelCsvDoc(richTextBox1.SelectedText);
        }

        public DummyDoc ParentDoc { get; set; }

        private void pictureBox1_Click(object sender, System.EventArgs e)
        {
            new Task(() => {
                richTextBox1.PerformSafely(() =>
                {
                    richTextBox1.Text = string.Empty;
                });
                
            }).Start();
        }

        private async void richTextBox1_SelectionChanged(object sender, EventArgs e)
        {
            if (richTextBox1.SelectedText.Length > 0)
            {
                contextMenuStrip1.Enabled = true;
            }
            else
            {
                contextMenuStrip1.Enabled = false;
            }
        }
    }
}