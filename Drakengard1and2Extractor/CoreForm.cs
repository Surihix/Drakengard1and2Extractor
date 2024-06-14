using Drakengard1and2Extractor.BinExtraction;
using Drakengard1and2Extractor.FileExtraction;
using Drakengard1and2Extractor.ImageConversion;
using Drakengard1and2Extractor.Support;
using System;
using System.IO;
using System.Windows.Forms;

namespace Drakengard1and2Extractor
{
    public partial class CoreForm : Form
    {
        public CoreForm()
        {
            InitializeComponent();

            if (!File.Exists("minilzo.dll"))
            {
                SharedMethods.AppMsgBox("Missing minilz0.dll file.\nPlease check if this dll file is present next to the exe file.", "Error", MessageBoxIcon.Error);
                Environment.Exit(1);
            }

            ChecklzoDll();

            if (!File.Exists("AppHelp.txt"))
            {
                SharedMethods.AppMsgBox("The AppHelp.txt file is missing\nPlease ensure that this file is present next to the app to use the Help option.", "Warning", MessageBoxIcon.Warning);
            }

            Drk1RadioButton.Checked = true;

            StatusTextBox.BackColor = System.Drawing.SystemColors.Window;
            StatusTextBox.Text = "App was launched!";
            StatusTextBox.AppendText(SharedMethods.NewLineChara);
            StatusTextBox.AppendText(SharedMethods.NewLineChara);
        }


        private void ChecklzoDll()
        {
            var x86DllSha256 = "d414fad15b356f33bf02479bd417d2df767ee102180aae718ef1135146da2884";
            var x64DllSha256 = "ea006fafb08dd554657b1c81e45c92e88d663aca0c79c48ae1f3dca22e1e2314";
            string dllBuildHash;

            var appArchitecture = System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture;

            using (var dllStream = new FileStream("minilzo.dll", FileMode.Open, FileAccess.Read))
            {
                using (System.Security.Cryptography.SHA256 dllSHA256 = System.Security.Cryptography.SHA256.Create())
                {
                    dllBuildHash = BitConverter.ToString(dllSHA256.ComputeHash(dllStream)).Replace("-", "").ToLower();
                }
            }

            switch (appArchitecture)
            {
                case System.Runtime.InteropServices.Architecture.X86:
                    if (!dllBuildHash.Equals(x86DllSha256))
                    {
                        SharedMethods.AppMsgBox("Detected incompatible minilz0.dll file.\nPlease check if the dll file included with this build of the app is the correct one.", "Error", MessageBoxIcon.Error);
                        Environment.Exit(1);
                    }
                    break;

                case System.Runtime.InteropServices.Architecture.X64:
                    if (!dllBuildHash.Equals(x64DllSha256))
                    {
                        SharedMethods.AppMsgBox("Detected incompatible minilz0.dll file.\nPlease check if the dll file included with this build of the app is the correct one.", "Error", MessageBoxIcon.Error);
                        Environment.Exit(1);
                    }
                    break;
            }
        }


        private void CoreForm_Shown(object sender, EventArgs e)
        {
            LoggingMethods.SetCoreFormStatusBox();
        }


        private void ExtBinBtn_MouseHover(object sender, EventArgs e)
        {
            BINtoolTip.Show("Extract a compatible .BIN file.", ExtBinBtn);
        }
        public void ExtBinBtn_Click(object sender, EventArgs e)
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
                        StatusTextBox.AppendText("Game is set to Drakengard 1");
                        StatusTextBox.AppendText(SharedMethods.NewLineChara);
                        StatusTextBox.AppendText(SharedMethods.NewLineChara);

                        EnableDisableControls(false);

                        var readHeader = SharedMethods.HeaderCheck(mbinFile);

                        if (readHeader == "fpk")
                        {
                            var pathResult = MessageBox.Show("Try and generate file paths if a .lst file is available in the fpk file?\nWarning: This is an experimental option and if you have gotten any errors with the 'YES' option before for the currently selected fpk file, then select the 'NO' option.", "Path Generation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                            var generateLstPaths = SharedMethods.SetBoolFromDlgResult(pathResult);

                            System.Threading.Tasks.Task.Run(() =>
                            {
                                try
                                {
                                    LoggingMethods.LogMessage("Extracting files from " + Path.GetFileName(mbinFile) + "....");
                                    Drk1BIN.ExtractBin(mbinFile, generateLstPaths);
                                }
                                finally
                                {
                                    BeginInvoke(new Action(() => EnableDisableControls(true)));
                                }
                            });
                        }
                        else
                        {
                            StatusTextBox.AppendText("Error: Unable to detect fpk header");
                            StatusTextBox.AppendText(SharedMethods.NewLineChara);

                            SharedMethods.AppMsgBox("This is not a valid Drakengard 1 .bin file", "Error", MessageBoxIcon.Error);
                            StatusTextBox.AppendText("Extraction failed!");

                            EnableDisableControls(true);
                            return;
                        }
                    }

                    if (Drk2RadioButton.Checked)
                    {
                        StatusTextBox.AppendText("Game is set to Drakengard 2");
                        StatusTextBox.AppendText(SharedMethods.NewLineChara);
                        StatusTextBox.AppendText(SharedMethods.NewLineChara);

                        EnableDisableControls(false);

                        var readHeader = SharedMethods.HeaderCheck(mbinFile);

                        if (readHeader == "dpk")
                        {
                            System.Threading.Tasks.Task.Run(() =>
                            {
                                try
                                {
                                    LoggingMethods.LogMessage("Extracting files from " + Path.GetFileName(mbinFile) + "....");
                                    Drk2BIN.ExtractBin(mbinFile);
                                }
                                finally
                                {
                                    BeginInvoke(new Action(() => EnableDisableControls(true)));
                                }
                            });
                        }
                        else
                        {
                            StatusTextBox.AppendText("Error: Unable to detect dpk header");
                            StatusTextBox.AppendText(SharedMethods.NewLineChara);

                            SharedMethods.AppMsgBox("This is not a valid Drakengard 2 .bin file", "Error", MessageBoxIcon.Error);
                            StatusTextBox.AppendText("Extraction failed!");

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


        private void ExtFpkBtn_MouseHover(object sender, EventArgs e)
        {
            FPKtoolTip.Show("Extract a .fpk file.", ExtFpkBtn);

        }
        private void ExtFpkBtn_Click(object sender, EventArgs e)
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
                    var fpkFile = fpkSelect.FileName;
                    EnableDisableControls(false);

                    var readHeader = SharedMethods.HeaderCheck(fpkFile);

                    if (readHeader == "fpk")
                    {
                        var pathResult = MessageBox.Show("Try and generate file paths if a .lst file is available in the fpk file?\nWarning: This is an experimental option and if you have gotten any errors with the 'YES' option before for the currently selected fpk file, then select the 'NO' option.", "Path Generation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        var generateLstPaths = SharedMethods.SetBoolFromDlgResult(pathResult);

                        System.Threading.Tasks.Task.Run(() =>
                        {
                            try
                            {
                                LoggingMethods.LogMessage("Extracting files from " + Path.GetFileName(fpkFile) + "....");
                                FileFPK.ExtractFPK(fpkFile, generateLstPaths, true);
                            }
                            finally
                            {
                                BeginInvoke(new Action(() => EnableDisableControls(true)));
                            }
                        });
                    }
                    else
                    {
                        StatusTextBox.AppendText("Error: Unable to detect fpk header");
                        StatusTextBox.AppendText(SharedMethods.NewLineChara);

                        SharedMethods.AppMsgBox("Unable to detect fpk header", "Error", MessageBoxIcon.Error);
                        StatusTextBox.AppendText("Extraction failed!");

                        EnableDisableControls(true);
                        return;
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


        private void ExtDpkBtn_MouseHover(object sender, EventArgs e)
        {
            DPKtoolTip.Show("Extract a .dpk file from Drakengard 2.", ExtDpkBtn);
        }
        private void ExtDpkBtn_Click(object sender, EventArgs e)
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
                    var dpkFile = dpkSelect.FileName;
                    EnableDisableControls(false);

                    var readHeader = SharedMethods.HeaderCheck(dpkFile);

                    if (readHeader == "dpk")
                    {
                        System.Threading.Tasks.Task.Run(() =>
                        {
                            try
                            {
                                LoggingMethods.LogMessage("Extracting files from " + Path.GetFileName(dpkFile) + "....");
                                FileDPK.ExtractDPK(dpkFile, true);
                            }
                            finally
                            {
                                BeginInvoke(new Action(() => EnableDisableControls(true)));
                            }
                        });
                    }
                    else
                    {
                        StatusTextBox.AppendText("Error: Unable to detect dpk header");
                        StatusTextBox.AppendText(SharedMethods.NewLineChara);

                        SharedMethods.AppMsgBox("Unable to detect dpk header", "Error", MessageBoxIcon.Error);
                        StatusTextBox.AppendText("Extraction failed!");

                        EnableDisableControls(true);
                        return;
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


        private void ExtKpsBtn_MouseHover(object sender, EventArgs e)
        {
            KPStoolTip.Show("Extract a .kps file", ExtKpsBtn);
        }
        private void ExtKpsBtn_Click(object sender, EventArgs e)
        {
            try
            {
                var kpsSelect = new OpenFileDialog
                {
                    Filter = "KPS type files (*.kps)" + $"|*.kps",
                    RestoreDirectory = true
                };

                if (kpsSelect.ShowDialog() == DialogResult.OK)
                {
                    var kpsFile = kpsSelect.FileName;
                    EnableDisableControls(false);

                    var readHeader = SharedMethods.HeaderCheck(kpsFile);

                    if (readHeader == "KPS_")
                    {
                        var shiftJISResult = MessageBox.Show("Parse the text data in Japanese Encoding (shift-jis) format ?", "ShiftJIS", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        var shiftJISParse = SharedMethods.SetBoolFromDlgResult(shiftJISResult);

                        System.Threading.Tasks.Task.Run(() =>
                        {
                            try
                            {
                                LoggingMethods.LogMessage("Extracting " + Path.GetFileName(kpsFile) + "....");
                                FileKPS.ExtractKPS(kpsFile, shiftJISParse, true);
                            }
                            finally
                            {
                                BeginInvoke(new Action(() => EnableDisableControls(true)));
                            }
                        });
                    }
                    else
                    {
                        StatusTextBox.AppendText("Error: Unable to detect kps header");
                        StatusTextBox.AppendText(SharedMethods.NewLineChara);

                        SharedMethods.AppMsgBox("Unable to detect kps header", "Error", MessageBoxIcon.Error);
                        StatusTextBox.AppendText("Extraction failed!");

                        EnableDisableControls(true);
                        return;
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


        private void BatchModeBtn_MouseHover(object sender, EventArgs e)
        {
            BatchModeToolTip.Show("Open Batch Mode window for File Extraction and Image Conversion functions.", BatchModeBtn);
        }
        private void BatchModeBtn_Click(object sender, EventArgs e)
        {
            var batchModeForm = new BatchForm();
            batchModeForm.ShowDialog();
        }


        private void ConvertZIMBtn_MouseHover(object sender, EventArgs e)
        {
            ZIMtoolTip.Show("Convert a .zim image file to bmp, dds or png formats.", ConvertZIMBtn);
        }
        private void ConvertZIMBtn_Click(object sender, EventArgs e)
        {
            try
            {
                var zimSelect = new OpenFileDialog
                {
                    Filter = "ZIM type files (*.zim)" + $"|*.zim",
                    RestoreDirectory = true
                };

                if (zimSelect.ShowDialog() == DialogResult.OK)
                {
                    var zimFile = zimSelect.FileName;
                    EnableDisableControls(false);

                    var readHeader = SharedMethods.HeaderCheck(zimFile);

                    if (readHeader == "wZIM")
                    {
                        StatusTextBox.AppendText("Converting " + Path.GetFileName(zimFile) + "....");
                        StatusTextBox.AppendText(SharedMethods.NewLineChara);

                        using (var bppReader = new BinaryReader(File.Open(zimFile, FileMode.Open, FileAccess.Read)))
                        {
                            bppReader.BaseStream.Position = 82;

                            if (bppReader.ReadByte() == 64)
                            {
                                SharedMethods.AppMsgBox("Detected 4bpp image.\nDo not use the alpha compensation setting when saving the image in png or dds formats.", "Warning", MessageBoxIcon.Warning);
                            }
                        }

                        ImgOptions.IsClosedByConvtBtn = false;
                        var converterWindow = new ZIMForm();
                        converterWindow.ShowDialog();

                        if (ImgOptions.IsClosedByConvtBtn)
                        {
                            System.Threading.Tasks.Task.Run(() =>
                            {
                                try
                                {
                                    LoggingMethods.LogMessage("Converting....");
                                    ImgZIM.ConvertZIM(zimFile, true);
                                }
                                finally
                                {
                                    BeginInvoke(new Action(() => EnableDisableControls(true)));
                                }
                            });
                        }
                        else
                        {
                            StatusTextBox.AppendText("Conversion cancelled!");
                            EnableDisableControls(true);
                        }
                    }
                    else
                    {
                        StatusTextBox.AppendText("Error: Unable to detect zim header");
                        StatusTextBox.AppendText(SharedMethods.NewLineChara);

                        SharedMethods.AppMsgBox("Unable to detect zim header", "Error", MessageBoxIcon.Error);
                        StatusTextBox.AppendText("Conversion failed!");

                        EnableDisableControls(true);
                        return;
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


        private void ConvertSPK0Btn_MouseHover(object sender, EventArgs e)
        {
            SPK0toolTip.Show("Extracts a .spk0 file and converts the images inside the file to bmp, dds or png formats.", ConvertSPK0Btn);
        }
        private void ConvertSPK0Btn_Click(object sender, EventArgs e)
        {
            try
            {
                var spk0Select = new OpenFileDialog
                {
                    Filter = "SPK0 type files (*.spk0)" + $"|*.spk0",
                    RestoreDirectory = true
                };

                if (spk0Select.ShowDialog() == DialogResult.OK)
                {
                    var spk0File = spk0Select.FileName;
                    EnableDisableControls(false);

                    var readHeader = SharedMethods.HeaderCheck(spk0File);

                    if (readHeader == "SPK0")
                    {
                        StatusTextBox.AppendText("Converting " + Path.GetFileName(spk0File) + "....");
                        StatusTextBox.AppendText(SharedMethods.NewLineChara);

                        ImgOptions.IsClosedByConvtBtn = false;
                        var converterWindow = new SPK0Form();
                        converterWindow.ShowDialog();

                        if (ImgOptions.IsClosedByConvtBtn)
                        {
                            System.Threading.Tasks.Task.Run(() =>
                            {
                                try
                                {
                                    LoggingMethods.LogMessage("Converting....");
                                    ImgSPK0.ConvertSPK0(spk0File, true);
                                }
                                finally
                                {
                                    BeginInvoke(new Action(() => EnableDisableControls(true)));
                                }
                            });
                        }
                        else
                        {
                            StatusTextBox.AppendText("Conversion cancelled!");
                            EnableDisableControls(true);
                        }
                    }
                    else
                    {
                        StatusTextBox.AppendText("Error: Unable to detect spk0 header");
                        StatusTextBox.AppendText(SharedMethods.NewLineChara);

                        SharedMethods.AppMsgBox("Unable to detect spk0 header", "Error", MessageBoxIcon.Error);
                        StatusTextBox.AppendText("Conversion failed!");

                        EnableDisableControls(true);
                        return;
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


        private void EnableDisableControls(bool isEnabled)
        {
            ExtBinBtn.Enabled = isEnabled;
            ExtFpkBtn.Enabled = isEnabled;
            ExtDpkBtn.Enabled = isEnabled;
            ExtKpsBtn.Enabled = isEnabled;
            BatchModeBtn.Enabled = isEnabled;
            ConvertZIMBtn.Enabled = isEnabled;
            ConvertSPK0Btn.Enabled = isEnabled;
            Drk1RadioButton.Enabled = isEnabled;
            Drk2RadioButton.Enabled = isEnabled;
            StatusDelBtn.Enabled = isEnabled;
        }


        private void StatusDelBtn_Click(object sender, EventArgs e)
        {
            StatusTextBox.Clear();
        }


        private void AboutlinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var AboutWindow = new AboutForm();
            AboutWindow.ShowDialog();
        }


        private void HelpLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("AppHelp.txt");
            }
            catch (Exception ex)
            {
                SharedMethods.AppMsgBox("" + ex, "Error", MessageBoxIcon.Error);
                LoggingMethods.LogException("Exception: " + ex);
            }
        }
    }
}