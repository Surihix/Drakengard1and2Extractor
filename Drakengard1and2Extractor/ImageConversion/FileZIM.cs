using Drakengard1and2Extractor.Support;
using System;
using System.IO;
using System.Windows.Forms;

namespace Drakengard1and2Extractor.ImageConversion
{
    public partial class FileZIM : Form
    {
        private readonly string _ZimFileVar;

        public void ZIMConvertButtons(bool isEnabled)
        {
            ZimSaveAsComboBox.Enabled = isEnabled;
            ConvertZIMImgBtn.Enabled = isEnabled;
            UnSwizzleCheckBox.Enabled = isEnabled;

            if (ZimSaveAsComboBox.SelectedIndex == 1 || ZimSaveAsComboBox.SelectedIndex == 2)
            {
                ZimAlphaCompNumericUpDown.Enabled = isEnabled;
            }
        }

        public FileZIM(string zimFile)
        {
            try
            {
                _ZimFileVar = zimFile;

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
                    CommonMethods.AppMsgBox("Detected 4bpp image.\nDo not use the alpha compensation setting when saving the image in png or dds formats.", "Warning", MessageBoxIcon.Warning);
                }

                InitializeComponent();

                ZimSaveAsComboBox.SelectedIndex = 0;
                ZimAlphaCompNumericUpDown.Enabled = false;
            }
            catch (Exception ex)
            {
                CommonMethods.AppMsgBox("" + ex, "Error", MessageBoxIcon.Error);
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

                ZIMConvertButtons(false);

                var imgOptions = new ImgOptions();
                var zimFileDir = Path.GetFullPath(_ZimFileVar);
                var extractDir = Path.GetDirectoryName(zimFileDir) + "/";
                imgOptions.AlphaIncrease = (int)ZimAlphaCompNumericUpDown.Value;

                using (FileStream zimStream = new FileStream(_ZimFileVar, FileMode.Open, FileAccess.Read))
                {
                    using (BinaryReader zimReader = new BinaryReader(zimStream))
                    {
                        zimReader.BaseStream.Position = 44;
                        imgOptions.Width = zimReader.ReadUInt16();
                        imgOptions.Height = zimReader.ReadUInt16();

                        zimReader.BaseStream.Position = 52;
                        var imgSize = zimReader.ReadUInt32();

                        zimReader.BaseStream.Position = 72;
                        var paletteSection = zimReader.ReadUInt32();
                        var palSize = zimReader.ReadUInt32();

                        zimReader.BaseStream.Position = 82;
                        var bppFlag = zimReader.ReadByte();

                        // Process pixel data
                        zimStream.Seek(352, SeekOrigin.Begin);
                        byte[] pixelsBuffer = new byte[imgSize];
                        _ = zimStream.Read(pixelsBuffer, 0, pixelsBuffer.Length);

                        if (UnSwizzleCheckBox.Checked && bppFlag == 48)
                        {
                            byte[] unswizzledPixelsBuffer = pixelsBuffer.UnSwizzlePixels(imgOptions);
                            pixelsBuffer = unswizzledPixelsBuffer;                            
                        }

                        byte[] finalizedPixels = new byte[] { };
                        if (bppFlag == 64)
                        {
                            byte[] convertedPixels = ConvertPixelsTo8Bpp(pixelsBuffer);
                            finalizedPixels = convertedPixels;
                        }
                        else
                        {
                            finalizedPixels = pixelsBuffer;
                        }

                        // Process palette data
                        zimStream.Seek(paletteSection + 160, SeekOrigin.Begin);
                        byte[] paletteBuffer = new byte[palSize];
                        _ = zimStream.Read(paletteBuffer, 0, paletteBuffer.Length);

                        byte[] finalizedPalette = new byte[palSize];
                        if (bppFlag == 48)
                        {
                            finalizedPalette = paletteBuffer.UnSwizzlePalette();
                            File.WriteAllBytes("test_Palette", finalizedPalette);
                        }
                        else
                        {
                            finalizedPalette = paletteBuffer;
                        }

                        File.WriteAllBytes("test_Pixels", finalizedPixels);

                        // Convert according to the
                        // specified format
                        string outImgPath;
                        switch (ZimSaveAsComboBox.SelectedIndex)
                        {
                            case 0:
                                outImgPath = extractDir + Path.GetFileNameWithoutExtension(_ZimFileVar) + ".bmp";
                                imgOptions.ImageFormat = System.Drawing.Imaging.ImageFormat.Bmp;

                                IfOutImgFileExistsDel(outImgPath);
                                finalizedPixels.CreateBmpPng(finalizedPalette, imgOptions, outImgPath);
                                break;

                            case 1:
                                outImgPath = extractDir + Path.GetFileNameWithoutExtension(_ZimFileVar) + ".dds";

                                IfOutImgFileExistsDel(outImgPath);
                                finalizedPixels.CreateDDS(finalizedPalette, imgOptions, outImgPath);
                                break;

                            case 2:
                                outImgPath = extractDir + Path.GetFileNameWithoutExtension(_ZimFileVar) + ".png";
                                imgOptions.ImageFormat = System.Drawing.Imaging.ImageFormat.Png;

                                IfOutImgFileExistsDel(outImgPath);
                                finalizedPixels.CreateBmpPng(finalizedPalette, imgOptions, outImgPath);
                                break;
                        }
                    }
                }

                CommonMethods.AppMsgBox("Converted " + Path.GetFileName(_ZimFileVar) + " file", "Success", MessageBoxIcon.Information);

                ZIMConvertButtons(true);

                ConvertZIMImgBtn.Text = "Convert";
            }
            catch (Exception ex)
            {
                CommonMethods.AppMsgBox("" + ex, "Error", MessageBoxIcon.Error);
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