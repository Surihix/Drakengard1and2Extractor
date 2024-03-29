﻿using Drakengard1and2Extractor.Libraries;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Drakengard1and2Extractor.Tools
{
    public partial class FileSPK0 : Form
    {
        private readonly string Spk0FileVar;

        public FileSPK0(string spk0File)
        {
            try
            {
                Spk0FileVar = spk0File;

                InitializeComponent();

                Spk0SaveAsComboBox.SelectedIndex = 0;
                Spk0AlphaCompNumericUpDown.Enabled = false;
            }
            catch (Exception ex)
            {
                CmnMethods.AppMsgBox("" + ex, "Error", MessageBoxIcon.Error);
            }
        }


        private void Spk0SaveAsComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (Spk0SaveAsComboBox.SelectedIndex.Equals(0))
            {
                Spk0AlphaCompNumericUpDown.Enabled = false;
            }
            if (Spk0SaveAsComboBox.SelectedIndex.Equals(1) || Spk0SaveAsComboBox.SelectedIndex.Equals(2))
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

            Spk0SaveAsComboBox.Enabled = false;
            ConvertSPK0ImgBtn.Enabled = false;

            if (Spk0SaveAsComboBox.SelectedIndex.Equals(1) || Spk0SaveAsComboBox.SelectedIndex.Equals(2))
            {
                Spk0AlphaCompNumericUpDown.Enabled = false;
            }

            var saveAsComboBoxItemIndex = Spk0SaveAsComboBox.SelectedIndex;
            var spk0AlphaCompNumericUpDownVal = (int)Spk0AlphaCompNumericUpDown.Value;

            Task.Run(() =>
            {
                try
                {
                    try
                    {
                        var spk0FileDir = Path.GetFullPath(Spk0FileVar);
                        var extractDir = Path.GetDirectoryName(spk0FileDir) + "/" + Path.GetFileName(Spk0FileVar) + "_extracted";

                        if (Directory.Exists(extractDir))
                        {
                            Directory.Delete(extractDir, true);
                        }
                        Directory.CreateDirectory(extractDir);


                        using (FileStream spk0Stream = new FileStream(Spk0FileVar, FileMode.Open, FileAccess.Read))
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


                                using (FileStream dmt0Stream = new FileStream(extractDir + "/" + "DMT0", FileMode.OpenOrCreate, FileAccess.Write))
                                {
                                    spk0Stream.CopyTo(dmt0Stream, 32, dmt0Size);
                                }


                                using (FileStream grf1Stream = new FileStream(extractDir + "/" + "GRF1", FileMode.OpenOrCreate, FileAccess.ReadWrite))
                                {
                                    spk0Stream.CopyTo(grf1Stream, grf1SubChunkPos, grf1Size);

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

                                        PS2UnSwizzlers.UnSwizzlePalette(ref palBuffer);

                                        using (MemoryStream paletteStream = new MemoryStream())
                                        {
                                            paletteStream.Write(palBuffer, 0, palBuffer.Length);

                                            using (BinaryReader paletteReader = new BinaryReader(paletteStream))
                                            {

                                                uint imgReadPosStart = 20;
                                                var imgFCount = 1;
                                                for (int i = 0; i < totalImgCount; i++)
                                                {
                                                    grf1Reader.BaseStream.Position = imgReadPosStart;
                                                    var imgStartPos = grf1Reader.ReadUInt32();

                                                    grf1Reader.BaseStream.Position = imgStartPos + 96;
                                                    var width = grf1Reader.ReadUInt32();
                                                    var height = grf1Reader.ReadUInt32();
                                                    var imgSize = width * height;

                                                    using (MemoryStream pixelsStream = new MemoryStream())
                                                    {
                                                        grf1Stream.CopyTo(pixelsStream, imgStartPos + 144, imgSize);

                                                        using (BinaryReader pixelsReader = new BinaryReader(pixelsStream))
                                                        {

                                                            string outImgPath;
                                                            switch (saveAsComboBoxItemIndex)
                                                            {
                                                                case 0:
                                                                    outImgPath = extractDir + "/" + "GRF1_img_" + imgFCount + ".bmp";
                                                                    ImageWriters.BmpPng((ushort)height, (ushort)width, spk0AlphaCompNumericUpDownVal, pixelsReader, paletteReader, ImageWriters.SaveAs.bmp, outImgPath);
                                                                    break;

                                                                case 1:
                                                                    outImgPath = extractDir + "/" + "GRF1_img_" + imgFCount + ".dds";
                                                                    ImageWriters.DDS(outImgPath, height, width, spk0AlphaCompNumericUpDownVal, pixelsReader, paletteReader);
                                                                    break;

                                                                case 2:
                                                                    outImgPath = extractDir + "/" + "GRF1_img_" + imgFCount + ".png";
                                                                    ImageWriters.BmpPng((ushort)height, (ushort)width, spk0AlphaCompNumericUpDownVal, pixelsReader, paletteReader, ImageWriters.SaveAs.png, outImgPath);
                                                                    break;
                                                            }
                                                        }
                                                    }

                                                    imgFCount++;
                                                    imgReadPosStart += 32;
                                                }
                                            }
                                        }
                                    }
                                }


                                using (FileStream dls0Stream = new FileStream(extractDir + "/" + "DLS0", FileMode.OpenOrCreate, FileAccess.Write))
                                {
                                    spk0Stream.CopyTo(dls0Stream, dls0SubChunkPos, dls0Size);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        CmnMethods.AppMsgBox("" + ex, "Error", MessageBoxIcon.Error);
                        Close();
                    }
                }
                finally
                {
                    CmnMethods.AppMsgBox("Converted " + Path.GetFileName(Spk0FileVar) + " file", "Success", MessageBoxIcon.Information);

                    BeginInvoke(new Action(() => Spk0SaveAsComboBox.Enabled = true));
                    BeginInvoke(new Action(() => ConvertSPK0ImgBtn.Enabled = true));

                    if (saveAsComboBoxItemIndex.Equals(1) || saveAsComboBoxItemIndex.Equals(2))
                    {
                        BeginInvoke(new Action(() => Spk0AlphaCompNumericUpDown.Enabled = true));
                    }

                    BeginInvoke(new Action(() => ConvertSPK0ImgBtn.Text = "Convert"));
                }
            });
        }
    }
}