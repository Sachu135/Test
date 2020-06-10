using DockSample.lib;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using UIFunctionality;
using UIFunctionality.Common;

namespace DockSample
{
    public partial class DummyOutputWindow : ToolWindow
    {
        StudioConfig studioConfig;
        public DummyOutputWindow()
        {
            InitializeComponent();
            //richTextBox1.Enabled = false;
            richTextBox1.ForeColor = System.Drawing.Color.FromArgb(0, 0, 255);
            picExecute.Enabled = true;
            picStop.Enabled = false;
            picStop.Image = global::DockSample.Properties.Resources.stopDisable;
            studioConfig = new StudioConfig(AppDomain.CurrentDomain.BaseDirectory).GetStudioConfigFromFile();
        }
        public DummyDoc ParentDoc { get; set; }
        private void picExecute_Click(object sender, System.EventArgs e)
        {
            var t = new Task(() =>
            {
                try
                {
                    ParentDoc.TxtEditor.PerformSafely(() => {
                        ParentDoc.TxtEditor.Enabled = false;
                    });
                    picExecute.PerformSafely(() =>
                    {
                        picExecute.Enabled = false;
                        picExecute.Image = global::DockSample.Properties.Resources.executeDisable1;
                    });
                    picStop.PerformSafely(() =>
                    {
                        picStop.Enabled = true;
                        picStop.Image = global::DockSample.Properties.Resources.stop2;
                    });
                    richTextBox1.PerformSafely(() =>
                    {
                        richTextBox1.Text += string.Format("{0}: {1}", DateTime.Now.ToString(), "saving file..");
                        richTextBox1.Text += Environment.NewLine;
                        SSHManager.WriteFileContent(studioConfig.sSHClientInfo.IPAddress, studioConfig.sSHClientInfo.UserName, studioConfig.sSHClientInfo.Password, ParentDoc.ToolTipText, ParentDoc.Text);
                        richTextBox1.Text += string.Format("{0}: {1}", DateTime.Now.ToString(), "File saved..");
                        richTextBox1.Text += Environment.NewLine;
                        richTextBox1.Text += "---------------------------------------------------------------------";
                        richTextBox1.Text += Environment.NewLine;

                        richTextBox1.Text += string.Format("{0}: {1}", DateTime.Now.ToString(), "executing script file..");
                        richTextBox1.Text += Environment.NewLine;
                        richTextBox1.Text += "---------------------------------------------------------------------";
                        richTextBox1.Text += Environment.NewLine;
                        SSHManager.ExecuteCommandOnConsole(studioConfig.sSHClientInfo.IPAddress, studioConfig.sSHClientInfo.UserName, studioConfig.sSHClientInfo.Password, "python", ParentDoc.ToolTipText, richTextBox1,
                            ()=>{
                                picExecute.PerformSafely(() =>
                                {
                                    picExecute.Enabled = true;
                                    picExecute.Image = global::DockSample.Properties.Resources.execute2;
                                });
                                picStop.PerformSafely(() =>
                                {
                                    picStop.Enabled = false;
                                    picStop.Image = global::DockSample.Properties.Resources.stopDisable;
                                });
                            });
                    });

                    //SSHManager.ExecuteCommandOnConsole

                }
                catch (Exception ex)
                {
                    richTextBox1.PerformSafely(() =>
                    {
                        richTextBox1.Text += string.Format("{0}: {1}", "Error found", ex.Message);
                    });
                    
                }
                finally
                {
                    picExecute.PerformSafely(() =>
                    {
                        picExecute.Enabled = true;
                        picExecute.Image = global::DockSample.Properties.Resources.execute2;
                    });
                    picStop.PerformSafely(() =>
                    {
                        picStop.Enabled = false;
                        picStop.Image = global::DockSample.Properties.Resources.stopDisable;
                    });
                    ParentDoc.TxtEditor.PerformSafely(() => {
                        ParentDoc.TxtEditor.Enabled = true;
                    });
                }

            });
            t.Start();
        }

        private void picStop_Click(object sender, System.EventArgs e)
        {
            new Task(() => {
                picExecute.PerformSafely(() =>
                {
                    picExecute.Enabled = true;
                    picExecute.Image = global::DockSample.Properties.Resources.execute2;
                });
                picStop.PerformSafely(() =>
                {
                    picStop.Enabled = false;
                    picStop.Image = global::DockSample.Properties.Resources.stopDisable;
                });
                ParentDoc.TxtEditor.PerformSafely(() => {
                    ParentDoc.TxtEditor.Enabled = true;
                });
            }).Start();
            
        }

        private void pictureBox1_Click(object sender, System.EventArgs e)
        {
            new Task(() => {
                richTextBox1.PerformSafely(() =>
                {
                    richTextBox1.Text = string.Empty;
                });
                
            }).Start();
        }
    }
}