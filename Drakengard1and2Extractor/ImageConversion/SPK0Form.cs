using Drakengard1and2Extractor.Support;
using Drakengard1and2Extractor.Support.ImageHelpers;
using System;
using System.IO;
using System.Windows.Forms;

namespace Drakengard1and2Extractor.ImageConversion
{
    public partial class SPK0Form : Form
    {
        private readonly string _Spk0File;

        public void SPK0ConvertControls(bool isEnabled)
        {
            Spk0SaveAsComboBox.Enabled = isEnabled;
            ConvertSPK0ImgBtn.Enabled = isEnabled;

            if (Spk0SaveAsComboBox.SelectedIndex == 1 || Spk0SaveAsComboBox.SelectedIndex == 2)
            {
                Spk0AlphaCompNumericUpDown.Enabled = isEnabled;
            }
        }

        public SPK0Form(string spk0File)
        {
            try
            {
                _Spk0File = spk0File;

                InitializeComponent();

                Spk0SaveAsComboBox.SelectedIndex = 0;
                Spk0AlphaCompNumericUpDown.Enabled = false;
            }
            catch (Exception ex)
            {
                CommonMethods.AppMsgBox("" + ex, "Error", MessageBoxIcon.Error);
            }
        }


        private void Spk0SaveAsComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (Spk0SaveAsComboBox.SelectedIndex == 0)
            {
                Spk0AlphaCompNumericUpDown.Enabled = false;
            }
            if (Spk0SaveAsComboBox.SelectedIndex == 1 || Spk0SaveAsComboBox.SelectedIndex == 2)
            {
                Spk0AlphaCompNumericUpDown.Enabled = true;
            }
        }


        private void ConvertSPK0ImgBtn_MouseHover(object sender, EventArgs e)
        {
            ConvertSpk0ImgBtnToolTip.Show("Converts and saves the image files to one of the selected formats", ConvertSPK0ImgBtn);
        }
        private void ConvertSPK0ImgBtn_Click(object sender, EventArgs e)
        {
            ConvertSPK0ImgBtn.Text = "Converting...";

            SPK0ConvertControls(false);

            var saveAsComboBoxItemIndex = Spk0SaveAsComboBox.SelectedIndex;
            var spk0AlphaCompNumericUpDownVal = (int)Spk0AlphaCompNumericUpDown.Value;

            System.Threading.Tasks.Task.Run(() =>
            {
                try
                {
                    try
                    {
                        var extractDir = Path.Combine(Path.GetDirectoryName(_Spk0File), Path.GetFileName(_Spk0File) + "_extracted");

                        CommonMethods.IfFileDirExistsDel(extractDir, CommonMethods.DelSwitch.directory);
                        Directory.CreateDirectory(extractDir);


                        using (FileStream spk0Stream = new FileStream(_Spk0File, FileMode.Open, FileAccess.Read))
                        {
                            using (BinaryReader spk0Reader = new BinaryReader(spk0Stream))
                            {
                                spk0Reader.BaseStream.Position = 24;
                                var dls0SubChunkPos = spk0Reader.ReadUInt32();

                                spk0Reader.BaseStream.Position = 40;
                                var dmt0Size = spk0Reader.ReadUInt32();
                                var grf1SubChunkPos = dmt0Size + 32;

                                spk0Reader.BaseStream.Position = dls0SubChunkPos;
                                var grf1EndPos = spk0Reader.BaseStream.Position;
                                var grf1Size = grf1EndPos - dmt0Size - 32;
                                var dls0Size = spk0Stream.Length - grf1EndPos;


                                using (FileStream dmt0Stream = new FileStream(Path.Combine(extractDir, "DMT0"), FileMode.OpenOrCreate, FileAccess.Write))
                                {
                                    spk0Stream.Seek(32, SeekOrigin.Begin);
                                    spk0Stream.CopyStreamTo(dmt0Stream, dmt0Size, false);
                                }


                                using (FileStream grf1Stream = new FileStream(Path.Combine(extractDir, "GRF1"), FileMode.OpenOrCreate, FileAccess.ReadWrite))
                                {
                                    spk0Stream.Seek(grf1SubChunkPos, SeekOrigin.Begin);
                                    spk0Stream.CopyStreamTo(grf1Stream, grf1Size, false);

                                    var imgOptions = new ImgOptions();

                                    using (BinaryReader grf1Reader = new BinaryReader(grf1Stream))
                                    {
                                        grf1Reader.BaseStream.Position = 4;
                                        var totalImgCount = grf1Reader.ReadUInt32();

                                        grf1Reader.BaseStream.Position = 24;
                                        var imgPalPos = grf1Reader.ReadUInt32();

                                        grf1Reader.BaseStream.Position = imgPalPos + 96;
                                        var palDim1 = grf1Reader.ReadUInt32();
                                        var palDim2 = grf1Reader.ReadUInt32();
                                        var palSize = (palDim1 * 2) * (palDim2 * 2);

                                        grf1Stream.Seek(imgPalPos + 144, SeekOrigin.Begin);
                                        byte[] palBuffer = new byte[palSize];
                                        grf1Stream.Read(palBuffer, 0, palBuffer.Length);

                                        byte[] finalizedPalBuffer = PS2Unswizzlers.UnSwizzlePalette(palBuffer);


                                        uint imgReadPosStart = 20;
                                        var imgFCount = 1;
                                        for (int i = 0; i < totalImgCount; i++)
                                        {
                                            grf1Reader.BaseStream.Position = imgReadPosStart;
                                            var imgStartPos = grf1Reader.ReadUInt32();

                                            grf1Reader.BaseStream.Position = imgStartPos + 96;
                                            imgOptions.Width = (int)grf1Reader.ReadUInt32();
                                            imgOptions.Height = (int)grf1Reader.ReadUInt32();
                                            var imgSize = imgOptions.Width * imgOptions.Height;

                                            grf1Stream.Seek(imgStartPos + 144, SeekOrigin.Begin);
                                            var pixelsBuffer = new byte[imgSize];
                                            _ = grf1Stream.Read(pixelsBuffer, 0, imgSize);


                                            string outImgPath;
                                            switch (saveAsComboBoxItemIndex)
                                            {
                                                case 0:
                                                    outImgPath = Path.Combine(extractDir, "GRF1_img_" + imgFCount + ".bmp");
                                                    imgOptions.ImageFormat = System.Drawing.Imaging.ImageFormat.Bmp;

                                                    BmpPngExporter.CreateBmpPng(pixelsBuffer, palBuffer, imgOptions, outImgPath);
                                                    break;

                                                case 1:
                                                    outImgPath = Path.Combine(extractDir, "GRF1_img_" + imgFCount + ".dds");
                                                    DDSimgExporter.CreateDDS(pixelsBuffer, palBuffer, imgOptions, outImgPath);
                                                    break;

                                                case 2:
                                                    outImgPath = Path.Combine(extractDir, "GRF1_img_" + imgFCount + ".png");
                                                    imgOptions.ImageFormat = System.Drawing.Imaging.ImageFormat.Png;

                                                    BmpPngExporter.CreateBmpPng(pixelsBuffer, palBuffer, imgOptions, outImgPath);
                                                    break;
                                            }

                                            imgFCount++;
                                            imgReadPosStart += 32;
                                        }
                                    }
                                }


                                using (FileStream dls0Stream = new FileStream(Path.Combine(extractDir, "DLS0"), FileMode.OpenOrCreate, FileAccess.Write))
                                {
                                    spk0Stream.Seek(dls0SubChunkPos, SeekOrigin.Begin);
                                    spk0Stream.CopyStreamTo(dls0Stream, dls0Size, false);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        CommonMethods.AppMsgBox("" + ex, "Error", MessageBoxIcon.Error);
                        LoggingMethods.LogMessage(CommonMethods.NewLineChara);
                        LoggingMethods.LogException("Exception: " + ex);
                        Close();
                    }
                }
                finally
                {
                    LoggingMethods.LogMessage(CommonMethods.NewLineChara);
                    LoggingMethods.LogMessage("Conversion has completed!");
                    LoggingMethods.LogMessage(CommonMethods.NewLineChara);

                    CommonMethods.AppMsgBox("Converted " + Path.GetFileName(_Spk0File) + " file", "Success", MessageBoxIcon.Information);

                    BeginInvoke(new Action(() => SPK0ConvertControls(true)));
                    BeginInvoke(new Action(() => ConvertSPK0ImgBtn.Text = "Convert"));
                }
            });
        }
    }
}