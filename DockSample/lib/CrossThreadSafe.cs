using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DockSample.lib
{
    static class CrossThreadSafe
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

        public static void PerformSafely(this Form target, Action action)
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

        public static void PerformSafely(this DummyDoc target, Action action)
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
