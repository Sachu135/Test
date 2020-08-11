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
using CefSharp;
using CustomControls;

namespace DockSample
{
    public partial class BrowserDoc :  DockContent
    {
        private string Url;
        private ChromiumWebBrowser browser;
        private UCLoaderForm loader;

        public class DownloadHandler : CefSharp.IDownloadHandler
        {
            BrowserDoc parent;
            UCLoaderForm loader;

            public event EventHandler<CefSharp.DownloadItem> OnBeforeDownloadFired;

            public event EventHandler<CefSharp.DownloadItem> OnDownloadUpdatedFired;

            public DownloadHandler(BrowserDoc parentObj, UCLoaderForm loaderObj)
            {
                this.parent = parentObj;
                this.loader = loaderObj;
            }
            public void OnBeforeDownload(IWebBrowser chromiumWebBrowser, CefSharp.IBrowser browser, DownloadItem downloadItem, IBeforeDownloadCallback callback)
            {
                OnBeforeDownloadFired?.Invoke(this, downloadItem);

                if (!callback.IsDisposed)
                {
                    using (callback)
                    {
                        callback.Continue(downloadItem.SuggestedFileName, showDialog: true);
                    }
                }
            }

            public void OnDownloadUpdated(IWebBrowser chromiumWebBrowser, CefSharp.IBrowser browser, DownloadItem downloadItem, IDownloadItemCallback callback)
            {
                new Task(() => {
                    parent.PerformSafely(() =>
                    {
                        if (!loader.Visible)
                        {
                            loader.ShowDialog(parent, "Downloading...");
                        }
                    });
                }).Start();
                
                OnDownloadUpdatedFired?.Invoke(this, downloadItem);

                if (downloadItem.IsComplete || downloadItem.IsCancelled)
                {
                    new Task(() =>
                    {
                        loader.PerformSafely(() =>
                        {
                            loader.Hide();
                            loader.Close();
                        });
                    }).Start();
                }
            }
        }

        public BrowserDoc()
        { 
        }
        public BrowserDoc(string url, string tabText)
        {
            InitializeComponent();
            AutoScaleMode = AutoScaleMode.Dpi;
            DockAreas = DockAreas.Document | DockAreas.Float;
            Url = url;
            this.TabText = tabText;
            loader = new UCLoaderForm();
            browser = new ChromiumWebBrowser(Url);
            browser.DownloadHandler = new DownloadHandler(this, loader);
            
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
                this.PerformSafely(() =>
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
                this.PerformSafely(() =>
                {
                    if (!loader.Visible)
                    {
                        loader.ShowDialog(this);
                    }
                    
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