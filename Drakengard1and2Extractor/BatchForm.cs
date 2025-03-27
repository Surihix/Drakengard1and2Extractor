using Drakengard1and2Extractor.FileExtraction;
using Drakengard1and2Extractor.ImageConversion;
using Drakengard1and2Extractor.Support;
using Ookii.Dialogs.WinForms;
using System;
using System.IO;
using System.Windows.Forms;

namespace Drakengard1and2Extractor
{
    public partial class BatchForm : Form
    {
        public BatchForm()
        {
            InitializeComponent();

            BatchStatusTextBox.BackColor = System.Drawing.SystemColors.Window;
        }


        private void BatchForm_Shown(object sender, EventArgs e)
        {
            LoggingMethods.SetBatchFormStatusBox();
        }


        private void BatchExtractFPKBtn_MouseHover(object sender, EventArgs e)
        {
            BatchExtractFPKtoolTip.Show("Extract all FPK files present inside a folder.", BatchFPKBtn);
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

                    SharedMethods.CheckLzoDll(false);

                    LoggingMethods.LogMessage("Extracting fpk files....");

                    var fpkDir = fpkDirSelect.SelectedPath + "\\";
                    var fpkFilesInDir = Directory.GetFiles(fpkDir, "*.fpk", SearchOption.TopDirectoryOnly);

                    var pathResult = MessageBox.Show("Try and generate file paths if .lst files are available in the fpk files?\nWarning: This is an experimental option and if you are unsure of what to use, then its recommended to select the 'NO' option.", "Path Generation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    var generateLstPaths = SharedMethods.SetBoolFromDlgResult(pathResult);

                    System.Threading.Tasks.Task.Run(() =>
                    {
                        try
                        {
                            foreach (var fpkFile in fpkFilesInDir)
                            {
                                var foundHeader = SharedMethods.GetHeaderString(fpkFile);

                                if (foundHeader == "fpk")
                                {
                                    ExtFPK.ExtractFPK(fpkFile, generateLstPaths, false);
                                    LoggingMethods.LogMessage("Extracted " + Path.GetFileName(fpkFile));
                                }
                            }
                        }
                        finally
                        {
                            LoggingMethods.LogMessage(SharedMethods.NewLineChara);
                            LoggingMethods.LogMessage("Batch extraction completed!");

                            SharedMethods.AppMsgBox("Finished extracting fpk files from the folder", "Success", MessageBoxIcon.Information);
                            BeginInvoke(new Action(() => EnableDisableControls(true)));
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                SharedMethods.AppMsgBox("" + ex, "Error", MessageBoxIcon.Error);
                LoggingMethods.LogException("Exception: " + ex);
                Close();
            }
        }


        private void BatchExtractDPKBtn_MouseHover(object sender, EventArgs e)
        {
            BatchExtractDPKtoolTip.Show("Extract all DPK files present inside a folder.", BatchDPKBtn);
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
                    LoggingMethods.LogMessage("Extracting dpk files....");

                    var dpkDir = dpkDirSelect.SelectedPath + "\\";
                    var dpkFilesInDir = Directory.GetFiles(dpkDir, "*.dpk", SearchOption.TopDirectoryOnly);

                    System.Threading.Tasks.Task.Run(() =>
                    {
                        try
                        {
                            foreach (var dpkFile in dpkFilesInDir)
                            {
                                var foundHeader = SharedMethods.GetHeaderString(dpkFile);

                                if (foundHeader == "dpk")
                                {
                                    ExtDPK.ExtractDPK(dpkFile, false);
                                    LoggingMethods.LogMessage("Extracted " + Path.GetFileName(dpkFile));
                                }
                            }
                        }
                        finally
                        {
                            LoggingMethods.LogMessage(SharedMethods.NewLineChara);
                            LoggingMethods.LogMessage("Batch extraction completed!");

                            SharedMethods.AppMsgBox("Finished extracting dpk files from the folder", "Success", MessageBoxIcon.Information);
                            BeginInvoke(new Action(() => EnableDisableControls(true)));
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                SharedMethods.AppMsgBox("" + ex, "Error", MessageBoxIcon.Error);
                LoggingMethods.LogException("Exception: " + ex);
                Close();
            }
        }


        private void BatchExtractKPSBtn_MouseHover(object sender, EventArgs e)
        {
            BatchExtractKPStoolTip.Show("Extract all KPS files present inside a folder.", BatchKPSBtn);
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
                    LoggingMethods.LogMessage("Extracting kps files....");

                    var kpsDir = kpsDirSelect.SelectedPath + "\\";
                    var kpsFilesInDir = Directory.GetFiles(kpsDir, "*.kps", SearchOption.TopDirectoryOnly);

                    var shiftJISResult = MessageBox.Show("Parse the text data in Japanese Encoding (shift-jis) format ?", "ShiftJIS", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    var shiftJISParse = SharedMethods.SetBoolFromDlgResult(shiftJISResult);

                    System.Threading.Tasks.Task.Run(() =>
                    {
                        try
                        {
                            foreach (var kpsFile in kpsFilesInDir)
                            {
                                var foundHeader = SharedMethods.GetHeaderString(kpsFile);

                                if (foundHeader == "KPS_")
                                {
                                    ExtKPS.ExtractKPS(kpsFile, shiftJISParse, false);
                                    LoggingMethods.LogMessage("Extracted " + Path.GetFileName(kpsFile));
                                }
                            }
                        }
                        finally
                        {
                            LoggingMethods.LogMessage(SharedMethods.NewLineChara);
                            LoggingMethods.LogMessage("Batch extraction completed!");

                            SharedMethods.AppMsgBox("Finished extracting kps files from the folder", "Success", MessageBoxIcon.Information);
                            BeginInvoke(new Action(() => EnableDisableControls(true)));
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                SharedMethods.AppMsgBox("" + ex, "Error", MessageBoxIcon.Error);
                LoggingMethods.LogException("Exception: " + ex);
                Close();
            }
        }


        private void BatchConvertZIMBtn_MouseHover(object sender, EventArgs e)
        {
            BatchZIMtoolTip.Show("Convert all ZIM files present inside a folder.", BatchZIMBtn);
        }
        private void BatchConvertZIMBtn_Click(object sender, EventArgs e)
        {
            try
            {
                var currentWindow = (BatchForm)Application.OpenForms[1];
                var zimDirSelect = new VistaFolderBrowserDialog
                {
                    Description = "Select a folder that has zim files",
                    UseDescriptionForTitle = true
                };

                if (zimDirSelect.ShowDialog(currentWindow.Handle) == true)
                {
                    EnableDisableControls(false);
                    LoggingMethods.LogMessage("Converting zim files....");

                    var zimDir = zimDirSelect.SelectedPath + "\\";
                    var zimFilesInDir = Directory.GetFiles(zimDir, "*.zim", SearchOption.TopDirectoryOnly);

                    ImgOptions.IsClosedByConvtBtn = false;
                    var converterWindow = new ZIMForm();
                    converterWindow.ShowDialog();

                    if (ImgOptions.IsClosedByConvtBtn)
                    {
                        System.Threading.Tasks.Task.Run(() =>
                        {
                            try
                            {
                                foreach (var zimFile in zimFilesInDir)
                                {
                                    var foundHeader = SharedMethods.GetHeaderString(zimFile);

                                    if (foundHeader == "wZIM")
                                    {
                                        ImgZIM.ConvertZIM(zimFile, false);
                                        LoggingMethods.LogMessage("Converted " + Path.GetFileName(zimFile));
                                    }
                                }
                            }
                            finally
                            {
                                LoggingMethods.LogMessage(SharedMethods.NewLineChara);
                                LoggingMethods.LogMessage("Batch conversion completed!");

                                SharedMethods.AppMsgBox("Finished converting zim files from the folder", "Success", MessageBoxIcon.Information);
                                BeginInvoke(new Action(() => EnableDisableControls(true)));
                            }
                        });
                    }
                    else
                    {
                        LoggingMethods.LogMessage(SharedMethods.NewLineChara);
                        LoggingMethods.LogMessage("Conversion cancelled!");
                        EnableDisableControls(true);
                    }
                }
            }
            catch (Exception ex)
            {
                SharedMethods.AppMsgBox("" + ex, "Error", MessageBoxIcon.Error);
                LoggingMethods.LogException("Exception: " + ex);
                Close();
            }
        }


        private void BatchConvertSPK0Btn_MouseHover(object sender, EventArgs e)
        {
            BatchSPK0toolTip.Show("Convert all SPK0 files present inside a folder.", BatchSPK0Btn);
        }
        private void BatchConvertSPK0Btn_Click(object sender, EventArgs e)
        {
            try
            {
                var currentWindow = (BatchForm)Application.OpenForms[1];
                var spk0DirSelect = new VistaFolderBrowserDialog
                {
                    Description = "Select a folder that has spk0 files",
                    UseDescriptionForTitle = true
                };

                if (spk0DirSelect.ShowDialog(currentWindow.Handle) == true)
                {
                    EnableDisableControls(false);
                    LoggingMethods.LogMessage("Converting spk0 files....");

                    var spk0Dir = spk0DirSelect.SelectedPath + "\\";
                    var spk0FilesInDir = Directory.GetFiles(spk0Dir, "*.spk0", SearchOption.TopDirectoryOnly);

                    ImgOptions.IsClosedByConvtBtn = false;
                    var converterWindow = new SPK0Form();
                    converterWindow.ShowDialog();

                    if (ImgOptions.IsClosedByConvtBtn)
                    {
                        System.Threading.Tasks.Task.Run(() =>
                        {
                            try
                            {
                                foreach (var spk0File in spk0FilesInDir)
                                {
                                    var foundHeader = SharedMethods.GetHeaderString(spk0File);

                                    if (foundHeader == "SPK0")
                                    {
                                        ImgSPK0.ConvertSPK0(spk0File, false);
                                        LoggingMethods.LogMessage("Converted " + Path.GetFileName(spk0File));
                                    }
                                }
                            }
                            finally
                            {
                                LoggingMethods.LogMessage(SharedMethods.NewLineChara);
                                LoggingMethods.LogMessage("Batch conversion completed!");

                                SharedMethods.AppMsgBox("Finished converting spk0 files from the folder", "Success", MessageBoxIcon.Information);
                                BeginInvoke(new Action(() => EnableDisableControls(true)));
                            }
                        });
                    }
                    else
                    {
                        LoggingMethods.LogMessage(SharedMethods.NewLineChara);
                        LoggingMethods.LogMessage("Conversion cancelled!");
                        EnableDisableControls(true);
                    }
                }
            }
            catch (Exception ex)
            {
                SharedMethods.AppMsgBox("" + ex, "Error", MessageBoxIcon.Error);
                LoggingMethods.LogException("Exception: " + ex);
                Close();
            }

        }


        private void EnableDisableControls(bool isEnabled)
        {
            BatchDPKBtn.Enabled = isEnabled;
            BatchFPKBtn.Enabled = isEnabled;
            BatchKPSBtn.Enabled = isEnabled;
            BatchZIMBtn.Enabled = isEnabled;
            BatchSPK0Btn.Enabled = isEnabled;
            BatchStatusDelBtn.Enabled = isEnabled;
        }


        private void BatchStatusDelBtn_Click(object sender, EventArgs e)
        {
            BatchStatusTextBox.Clear();
        }


        private void BatchForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            LoggingMethods.SetCoreFormStatusBox();
            Dispose();
            Hide();
        }
    }
}