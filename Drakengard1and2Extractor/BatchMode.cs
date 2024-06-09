using Drakengard1and2Extractor.Support;
using Drakengard1and2Extractor.FileExtraction;
using Ookii.Dialogs.WinForms;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Drakengard1and2Extractor
{
    public partial class BatchMode : Form
    {
        public BatchMode(bool isDrk2RadioBtnChecked)
        {
            InitializeComponent();

            if (isDrk2RadioBtnChecked.Equals(false))
            {
                BatchExtractDPKBtn.Enabled = false;
            }
        }


        public void StatusMsg(string message)
        {
            BatchStatusListBox.Items.Add(message);
            BatchStatusListBox.SelectedIndex = BatchStatusListBox.Items.Count - 1;
            BatchStatusListBox.SelectedIndex = -1;
        }


        private void BatchExtractFPKBtn_MouseHover(object sender, EventArgs e)
        {
            BatchExtractFPKtoolTip.Show("Extract all FPK files present inside a folder", BatchExtractFPKBtn);
        }
        private void BatchExtractFPKBtn_Click(object sender, EventArgs e)
        {
            try
            {
                var currentWindow = (BatchMode)Application.OpenForms[1];
                var fpkDirSelect = new VistaFolderBrowserDialog();
                fpkDirSelect.Description = "Select a folder that has fpk files";
                fpkDirSelect.UseDescriptionForTitle = true;

                if (fpkDirSelect.ShowDialog(currentWindow.Handle) == true)
                {
                    DisableButtons();
                    BatchStatusListBox.Items.Clear();
                    StatusMsg("Extracting fpk files....");

                    var fpkDir = fpkDirSelect.SelectedPath + "\\";
                    var fpkFilesInDir = Directory.GetFiles(fpkDir, "*.fpk", SearchOption.TopDirectoryOnly);

                    Task.Run(() =>
                    {
                        try
                        {
                            foreach (var fpkFile in fpkFilesInDir)
                            {
                                var readHeader = CommonMethods.HeaderCheck(fpkFile);

                                if (readHeader == "fpk")
                                {
                                    FileFPK.ExtractFPK(fpkFile, false);
                                }
                            }
                        }
                        finally
                        {
                            CommonMethods.AppMsgBox("Finished extracting fpk files from the folder", "Success", MessageBoxIcon.Information);
                            BatchStatusListBox.BeginInvoke((Action)(() => StatusMsg("")));
                            BatchStatusListBox.BeginInvoke((Action)(() => StatusMsg("Batch extraction completed")));
                            BeginInvoke(new Action(() => EnableButtons()));
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                CommonMethods.AppMsgBox("" + ex, "Error", MessageBoxIcon.Error);
                Close();
            }
        }


        private void BatchExtractDPKBtn_MouseHover(object sender, EventArgs e)
        {
            BatchExtractDPKtoolTip.Show("Extract all DPK files present inside a folder", BatchExtractDPKBtn);
        }
        private void BatchExtractDPKBtn_Click(object sender, EventArgs e)
        {
            try
            {
                var currentWindow = (BatchMode)Application.OpenForms[1];
                var dpkDirSelect = new VistaFolderBrowserDialog();
                dpkDirSelect.Description = "Select a folder that has dpk files";
                dpkDirSelect.UseDescriptionForTitle = true;

                if (dpkDirSelect.ShowDialog(currentWindow.Handle) == true)
                {
                    DisableButtons();
                    BatchStatusListBox.Items.Clear();
                    StatusMsg("Extracting dpk files....");

                    var dpkDir = dpkDirSelect.SelectedPath + "\\";
                    var dpkFilesInDir = Directory.GetFiles(dpkDir, "*.dpk", SearchOption.TopDirectoryOnly);

                    Task.Run(() =>
                    {
                        try
                        {
                            foreach (var dpkFile in dpkFilesInDir)
                            {
                                var readHeader = CommonMethods.HeaderCheck(dpkFile);

                                if (readHeader == "dpk")
                                {
                                    FileDPK.ExtractDPK(dpkFile, false);
                                }
                            }
                        }
                        finally
                        {
                            CommonMethods.AppMsgBox("Finished extracting dpk files from the folder", "Success", MessageBoxIcon.Information);

                            BatchStatusListBox.BeginInvoke((Action)(() => StatusMsg("")));
                            BatchStatusListBox.BeginInvoke((Action)(() => StatusMsg("Batch extraction completed")));
                            BeginInvoke(new Action(() => EnableButtons()));
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                CommonMethods.AppMsgBox("" + ex, "Error", MessageBoxIcon.Error);
                Close();
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