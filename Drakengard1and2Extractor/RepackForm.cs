using Drakengard1and2Extractor.Support;
using System;
using System.Windows.Forms;

namespace Drakengard1and2Extractor.FileRepack
{
    public partial class RepackForm : Form
    {
        public RepackForm()
        {
            InitializeComponent();

            RepackStatusTextBox.BackColor = System.Drawing.SystemColors.Window;
        }

        private void RepackForm_Shown(object sender, EventArgs e)
        {
            LoggingMethods.SetRepackFormStatusBox();
        }


        private void RpkBinBtn_MouseHover(object sender, EventArgs e)
        {
            BINRpkToolTip.Show("Repack a compatible .BIN file.", RpkBinBtn);
        }
        private void RpkBinBtn_Click(object sender, EventArgs e)
        {

        }


        private void RpkFpkBtn_MouseHover(object sender, EventArgs e)
        {
            RpkFpkToolTip.Show("Repack a .fpk file.", RpkFpkBtn);
        }
        private void RpkFpkBtn_Click(object sender, EventArgs e)
        {

        }


        private void RpkDpkBtn_MouseHover(object sender, EventArgs e)
        {
            RpkDpkToolTip.Show("Repack a .dpk file.", RpkDpkBtn);
        }
        private void RpkDpkBtn_Click(object sender, EventArgs e)
        {

        }


        private void RpkKpsBtn_MouseHover(object sender, EventArgs e)
        {
            RpkKpsToolTip.Show("Repack a .kps file.", RpkKpsBtn);
        }
        private void RpkKpsBtn_Click(object sender, EventArgs e)
        {

        }


        private void EnableDisableControls(bool isEnabled)
        {
            BinRepackGroupBox.Enabled = isEnabled;
            FileRpkGrp.Enabled = isEnabled;
        }


        private void RepackStatusDelBtn_Click(object sender, EventArgs e)
        {
            RepackStatusTextBox.Clear();
        }


        private void RepackForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            LoggingMethods.SetCoreFormStatusBox();
            Dispose();
            Hide();
        }
    }
}