using System;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.IO;
using EasyScintilla.Stylers;
using ScintillaNET;
using System.Threading.Tasks;
using DockSample.lib;
using SFTPEntities;
using CefSharp.WinForms;
using DockSample.Controls;

namespace DockSample
{
    public partial class BrowserDoc :  DockContent
    {
        private string Url;
        private ChromiumWebBrowser browser;
        private UCLoaderForm loader;
        public BrowserDoc(string url, string tabText)
        {
            InitializeComponent();
            AutoScaleMode = AutoScaleMode.Dpi;
            DockAreas = DockAreas.Document | DockAreas.Float;
            Url = url;
            this.TabText = tabText;
            browser = new ChromiumWebBrowser(Url);
            loader = new UCLoaderForm();

        }

        private void menuItem2_Click(object sender, System.EventArgs e)
        {
            MessageBox.Show("This is to demostrate menu item has been successfully merged into the main form. Form Text=" + Text);
        }

        private void menuItemCheckTest_Click(object sender, System.EventArgs e)
        {
            menuItemCheckTest.Checked = !menuItemCheckTest.Checked;
        }

        private async void DummyDoc_Load(object sender, EventArgs e)
        {
            new Task(() => {
                loader.PerformSafely(() =>
                {
                    loader.ShowDialog(this);
                    //loader.Close();
                });
            }).Start();
            new Task(() =>
            {
                
                panel1.PerformSafely(() =>
                {
                    //browser.LoadingStateChanged += Browser_LoadingStateChanged;
                    browser.FrameLoadStart += Browser_FrameLoadStart;
                    browser.FrameLoadEnd += Browser_FrameLoadEnd;
                    panel1.Controls.Add(browser);
                });

            }).Start();
            //loader.ShowDialog(this);

        }

        private async void Browser_FrameLoadEnd(object sender, CefSharp.FrameLoadEndEventArgs e)
        {
            new Task(() =>
            {
                loader.PerformSafely(() =>
                {
                    loader.Hide();
                    //loader.Close();
                });
            }).Start();
            
            //MessageBox.Show("started");
        }

        private async void Browser_FrameLoadStart(object sender, CefSharp.FrameLoadStartEventArgs e)
        {
            new Task(() =>
            {
                loader.PerformSafely(() =>
                {
                    loader.ShowDialog(this);
                    //loader.Close();
                });
            }).Start();
            //MessageBox.Show("ended");
        }

        private void Browser_LoadingStateChanged(object sender, CefSharp.LoadingStateChangedEventArgs e)
        {
            var dd = 1;
            if (!e.IsLoading)
            {
                MessageBox.Show("done");
            }
        }
    }
}