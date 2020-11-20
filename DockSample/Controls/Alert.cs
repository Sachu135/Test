using DockSample.lib;
using DockSample.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DockSample
{
    public partial class Form_Alert : Form
    {
        public Form_Alert()
        {
            InitializeComponent();
        }

        public enum enmAction
        {
            wait,
            start,
            close
        }

        public enum enmType
        {
            Success,
            Warning,
            Error,
            Info
        }
        private Form_Alert.enmAction action;

        private int x, y;

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            switch(this.action)
            {
                case enmAction.wait:
                    timer1.Interval = 10000;
                    action = enmAction.close;
                    break;
                case Form_Alert.enmAction.start:
                    this.timer1.Interval = 1;
                    this.Opacity += 0.1;
                    if (this.x < this.Location.X)
                    {
                        this.Left--;
                    }
                    else
                    {
                        if (this.Opacity == 1.0)
                        {
                            action = Form_Alert.enmAction.wait;
                        }
                    }
                    break;
                case enmAction.close:
                    timer1.Interval = 1;
                    this.Opacity -= 0.1;

                    this.Left -= 3;
                    if (base.Opacity == 0.0)
                    {
                        base.Close();
                    }
                    break;
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            timer1.Interval = 1;
            action = enmAction.close;
        }

        private void Form_Alert_Load(object sender, EventArgs e)
        {
            lblMsg.PerformSafely(() => {
                lblMsg.AutoSize = true;
            });
        }

        public void showAlert(string msg, enmType type, IWin32Window owner)
        {
            this.Opacity = 0.0;
            this.StartPosition = FormStartPosition.Manual;
            string fname;

            for (int i = 1; i < 50; i++)
            {
                fname = "alert" + i.ToString();
                Form_Alert frm = (Form_Alert)Application.OpenForms[fname];

                if (frm == null)
                {
                    this.Name = fname;
                    ///bottom right
                    this.x = Screen.PrimaryScreen.WorkingArea.Width - this.Width + 25;
                    //this.y = Screen.PrimaryScreen.WorkingArea.Height - this.Height * i - 5 * i;
                    this.y = Screen.PrimaryScreen.WorkingArea.Height - this.Height - 15;

                    ///top center
                    //this.x = Screen.PrimaryScreen.WorkingArea.Width / 2 - 20;
                    //this.y = 50 * i + 5 * i;

                    this.Location = new Point(this.x, this.y);
                    break;
                }

            }
            this.x = Screen.PrimaryScreen.WorkingArea.Width - base.Width - 5;

            switch (type)
            {
                case enmType.Success:
                    this.pictureBox1.Image = Resources.alert_success;
                    this.BackColor = Color.SeaGreen;
                    break;
                case enmType.Error:
                    this.pictureBox1.Image = Resources.alert_error;
                    this.BackColor = Color.DarkRed;
                    break;
                case enmType.Info:
                    this.pictureBox1.Image = Resources.alert_info;
                    //this.BackColor = Color.RoyalBlue;
                    this.BackColor = Color.FromArgb(51, 181, 229);
                    break;
                case enmType.Warning:
                    this.pictureBox1.Image = Resources.alert_warning;
                    this.BackColor = Color.DarkOrange;
                    break;
            }

            lblMsg.PerformSafely(() =>
            {
                lblMsg.Text = msg;
            });

            int lw = lblMsg.Width;
            int lh = lblMsg.Height;
            Size sizeofForm = new Size(this.Width, this.Height + 5);
            if (this.Width < lw || this.Height < lh)
            {
                sizeofForm.Width = this.Width;
                sizeofForm.Height = lh + 35;
                this.y = this.y - lh;
                this.Location = new Point(this.x, this.y);

                //sizeofForm.Width = this.Width;
                //sizeofForm.Height = lh + 35;
                //this.x = this.x / 2 + 155;
                //this.Location = new Point(this.x, this.y);
            }
            this.Size = sizeofForm;
            this.BringToFront();
            //this.PerformSafely(() =>
            //{
            //    this.Show(owner);
            //});
            this.Show(owner);
            this.action = enmAction.start;
            this.timer1.Interval = 1;
            this.timer1.Start();
        }

        private void lblMsg_Resize(object sender, EventArgs e)
        {
            ////////int tw = 0;
            ////////int i = 0;
            ////////String s = lblMsg.Text;
            ////////String c = String.Empty;
            ////////Font f = lblMsg.Font;
            Control p = lblMsg.Parent;
            ////////int lw = lblMsg.Width;
            int cw = p.Width;
            ////////StringBuilder sb = null;

            lblMsg.PerformSafely(() => {
                lblMsg.MaximumSize = new Size(cw + 5, 0);
                lblMsg.AutoSize = true;
            });

            ////////if (lw > cw)
            ////////{

            ////////    while (tw < cw)
            ////////    {
            ////////        i += 1;
            ////////        c = s.Substring(0, i);
            ////////        //calculate the length of string
            ////////        Size txtSize = TextRenderer.MeasureText(c, f);
            ////////        //get the width of text 
            ////////        tw = txtSize.Width;
            ////////    }
            ////////    //decrease the length of text 
            ////////    i -= 1;
            ////////    //insert brakes
            ////////    sb = new StringBuilder();
            ////////    int j = 0;
            ////////    while (j < s.Length)
            ////////    {
            ////////        if (j + i > s.Length)
            ////////            i = s.Length - j;
            ////////        c = s.Substring(j, i);// + "/r/n";
            ////////        sb.AppendLine(c);
            ////////        j += i;
            ////////    }
            ////////    lblMsg.Text = sb.ToString();
            ////////}
        }
    }
}
