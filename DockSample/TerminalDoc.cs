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
    public partial class TerminalDoc :  DockContent
    {
        private UCLoaderForm loader;
        private bool _isWindow = false;

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

        public TerminalDoc()
        { 
        }
        public TerminalDoc(string tabText, bool isWindow = false)
        {
            InitializeComponent();
            AutoScaleMode = AutoScaleMode.Dpi;
            DockAreas = DockAreas.Document | DockAreas.Float;
            this.TabText = tabText;
            this._isWindow = isWindow;

            loader = new UCLoaderForm();
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
                });

            }).Start();
            //loader.ShowDialog(this);
        }
    }
}