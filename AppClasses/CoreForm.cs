using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Drakengard1and2Extractor.AppClasses
{
    public partial class CoreForm : Form
    {
        public CoreForm()
        {
            InitializeComponent();
            if (!File.Exists("minilzo.dll"))
            {
                CmnMethods.AppMsgBox("Missing minilz0.dll file.\nPlease check if this dll file is present next to the exe file.", "Error", MessageBoxIcon.Error);
                Environment.Exit(0);
            }
            if (!File.Exists("AppHelp.txt"))
            {
                CmnMethods.AppMsgBox("The AppHelp.txt file is missing\nPlease ensure that this file is present next to the app to use the Help option.", "Warning", MessageBoxIcon.Warning);
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
            BINtoolTip.Show("Extract a compatible .BIN file", ExtBinBtn);
        }
        public void ExtBinBtn_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog Bin_Select = new OpenFileDialog();
                if (Drk1RadioButton.Checked == true)
                {
                    Bin_Select.Filter = "Drakengard 1 BIN files" + $"|*.bin";
                }
                if (Drk2RadioButton.Checked == true)
                {
                    Bin_Select.Filter = "Drakengard 2 BIN files" + $"|*.bin";
                }

                Bin_Select.RestoreDirectory = true;

                if (Bin_Select.ShowDialog() == DialogResult.OK)
                {
                    StatusListBox.Items.Clear();

                    string mbin_file = Bin_Select.FileName;

                    if (Drk1RadioButton.Checked == true)
                    {
                        StatusMsg("Game is set to Drakengard 1");
                        StatusMsg("");
                        DisableButtons();

                        var Header = "";
                        HeaderCheck(mbin_file, ref Header);

                        if (!Header.StartsWith("fpk"))
                        {
                            StatusMsg("Error: Unable to detect fpk header");
                            StatusMsg("");
                            CmnMethods.AppMsgBox("Unable to detect fpk header", "Error", MessageBoxIcon.Error);
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
                                    StatusListBox.BeginInvoke((Action)(() => StatusMsg("Extracting files from " + mbin_file + "....")));
                                    Drk1BIN.ExtractBin(mbin_file);
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

                        var Header = "";
                        HeaderCheck(mbin_file, ref Header);
                        if (!Header.StartsWith("dpk"))
                        {
                            StatusMsg("Error: Unable to detect dpk header");
                            StatusMsg("");
                            CmnMethods.AppMsgBox("Unable to detect dpk header", "Error", MessageBoxIcon.Error);
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
                                    StatusListBox.BeginInvoke((Action)(() => StatusMsg("Extracting files from " + mbin_file + "....")));
                                    Drk2BIN.ExtractBin(mbin_file);
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
            }
            catch (Exception ex)
            {
                CmnMethods.AppMsgBox(ex.Message, "Error", MessageBoxIcon.Error);
            }
        }


        private void ExtFpkBtn_MouseHover(object sender, EventArgs e)
        {
            FPKtoolTip.Show("Extract a .fpk file", ExtFpkBtn);

        }
        private void ExtFpkBtn_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog fpk_select = new OpenFileDialog();
                fpk_select.Filter = "FPK type files (*.fpk)" + $"|*.fpk";
                fpk_select.RestoreDirectory = true;

                if (fpk_select.ShowDialog() == DialogResult.OK)
                {
                    StatusListBox.Items.Clear();
                    string fpk_file = fpk_select.FileName;

                    var Header = "";
                    HeaderCheck(fpk_file, ref Header);
                    DisableButtons();

                    if (!Header.StartsWith("fpk"))
                    {
                        StatusMsg("Error: Unable to detect fpk header");
                        StatusMsg("");
                        CmnMethods.AppMsgBox("Unable to detect fpk header", "Error", MessageBoxIcon.Error);
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
                                StatusListBox.BeginInvoke((Action)(() => StatusMsg("Extracting files from " + fpk_file + "....")));
                                FileFPK.ExtractFPK(fpk_file);
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
                CmnMethods.AppMsgBox(ex.Message, "Error", MessageBoxIcon.Error);
            }
        }


        private void ExtDpkBtn_MouseHover(object sender, EventArgs e)
        {
            DPKtoolTip.Show("Extract a .dpk file from Drakengard 2", ExtDpkBtn);
        }
        private void ExtDpkBtn_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog dpk_select = new OpenFileDialog();
                dpk_select.Filter = "DPK type files (*.dpk)" + $"|*.dpk";
                dpk_select.RestoreDirectory = true;

                if (dpk_select.ShowDialog() == DialogResult.OK)
                {
                    StatusListBox.Items.Clear();
                    string dpk_file = dpk_select.FileName;

                    var Header = "";
                    HeaderCheck(dpk_file, ref Header);

                    if (!Header.StartsWith("dpk"))
                    {
                        StatusMsg("Error: Unable to detect dpk header");
                        StatusMsg("");
                        CmnMethods.AppMsgBox("Unable to detect dpk header", "Error", MessageBoxIcon.Error);
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
                                StatusListBox.BeginInvoke((Action)(() => StatusMsg("Extracting files from " + dpk_file + "....")));
                                FileDPK.ExtractDPK(dpk_file);
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
                CmnMethods.AppMsgBox(ex.Message, "Error", MessageBoxIcon.Error);
            }
        }


        private void ConvertZIMBtn_MouseHover(object sender, EventArgs e)
        {
            ZIMtoolTip.Show("Convert a .zim image file to bmp or png formats", ConvertZIMBtn);
        }
        private void ConvertZIMBtn_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog zim_select = new OpenFileDialog();
                zim_select.Filter = "ZIM type files (*.zim)" + $"|*.zim";
                zim_select.RestoreDirectory = true;

                if (zim_select.ShowDialog() == DialogResult.OK)
                {
                    StatusListBox.Items.Clear();
                    string zim_file = zim_select.FileName;

                    var Header = "";
                    HeaderCheck(zim_file, ref Header);
                    DisableButtons();

                    if (!Header.StartsWith("wZIM"))
                    {
                        StatusMsg("Error: Unable to detect zim header");
                        StatusMsg("");
                        CmnMethods.AppMsgBox("Unable to detect zim header", "Error", MessageBoxIcon.Error);
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
                                StatusListBox.BeginInvoke((Action)(() => StatusMsg("Converting " + zim_file + "....")));
                                FileZIM ConverterWindow = new FileZIM(zim_file);
                                ConverterWindow.ShowDialog();
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
                CmnMethods.AppMsgBox(ex.Message, "Error", MessageBoxIcon.Error);
            }
        }


        private void ConvertSPK0Btn_MouseHover(object sender, EventArgs e)
        {
            SPK0toolTip.Show("Extracts a .spk0 file and converts the images inside the file to bmp or png formats", ConvertSPK0Btn);
        }
        private void ConvertSPK0Btn_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog spk0_select = new OpenFileDialog();
                spk0_select.Filter = "SPK0 type files (*.spk0)" + $"|*.spk0";
                spk0_select.RestoreDirectory = true;

                if (spk0_select.ShowDialog() == DialogResult.OK)
                {
                    StatusListBox.Items.Clear();
                    string spk0_file = spk0_select.FileName;

                    var Header = "";
                    HeaderCheck(spk0_file, ref Header);
                    DisableButtons();

                    if (!Header.StartsWith("SPK0"))
                    {
                        StatusMsg("Error: Unable to detect spk0 header");
                        StatusMsg("");
                        CmnMethods.AppMsgBox("Unable to detect spk0 header", "Error", MessageBoxIcon.Error);
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
                                StatusListBox.BeginInvoke((Action)(() => StatusMsg("Converting " + spk0_file + "....")));
                                FileSPK0 ConverterWindow = new FileSPK0(spk0_file);
                                ConverterWindow.ShowDialog();
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
                CmnMethods.AppMsgBox(ex.Message, "Error", MessageBoxIcon.Error);
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


        private void DisableButtons()
        {
            ExtBinBtn.Enabled = false;
            ExtFpkBtn.Enabled = false;
            ConvertZIMBtn.Enabled = false;
            ConvertSPK0Btn.Enabled = false;
            if (Drk2RadioButton.Checked == true)
            {
                ExtDpkBtn.Enabled = false;
            }
            Drk1RadioButton.Enabled = false;
            Drk2RadioButton.Enabled = false;
        }


        private static void HeaderCheck(string FileName, ref string HeaderVar)
        {
            using (FileStream ExtnCheck = new FileStream(FileName, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader ExtnCheckReader = new BinaryReader(ExtnCheck))
                {
                    ExtnCheckReader.BaseStream.Position = 0;
                    var HeaderChars = ExtnCheckReader.ReadChars(4);
                    HeaderVar = string.Join("", HeaderChars);
                }

                ExtnCheck.Dispose();
            }
        }


        private void EnableButtons()
        {
            ExtBinBtn.Enabled = true;
            ExtFpkBtn.Enabled = true;
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
            AppAbout AboutWindow = new AppAbout();
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
                CmnMethods.AppMsgBox(ex.Message, "Error", MessageBoxIcon.Error);
            }
        }
    }
}