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
using System.Drawing;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace DockSample
{
    public partial class BrowserDoc :  DockContent
    {
        private string Url;
        private ChromiumWebBrowser browser;
        private ChromiumWebBrowser popup_browser;
        private UCLoaderForm loader;
        private BrowserDoc self;
        private string strPublicKey = string.Empty;
        public List<IWindowInfo> listWindowInfo = new List<IWindowInfo>();

        MainForm mainFrm;

        public string PublicKey { get {return strPublicKey; } set { strPublicKey = value; } }

        #region 1
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, int wParam, int lParam);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
        #endregion

        #region 3
        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        private const int GWL_STYLE = -16;

        private const int WS_SYSMENU = 0x80000;
        public static int WS_BORDER = 0x00800000; //window with border
        public static int WS_DLGFRAME = 0x00400000; //window with double border but no title
        private const int WM_SYSCOMMAND = 0x112;
        private const int SC_MINIMIZE = 0xF020;
        private const int SC_MAXIMIZE = 0xF030;
        private const int WS_VISIBLE = 0x10000000;
        private const int WS_CAPTION = 0x00C00000;
        private const int WS_THICKFRAME = 0x00040000;
        #endregion


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
        public BrowserDoc(string url, string tabText, MainForm frm = null)
        {
            InitializeComponent();
            self = this;
            AutoScaleMode = AutoScaleMode.Dpi;
            DockAreas = DockAreas.Document | DockAreas.Float;
            Url = url;
            this.TabText = tabText;

            mainFrm = frm;

            loader = new UCLoaderForm();
            browser = new ChromiumWebBrowser(Url);
            popup_browser = new ChromiumWebBrowser("");
            browser.DownloadHandler = new DownloadHandler(this, loader);

            this.FormClosing += BrowserDoc_FormClosing;
        }

        private void BrowserDoc_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(this.listWindowInfo.Count > 0)
            {
                foreach (var item in listWindowInfo)
                    item.Dispose();
            }
        }

        public void Alert(string msg, Form_Alert.enmType type)
        {
            Task t = new Task(() => {
                this.PerformSafely(() => {
                    Form_Alert frm = new Form_Alert();
                    frm.BringToFront();
                    frm.showAlert(msg, type, this);
                });
            });
            t.Start();
        }

        //private void menuItem2_Click(object sender, System.EventArgs e)
        //{
        //    MessageBox.Show("This is to demostrate menu item has been successfully merged into the main form. Form Text=" + Text);
        //}
        //private void menuItemCheckTest_Click(object sender, System.EventArgs e)
        //{
        //    menuItemCheckTest.Checked = !menuItemCheckTest.Checked;
        //}

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
                btnAmbariInfo.PerformSafely(() =>
                {
                    if(this.TabText == "Cluster Setup")
                    {
                        btnAmbariInfo.Visible = true;
                    }
                });

                panel1.PerformSafely(() =>
                {
                    //browser.LoadingStateChanged += Browser_LoadingStateChanged;
                    browser.FrameLoadStart += Browser_FrameLoadStart;
                    browser.FrameLoadEnd += Browser_FrameLoadEnd;
                    browser.LoadingStateChanged += Browser_LoadingStateChanged;

                    LifespanHandler life = new LifespanHandler();
                    browser.LifeSpanHandler = life;
                    life.popup_request += life_popup_request;

                    panel1.Controls.Add(browser);

                    loader.PerformSafely(() =>
                    {
                        loader.Hide();
                    });
                });

            }).Start();
            //loader.ShowDialog(this);
        }

        private void life_popup_request(string popup_request)
        {
            //this.Invoke((MethodInvoker)delegate ()
            //{
            //    string title = "TabPage " + (tabControl1.TabCount + 1).ToString();
            //    TabPage myTabPage = new TabPage(title);
            //    tabControl1.TabPages.Add(myTabPage);
            //    tabControl1.SelectTab(tabControl1.TabCount - 1);
            //    ChromiumWebBrowser chromeBrowser = new ChromiumWebBrowser(popup_request);
            //    chromeBrowser.Parent = myTabPage;
            //    chromeBrowser.Dock = DockStyle.Fill;
            //});

            //popup_browser.Load(popup_request);
            //popup_browser.LoadingStateChanged += Popup_browser_LoadingStateChanged;
            //popup_browser.FrameLoadEnd += Popup_browser_FrameLoadEnd;

            if (mainFrm != null)
            {
                mainFrm.dockPanel.PerformSafely(() =>
                {
                    //BrowserDoc dummyDoc = new BrowserDoc(@"http://localhost:8888/notebooks/Untitled10.ipynb?kernel_name=python3", this.TabText + "1");
                    BrowserDoc dummyDoc = new BrowserDoc(popup_request, this.TabText + "1");
                    if (mainFrm.dockPanel.DocumentStyle == DocumentStyle.SystemMdi)
                    {
                        dummyDoc.MdiParent = this;
                        dummyDoc.Show();
                    }
                    else
                        dummyDoc.Show(mainFrm.dockPanel);
                });
            }
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
            //var dd = 1;
            //if (!e.IsLoading)
            //{
            //    //MessageBox.Show("done");
            //    this.Alert("done", Form_Alert.enmType.Success);
            //}
            if (e.IsLoading == false)
            {
                //var myURL = this.Url;
                //string strNewInnerHTML = string.Format(@"<a href='/tree?token={0}' title='dashboard'>Kockpit Notebook</a>", "852254a0c4273cc510a30104b785b5a6f5f9ca5798a9c433");
                browser.ExecuteScriptAsync("document.getElementById('ipython_notebook').innerHTML = 'Kockpit Notebook'");
            }
        }

        private void btnAmbariInfo_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(strPublicKey);
            //MessageBox.Show("Copied Successfully..");
            this.Alert("Copied Successfully..", Form_Alert.enmType.Success);
        }

        private void btnAmbariInfo_MouseHover(object sender, EventArgs e)
        {
            System.Windows.Forms.ToolTip ToolTip1 = new System.Windows.Forms.ToolTip();
            ToolTip1.ToolTipTitle = "Public Key (Click to copy)";
            ToolTip1.BackColor = Color.Khaki;
            ToolTip1.SetToolTip(this.btnAmbariInfo, strPublicKey);
        }
    }

    public class LifespanHandler : ILifeSpanHandler
    {
        //event that receive url popup
        public event Action<string> popup_request;

        public bool OnBeforePopup(IWebBrowser browserControl, CefSharp.IBrowser browser, IFrame frame, 
            string targetUrl, string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, 
            IPopupFeatures popupFeatures, IWindowInfo windowInfo, IBrowserSettings browserSettings, ref bool noJavascriptAccess, out IWebBrowser newBrowser)
        {
            //get url popup
            if (this.popup_request != null)
                this.popup_request(targetUrl);

            //stop open popup window
            newBrowser = null;
            return true;
        }
        public bool DoClose(IWebBrowser browserControl, CefSharp.IBrowser browser)
        { return false; }

        public void OnBeforeClose(IWebBrowser browserControl, CefSharp.IBrowser browser) { }

        public void OnAfterCreated(IWebBrowser browserControl, CefSharp.IBrowser browser) { }
    }

    //public class LifespanHandler : ILifeSpanHandler
    //{
    //    //event that receive url popup
    //    public event Action<string> popup_request;
    //    BrowserDoc document;

    //    public LifespanHandler(BrowserDoc doc)
    //    {
    //        document = doc;
    //    }
    //    public bool OnBeforePopup(IWebBrowser browserControl, CefSharp.IBrowser browser, IFrame frame, 
    //        string targetUrl, string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, 
    //        IPopupFeatures popupFeatures, IWindowInfo windowInfo, IBrowserSettings browserSettings, 
    //        ref bool noJavascriptAccess, out IWebBrowser newBrowser)
    //    {
    //        //get url popup
    ////        if (this.popup_request != null)
    ////            this.popup_request(targetUrl);

    //        //document.listWindowInfo.Add(windowInfo);

    //        //stop open popup
    //        newBrowser = null;
    //        //newBrowser = new ChromiumWebBrowser(targetUrl);

    //document.PerformSafely(() =>
    //{
    //    //windowInfo.SetAsChild(document.Handle);
    //    windowInfo.ParentWindowHandle = document.Handle;
    //});
    //        return false;
    //    }


    //    public bool DoClose(IWebBrowser browserControl, CefSharp.IBrowser browser)
    //    { return false; }


    //    public void OnBeforeClose(IWebBrowser browserControl, CefSharp.IBrowser browser) { }


    //    public void OnAfterCreated(IWebBrowser browserControl, CefSharp.IBrowser browser) 
    //    {
    //    }

    //}

}