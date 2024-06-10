using Drakengard1and2Extractor.FileExtraction;
using Drakengard1and2Extractor.Support;
using Drakengard1and2Extractor.Support.LoggingHelpers;
using Ookii.Dialogs.WinForms;
using System;
using System.IO;
using System.Windows.Forms;

namespace Drakengard1and2Extractor
{
    public partial class BatchForm : Form
    {
        private static readonly string _NewLineChara = Environment.NewLine;

        public BatchForm()
        {
            InitializeComponent();

            BatchStatusTextBox.BackColor = System.Drawing.SystemColors.Window;
        }


        private void BatchExtractFPKBtn_MouseHover(object sender, EventArgs e)
        {
            BatchExtractFPKtoolTip.Show("Extract all FPK files present inside a folder", BatchExtractFPKBtn);
        }
        private void BatchExtractFPKBtn_Click(object sender, EventArgs e)
        {
            try
            {
                var currentWindow = (BatchForm)Application.OpenForms[1];
                var fpkDirSelect = new VistaFolderBrowserDialog
                {
                    Description = "Select a folder that has fpk files",
                    UseDescriptionForTitle = true
                };

                if (fpkDirSelect.ShowDialog(currentWindow.Handle) == true)
                {
                    EnableDisableControls(false);
                    BatchFormLogHelper.LogMessage("Extracting fpk files....");

                    var fpkDir = fpkDirSelect.SelectedPath + "\\";
                    var fpkFilesInDir = Directory.GetFiles(fpkDir, "*.fpk", SearchOption.TopDirectoryOnly);

                    System.Threading.Tasks.Task.Run(() =>
                    {
                        try
                        {
                            foreach (var fpkFile in fpkFilesInDir)
                            {
                                var readHeader = CommonMethods.HeaderCheck(fpkFile);

                                if (readHeader == "fpk")
                                {
                                    FileFPK.ExtractFPK(fpkFile, false);
                                    BatchFormLogHelper.LogMessage("Extracted " + Path.GetFileName(fpkFile));
                                }
                            }
                        }
                        finally
                        {
                            BatchFormLogHelper.LogMessage(_NewLineChara);
                            BatchFormLogHelper.LogMessage("Batch extraction completed!");

                            CommonMethods.AppMsgBox("Finished extracting fpk files from the folder", "Success", MessageBoxIcon.Information);
                            BeginInvoke(new Action(() => EnableDisableControls(true)));
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                CommonMethods.AppMsgBox("" + ex, "Error", MessageBoxIcon.Error);
                BatchFormLogHelper.LogException("Exception: " + ex);
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
                var currentWindow = (BatchForm)Application.OpenForms[1];
                var dpkDirSelect = new VistaFolderBrowserDialog
                {
                    Description = "Select a folder that has dpk files",
                    UseDescriptionForTitle = true
                };

                if (dpkDirSelect.ShowDialog(currentWindow.Handle) == true)
                {
                    EnableDisableControls(false);
                    BatchFormLogHelper.LogMessage("Extracting dpk files....");

                    var dpkDir = dpkDirSelect.SelectedPath + "\\";
                    var dpkFilesInDir = Directory.GetFiles(dpkDir, "*.dpk", SearchOption.TopDirectoryOnly);

                    System.Threading.Tasks.Task.Run(() =>
                    {
                        try
                        {
                            foreach (var dpkFile in dpkFilesInDir)
                            {
                                var readHeader = CommonMethods.HeaderCheck(dpkFile);

                                if (readHeader == "dpk")
                                {
                                    FileDPK.ExtractDPK(dpkFile, false);
                                    BatchFormLogHelper.LogMessage("Extracted " + Path.GetFileName(dpkFile));
                                }
                            }
                        }
                        finally
                        {
                            BatchFormLogHelper.LogMessage(_NewLineChara);
                            BatchFormLogHelper.LogMessage("Batch extraction completed!");

                            CommonMethods.AppMsgBox("Finished extracting dpk files from the folder", "Success", MessageBoxIcon.Information);
                            BeginInvoke(new Action(() => EnableDisableControls(true)));
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                CommonMethods.AppMsgBox("" + ex, "Error", MessageBoxIcon.Error);
                BatchFormLogHelper.LogException("Exception: " + ex);
                Dispose();
                Close();
            }
        }


        private void BatchExtractKPSBtn_MouseHover(object sender, EventArgs e)
        {
            BatchExtractKPStoolTip.Show("Extract all KPS files present inside a folder", BatchExtractKPSBtn);
        }
        private void BatchExtractKPSBtn_Click(object sender, EventArgs e)
        {
            try
            {
                var currentWindow = (BatchForm)Application.OpenForms[1];
                var kpsDirSelect = new VistaFolderBrowserDialog
                {
                    Description = "Select a folder that has kps files",
                    UseDescriptionForTitle = true
                };

                if (kpsDirSelect.ShowDialog(currentWindow.Handle) == true)
                {
                    EnableDisableControls(false);
                    BatchFormLogHelper.LogMessage("Extracting kps files....");

                    var kpsDir = kpsDirSelect.SelectedPath + "\\";
                    var kpsFilesInDir = Directory.GetFiles(kpsDir, "*.kps", SearchOption.TopDirectoryOnly);

                    var shiftJISParse = false;

                    var shiftJISResult = MessageBox.Show("Parse the text data in Japanese Encoding (shift-jis) format ? ", "ShiftJIS", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (shiftJISResult == DialogResult.Yes)
                    {
                        shiftJISParse = true;
                    }

                    System.Threading.Tasks.Task.Run(() =>
                    {
                        try
                        {
                            foreach (var kpsFile in kpsFilesInDir)
                            {
                                var readHeader = CommonMethods.HeaderCheck(kpsFile);

                                if (readHeader == "KPS_")
                                {
                                    FileKPS.ExtractKPS(kpsFile, shiftJISParse, false);
                                    BatchFormLogHelper.LogMessage("Extracted " + Path.GetFileName(kpsFile));
                                }
                            }
                        }
                        finally
                        {
                            BatchFormLogHelper.LogMessage(_NewLineChara);
                            BatchFormLogHelper.LogMessage("Batch extraction completed!");

                            CommonMethods.AppMsgBox("Finished extracting kps files from the folder", "Success", MessageBoxIcon.Information);
                            BeginInvoke(new Action(() => EnableDisableControls(true)));
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                CommonMethods.AppMsgBox("" + ex, "Error", MessageBoxIcon.Error);
                BatchFormLogHelper.LogException("Exception: " + ex);
                Close();
            }
        }


        private void EnableDisableControls(bool isEnabled)
        {
            BatchExtractDPKBtn.Enabled = isEnabled;
            BatchExtractFPKBtn.Enabled = isEnabled;
            BatchExtractKPSBtn.Enabled = isEnabled;
        }

        private void BatchStatusDelBtn_Click(object sender, EventArgs e)
        {
            BatchStatusTextBox.Clear();
        }

        private void BatchForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Dispose();
            Hide();
        }

        private void BatchForm_Shown(object sender, EventArgs e)
        {
            BatchFormLogHelper.SetStatusTxtBox();
        }
    }
}