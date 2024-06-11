using System;
using System.Windows.Forms;

namespace Drakengard1and2Extractor
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
        }

        private void AboutWindowBtn_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
