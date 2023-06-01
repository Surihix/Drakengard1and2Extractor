using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Drakengard1and2Extractor.AppClasses
{
    public partial class BatchMode : Form
    {
        public BatchMode()
        {
            InitializeComponent();
        }


        public void StatusMsg(string message)
        {
            StatusListBox.Items.Add(message);
            StatusListBox.SelectedIndex = StatusListBox.Items.Count - 1;
            StatusListBox.SelectedIndex = -1;
        }


        private void BatchExtractFPKBtn_MouseHover(object sender, EventArgs e)
        {
            BatchExtractFPKtoolTip.Show("Extract all FPK files present inside a folder", BatchExtractFPKBtn);
        }
        private void BatchExtractFPKBtn_Click(object sender, EventArgs e)
        {
            var currentWindow = (BatchMode)Application.OpenForms[1];
            var fpkDirSelect = new FolderPicker();

            if (fpkDirSelect.ShowDialog(currentWindow.Handle) == true)
            {
                DisableButtons();
                StatusListBox.Items.Clear();
                StatusMsg("Extracting fpk files....");

                var fpkDir = fpkDirSelect.ResultName + "\\";
                var fpkFilesInDir = Directory.GetFiles(fpkDir, "*.fpk", SearchOption.TopDirectoryOnly);

                Task.Run(() =>
                {
                    try
                    {
                        foreach (var fpkFile in fpkFilesInDir)
                        {
                            var readHeader = "";
                            CmnMethods.HeaderCheck(fpkFile, ref readHeader);

                            if (readHeader.StartsWith("fpk"))
                            {
                                FileFPK.ExtractFPK(fpkFile, false);
                            }
                        }
                    }
                    finally
                    {
                        StatusListBox.BeginInvoke((Action)(() => StatusMsg("")));
                        StatusListBox.BeginInvoke((Action)(() => StatusMsg("Batch extraction completed")));

                        CmnMethods.AppMsgBox("Finished extracting fpk files from the folder", "Success", MessageBoxIcon.Information);
                        BeginInvoke(new Action(() => EnableButtons()));
                    }
                });
            }
        }


        private void BatchExtractDPKBtn_MouseHover(object sender, EventArgs e)
        {
            BatchExtractDPKtoolTip.Show("Extract all DPK files present inside a folder", BatchExtractDPKBtn);
        }
        private void BatchExtractDPKBtn_Click(object sender, EventArgs e)
        {
            var currentWindow = (BatchMode)Application.OpenForms[1];
            var dpkDirSelect = new FolderPicker();

            if (dpkDirSelect.ShowDialog(currentWindow.Handle) == true)
            {
                DisableButtons();
                StatusListBox.Items.Clear();
                StatusMsg("Extracting dpk files....");

                var dpkDir = dpkDirSelect.ResultName + "\\";
                var dpkFilesInDir = Directory.GetFiles(dpkDir, "*.dpk", SearchOption.TopDirectoryOnly);

                Task.Run(() =>
                {
                    try
                    {
                        foreach (var dpkFile in dpkFilesInDir)
                        {
                            var readHeader = "";
                            CmnMethods.HeaderCheck(dpkFile, ref readHeader);

                            if (readHeader.StartsWith("dpk"))
                            {
                                FileDPK.ExtractDPK(dpkFile, false);
                            }
                        }
                    }
                    finally
                    {
                        StatusListBox.BeginInvoke((Action)(() => StatusMsg("")));
                        StatusListBox.BeginInvoke((Action)(() => StatusMsg("Batch extraction completed")));

                        CmnMethods.AppMsgBox("Finished extracting dpk files from the folder", "Success", MessageBoxIcon.Information);
                        BeginInvoke(new Action(() => EnableButtons()));
                    }
                });
            }
        }


        private void DisableButtons()
        {
            BatchExtractDPKBtn.Enabled = false;
            BatchExtractFPKBtn.Enabled = false;
        }

        private void EnableButtons()
        {
            BatchExtractDPKBtn.Enabled = true;
            BatchExtractFPKBtn.Enabled = true;
        }
    }
}