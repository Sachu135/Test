using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DockSample.lib;

namespace DockSample.Controls
{
    public partial class AddTask : Form
    {
        int _mode;
        public int mode { set { _mode = value; } }
        public AddTask()
        {
            InitializeComponent();
        }

        public Action<Dictionary<string,string>, int> SaveClicked;

        public void SetPanelVisible(bool visible)
        {
            panel2.PerformSafely(() => {
                panel2.Visible = visible;
            });
        }
        public string InfoMessage
        {
            get { return this.label2.Text; }
            set
            {

                label2.PerformSafely(() => {
                    label2.Text = value;
                });

            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> diResponse = new Dictionary<string, string>();
            label2.Text = string.Empty;
            panel2.Visible = false;

            DateTime dStartDate = Convert.ToDateTime(dtStartDate.Text.Trim());
            DateTime dDueDate = Convert.ToDateTime(dtDueDate.Text.Trim());

            if (_mode == 1)
            {
                if (string.IsNullOrEmpty(txtNewList.Text.Trim()))
                {
                    panel2.Visible = true;
                    label2.Text = "Please mention the list name";
                }
                else if (dStartDate.Date > dDueDate.Date)
                {
                    panel2.Visible = true;
                    label2.Text = "Due date should be greater then or equal to start date";
                }
                else
                {
                    diResponse.Add("list_name", txtNewList.Text.Trim());
                    diResponse.Add("creation_date", dStartDate.ToShortDateString());
                    diResponse.Add("due_date", dDueDate.ToShortDateString());
                    SaveClicked(diResponse, _mode);
                }
            }
            else if(_mode == 2)
            {
                
                if (string.IsNullOrEmpty(txtTaskTitle.Text.Trim()))
                {
                    panel2.Visible = true;
                    label2.Text = "Please mention the task title";
                }
                else if (dStartDate.Date > dDueDate.Date)
                {
                    panel2.Visible = true;
                    label2.Text = "Due date should be greater then or equal to start date";
                }
                else
                {
                    diResponse.Add("title", txtTaskTitle.Text.Trim());
                    diResponse.Add("description", txtDescription.Text.Trim());
                    diResponse.Add("creation_date", dStartDate.ToShortDateString());
                    diResponse.Add("due_date", dDueDate.ToShortDateString());
                    SaveClicked(diResponse, _mode);
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Task t = new Task(() =>
            {
                this.PerformSafely(() =>
                {
                    panel2.Visible = false;
                    txtNewList.Text = string.Empty;
                    txtTaskTitle.Text = string.Empty;
                    txtDescription.Text = string.Empty;
                    dtDueDate.Text = DateTime.Now.ToString();
                    dtStartDate.Text = DateTime.Now.ToString();
                    this.Hide();
                });
            });
            t.Start();
        }

        private void AddTask_Load(object sender, EventArgs e)
        {
            this.PerformSafely(() =>
            {
                panel2.Visible = false;
                txtNewList.Text = string.Empty;
                txtTaskTitle.Text = string.Empty;
                txtDescription.Text = string.Empty;
                dtDueDate.Text = DateTime.Now.ToString();
                dtStartDate.Text = DateTime.Now.ToString();

                lblCreateList.Visible = txtNewList.Visible = (_mode == 1) ? true : false;
                lblTaskTitle.Visible = lblDescription.Visible = (_mode == 1) ? false : true;
                txtTaskTitle.Visible = txtDescription.Visible = (_mode == 1) ? false : true;
                label2.Text = string.Empty;
            });
        }
    }
}
