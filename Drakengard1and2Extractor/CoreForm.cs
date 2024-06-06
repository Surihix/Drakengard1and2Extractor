﻿using Drakengard1and2Extractor.Support;
using Drakengard1and2Extractor.Tools;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Drakengard1and2Extractor
{
    public partial class CoreForm : Form
    {
        public static readonly string NewLineChara = Environment.NewLine;

        public CoreForm()
        {
            InitializeComponent();
            if (!File.Exists("minilzo.dll"))
            {
                CommonMethods.AppMsgBox("Missing minilz0.dll file.\nPlease check if this dll file is present next to the exe file.", "Error", MessageBoxIcon.Error);
                Environment.Exit(1);
            }

            ChecklzoDll();

            if (!File.Exists("AppHelp.txt"))
            {
                CommonMethods.AppMsgBox("The AppHelp.txt file is missing\nPlease ensure that this file is present next to the app to use the Help option.", "Warning", MessageBoxIcon.Warning);
            }

            Drk1RadioButton.Checked = true;

            StatusTextBox.BackColor = SystemColors.Window;
            StatusTextBox.Text = "App was launched!";
            StatusTextBox.AppendText(NewLineChara);
            StatusTextBox.AppendText(NewLineChara);
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

                    if (Drk1RadioButton.Checked == true)
                    {
                        StatusTextBox.AppendText("Game is set to Drakengard 1");
                        StatusTextBox.AppendText(NewLineChara);
                        StatusTextBox.AppendText(NewLineChara);

                        EnableDisableControls(false);

                        var readHeader = "";
                        CommonMethods.HeaderCheck(mbinFile, ref readHeader);

                        if (!readHeader.StartsWith("fpk"))
                        {
                            StatusTextBox.AppendText("Error: Unable to detect fpk header");
                            StatusTextBox.AppendText(NewLineChara);

                            CommonMethods.AppMsgBox("Unable to detect fpk header", "Error", MessageBoxIcon.Error);
                            StatusTextBox.AppendText("Extraction failed!");

                            EnableDisableControls(true);
                            return;
                        }
                        else
                        {
                            Task.Run(() =>
                            {
                                try
                                {
                                    LoggingHelpers.LogMessage("Extracting files from " + Path.GetFileName(mbinFile) + "....");
                                    Drk1BIN.ExtractBin(mbinFile);
                                }
                                finally
                                {
                                    LoggingHelpers.LogMessage("Extraction has completed");

                                    BeginInvoke(new Action(() => EnableDisableControls(true)));
                                }
                            });
                        }
                    }

                    if (Drk2RadioButton.Checked == true)
                    {
                        StatusTextBox.AppendText("Game is set to Drakengard 2");
                        StatusTextBox.AppendText(NewLineChara);
                        StatusTextBox.AppendText(NewLineChara);

                        EnableDisableControls(false);

                        var readHeader = "";
                        CommonMethods.HeaderCheck(mbinFile, ref readHeader);
                        if (!readHeader.StartsWith("dpk"))
                        {
                            StatusTextBox.AppendText("Error: Unable to detect dpk header");
                            StatusTextBox.AppendText(NewLineChara);

                            CommonMethods.AppMsgBox("Unable to detect dpk header", "Error", MessageBoxIcon.Error);
                            StatusTextBox.AppendText("Extraction failed!");

                            EnableDisableControls(true);
                            return;
                        }
                        else
                        {
                            Task.Run(() =>
                            {
                                try
                                {
                                    LoggingHelpers.LogMessage("Extracting files from " + Path.GetFileName(mbinFile) + "....");
                                    Drk2BIN.ExtractBin(mbinFile);
                                }
                                finally
                                {
                                    LoggingHelpers.LogMessage("Extraction has completed");

                                    BeginInvoke(new Action(() => EnableDisableControls(true)));
                                }
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CommonMethods.AppMsgBox("" + ex, "Error", MessageBoxIcon.Error);
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
                var fpkSelect = new OpenFileDialog();
                fpkSelect.Filter = "FPK type files (*.fpk)" + $"|*.fpk";
                fpkSelect.RestoreDirectory = true;

                if (fpkSelect.ShowDialog() == DialogResult.OK)
                {
                    var fpkFile = fpkSelect.FileName;
                    EnableDisableControls(false);

                    var readHeader = "";
                    CommonMethods.HeaderCheck(fpkFile, ref readHeader);

                    if (!readHeader.StartsWith("fpk"))
                    {
                        StatusTextBox.AppendText("Error: Unable to detect fpk header");
                        StatusTextBox.AppendText(NewLineChara);

                        CommonMethods.AppMsgBox("Unable to detect fpk header", "Error", MessageBoxIcon.Error);
                        StatusTextBox.AppendText("Extraction failed!");

                        EnableDisableControls(true);
                        return;
                    }
                    else
                    {
                        Task.Run(() =>
                        {
                            try
                            {
                                LoggingHelpers.LogMessage("Extracting files from " + Path.GetFileName(fpkFile) + "....");
                                FileFPK.ExtractFPK(fpkFile, true);
                            }
                            finally
                            {
                                LoggingHelpers.LogMessage("Extraction has completed");

                                BeginInvoke(new Action(() => EnableDisableControls(true)));
                            }
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                CommonMethods.AppMsgBox("" + ex, "Error", MessageBoxIcon.Error);
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
                var dpkSelect = new OpenFileDialog();
                dpkSelect.Filter = "DPK type files (*.dpk)" + $"|*.dpk";
                dpkSelect.RestoreDirectory = true;

                if (dpkSelect.ShowDialog() == DialogResult.OK)
                {
                    var dpkFile = dpkSelect.FileName;
                    EnableDisableControls(false);

                    var readHeader = "";
                    CommonMethods.HeaderCheck(dpkFile, ref readHeader);

                    if (!readHeader.StartsWith("dpk"))
                    {
                        StatusTextBox.AppendText("Error: Unable to detect dpk header");
                        StatusTextBox.AppendText(NewLineChara);

                        CommonMethods.AppMsgBox("Unable to detect dpk header", "Error", MessageBoxIcon.Error);
                        StatusTextBox.AppendText("Extraction failed");

                        EnableDisableControls(true);
                        return;
                    }
                    else
                    {
                        Task.Run(() =>
                        {
                            try
                            {
                                LoggingHelpers.LogMessage("Extracting files from " + Path.GetFileName(dpkFile) + "....");
                                FileDPK.ExtractDPK(dpkFile, true);
                            }
                            finally
                            {
                                LoggingHelpers.LogMessage("Extraction has completed");

                                BeginInvoke(new Action(() => EnableDisableControls(true)));
                            }
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                CommonMethods.AppMsgBox("" + ex, "Error", MessageBoxIcon.Error);
            }
        }


        private void ExtKpsBtn_MouseHover(object sender, EventArgs e)
        {
            KPStoolTip.Show("Extract a .kps file", ExtKpsBtn);
        }
        private void ExtKpsBtn_Click(object sender, EventArgs e)
        {

        }


        private void BatchModeBtn_MouseHover(object sender, EventArgs e)
        {
            BatchModeToolTip.Show("Batch extracts all FPK or DPK files present in a directory.", BatchModeBtn);
        }
        private void BatchModeBtn_Click(object sender, EventArgs e)
        {
            var isDrk2RadioBtnChecked = false;
            if (Drk2RadioButton.Checked.Equals(true))
            {
                isDrk2RadioBtnChecked = true;
            }

            var batchMode = new BatchMode(isDrk2RadioBtnChecked);
            batchMode.ShowDialog();
        }


        private void ConvertZIMBtn_MouseHover(object sender, EventArgs e)
        {
            ZIMtoolTip.Show("Convert a .zim image file to bmp, dds or png formats.", ConvertZIMBtn);
        }
        private void ConvertZIMBtn_Click(object sender, EventArgs e)
        {
            try
            {
                var zimSelect = new OpenFileDialog();
                zimSelect.Filter = "ZIM type files (*.zim)" + $"|*.zim";
                zimSelect.RestoreDirectory = true;

                if (zimSelect.ShowDialog() == DialogResult.OK)
                {
                    var zimFile = zimSelect.FileName;
                    EnableDisableControls(false);

                    var readHeader = "";
                    CommonMethods.HeaderCheck(zimFile, ref readHeader);

                    if (!readHeader.StartsWith("wZIM"))
                    {
                        StatusTextBox.AppendText("Error: Unable to detect zim header");
                        StatusTextBox.AppendText(NewLineChara);

                        CommonMethods.AppMsgBox("Unable to detect zim header", "Error", MessageBoxIcon.Error);
                        StatusTextBox.AppendText("Conversion failed!");

                        EnableDisableControls(true);
                        return;
                    }
                    else
                    {
                        Task.Run(() =>
                        {
                            try
                            {
                                LoggingHelpers.LogMessage("Converting " + Path.GetFileName(zimFile) + "....");
                                var converterWindow = new FileZIM(zimFile);
                                converterWindow.ShowDialog();
                            }
                            finally
                            {
                                LoggingHelpers.LogMessage("Conversion has completed");

                                BeginInvoke(new Action(() => EnableDisableControls(true)));
                            }
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                CommonMethods.AppMsgBox("" + ex, "Error", MessageBoxIcon.Error);
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
                var spk0Select = new OpenFileDialog();
                spk0Select.Filter = "SPK0 type files (*.spk0)" + $"|*.spk0";
                spk0Select.RestoreDirectory = true;

                if (spk0Select.ShowDialog() == DialogResult.OK)
                {
                    var spk0File = spk0Select.FileName;
                    EnableDisableControls(false);

                    var readHeader = "";
                    CommonMethods.HeaderCheck(spk0File, ref readHeader);

                    if (!readHeader.StartsWith("SPK0"))
                    {
                        StatusTextBox.AppendText("Error: Unable to detect spk0 header");
                        StatusTextBox.AppendText(NewLineChara);

                        CommonMethods.AppMsgBox("Unable to detect spk0 header", "Error", MessageBoxIcon.Error);
                        StatusTextBox.AppendText("Conversion failed!");

                        EnableDisableControls(true);
                        return;
                    }
                    else
                    {
                        Task.Run(() =>
                        {
                            try
                            {
                                LoggingHelpers.LogMessage("Converting " + Path.GetFileName(spk0File) + "....");
                                var converterWindow = new FileSPK0(spk0File);
                                converterWindow.ShowDialog();
                            }
                            finally
                            {
                                LoggingHelpers.LogMessage("Conversion has completed");

                                BeginInvoke(new Action(() => EnableDisableControls(true)));
                            }
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                CommonMethods.AppMsgBox("" + ex, "Error", MessageBoxIcon.Error);
            }
        }


        private void Drk1RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (Drk1RadioButton.Checked == true)
            {
                ExtBinBtn.Enabled = true;
                ExtFpkBtn.Enabled = true;
                ExtDpkBtn.Enabled = false;
            }
        }


        private void Drk2RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (Drk2RadioButton.Checked == true)
            {
                ExtBinBtn.Enabled = true;
                ExtFpkBtn.Enabled = true;
                ExtDpkBtn.Enabled = true;
            }
        }


        public void ChecklzoDll()
        {
            var x86DllSha256 = "d414fad15b356f33bf02479bd417d2df767ee102180aae718ef1135146da2884";
            var x64DllSha256 = "ea006fafb08dd554657b1c81e45c92e88d663aca0c79c48ae1f3dca22e1e2314";
            string dllBuildHash;

            var appArchitecture = RuntimeInformation.ProcessArchitecture;
            using (FileStream lzoDllStream = new FileStream("minilzo.dll", FileMode.Open, FileAccess.Read))
            {
                using (SHA256 dllSHA256 = SHA256.Create())
                {
                    dllBuildHash = BitConverter.ToString(dllSHA256.ComputeHash(lzoDllStream)).Replace("-", "").ToLower();
                }
            }

            switch (appArchitecture)
            {
                case Architecture.X86:
                    if (!dllBuildHash.Equals(x86DllSha256))
                    {
                        CommonMethods.AppMsgBox("Detected incompatible minilz0.dll file.\nPlease check if the dll file included with this build of the app is the correct one.", "Error", MessageBoxIcon.Error);
                        Environment.Exit(0);
                    }
                    break;

                case Architecture.X64:
                    if (!dllBuildHash.Equals(x64DllSha256))
                    {
                        CommonMethods.AppMsgBox("Detected incompatible minilz0.dll file.\nPlease check if the dll file included with this build of the app is the correct one.", "Error", MessageBoxIcon.Error);
                        Environment.Exit(0);
                    }
                    break;
            }
        }


        private void EnableDisableControls(bool isEnabled)
        {
            ExtBinBtn.Enabled = isEnabled;
            ExtFpkBtn.Enabled = isEnabled;
            ExtKpsBtn.Enabled = isEnabled;
            BatchModeBtn.Enabled = isEnabled;
            ConvertZIMBtn.Enabled = isEnabled;
            ConvertSPK0Btn.Enabled = isEnabled;
            if (Drk2RadioButton.Checked == isEnabled)
            {
                ExtDpkBtn.Enabled = isEnabled;
            }
            Drk1RadioButton.Enabled = isEnabled;
            Drk2RadioButton.Enabled = isEnabled;
        }


        private void StatusDelBtn_Click(object sender, EventArgs e)
        {
            StatusTextBox.Clear();
        }


        private void AboutlinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var AboutWindow = new AppAbout();
            AboutWindow.ShowDialog();
        }


        private void HelpLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process.Start("AppHelp.txt");
            }
            catch (Exception ex)
            {
                CommonMethods.AppMsgBox("" + ex, "Error", MessageBoxIcon.Error);
            }
        }
    }
}