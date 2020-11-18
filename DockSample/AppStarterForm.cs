using DockSample.Controls;
using DockSample.lib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management.Automation.Language;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.ServiceProcess;
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

            if(CheckForInternetConnection() == false)
            {
                MessageBox.Show("Please check your internet connection..", "Connection Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                splashScreen.Hide();
                this.Hide();
                //this.Close();
                //Application.Exit();
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            else
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings["Machine"] == null)
                    settings.Add("Machine", Environment.MachineName);
                else
                    settings["Machine"].Value = Environment.MachineName;

                if(settings["License"] == null)
                    settings.Add("License", string.Empty);

                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);

                if (settings["License"] != null)
                {
                    string strLicense = settings["License"].Value.ToString();
                    bool lLicenseExpire = LicenseExpireAsync(strLicense, settings).Result;
                    if (string.IsNullOrEmpty(strLicense) || lLicenseExpire == true)
                    {
                        //code to uninstall the running windows services if exists
                        bool lInstalled = IsServiceInstalled("KockpitStudioService");
                        var asmPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WindowsService.exe");
                        if (File.Exists(asmPath) && lInstalled)
                            KockpitStudioServiceAssistant.Uninstall(Assembly.LoadFrom(asmPath));

                        LicenseForm frm = new LicenseForm(settings["Machine"].Value, 
                            lLicenseExpire,() => {
                            splashScreen.Hide();
                            this.Hide();
                        });
                        frm.ShowDialog(this);
                        if(frm.DialogResult == DialogResult.OK)
                        {
                            Launch(splashScreen);
                        }
                        else
                        {
                            splashScreen.Hide();
                            this.Hide();
                            //Application.Exit();
                            System.Diagnostics.Process.GetCurrentProcess().Kill();
                        }
                    }
                    else
                    {
                        Launch(splashScreen);
                    }
                }

            //code to check the app.config for key
            //var section = (LicenseValuesSection)ConfigurationManager.GetSection("LicenceConfig");
            //var licenseElements = (from object value in section.Values
            //                    select (LicenseElement)value)
            //                    .ToList();

            //if (licenseElements != null)
            //{
            //    //if(string.IsNullOrEmpty(licenseElements))
            //}

            //var t = new Task(() =>
            //{
                //////////////var configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");
                //////////////if (!File.Exists(configPath))
                //////////////{
                //////////////    ConfigurationForm frm = new ConfigurationForm("Please enter Workspace configuration information.", () =>
                //////////////    {
                //////////////        splashScreen.Hide();
                //////////////        this.Hide();
                //////////////    });
                //////////////    //this.PerformSafely(() => { frm.ShowDialog(this); });
                //////////////    frm.ShowDialog(this);
                //////////////    //splashScreen.PerformSafely(() => { splashScreen.Hide(); });
                //////////////    //this.PerformSafely(() => { this.Hide(); });
                //////////////}
                //////////////else
                //////////////{
                //////////////    try
                //////////////    {

                //////////////        var configText = File.ReadAllText(configPath);
                //////////////        var config = new StudioConfig(AppDomain.CurrentDomain.BaseDirectory).GetStudioConfigFromFile();
                //////////////        var studioConfig = config.SaveAndLoadConfig();
                //////////////        splashScreen.Hide();
                //////////////        MainForm frm = new MainForm(studioConfig, () =>
                //////////////        {
                //////////////            this.Hide();
                //////////////        });
                //////////////        frm.ShowDialog();
                //////////////        //splashScreen.Hide();
                //////////////        //this.Hide();
                //////////////    }
                //////////////    catch (Exception ex)
                //////////////    {
                //////////////        splashScreen.Hide();
                //////////////        ConfigurationForm frm = new ConfigurationForm("Please enter valid Workspace configuration information.", () => { });
                //////////////        frm.ShowDialog(this);
                //////////////    }
                //////////////    //splashScreen.PerformSafely(() => { splashScreen.Hide(); });
                //////////////    //this.PerformSafely(() => { this.Hide(); });
                //////////////}
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
        }

        private void AppStarterForm_Load(object sender, EventArgs e)
        {
            //this.Hide();
        }

        private void Launch(SplashScreen splashScreen)
        {
            var configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");
            if (!File.Exists(configPath))
            {
                ConfigurationForm frm = new ConfigurationForm("Please enter Workspace configuration information.", () =>
                {
                    splashScreen.Hide();
                    this.Hide();
                });
                frm.ShowDialog(this);
            }
            else
            {
                try
                {

                    var configText = File.ReadAllText(configPath);
                    var config = new StudioConfig(AppDomain.CurrentDomain.BaseDirectory).GetStudioConfigFromFile();
                    var studioConfig = config.SaveAndLoadConfig();
                    splashScreen.Hide();
                    MainForm frm = new MainForm(studioConfig, () =>
                    {
                        this.Hide();
                    });
                    frm.ShowDialog();
                }
                catch (Exception ex)
                {
                    splashScreen.Hide();
                    ConfigurationForm frm = new ConfigurationForm("Please enter valid Workspace configuration information.", () => { });
                    frm.ShowDialog(this);
                }
            }
        }

        private async Task<bool> LicenseExpireAsync(string _License, KeyValueConfigurationCollection settings)
        {
            try
            {
                //code to check the expiry for license
                if (!string.IsNullOrEmpty(_License))
                {
                    string decodedCode = UIFunctionality.Common.License.Decrypt(_License);
                    string[] strArr = decodedCode.Split(new char[] { '|' }, StringSplitOptions.None);

                    if (settings["Machine"].Value.ToString().Trim().ToLower() != strArr[0].ToLower().Trim())
                    {
                        return true;
                    }
                    else
                    {
                        //api call to check the start date and end date
                        //string apiUrl = "http://worldtimeapi.org/api/timezone/Asia/Kolkata";
                        string apiUrl = "http://worldclockapi.com/api/json/utc/now";
                        //string apiUrl = "http://worldtimeapi.org/api/timezone/Etc/UTC";
                        using (var client = new HttpClient())
                        {
                            using (var response = client.GetAsync(apiUrl).Result)
                            {
                                if (response.IsSuccessStatusCode)
                                {
                                    string json = await response.Content.ReadAsStringAsync();
                                    WorldClockAPI clockAPI = JsonConvert.DeserializeObject<WorldClockAPI>(json);

                                    DateTime dtCurrentDatetime = Convert.ToDateTime(clockAPI.currentDateTime);
                                    DateTime dtStartDateime = Convert.ToDateTime(strArr[1]);
                                    DateTime dtEndDateime = Convert.ToDateTime(strArr[2]);

                                    if (dtCurrentDatetime.Date >= dtStartDateime.Date
                                        && dtCurrentDatetime.Date <= dtEndDateime.Date)
                                        return false;
                                    else
                                        return true;
                                }
                                else
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
                else
                    return false;
            }
            catch (Exception)
            {
                return true;
            }
        }

        public bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://google.com/generate_204"))
                    return true;
            }
            catch
            {
                return false;
            }
        }

        private bool IsServiceInstalled(string strServiceName)
        {
            try
            {
                var serviceExists = ServiceController.GetServices().Any(s => s.ServiceName == strServiceName);
                return Convert.ToBoolean(serviceExists);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
