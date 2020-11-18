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

namespace DockSample
{
    public partial class BrowserDoc :  DockContent
    {
        private string Url;
        private ChromiumWebBrowser browser;
        private UCLoaderForm loader;
        private string strPublicKey = string.Empty;
        private bool _isWindow = false;

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
        public BrowserDoc(string url, string tabText, bool isWindow = false)
        {
            InitializeComponent();
            AutoScaleMode = AutoScaleMode.Dpi;
            DockAreas = DockAreas.Document | DockAreas.Float;
            Url = url;
            this.TabText = tabText;
            this._isWindow = isWindow;
            loader = new UCLoaderForm();
            if (!isWindow)
            {
                browser = new ChromiumWebBrowser(Url);
                browser.DownloadHandler = new DownloadHandler(this, loader);
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
                    if (_isWindow && this.TabText == "Terminal")
                    {
                        Process p = Process.Start(
                                   new ProcessStartInfo()
                                   {
                                       FileName = "cmd.exe",
                                       WindowStyle = ProcessWindowStyle.Maximized
                                   });
                        Thread.Sleep(500);
                        IntPtr value = SetParent(p.MainWindowHandle, panel1.Handle);
                        SendMessage(p.MainWindowHandle, WM_SYSCOMMAND, SC_MAXIMIZE, 0);
                        SetWindowLong(p.MainWindowHandle, GWL_STYLE, GetWindowLong(p.MainWindowHandle, GWL_STYLE) & ~(WS_CAPTION | SC_MINIMIZE | SC_MAXIMIZE | WS_SYSMENU));


                        //LONG lStyle = GetWindowLong(hwnd, GWL_STYLE);
                        //lStyle &= ~(WS_CAPTION | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX | WS_SYSMENU);
                        //SetWindowLong(hwnd, GWL_STYLE, lStyle);

                        loader.PerformSafely(() =>
                        {
                            loader.Hide();
                        });
                    }
                    else if(_isWindow && this.TabText == "Health Check")
                    {
                        //code to open ntop
                        Process p = Process.Start(
                                   new ProcessStartInfo("cmd.exe", @"/c ntop")
                                   {
                                       WindowStyle = ProcessWindowStyle.Maximized
                                   });
                        Thread.Sleep(500);
                        IntPtr value = SetParent(p.MainWindowHandle, panel1.Handle);
                        SendMessage(p.MainWindowHandle, WM_SYSCOMMAND, SC_MAXIMIZE, 0);
                        //SetWindowLong(p.MainWindowHandle, GWL_STYLE, GetWindowLong(p.MainWindowHandle, GWL_STYLE) & ~WS_SYSMENU);
                        SetWindowLong(p.MainWindowHandle, GWL_STYLE, GetWindowLong(p.MainWindowHandle, GWL_STYLE) & ~(WS_CAPTION | SC_MINIMIZE | SC_MAXIMIZE | WS_SYSMENU));

                        loader.PerformSafely(() =>
                        {
                            loader.Hide();
                        });
                    }
                    else
                    {
                        //browser.LoadingStateChanged += Browser_LoadingStateChanged;
                        browser.FrameLoadStart += Browser_FrameLoadStart;
                        browser.FrameLoadEnd += Browser_FrameLoadEnd;
                        panel1.Controls.Add(browser);
                        loader.PerformSafely(() =>
                        {
                            loader.Hide();
                        });
                    }
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
                //MessageBox.Show("done");
                this.Alert("done", Form_Alert.enmType.Success);
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
}