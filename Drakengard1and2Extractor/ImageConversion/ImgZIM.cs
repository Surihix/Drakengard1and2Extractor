using Drakengard1and2Extractor.Support;
using Drakengard1and2Extractor.Support.ImageHelpers;
using System.IO;
using System.Windows.Forms;

namespace Drakengard1and2Extractor.ImageConversion
{
    internal class ImgZIM
    {
        public static void ConvertZIM(string zimFile, bool isSingleFile)
        {
            var zimFileDir = Path.GetFullPath(zimFile);
            var extractDir = Path.GetDirectoryName(zimFileDir);
            var zimNameNoExtn = Path.GetFileNameWithoutExtension(zimFile);

            using (FileStream zimStream = new FileStream(zimFile, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader zimReader = new BinaryReader(zimStream))
                {
                    zimReader.BaseStream.Position = 44;
                    ImgOptions.Width = zimReader.ReadUInt16();
                    ImgOptions.Height = zimReader.ReadUInt16();

                    zimReader.BaseStream.Position = 52;
                    var pixelSize = zimReader.ReadUInt32();

                    zimReader.BaseStream.Position = 72;
                    var paletteSection = zimReader.ReadUInt32();
                    var palSize = zimReader.ReadUInt32();

                    zimReader.BaseStream.Position = 82;
                    var bppFlag = zimReader.ReadByte();

                    zimStream.Seek(352, SeekOrigin.Begin);
                    byte[] pixelsBuffer = new byte[pixelSize];
                    _ = zimStream.Read(pixelsBuffer, 0, pixelsBuffer.Length);

                    if (ImgOptions.UnswizzlePixels && bppFlag == 48)
                    {
                        byte[] unswizzledPixelsBuffer = PS2Unswizzlers.UnswizzlePixels(pixelsBuffer);
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

                    zimStream.Seek(paletteSection + 160, SeekOrigin.Begin);
                    byte[] paletteBuffer = new byte[palSize];
                    _ = zimStream.Read(paletteBuffer, 0, paletteBuffer.Length);

                    byte[] finalizedPalette = new byte[palSize];
                    if (bppFlag == 48)
                    {
                        finalizedPalette = PS2Unswizzlers.UnswizzlePalette(paletteBuffer);
                    }
                    else
                    {
                        finalizedPalette = paletteBuffer;
                    }


                    string outImgPath;
                    switch (ImgOptions.SaveAsIndex)
                    {
                        case 0:
                            outImgPath = Path.Combine(extractDir, zimNameNoExtn + ".bmp");
                            ImgOptions.ImageFormat = System.Drawing.Imaging.ImageFormat.Bmp;

                            SharedMethods.IfFileDirExistsDel(outImgPath, SharedMethods.DelSwitch.file);
                            BmpPngExporter.CreateBmpPng(finalizedPixels, finalizedPalette, outImgPath);
                            break;

                        case 1:
                            outImgPath = Path.Combine(extractDir, zimNameNoExtn + ".dds");

                            SharedMethods.IfFileDirExistsDel(outImgPath, SharedMethods.DelSwitch.file);
                            DDSimgExporter.CreateDDS(finalizedPixels, finalizedPalette, outImgPath);
                            break;

                        case 2:
                            outImgPath = Path.Combine(extractDir, zimNameNoExtn + ".png");
                            ImgOptions.ImageFormat = System.Drawing.Imaging.ImageFormat.Png;

                            SharedMethods.IfFileDirExistsDel(outImgPath, SharedMethods.DelSwitch.file);
                            BmpPngExporter.CreateBmpPng(finalizedPixels, finalizedPalette, outImgPath);
                            break;
                    }
                }
            }

            if (isSingleFile)
            {
                LoggingMethods.LogMessage(SharedMethods.NewLineChara);
                LoggingMethods.LogMessage("Conversion has completed!");
                LoggingMethods.LogMessage(SharedMethods.NewLineChara);

                SharedMethods.AppMsgBox("Converted " + Path.GetFileName(zimFile) + " file", "Success", MessageBoxIcon.Information);
            }
        }


        private static byte[] ConvertPixelsTo8Bpp(byte[] pixelsBuffer)
        {
            var newPixelsBuffer = new byte[pixelsBuffer.Length * 2];
            var writePos = 0;

            for (int b = 0; b < pixelsBuffer.Length; b++)
            {
                var currentPixel = pixelsBuffer[b];

                newPixelsBuffer[writePos] = (byte)(currentPixel & 0xf);
                newPixelsBuffer[writePos + 1] = (byte)(currentPixel >> 4);

                writePos += 2;
            }

            return newPixelsBuffer;
        }
    }
}