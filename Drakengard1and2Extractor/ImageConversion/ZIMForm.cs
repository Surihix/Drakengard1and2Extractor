using System;
using System.Windows.Forms;

namespace Drakengard1and2Extractor.ImageConversion
{
    public partial class ZIMForm : Form
    {
        public ZIMForm()
        {
            InitializeComponent();

            ZimSaveAsComboBox.SelectedIndex = 0;
            ZimAlphaCompNumericUpDown.Enabled = false;
        }


        private void ZimSaveAsComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (ZimSaveAsComboBox.SelectedIndex == 0)
            {
                ZimAlphaCompNumericUpDown.Enabled = false;
            }
            if (ZimSaveAsComboBox.SelectedIndex == 1 || ZimSaveAsComboBox.SelectedIndex == 2)
            {
                ZimAlphaCompNumericUpDown.Enabled = true;
            }
        }


        private void UnSwizzleCheckBox_MouseHover(object sender, EventArgs e)
        {
            UnSwizzleChkBoxToolTip.Show("Warning: Use this option only when the converted image looks corrupt.", UnSwizzleCheckBox);
        }


        private void ConvertZIMImgBtn_MouseHover(object sender, EventArgs e)
        {
            ConvertZimImgBtnToolTip.Show("Converts and saves the image file to one of the selected formats", ConvertZIMImgBtn);
        }
        private void ConvertZIMImgBtn_Click(object sender, EventArgs e)
        {
            ConverterWindow.SaveAsIndex = ZimSaveAsComboBox.SelectedIndex;
            ConverterWindow.AlphaIncrease = (int)ZimAlphaCompNumericUpDown.Value;
            ConverterWindow.UnswizzlePixels = UnSwizzleCheckBox.Checked;

            ConverterWindow.IsClosedByConvtBtn = true;

            Dispose();
            Close();
        }
    }
}