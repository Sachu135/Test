using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace DockSample
{
    public partial class ToolWindow : DockContent
    {
        public ToolWindow()
        {
            InitializeComponent();
            AutoScaleMode = AutoScaleMode.Dpi;
        }

        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripItem item = e.ClickedItem;
            if (item != null)
            {
                if (item.Name.ToLower().Trim() == "contextmenuclose")
                {
                    //close the form
                    this.Hide();
                }
            }
        }
    }

    public partial class ToolWindowDesign : ToolWindow
    {
        public ToolWindowDesign() : base()
        {
        }
    }
}