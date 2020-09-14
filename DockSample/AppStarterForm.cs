using DockSample.Controls;
using DockSample.lib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UIFunctionality.Common;

namespace DockSample
{
    public partial class AppStarterForm : Form
    {
        public AppStarterForm()
        {
            InitializeComponent();

            var splashScreen = new SplashScreen();
            splashScreen.Visible = true;
            splashScreen.TopMost = true;
            Thread.Sleep(4000);
            //this.Close();
            //splashScreen.Hide();
            //this.Hide();
            //var mainForm = new MainForm();
            //mainForm.Show();

            //var t = new Task(() =>
            //{
            var configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");
            if (!File.Exists(configPath))
            {
                ConfigurationForm frm = new ConfigurationForm("Please enter Linux configuration information.", ()=> {
                    splashScreen.Hide();
                    this.Hide();
                });
                //this.PerformSafely(() => { frm.ShowDialog(this); });
                frm.ShowDialog(this);
                //splashScreen.PerformSafely(() => { splashScreen.Hide(); });
                //this.PerformSafely(() => { this.Hide(); });
            }
            else
            {
                try
                {
                    
                    var configText = File.ReadAllText(configPath);
                    var config = new StudioConfig(AppDomain.CurrentDomain.BaseDirectory).GetStudioConfigFromFile();
                    var studioConfig = config.SaveAndLoadConfig();
                    splashScreen.Hide();
                    MainForm frm = new MainForm(studioConfig, ()=> {
                        this.Hide();
                    });
                    frm.ShowDialog();
                    //splashScreen.Hide();
                    //this.Hide();
                }
                catch (Exception ex)
                {
                    splashScreen.Hide();
                    ConfigurationForm frm = new ConfigurationForm("Please enter valid Linux configuration information.", ()=> { });
                    frm.ShowDialog(this);
                }
                //splashScreen.PerformSafely(() => { splashScreen.Hide(); });
                //this.PerformSafely(() => { this.Hide(); });
            }
            //});
            //t.Start();
            
            /*
            var t = new Task(() =>
            {
                var splashScreen = new SplashScreen();
                splashScreen.Visible = true;
                splashScreen.TopMost = true;
                Thread.Sleep(4000);
                splashScreen.Hide();
                this.Hide();
                var mainForm = new MainForm();
                mainForm.Show();
                //var configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");
                //if (!File.Exists(configPath))
                //{
                //    _splashScreen.PerformSafely(() =>
                //    {
                //        _splashScreen.Visible = false;
                //        _showSplash = false;
                //    });

                //    ConfigurationForm frm = new ConfigurationForm();
                //    frm.ShowDialog(this);
                //}
            });
            t.Start();
            t.Wait();
            */
        }

        private void AppStarterForm_Load(object sender, EventArgs e)
        {
            //this.Hide();
        }
    }
}
