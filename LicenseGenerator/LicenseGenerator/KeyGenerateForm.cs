using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LicenseGenerator
{
    public partial class KeyGenerateForm : Form
    {
        public KeyGenerateForm()
        {
            InitializeComponent();
        }

        private bool ValidateEntry()
        {
            if (string.IsNullOrEmpty(txtMachineName.Text.Trim()))
            {
                MessageBox.Show("Please mention machine name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrEmpty(txtDays.Text.Trim()))
            {
                MessageBox.Show("Please mention no. of days", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void btnGenerateKey_Click(object sender, EventArgs e)
        {
            if (ValidateEntry())
            {
                string strkey = string.Empty;
                strkey = txtMachineName.Text.Trim() + "|" + dtStartDate.Text.Trim() + "|" + dtEndDate.Text.Trim();
                txtKey.Text = UIFunctionality.Common.License.Encrypt(strkey);
            }

            #region [Commented]


            ////Checksum chk;
            ////string strMachineName;
            ////string strStartDate;
            ////string strEndDate;

            ////uint csum;
            ////bool useBase10 = false;
            ////int numberLength = 5;

            ////chk = new Checksum();
            ////chk.ChecksumAlgorithm = Checksum.ChecksumType.Type2;

            ////strMachineName = txtMachineName.Text.Trim();// + "|";
            ////chk.CalculateChecksum(strMachineName);
            ////csum = chk.ChecksumNumber;
            ////strkey += NumberDisplay.CreateNumberString(csum, numberLength, useBase10) + "-";

            ////strStartDate = dtStartDate.Text.Trim();// + "|";
            ////chk.CalculateChecksum(strStartDate);
            ////csum = chk.ChecksumNumber;
            ////strkey += NumberDisplay.CreateNumberString(csum, numberLength, useBase10) + "-";

            ////strEndDate = dtEndDate.Text.Trim();// + "|";
            ////chk.CalculateChecksum(strEndDate);
            ////csum = chk.ChecksumNumber;
            ////strkey += NumberDisplay.CreateNumberString(csum, numberLength, useBase10);

            //chk.CalculateChecksum(Encryption.Encrypt());
            //csum = chk.ChecksumNumber;
            //strkey += NumberDisplay.CreateNumberString(csum, numberLength, useBase10);
            //txtKey.Text = strkey;

            #endregion
        }

        private void CalcDate()
        {
			try
			{
				if (string.IsNullOrEmpty(txtDays.Text.Trim()))
				{
					MessageBox.Show("Please mention no. of days");
					return;
				}
				int nDays = Convert.ToInt32(txtDays.Text.Trim());
				if (nDays <= 0)
				{
					MessageBox.Show("Days should be greater then 0");
					return;
				}
				DateTime tStart = Convert.ToDateTime(dtStartDate.Text.Trim());
				DateTime tCurrent = DateTime.Now;
				if (tStart.Date < tCurrent.Date)
				{
					MessageBox.Show("Start Date should be greater then current date");
					return;
				}
				DateTime tEnd = tStart.Date.AddDays(nDays);
				dtEndDate.Text = tEnd.ToString();
			}
			catch (Exception)
			{
				throw;
			}
		}

        private void ResetControl()
        {
            txtMachineName.Text = string.Empty;
            txtDays.Text = "0";
            dtStartDate.Text = DateTime.Now.ToString();
            dtEndDate.Text = DateTime.Now.ToString();
            txtKey.Text = string.Empty;
        }

        private void dtStartDate_ValueChanged(object sender, EventArgs e)
        {
            CalcDate();
		}

        private void txtDays_Validated(object sender, EventArgs e)
        {
            CalcDate();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ResetControl();
        }
    }
}
