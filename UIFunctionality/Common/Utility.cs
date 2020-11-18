using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace UIFunctionality.Common
{
    public static class Utility
    {
        public static void fitFormToScreen(Form form, int h, int w)
        {
            //scale the form to the current screen resolution
            form.Height = (int)((float)form.Height + ((float)Screen.PrimaryScreen.Bounds.Size.Height / (float)h));
            form.Width = (int)((float)form.Width + ((float)Screen.PrimaryScreen.Bounds.Size.Width / (float)w));

            //here font is scaled like width
            form.Font = new Font(form.Font.FontFamily, form.Font.Size + ((float)Screen.PrimaryScreen.Bounds.Size.Width / (float)w));

            foreach (Control item in form.Controls)
            {
                fitControlsToScreen(item, h, w);
            }
        }


        static void fitControlsToScreen(Control cntrl, int h, int w)
        {
            if (Screen.PrimaryScreen.Bounds.Size.Height > 800)
            {
                cntrl.Height = (int)((float)cntrl.Height + ((float)Screen.PrimaryScreen.Bounds.Size.Height / (float)h));
                cntrl.Top = (int)((float)cntrl.Top + ((float)Screen.PrimaryScreen.Bounds.Size.Height / (float)h));
            }
            if (Screen.PrimaryScreen.Bounds.Size.Width > 1400)
            {
                cntrl.Width = (int)((float)cntrl.Width + ((float)Screen.PrimaryScreen.Bounds.Size.Width / (float)w));
                cntrl.Left = (int)((float)cntrl.Left + ((float)Screen.PrimaryScreen.Bounds.Size.Width / (float)w));

                cntrl.Font = new Font(cntrl.Font.FontFamily, cntrl.Font.Size * ((float)Screen.PrimaryScreen.Bounds.Size.Width / (float)w));
            }

            foreach (Control item in cntrl.Controls)
            {
                fitControlsToScreen(item, h, w);
            }
        }
    }
}
