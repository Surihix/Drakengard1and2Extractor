using System;
using System.Windows.Forms;

namespace Drakengard1and2Extractor.ImageConversion
{
    public partial class SPK0Form : Form
    {
        public SPK0Form()
        {
            InitializeComponent();

            Spk0SaveAsComboBox.SelectedIndex = 0;
            Spk0AlphaCompNumericUpDown.Enabled = false;
        }


        private void Spk0SaveAsComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (Spk0SaveAsComboBox.SelectedIndex == 0)
            {
                Spk0AlphaCompNumericUpDown.Enabled = false;
            }
            if (Spk0SaveAsComboBox.SelectedIndex == 1 || Spk0SaveAsComboBox.SelectedIndex == 2)
            {
                Spk0AlphaCompNumericUpDown.Enabled = true;
            }
        }


        private void ConvertSPK0ImgBtn_MouseHover(object sender, EventArgs e)
        {
            ConvertSpk0ImgBtnToolTip.Show("Converts and saves the image files to one of the selected formats", ConvertSPK0ImgBtn);
        }
        private void ConvertSPK0ImgBtn_Click(object sender, EventArgs e)
        {
            ConverterWindow.SaveAsIndex = Spk0SaveAsComboBox.SelectedIndex;
            ConverterWindow.AlphaIncrease = (int)Spk0AlphaCompNumericUpDown.Value;

            ConverterWindow.IsClosedByConvtBtn = true;

            Dispose();
            Close();
        }
    }
}