using Drakengard1and2Extractor.Libraries;
using System;
using System.IO;
using System.Windows.Forms;

namespace Drakengard1and2Extractor.Tools
{
    public partial class FileZIM : Form
    {
        private readonly string ZimFileVar;

        public FileZIM(string zimFile)
        {
            try
            {
                ZimFileVar = zimFile;

                var bppValue = 0;
                using (FileStream fs = new FileStream(zimFile, FileMode.Open, FileAccess.Read))
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        br.BaseStream.Position = 82;
                        bppValue = br.ReadByte();
                    }
                }

                if (bppValue == 64)
                {
                    CmnMethods.AppMsgBox("Detected 4bpp image.\nDo not use the alpha compensation setting when saving the image in png or dds formats.", "Warning", MessageBoxIcon.Warning);
                }

                InitializeComponent();

                ZimSaveAsComboBox.SelectedIndex = 0;
                ZimAlphaCompNumericUpDown.Enabled = false;
            }
            catch (Exception ex)
            {
                CmnMethods.AppMsgBox("" + ex, "Error", MessageBoxIcon.Error);
            }
        }


        private void ZimSaveAsComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (ZimSaveAsComboBox.SelectedIndex.Equals(0))
            {
                ZimAlphaCompNumericUpDown.Enabled = false;
            }
            if (ZimSaveAsComboBox.SelectedIndex.Equals(1) || ZimSaveAsComboBox.SelectedIndex.Equals(2))
            {
                ZimAlphaCompNumericUpDown.Enabled = true;
            }
        }


        private void UnSwizzleCheckBox_MouseHover(object sender, EventArgs e)
        {
            UnSwizzleChkBoxToolTip.Show("Warning: Use this option only when the converted image looks corrupt.", UnSwizzleCheckBox);
        }


        private void ConvertZIMImgBtn_MouseHover(object sender, EventArgs e)
        {
            ConvertZimImgBtnToolTip.Show("Converts and saves the image file to one of the selected formats", ConvertZIMImgBtn);
        }
        private void ConvertZIMImgBtn_Click(object sender, EventArgs e)
        {
            try
            {
                ConvertZIMImgBtn.Text = "Converting...";

                ZimSaveAsComboBox.Enabled = false;
                ConvertZIMImgBtn.Enabled = false;
                UnSwizzleCheckBox.Enabled = false;

                if (ZimSaveAsComboBox.SelectedIndex.Equals(1) || ZimSaveAsComboBox.SelectedIndex.Equals(2))
                {
                    ZimAlphaCompNumericUpDown.Enabled = false;
                }

                var zimFileDir = Path.GetFullPath(ZimFileVar);
                var extractDir = Path.GetDirectoryName(zimFileDir) + "/";

                using (FileStream zimStream = new FileStream(ZimFileVar, FileMode.Open, FileAccess.Read))
                {
                    using (BinaryReader zimReader = new BinaryReader(zimStream))
                    {
                        zimReader.BaseStream.Position = 44;
                        var width = zimReader.ReadUInt16();
                        var height = zimReader.ReadUInt16();

                        zimReader.BaseStream.Position = 52;
                        var imgSize = zimReader.ReadUInt32();

                        zimReader.BaseStream.Position = 72;
                        var paletteSection = zimReader.ReadUInt32();
                        var palSize = zimReader.ReadUInt32();

                        zimReader.BaseStream.Position = 82;
                        var bppFlag = zimReader.ReadByte();


                        using (MemoryStream pixelsStream = new MemoryStream())
                        {
                            zimStream.Seek(352, SeekOrigin.Begin);
                            byte[] pixelsBuffer = new byte[imgSize];
                            var pixelDataToCopy = zimStream.Read(pixelsBuffer, 0, pixelsBuffer.Length);

                            if (UnSwizzleCheckBox.Checked.Equals(true) && bppFlag == 48)
                            {
                                PS2UnSwizzlers.UnSwizzlePixels(ref pixelsBuffer, width, height);
                            }

                            if (bppFlag == 64)
                            {
                                byte[] convertedPixels = ConvertPixelsTo8Bpp(pixelsBuffer);
                                pixelsStream.Write(convertedPixels, 0, convertedPixels.Length);
                            }
                            else
                            {
                                pixelsStream.Write(pixelsBuffer, 0, pixelDataToCopy);
                            }


                            using (MemoryStream paletteStream = new MemoryStream())
                            {
                                zimStream.Seek(paletteSection + 160, SeekOrigin.Begin);
                                byte[] paletteBuffer = new byte[palSize];
                                var paletteDataToCopy = zimStream.Read(paletteBuffer, 0, paletteBuffer.Length);

                                if (bppFlag == 48)
                                {
                                    PS2UnSwizzlers.UnSwizzlePalette(ref paletteBuffer);
                                }
                                paletteStream.Write(paletteBuffer, 0, paletteBuffer.Length);

                                using (BinaryReader pixelsReader = new BinaryReader(pixelsStream))
                                {
                                    using (BinaryReader paletteReader = new BinaryReader(paletteStream))
                                    {
                                        string outImgPath;
                                        switch (ZimSaveAsComboBox.SelectedIndex)
                                        {
                                            case 0:
                                                outImgPath = extractDir + Path.GetFileNameWithoutExtension(ZimFileVar) + ".bmp";
                                                IfOutImgFileExistsDel(outImgPath);
                                                ImageWriters.BmpPng(height, width, (int)ZimAlphaCompNumericUpDown.Value, pixelsReader, paletteReader, ImageWriters.SaveAs.bmp, outImgPath);
                                                break;

                                            case 1:
                                                outImgPath = extractDir + Path.GetFileNameWithoutExtension(ZimFileVar) + ".dds";
                                                IfOutImgFileExistsDel(outImgPath);
                                                ImageWriters.DDS(outImgPath, height, width, (int)ZimAlphaCompNumericUpDown.Value, pixelsReader, paletteReader);
                                                break;

                                            case 2:
                                                outImgPath = extractDir + Path.GetFileNameWithoutExtension(ZimFileVar) + ".png";
                                                IfOutImgFileExistsDel(outImgPath);
                                                ImageWriters.BmpPng(height, width, (int)ZimAlphaCompNumericUpDown.Value, pixelsReader, paletteReader, ImageWriters.SaveAs.png, outImgPath);
                                                break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                CmnMethods.AppMsgBox("Converted " + Path.GetFileName(ZimFileVar) + " file", "Success", MessageBoxIcon.Information);

                ZimSaveAsComboBox.Enabled = true;
                ConvertZIMImgBtn.Enabled = true;
                UnSwizzleCheckBox.Enabled = true;

                if (ZimSaveAsComboBox.SelectedIndex.Equals(1) || ZimSaveAsComboBox.SelectedIndex.Equals(2))
                {
                    ZimAlphaCompNumericUpDown.Enabled = true;
                }

                ConvertZIMImgBtn.Text = "Convert";
            }
            catch (Exception ex)
            {
                CmnMethods.AppMsgBox("" + ex, "Error", MessageBoxIcon.Error);
                Close();
            }
        }


        private byte[] ConvertPixelsTo8Bpp(byte[] pixelsBufferVar)
        {
            using (MemoryStream pixels4bpp = new MemoryStream())
            {
                pixels4bpp.Write(pixelsBufferVar, 0, pixelsBufferVar.Length);

                using (MemoryStream convertedPixelData = new MemoryStream())
                {
                    using (BinaryWriter convertedPixelDataWriter = new BinaryWriter(convertedPixelData))
                    {
                        using (BinaryReader pixels4bppReader = new BinaryReader(pixels4bpp))
                        {
                            uint readPos = 0;
                            byte pixelBits;
                            int pixelBit1;
                            int pixelBit2;
                            uint writePos = 0;
                            for (int c = 0; c < pixels4bpp.Length; c++)
                            {
                                pixels4bppReader.BaseStream.Position = readPos;
                                pixelBits = pixels4bppReader.ReadByte();

                                pixelBit1 = pixelBits >> 4;
                                pixelBit2 = pixelBits & 0xF;

                                convertedPixelDataWriter.BaseStream.Position = writePos;
                                convertedPixelDataWriter.Write((byte)pixelBit1);
                                convertedPixelDataWriter.Write((byte)pixelBit2);

                                readPos++;
                                writePos += 2;
                            }

                            byte[] convertedPixelsVar = convertedPixelData.ToArray();
                            return convertedPixelsVar;
                        }
                    }
                }
            }
        }


        static void IfOutImgFileExistsDel(string outImgFileName)
        {
            if (File.Exists(outImgFileName))
            {
                File.Delete(outImgFileName);
            }
        }
    }
}