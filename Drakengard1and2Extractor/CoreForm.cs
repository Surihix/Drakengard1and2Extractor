using Drakengard1and2Extractor.Support;
using Drakengard1and2Extractor.Tools;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Threading.Tasks;
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
                CommonMethods.AppMsgBox("Missing minilz0.dll file.\nPlease check if this dll file is present next to the exe file.", "Error", MessageBoxIcon.Error);
                Environment.Exit(0);
            }

            ChecklzoDll();

            if (!File.Exists("AppHelp.txt"))
            {
                CommonMethods.AppMsgBox("The AppHelp.txt file is missing\nPlease ensure that this file is present next to the app to use the Help option.", "Warning", MessageBoxIcon.Warning);
            }

            Drk1RadioButton.Checked = true;
        }


        public void StatusMsg(string message)
        {
            StatusListBox.Items.Add(message);
            StatusListBox.SelectedIndex = StatusListBox.Items.Count - 1;
            StatusListBox.SelectedIndex = -1;
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
                    StatusListBox.Items.Clear();

                    var mbinFile = binSelect.FileName;

                    if (Drk1RadioButton.Checked == true)
                    {
                        StatusMsg("Game is set to Drakengard 1");
                        StatusMsg("");
                        DisableButtons();

                        var readHeader = "";
                        CommonMethods.HeaderCheck(mbinFile, ref readHeader);

                        if (!readHeader.StartsWith("fpk"))
                        {
                            StatusMsg("Error: Unable to detect fpk header");
                            StatusMsg("");
                            CommonMethods.AppMsgBox("Unable to detect fpk header", "Error", MessageBoxIcon.Error);
                            StatusMsg("Extraction has completed");
                            EnableButtons();
                            return;
                        }
                        else
                        {
                            Task.Run(() =>
                            {
                                try
                                {
                                    StatusListBox.BeginInvoke((Action)(() => StatusMsg("Extracting files from " + Path.GetFileName(mbinFile) + "....")));
                                    Drk1BIN.ExtractBin(mbinFile);
                                }
                                finally
                                {
                                    StatusListBox.BeginInvoke((Action)(() => StatusMsg("")));
                                    StatusListBox.BeginInvoke((Action)(() => StatusMsg("Extraction has completed")));
                                    BeginInvoke(new Action(() => EnableButtons()));
                                }
                            });
                        }
                    }

                    if (Drk2RadioButton.Checked == true)
                    {
                        StatusMsg("Game is set to Drakengard 2");
                        StatusMsg("");
                        DisableButtons();

                        var readHeader = "";
                        CommonMethods.HeaderCheck(mbinFile, ref readHeader);
                        if (!readHeader.StartsWith("dpk"))
                        {
                            StatusMsg("Error: Unable to detect dpk header");
                            StatusMsg("");
                            CommonMethods.AppMsgBox("Unable to detect dpk header", "Error", MessageBoxIcon.Error);
                            StatusMsg("Extraction has completed");
                            EnableButtons();
                            return;
                        }
                        else
                        {
                            Task.Run(() =>
                            {
                                try
                                {
                                    StatusListBox.BeginInvoke((Action)(() => StatusMsg("Extracting files from " + Path.GetFileName(mbinFile) + "....")));
                                    Drk2BIN.ExtractBin(mbinFile);
                                }
                                finally
                                {
                                    StatusListBox.BeginInvoke((Action)(() => StatusMsg("")));
                                    StatusListBox.BeginInvoke((Action)(() => StatusMsg("Extraction has completed")));
                                    BeginInvoke(new Action(() => EnableButtons()));
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
                    StatusListBox.Items.Clear();
                    var fpkFile = fpkSelect.FileName;

                    var readHeader = "";
                    CommonMethods.HeaderCheck(fpkFile, ref readHeader);
                    DisableButtons();

                    if (!readHeader.StartsWith("fpk"))
                    {
                        StatusMsg("Error: Unable to detect fpk header");
                        StatusMsg("");
                        CommonMethods.AppMsgBox("Unable to detect fpk header", "Error", MessageBoxIcon.Error);
                        StatusMsg("Extraction has completed");
                        EnableButtons();
                        return;
                    }
                    else
                    {
                        Task.Run(() =>
                        {
                            try
                            {
                                StatusListBox.BeginInvoke((Action)(() => StatusMsg("Extracting files from " + Path.GetFileName(fpkFile) + "....")));
                                FileFPK.ExtractFPK(fpkFile, true);
                            }
                            finally
                            {
                                StatusListBox.BeginInvoke((Action)(() => StatusMsg("")));
                                StatusListBox.BeginInvoke((Action)(() => StatusMsg("Extraction has completed")));
                                StatusListBox.BeginInvoke((Action)(() => StatusMsg("")));
                                BeginInvoke(new Action(() => EnableButtons()));
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
                    StatusListBox.Items.Clear();
                    var dpkFile = dpkSelect.FileName;

                    var readHeader = "";
                    CommonMethods.HeaderCheck(dpkFile, ref readHeader);

                    if (!readHeader.StartsWith("dpk"))
                    {
                        StatusMsg("Error: Unable to detect dpk header");
                        StatusMsg("");
                        CommonMethods.AppMsgBox("Unable to detect dpk header", "Error", MessageBoxIcon.Error);
                        StatusMsg("Extraction has completed");
                        EnableButtons();
                        return;
                    }
                    else
                    {
                        Task.Run(() =>
                        {
                            try
                            {
                                StatusListBox.BeginInvoke((Action)(() => StatusMsg("Extracting files from " + Path.GetFileName(dpkFile) + "....")));
                                FileDPK.ExtractDPK(dpkFile, true);
                            }
                            finally
                            {
                                StatusListBox.BeginInvoke((Action)(() => StatusMsg("")));
                                StatusListBox.BeginInvoke((Action)(() => StatusMsg("Extraction has completed")));
                                StatusListBox.BeginInvoke((Action)(() => StatusMsg("")));
                                BeginInvoke(new Action(() => EnableButtons()));
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


        private void BatchModeBtn_MouseHover(object sender, EventArgs e)
        {
            BatchModeToolTip.Show("Batch extracts all FPK or DPK files present in a directory.", BatchModeBtn);
        }
        private void BatchModeBtn_Click(object sender, EventArgs e)
        {
            StatusListBox.Items.Clear();

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
                    StatusListBox.Items.Clear();
                    var zimFile = zimSelect.FileName;

                    var readHeader = "";
                    CommonMethods.HeaderCheck(zimFile, ref readHeader);
                    DisableButtons();

                    if (!readHeader.StartsWith("wZIM"))
                    {
                        StatusMsg("Error: Unable to detect zim header");
                        StatusMsg("");
                        CommonMethods.AppMsgBox("Unable to detect zim header", "Error", MessageBoxIcon.Error);
                        StatusMsg("Conversion has completed");
                        EnableButtons();
                        return;
                    }
                    else
                    {
                        Task.Run(() =>
                        {
                            try
                            {
                                StatusListBox.BeginInvoke((Action)(() => StatusMsg("Converting " + Path.GetFileName(zimFile) + "....")));
                                var converterWindow = new FileZIM(zimFile);
                                converterWindow.ShowDialog();
                            }
                            finally
                            {
                                StatusListBox.BeginInvoke((Action)(() => StatusMsg("")));
                                StatusListBox.BeginInvoke((Action)(() => StatusMsg("Conversion has completed")));
                                StatusListBox.BeginInvoke((Action)(() => StatusMsg("")));
                                BeginInvoke(new Action(() => EnableButtons()));
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
                    StatusListBox.Items.Clear();
                    var spk0File = spk0Select.FileName;

                    var readHeader = "";
                    CommonMethods.HeaderCheck(spk0File, ref readHeader);
                    DisableButtons();

                    if (!readHeader.StartsWith("SPK0"))
                    {
                        StatusMsg("Error: Unable to detect spk0 header");
                        StatusMsg("");
                        CommonMethods.AppMsgBox("Unable to detect spk0 header", "Error", MessageBoxIcon.Error);
                        StatusMsg("Conversion has completed");
                        EnableButtons();
                        return;
                    }
                    else
                    {
                        Task.Run(() =>
                        {
                            try
                            {
                                StatusListBox.BeginInvoke((Action)(() => StatusMsg("Converting " + Path.GetFileName(spk0File) + "....")));
                                var converterWindow = new FileSPK0(spk0File);
                                converterWindow.ShowDialog();
                            }
                            finally
                            {
                                StatusListBox.BeginInvoke((Action)(() => StatusMsg("")));
                                StatusListBox.BeginInvoke((Action)(() => StatusMsg("Conversion has completed")));
                                StatusListBox.BeginInvoke((Action)(() => StatusMsg("")));
                                BeginInvoke(new Action(() => EnableButtons()));
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


        private void DisableButtons()
        {
            ExtBinBtn.Enabled = false;
            ExtFpkBtn.Enabled = false;
            BatchModeBtn.Enabled = false;
            ConvertZIMBtn.Enabled = false;
            ConvertSPK0Btn.Enabled = false;
            if (Drk2RadioButton.Checked == true)
            {
                ExtDpkBtn.Enabled = false;
            }
            Drk1RadioButton.Enabled = false;
            Drk2RadioButton.Enabled = false;
        }

        private void EnableButtons()
        {
            ExtBinBtn.Enabled = true;
            ExtFpkBtn.Enabled = true;
            BatchModeBtn.Enabled = true;
            ConvertZIMBtn.Enabled = true;
            ConvertSPK0Btn.Enabled = true;
            Drk1RadioButton.Enabled = true;
            Drk2RadioButton.Enabled = true;
            if (Drk2RadioButton.Checked == true)
            {
                ExtDpkBtn.Enabled = true;
            }
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