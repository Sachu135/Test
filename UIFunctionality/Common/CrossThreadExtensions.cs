using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UIFunctionality
{
    static class CrossThreadExtensions
    {
        public static void PerformSafely(this Control target, Action action)
        {
            if (target.InvokeRequired)
            {
                target.Invoke(action);
            }
            else
            {
                action();
            }
        }

        //public static void PerformSafely(this Component target, Action action)
        //{
        //    if (target.InvokeRequired)
        //    {
        //        target.Invoke(action);
        //    }
        //    else
        //    {
        //        action();
        //    }
        //}


        public static void PerformSafely(this UserControl target, Action action)
        {
            if (target.InvokeRequired)
            {
                target.Invoke(action);
            }
            else
            {
                action();
            }
        }

    }
}
