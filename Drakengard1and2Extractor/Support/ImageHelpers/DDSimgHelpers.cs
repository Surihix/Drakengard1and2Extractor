using System;
using System.IO;

internal static class DDSimgHelpers
{
    public static void CreateDDS(this byte[] pixelsData, byte[] paletteData, ImgOptions imgOptions, string outImgPath)
    {
        using (var ddsStream = new FileStream(outImgPath, FileMode.OpenOrCreate, FileAccess.Write))
        {
            using (var ddsStreamWriter = new BinaryWriter(ddsStream))
            {
                // Pad the stream with
                // 128 null bytes
                for (int h = 0; h < 128; h++)
                {
                    ddsStreamWriter.BaseStream.WriteByte(0);
                }

                // Write common DDS header flags
                _ = ddsStreamWriter.BaseStream.Position = 0;
                ddsStreamWriter.Write(BitConverter.GetBytes((uint)542327876));

                _ = ddsStreamWriter.BaseStream.Position = 4;
                ddsStreamWriter.Write(BitConverter.GetBytes((uint)124));

                _ = ddsStreamWriter.BaseStream.Position = 8;
                ddsStreamWriter.Write(BitConverter.GetBytes((uint)4111));

                _ = ddsStreamWriter.BaseStream.Position = 12;
                ddsStreamWriter.Write(BitConverter.GetBytes((uint)imgOptions.Height));

                _ = ddsStreamWriter.BaseStream.Position = 16;
                ddsStreamWriter.Write(BitConverter.GetBytes((uint)imgOptions.Width));

                _ = ddsStreamWriter.BaseStream.Position = 28;
                ddsStreamWriter.Write(BitConverter.GetBytes(1));

                _ = ddsStreamWriter.BaseStream.Position = 76;
                ddsStreamWriter.Write(BitConverter.GetBytes((uint)32));

                // Write the mip related flag
                _ = ddsStreamWriter.BaseStream.Position = 108;
                ddsStreamWriter.Write(BitConverter.GetBytes((uint)4096));


                // Computes DDS pitch 
                uint pitch = ((uint)imgOptions.Width * 32 + 7) / 8;

                _ = ddsStreamWriter.BaseStream.Position = 20;
                ddsStreamWriter.Write(BitConverter.GetBytes(pitch));

                // Writes pixel format flags which are
                // common for R8G8B8A8
                _ = ddsStreamWriter.BaseStream.Position = 80;
                ddsStreamWriter.Write(BitConverter.GetBytes((uint)65));

                _ = ddsStreamWriter.BaseStream.Position = 88;
                ddsStreamWriter.Write(BitConverter.GetBytes((uint)32));

                _ = ddsStreamWriter.BaseStream.Position = 92;
                ddsStreamWriter.Write(BitConverter.GetBytes((uint)16711680));

                _ = ddsStreamWriter.BaseStream.Position = 96;
                ddsStreamWriter.Write(BitConverter.GetBytes((uint)65280));

                _ = ddsStreamWriter.BaseStream.Position = 100;
                ddsStreamWriter.Write(BitConverter.GetBytes((uint)255));

                _ = ddsStreamWriter.BaseStream.Position = 104;
                ddsStreamWriter.Write(BitConverter.GetBytes((uint)4278190080));


                // Write the image data
                long writePos = 128;
                for (int i = 0; i < imgOptions.Width * imgOptions.Height; i++)
                {
                    int palettePos = pixelsData[i];
                    palettePos *= 4;

                    byte red = paletteData[palettePos];
                    byte green = paletteData[palettePos + 1];
                    byte blue = paletteData[palettePos + 2];
                    int alpha = paletteData[palettePos + 3];

                    alpha += imgOptions.AlphaIncrease;
                    if (alpha > 128)
                    {
                        alpha = 128;
                    }

                    byte[] writeArray = new byte[4];
                    writeArray[0] = blue;
                    writeArray[1] = green;
                    writeArray[2] = red;
                    writeArray[3] = (byte)alpha;

                    ddsStreamWriter.BaseStream.Position = writePos;
                    ddsStreamWriter.Write(writeArray);

                    writePos += 4;
                }
            }
        }
    }
}