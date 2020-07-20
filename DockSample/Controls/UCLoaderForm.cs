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
    public partial class UCLoaderForm : Form
    {
        public UCLoaderForm()
        {
            InitializeComponent();
        }

        public new DialogResult ShowDialog(IWin32Window owner)
        {
            return base.ShowDialog(owner);
        }
        public DialogResult ShowDialog(IWin32Window owner, string msg = "")
        {
            label1.Text = string.IsNullOrEmpty(msg) ? "Wait..." : msg;
            return this.ShowDialog(owner);
        }
    }
}
