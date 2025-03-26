using Drakengard1and2Extractor.BinRepack;
using Drakengard1and2Extractor.Support;
using Ookii.Dialogs.WinForms;
using System;
using System.IO;
using System.Windows.Forms;

namespace Drakengard1and2Extractor.FileRepack
{
    public partial class RepackForm : Form
    {
        public RepackForm()
        {
            InitializeComponent();

            Drk1RadioButton.Checked = true;

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
            try
            {
                var binSelect = new OpenFileDialog();
                if (Drk1RadioButton.Checked == true)
                {
                    binSelect.Filter = "Drakengard 1 BIN files" + $"|*.bin";
                }
                if (Drk2RadioButton.Checked == true)
                {
                    binSelect.Filter = "Drakengard 2 BIN files" + $"|*.bin";
                }

                binSelect.RestoreDirectory = true;

                if (binSelect.ShowDialog() == DialogResult.OK)
                {
                    var mbinFile = binSelect.FileName;

                    if (Drk1RadioButton.Checked)
                    {
                        RepackStatusTextBox.AppendText("Game is set to Drakengard 1");
                        RepackStatusTextBox.AppendText(SharedMethods.NewLineChara);
                        RepackStatusTextBox.AppendText(SharedMethods.NewLineChara);

                        EnableDisableControls(false);

                        var foundHeader = SharedMethods.GetHeaderString(mbinFile);

                        if (foundHeader == "fpk")
                        {
                            var mbinDirSelect = new VistaFolderBrowserDialog
                            {
                                Description = "Select the unpacked bin folder",
                                UseDescriptionForTitle = true
                            };

                            var currentWindow = (RepackForm)Application.OpenForms[1];
                            if (mbinDirSelect.ShowDialog(currentWindow.Handle) == true)
                            {
                                var mbinDir = mbinDirSelect.SelectedPath;
                                EnableDisableControls(false);

                                System.Threading.Tasks.Task.Run(() =>
                                {
                                    try
                                    {
                                        LoggingMethods.LogMessage("Repacking files from " + Path.GetFileName(mbinFile) + "....");
                                        RpkDrk1BIN.RepackBin(mbinFile, mbinDir);
                                    }
                                    finally
                                    {
                                        BeginInvoke(new Action(() => EnableDisableControls(true)));
                                    }
                                });
                            }
                            else
                            {
                                EnableDisableControls(true);
                                return;
                            }
                        }
                        else
                        {
                            RepackStatusTextBox.AppendText("Error: Unable to detect fpk header");
                            RepackStatusTextBox.AppendText(SharedMethods.NewLineChara);

                            SharedMethods.AppMsgBox("This is not a valid Drakengard 1 .bin file", "Error", MessageBoxIcon.Error);
                            RepackStatusTextBox.AppendText("Extraction failed!");
                            RepackStatusTextBox.AppendText(SharedMethods.NewLineChara);
                            RepackStatusTextBox.AppendText(SharedMethods.NewLineChara);

                            EnableDisableControls(true);
                            return;
                        }
                    }

                    if (Drk2RadioButton.Checked)
                    {
                        RepackStatusTextBox.AppendText("Game is set to Drakengard 2");
                        RepackStatusTextBox.AppendText(SharedMethods.NewLineChara);
                        RepackStatusTextBox.AppendText(SharedMethods.NewLineChara);

                        EnableDisableControls(false);

                        var foundHeader = SharedMethods.GetHeaderString(mbinFile);

                        if (foundHeader == "dpk")
                        {
                            var mbinDirSelect = new VistaFolderBrowserDialog
                            {
                                Description = "Select the unpacked bin folder",
                                UseDescriptionForTitle = true
                            };

                            var currentWindow = (RepackForm)Application.OpenForms[1];
                            if (mbinDirSelect.ShowDialog(currentWindow.Handle) == true)
                            {
                                var mbinDir = mbinDirSelect.SelectedPath;
                                EnableDisableControls(false);

                                System.Threading.Tasks.Task.Run(() =>
                                {
                                    try
                                    {
                                        LoggingMethods.LogMessage("Repacking files from " + Path.GetFileName(mbinFile) + "....");
                                        RpkDrk2BIN.RepackBin(mbinFile, mbinDir);
                                    }
                                    finally
                                    {
                                        BeginInvoke(new Action(() => EnableDisableControls(true)));
                                    }
                                });
                            }
                            else
                            {
                                EnableDisableControls(true);
                                return;
                            }
                        }
                        else
                        {
                            RepackStatusTextBox.AppendText("Error: Unable to detect dpk header");
                            RepackStatusTextBox.AppendText(SharedMethods.NewLineChara);

                            SharedMethods.AppMsgBox("This is not a valid Drakengard 2 .bin file", "Error", MessageBoxIcon.Error);
                            RepackStatusTextBox.AppendText("Extraction failed!");
                            RepackStatusTextBox.AppendText(SharedMethods.NewLineChara);
                            RepackStatusTextBox.AppendText(SharedMethods.NewLineChara);

                            EnableDisableControls(true);
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SharedMethods.AppMsgBox("" + ex, "Error", MessageBoxIcon.Error);
                LoggingMethods.LogMessage(SharedMethods.NewLineChara);
                LoggingMethods.LogException("Exception: " + ex);
            }
        }


        private void RpkFpkBtn_MouseHover(object sender, EventArgs e)
        {
            RpkFpkToolTip.Show("Repack a .fpk file.", RpkFpkBtn);
        }
        private void RpkFpkBtn_Click(object sender, EventArgs e)
        {
            try
            {
                var fpkSelect = new OpenFileDialog
                {
                    Filter = "FPK type files (*.fpk)" + $"|*.fpk",
                    RestoreDirectory = true
                };

                if (fpkSelect.ShowDialog() == DialogResult.OK)
                {
                    var fpkDirSelect = new VistaFolderBrowserDialog
                    {
                        Description = "Select the unpacked fpk folder",
                        UseDescriptionForTitle = true
                    };

                    var currentWindow = (RepackForm)Application.OpenForms[1];
                    if (fpkDirSelect.ShowDialog(currentWindow.Handle) == true)
                    {
                        var fpkFile = fpkSelect.FileName;
                        var fpkDir = fpkDirSelect.SelectedPath;
                        EnableDisableControls(false);

                        System.Threading.Tasks.Task.Run(() =>
                        {
                            try
                            {
                                LoggingMethods.LogMessage("Repacking files from " + Path.GetFileName(fpkFile) + "....");
                                RpkFPK.RepackFPK(fpkFile, fpkDir);
                            }
                            finally
                            {
                                BeginInvoke(new Action(() => EnableDisableControls(true)));
                            }
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                SharedMethods.AppMsgBox("" + ex, "Error", MessageBoxIcon.Error);
                LoggingMethods.LogMessage(SharedMethods.NewLineChara);
                LoggingMethods.LogException("Exception: " + ex);
            }
        }


        private void RpkDpkBtn_MouseHover(object sender, EventArgs e)
        {
            RpkDpkToolTip.Show("Repack a .dpk file from Drakengard 2.", RpkDpkBtn);
        }
        private void RpkDpkBtn_Click(object sender, EventArgs e)
        {
            try
            {
                var dpkSelect = new OpenFileDialog
                {
                    Filter = "DPK type files (*.dpk)" + $"|*.dpk",
                    RestoreDirectory = true
                };

                if (dpkSelect.ShowDialog() == DialogResult.OK)
                {
                    var dpkDirSelect = new VistaFolderBrowserDialog
                    {
                        Description = "Select the unpacked dpk folder",
                        UseDescriptionForTitle = true
                    };

                    var currentWindow = (RepackForm)Application.OpenForms[1];
                    if (dpkDirSelect.ShowDialog(currentWindow.Handle) == true)
                    {
                        var dpkFile = dpkSelect.FileName;
                        var dpkDir = dpkDirSelect.SelectedPath;
                        EnableDisableControls(false);

                        System.Threading.Tasks.Task.Run(() =>
                        {
                            try
                            {
                                LoggingMethods.LogMessage("Repacking files from " + Path.GetFileName(dpkFile) + "....");
                                RpkDPK.RepackDPK(dpkFile, dpkDir);
                            }
                            finally
                            {
                                BeginInvoke(new Action(() => EnableDisableControls(true)));
                            }
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                SharedMethods.AppMsgBox("" + ex, "Error", MessageBoxIcon.Error);
                LoggingMethods.LogMessage(SharedMethods.NewLineChara);
                LoggingMethods.LogException("Exception: " + ex);
            }
        }


        private void RpkKpsBtn_MouseHover(object sender, EventArgs e)
        {
            RpkKpsToolTip.Show("Repack a .txt file to .kps file.", RpkKpsBtn);
        }
        private void RpkKpsBtn_Click(object sender, EventArgs e)
        {
            try
            {
                var txtSelect = new OpenFileDialog
                {
                    Filter = "Text files (*.txt)" + $"|*.txt",
                    RestoreDirectory = true
                };

                if (txtSelect.ShowDialog() == DialogResult.OK)
                {
                    var txtFile = txtSelect.FileName;
                    EnableDisableControls(false);

                    var shiftJISResult = MessageBox.Show("Parse the text data in Japanese Encoding (shift-jis) format ?", "ShiftJIS", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    var shiftJISParse = SharedMethods.SetBoolFromDlgResult(shiftJISResult);

                    System.Threading.Tasks.Task.Run(() =>
                    {
                        try
                        {
                            LoggingMethods.LogMessage("Repacking text data from " + Path.GetFileName(txtFile) + "....");
                            RpkKPS.RepackKPS(txtFile, shiftJISParse);
                        }
                        finally
                        {
                            BeginInvoke(new Action(() => EnableDisableControls(true)));
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                SharedMethods.AppMsgBox("" + ex, "Error", MessageBoxIcon.Error);
                LoggingMethods.LogMessage(SharedMethods.NewLineChara);
                LoggingMethods.LogException("Exception: " + ex);
            }
        }


        private void EnableDisableControls(bool isEnabled)
        {
            BinRepackGroupBox.Enabled = isEnabled;
            FileRpkGrp.Enabled = isEnabled;
            RepackStatusDelBtn.Enabled = isEnabled;
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