using Drakengard1and2Extractor.Support;
using Drakengard1and2Extractor.Support.ImageHelpers;
using System.IO;
using System.Windows.Forms;

namespace Drakengard1and2Extractor.ImageConversion
{
    internal class ImgZIM
    {
        public static void ConvertZIM(string zimFile, int zimAlphaCompValue, bool unswizzlePixels, int zimSaveAsCBoxIndex, bool isSingleFile)
        {
            var imgOptions = new ImgOptions();
            var zimFileDir = Path.GetFullPath(zimFile);
            var extractDir = Path.GetDirectoryName(zimFileDir);
            var zimNameNoExtn = Path.GetFileNameWithoutExtension(zimFile);
            imgOptions.AlphaIncrease = zimAlphaCompValue;

            using (FileStream zimStream = new FileStream(zimFile, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader zimReader = new BinaryReader(zimStream))
                {
                    zimReader.BaseStream.Position = 44;
                    imgOptions.Width = zimReader.ReadUInt16();
                    imgOptions.Height = zimReader.ReadUInt16();

                    zimReader.BaseStream.Position = 52;
                    var pixelSize = zimReader.ReadUInt32();

                    zimReader.BaseStream.Position = 72;
                    var paletteSection = zimReader.ReadUInt32();
                    var palSize = zimReader.ReadUInt32();

                    zimReader.BaseStream.Position = 82;
                    var bppFlag = zimReader.ReadByte();

                    // Process pixel data
                    zimStream.Seek(352, SeekOrigin.Begin);
                    byte[] pixelsBuffer = new byte[pixelSize];
                    _ = zimStream.Read(pixelsBuffer, 0, pixelsBuffer.Length);

                    if (unswizzlePixels && bppFlag == 48)
                    {
                        byte[] unswizzledPixelsBuffer = PS2Unswizzlers.UnSwizzlePixels(pixelsBuffer, imgOptions);
                        pixelsBuffer = unswizzledPixelsBuffer;
                    }

                    byte[] finalizedPixels = new byte[pixelSize];
                    if (bppFlag == 64)
                    {
                        finalizedPixels = ConvertPixelsTo8Bpp(pixelsBuffer);
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
                        finalizedPalette = PS2Unswizzlers.UnSwizzlePalette(paletteBuffer);
                    }
                    else
                    {
                        finalizedPalette = paletteBuffer;
                    }

                    // Convert according to the
                    // specified format
                    string outImgPath;
                    switch (zimSaveAsCBoxIndex)
                    {
                        case 0:
                            outImgPath = Path.Combine(extractDir, zimNameNoExtn + ".bmp");
                            imgOptions.ImageFormat = System.Drawing.Imaging.ImageFormat.Bmp;

                            CommonMethods.IfFileDirExistsDel(outImgPath, CommonMethods.DelSwitch.file);
                            BmpPngExporter.CreateBmpPng(finalizedPixels, finalizedPalette, imgOptions, outImgPath);
                            break;

                        case 1:
                            outImgPath = Path.Combine(extractDir, zimNameNoExtn + ".dds");

                            CommonMethods.IfFileDirExistsDel(outImgPath, CommonMethods.DelSwitch.file);
                            DDSimgExporter.CreateDDS(finalizedPixels, finalizedPalette, imgOptions, outImgPath);
                            break;

                        case 2:
                            outImgPath = Path.Combine(extractDir, zimNameNoExtn + ".png");
                            imgOptions.ImageFormat = System.Drawing.Imaging.ImageFormat.Png;

                            CommonMethods.IfFileDirExistsDel(outImgPath, CommonMethods.DelSwitch.file);
                            BmpPngExporter.CreateBmpPng(finalizedPixels, finalizedPalette, imgOptions, outImgPath);
                            break;
                    }
                }
            }

            if (isSingleFile)
            {
                LoggingMethods.LogMessage("Conversion has completed!");
                LoggingMethods.LogMessage(CommonMethods.NewLineChara);

                CommonMethods.AppMsgBox("Converted " + Path.GetFileName(zimFile) + " file", "Success", MessageBoxIcon.Information);
            }
        }


        private static byte[] ConvertPixelsTo8Bpp(byte[] pixelsBufferVar)
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
                                convertedPixelDataWriter.Write((byte)pixelBit2);
                                convertedPixelDataWriter.Write((byte)pixelBit1);

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
    }
}