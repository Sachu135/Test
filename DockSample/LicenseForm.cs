using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using DockSample.lib;
using Newtonsoft.Json;
using CustomControls;

namespace DockSample
{
    public partial class LicenseForm : Form
    {
        Action loadCompleted;
        UCLoaderForm loader;

        public LicenseForm()
        {
            InitializeComponent();
        }

        public LicenseForm(string MachineName, bool Expire, Action loadComp)
        {
            InitializeComponent();
            lblExpiryMsg.Text = Expire == false ? string.Empty : "Product License is expired..";
            lblExpiryMsg.ForeColor = Color.White;
            lblMachineName.Text = MachineName;
            loadCompleted = loadComp;
            this.Activated += LicenseForm_Activated;
        }

        private void btnActivate_Click(object sender, EventArgs e)
        {
            Task t = new Task(() =>
            {
                try
                {
                    lblExpiryMsg.PerformSafely(() =>
                    {
                        lblExpiryMsg.Text = string.Empty;
                    });
                    
                    if (string.IsNullOrEmpty(txtKey.Text.Trim()))
                    {
                        MessageBox.Show("Please mention license key", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtKey.PerformSafely(() =>
                        {
                            txtKey.Focus();
                        });
                    }
                    else
                    {
                        var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                        var settings = configFile.AppSettings.Settings;
                        if (ValidateKey(txtKey.Text.Trim(), settings).Result)
                        {
                            //code to update appsetting for key
                            settings["License"].Value = txtKey.Text.Trim();
                            configFile.Save(ConfigurationSaveMode.Modified);
                            ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);

                            this.DialogResult = DialogResult.OK;
                            //this.Close();
                        }
                        else
                        {
                            lblExpiryMsg.PerformSafely(() =>
                            {
                                lblExpiryMsg.Text = "Invalid License Key";
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    loader.PerformSafely(() =>
                    {
                        loader.Hide();
                        loader.Close();
                    });
                }
            });
            t.Start();
            loader.ShowDialog(this);
        }

        private void LicenseForm_Activated(object sender, EventArgs e)
        {
            if (loadCompleted != null)
            {
                loadCompleted();
            }
        }


        private async Task<bool> ValidateKey(string strKey, KeyValueConfigurationCollection settings)
        {
            try
            {
                string decodedCode = UIFunctionality.Common.License.Decrypt(strKey);
                string[] strArr = decodedCode.Split(new char[] { '|' }, StringSplitOptions.None);

                if (settings["Machine"].Value.ToString().Trim().ToLower() != strArr[0].ToLower().Trim())
                {
                    lblExpiryMsg.Text = "Invalid license key";
                    return false;
                }

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
                            {
                                if (settings["LicenseInfo"] == null)
                                    settings.Add("LicenseInfo", string.Format("Started Date: {0}; End Date: {1}", dtStartDateime.Date, dtEndDateime.Date));
                                
                                return true;
                            }
                            else
                                return false;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void LicenseForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //this.DialogResult = DialogResult.Cancel;
        }

        private void LicenseForm_Load(object sender, EventArgs e)
        {
            loader = new UCLoaderForm();
        }
    }
}
