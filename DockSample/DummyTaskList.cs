using System.Windows.Forms;

namespace DockSample
{
    public partial class DummyTaskList : ToolWindow
    {
        public DummyTaskList()
        {
            InitializeComponent();
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)(((((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft)
                        | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight)
                        | WeifenLuo.WinFormsUI.Docking.DockAreas.DockTop)
                        | WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom)));
            this.HideOnClose = true;
            this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockTopAutoHide;
            this.TabText = "Task List";
        }
    }
}