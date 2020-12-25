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
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net;
using System.Linq;
using System.Text;

namespace DockSample
{
    public partial class BrowserDoc :  DockContent
    {
        private string Url;
        private ChromiumWebBrowser browser;
        private UCLoaderForm loader;
        private BrowserDoc self;
        private string strPublicKey = string.Empty;
        public List<IWindowInfo> listWindowInfo = new List<IWindowInfo>();
        private List<CefSharp.Cookie> cookies = new List<CefSharp.Cookie>();

        private string _CurrentProjPath;
        public string CurrentProjPath { set {_CurrentProjPath = value; } }

        MainForm mainFrm;

        private bool _isAdvanceAnalytics;

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
        public BrowserDoc(string url, string tabText, MainForm frm = null, bool isAdvanceAnalytics = false)
        {
            InitializeComponent();
            self = this;
            AutoScaleMode = AutoScaleMode.Dpi;
            DockAreas = DockAreas.Document | DockAreas.Float;
            Url = url;
            this.TabText = tabText;

            mainFrm = frm;
            _isAdvanceAnalytics = isAdvanceAnalytics;

            loader = new UCLoaderForm();
            browser = new ChromiumWebBrowser(Url);
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

                    if (_isAdvanceAnalytics)
                    {
                        LifespanHandler life = new LifespanHandler();
                        browser.LifeSpanHandler = life;
                        life.popup_request += life_popup_request;
                        //life.onAfterCreated += Life_onAfterCreated;
                    }

                    panel1.Controls.Add(browser);

                    loader.PerformSafely(() =>
                    {
                        loader.Hide();
                    });
                });

            }).Start();
            //loader.ShowDialog(this);
        }

        private void Life_onAfterCreated()
        {
            //throw new NotImplementedException();

            //var host = new Uri(this.Url).Host;
            //var port = new Uri(this.Url).Port;
            //string dirApi = string.Format("http://{0}:{1}/api/contents", host, port);
            //var res = ApiGetResponse(dirApi).Result;
            //if (res != null)
            //{
            //}
        }

        private void life_popup_request(string popup_request)
        {
            if (_isAdvanceAnalytics)
            {
                var host = new Uri(this.Url).Host;
                var port = new Uri(this.Url).Port;

                this.Invoke((MethodInvoker)delegate ()
                {
                    if (popup_request.Equals("about:blank#blocked") || popup_request.Equals(string.Format("http://{0}:{1}/tree#", host, port)))
                    {
                        //string hostName = string.Format("http://{0}:{1}", host, port);
                        //string dirApi = string.Format("http://{0}:{1}/api/contents?type=directory", host, port);
                        //string dirApi = string.Format("http://{0}:{1}/api/contents", host, port);
                        //ApiPostResponse(dirApi, host);
                        //if (!string.IsNullOrEmpty(res.Result))
                        //{
                        //}

                        //directory
                        ////var directory = new DirectoryInfo(_CurrentProjPath);
                        ////var myFile = (from f in directory.GetFiles()
                        ////              orderby f.LastWriteTime descending
                        ////              select f).First();

                        ////if (myFile != null)
                        ////{
                        ////    if (myFile.Extension.Equals(".ipynb"))
                        ////    {
                        ////        //http://localhost:8888/api/contents 
                        ////        //{"type":"notebook"}
                        ////    }
                        ////    else if (myFile.Extension.Equals(".txt"))
                        ////    {
                        ////        //http://localhost:8888/api/contents 
                        ////        //{"ext":".txt","type":"file"}
                        ////    }
                        ////}
                        ///
                    }
                    else
                    {
                        if (mainFrm != null)
                        {
                            mainFrm.dockPanel.PerformSafely(() =>
                            {
                                BrowserDoc dummyDoc = new BrowserDoc(popup_request, this.TabText + "1", null, true);
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

            if (_isAdvanceAnalytics)
            {
                cookies = await browser.GetCookieManager().VisitAllCookiesAsync();
            }
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
            if (_isAdvanceAnalytics)
            {
                try
                {
                    if (e.IsLoading == false)
                    {
                        browser.ExecuteScriptAsync("document.getElementById('ipython_notebook').innerHTML = \"<span style='font-weight: bold;color:#296eaa;'>Kockpit Notebook</span><span style='font-size: 1rem;font-weight: bold;color:#296eaa;'>&nbsp; (ML)</span>\"");
                    }
                }
                catch (Exception)
                {
                }
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


        //public HttpResponseMessage MakeApiCall(string host, string api)

        //{
        //    // Create HttpClient
        //    var client = new HttpClient(new HttpClientHandler { UseDefaultCredentials = true }) { BaseAddress = new Uri(host) };
        //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //    // Make an API call and receive HttpResponseMessage
        //    HttpResponseMessage responseMessage = client.GetAsync(api, HttpCompletionOption.ResponseContentRead).GetAwaiter().GetResult();

        //    return responseMessage;
        //}

        private async Task<JupyterNoteBook> ApiGetResponse(string apiUrl)
        {
            JupyterNoteBook jupyterNb = new JupyterNoteBook();
            try
            {
                var baseAddress = new Uri(apiUrl);
                using (var handler = new HttpClientHandler { UseCookies = false })
                using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
                {
                    var message = new HttpRequestMessage(HttpMethod.Get, apiUrl);
                    message.Headers.Add("Cookie", "_xsrf=2|cf309033|59363b35f5723fdeb28efe1ceaa9851c|1605504683; username-localhost-9999=\"2|1:0|10:1605779692|23:username-localhost-9999|44:Zjc4NzY1NjE2OGY0NDE5NzllYjUwNTNiYzcyNDU0ZGI=|2147984a5c541726c6d6fd162e555d14596b7e9809e21ff5fbd7872aef8feae0\"; username-localhost-8890=\"2|1:0|10:1605853650|23:username-localhost-8890|44:MGEzMmRiMmFiYTY3NDJiZjk2M2NiYzdlMGJjNmQzN2I=|a945746a1ab4baaad939cfa350e65a7a00b96347720daf07538605374aa8f7b0\"; username-localhost-8889=\"2|1:0|10:1605853938|23:username-localhost-8889|44:Nzk0OWJkMWE3ZjU3NGUwMGEyNDdmYjA4M2MwYmM3M2Y=|39088bfe88fc4fc2d62bd23a5014a3d49ad04efe5d0a70f3500a53a4ccc35f5e\"; username-localhost-8888=\"2|1:0|10:1606194424|23:username-localhost-8888|44:YTc2ZTE4MjIzMGQwNGE1NDliOWU1YTc5NjBlMmFjOWI=|17fb775d94291e302ea7c6a12c3f5e82a3bd063d6acb1d6d947cfe2eb606e52a\"");
                    message.Headers.Add("X-XSRFToken", "2|cf309033|59363b35f5723fdeb28efe1ceaa9851c|1605504683");
                    var result = await client.SendAsync(message);
                    if (result.IsSuccessStatusCode)
                    {
                        string responseBody = await result.Content.ReadAsStringAsync();
                        jupyterNb = JsonConvert.DeserializeObject<JupyterNoteBook>(responseBody);
                        if (jupyterNb != null)
                        {
                            jupyterNb = jupyterNb.content.OrderByDescending(m => m.created).FirstOrDefault();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //return string.Empty;
            }

            return jupyterNb;
        }

        private async void ApiPostResponse(string apiUrl, string host)
        {
            string responseBody = string.Empty;
            try
            {
                var baseAddress = new Uri(apiUrl);
                using (var handler = new HttpClientHandler { UseCookies = false })
                using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
                {
                    //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var requestMessage = new HttpRequestMessage(HttpMethod.Post, apiUrl);
                    //requestMessage.Headers.Add("Host", "<calculated when request is sent>");
                    requestMessage.Headers.Add("Cookie", "_xsrf=2|cf309033|59363b35f5723fdeb28efe1ceaa9851c|1605504683; username-localhost-9999=\"2|1:0|10:1605779692|23:username-localhost-9999|44:Zjc4NzY1NjE2OGY0NDE5NzllYjUwNTNiYzcyNDU0ZGI=|2147984a5c541726c6d6fd162e555d14596b7e9809e21ff5fbd7872aef8feae0\"; username-localhost-8890=\"2|1:0|10:1605853650|23:username-localhost-8890|44:MGEzMmRiMmFiYTY3NDJiZjk2M2NiYzdlMGJjNmQzN2I=|a945746a1ab4baaad939cfa350e65a7a00b96347720daf07538605374aa8f7b0\"; username-localhost-8889=\"2|1:0|10:1605853938|23:username-localhost-8889|44:Nzk0OWJkMWE3ZjU3NGUwMGEyNDdmYjA4M2MwYmM3M2Y=|39088bfe88fc4fc2d62bd23a5014a3d49ad04efe5d0a70f3500a53a4ccc35f5e\"; username-localhost-8888=\"2|1:0|10:1606194424|23:username-localhost-8888|44:YTc2ZTE4MjIzMGQwNGE1NDliOWU1YTc5NjBlMmFjOWI=|17fb775d94291e302ea7c6a12c3f5e82a3bd063d6acb1d6d947cfe2eb606e52a\"");
                    requestMessage.Headers.Add("X-XSRFToken", "2|cf309033|59363b35f5723fdeb28efe1ceaa9851c|1605504683");
                    requestMessage.Content = new StringContent("{\"type\":\"notebook\"}", Encoding.UTF8, "application/json");
                    requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var result = await client.SendAsync(requestMessage);
                    if (result.IsSuccessStatusCode)
                    {
                        responseBody = await result.Content.ReadAsStringAsync();
                        //JupyterNoteBook jupyterNoteBook = JsonConvert.DeserializeObject<JupyterNoteBook>(responseBody);
                        //if (jupyterNoteBook != null)
                        //{
                        //    var vLastContent = jupyterNoteBook.content.OrderByDescending(m => m.created).FirstOrDefault();
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                //return string.Empty;
            }
        }
    }

    public class LifespanHandler : ILifeSpanHandler
    {
        //event that receive url popup
        public event Action<string> popup_request;

        public event Action onAfterCreated;

        public bool OnBeforePopup(IWebBrowser browserControl, CefSharp.IBrowser browser, IFrame frame, 
            string targetUrl, string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, 
            IPopupFeatures popupFeatures, IWindowInfo windowInfo, IBrowserSettings browserSettings, ref bool noJavascriptAccess, out IWebBrowser newBrowser)
        {
            //get url popup
            if (this.popup_request != null)
                this.popup_request(targetUrl);

            //stop open popup window
            newBrowser = null;
            //newBrowser = new ChromiumWebBrowser(targetUrl);
            return true;
        }
        public bool DoClose(IWebBrowser browserControl, CefSharp.IBrowser browser)
        { return false; }

        public void OnBeforeClose(IWebBrowser browserControl, CefSharp.IBrowser browser) { }

        public void OnAfterCreated(IWebBrowser browserControl, CefSharp.IBrowser browser) 
        {
            //this.onAfterCreated();
        }
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


    public class JupyterNoteBook
    {
        public string name { get; set; }
        public string created { get; set; }

        public List<JupyterNoteBook> content { get; set; }
    }

    public class param
    {
        public string type { get; set; }
        public string ext { get; set; }
    }

}