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
    }

    public partial class ToolWindowDesign : ToolWindow
    {
        public ToolWindowDesign() : base()
        {
        }
    }
}