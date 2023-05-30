using System.Buffers.Binary;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Drakengard1and2Extractor.Libraries
{
    public class ImageWriters
    {
        public static void BmpPng(ushort heightVar, ushort widthVar, int alphaIncreaseVar, BinaryReader pixelReaderVar,
            BinaryReader paletteReaderVar, SaveAs saveAsVar, string outImgPathVar)
        {
            int currentPixel;
            uint getPalettePos;
            uint palettePos;
            byte red = 0;
            byte green = 0;
            byte blue = 0;
            int alpha = 0;
            Color pixelColor;

            using (Bitmap finalImg = new Bitmap(widthVar, heightVar))
            {
                for (int y = 0; y < heightVar; y++)
                {
                    for (int x = 0; x < widthVar; x++)
                    {
                        currentPixel = (y * widthVar) + x;
                        pixelReaderVar.BaseStream.Position = currentPixel;
                        getPalettePos = pixelReaderVar.ReadByte();

                        palettePos = getPalettePos * 4;
                        ReadColorValues(paletteReaderVar, palettePos, ref red, ref green, ref blue, ref alpha, alphaIncreaseVar);

                        pixelColor = Color.FromArgb(alpha, red, green, blue);
                        finalImg.SetPixel(x, y, pixelColor);
                    }
                }

                switch (saveAsVar)
                {
                    case SaveAs.bmp:
                        finalImg.Save(outImgPathVar, ImageFormat.Bmp);
                        break;

                    case SaveAs.png:
                        finalImg.Save(outImgPathVar, ImageFormat.Png);
                        break;
                }
            }
        }
        public enum SaveAs
        {
            bmp,
            png
        }


        public static void DDS(string outImgPathVar, uint heightVar, uint widthVar, int alphaIncreaseVar, BinaryReader pixelReaderVar, BinaryReader paletteReaderVar)
        {
            using (FileStream ddsFile = new FileStream(outImgPathVar, FileMode.Append, FileAccess.Write))
            {
                using (BinaryWriter ddsFileWriter = new BinaryWriter(ddsFile))
                {
                    for (int h = 0; h < 128; h++)
                    {
                        ddsFile.WriteByte(0);
                    }

                    DDSHeaderWriter(ddsFileWriter, 0, 542327876);

                    DDSHeaderWriter(ddsFileWriter, 4, 124);

                    DDSHeaderWriter(ddsFileWriter, 8, 4111);

                    DDSHeaderWriter(ddsFileWriter, 12, heightVar);

                    DDSHeaderWriter(ddsFileWriter, 16, widthVar);

                    uint pitch = (widthVar * 32 + 7) / 8;
                    DDSHeaderWriter(ddsFileWriter, 20, pitch);

                    DDSHeaderWriter(ddsFileWriter, 28, 1);   // (mip offset)

                    DDSHeaderWriter(ddsFileWriter, 76, 32);

                    DDSHeaderWriter(ddsFileWriter, 80, 65);

                    DDSHeaderWriter(ddsFileWriter, 88, 32);

                    DDSHeaderWriter(ddsFileWriter, 92, 16711680);

                    DDSHeaderWriter(ddsFileWriter, 96, 65280);

                    DDSHeaderWriter(ddsFileWriter, 100, 255);

                    DDSHeaderWriter(ddsFileWriter, 104, 4278190080);

                    DDSHeaderWriter(ddsFileWriter, 108, 4096);


                    var pixelDataLength = widthVar * heightVar;
                    uint readPos = 0;
                    uint getPalettePos;
                    uint palettePos;
                    byte red = 0;
                    byte green = 0;
                    byte blue = 0;
                    int alpha = 0;
                    uint writePos = 128;
                    for (int i = 0; i < pixelDataLength; i++)
                    {
                        pixelReaderVar.BaseStream.Position = readPos;
                        getPalettePos = pixelReaderVar.ReadByte();
                        palettePos = getPalettePos * 4;

                        ReadColorValues(paletteReaderVar, palettePos, ref red, ref green, ref blue, ref alpha, alphaIncreaseVar);

                        ddsFileWriter.BaseStream.Position = writePos;
                        ddsFileWriter.Write(blue);
                        ddsFileWriter.Write(green);
                        ddsFileWriter.Write(red);
                        ddsFileWriter.Write((byte)alpha);

                        writePos += 4;
                        readPos++;
                    }
                }
            }
        }
        static void DDSHeaderWriter(BinaryWriter writerVar, uint writerPos, uint valueToWrite)
        {
            byte[] adjustBuffer = new byte[4];
            writerVar.BaseStream.Position = writerPos;
            BinaryPrimitives.WriteUInt32LittleEndian(adjustBuffer, valueToWrite);
            writerVar.Write(adjustBuffer);
        }


        public static void ReadColorValues(BinaryReader paletteReaderVar, uint readPos, ref byte redVar, ref byte greenVar, ref byte blueVar, ref int alphaVar, int alphaIncreaseVar)
        {
            paletteReaderVar.BaseStream.Position = readPos;
            redVar = paletteReaderVar.ReadByte();
            greenVar = paletteReaderVar.ReadByte();
            blueVar = paletteReaderVar.ReadByte();
            alphaVar = paletteReaderVar.ReadByte();

            alphaVar += alphaIncreaseVar;
            if (alphaVar > 128)
            {
                alphaVar = 128;
            }
        }
    }
}