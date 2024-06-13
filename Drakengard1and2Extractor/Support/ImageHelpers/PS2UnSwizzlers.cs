using System;

namespace Drakengard1and2Extractor.Support.ImageHelpers
{
    internal static class PS2Unswizzlers
    {
        public static byte[] UnswizzlePixels(byte[] pixelBufferVar)
        {
            byte[] swizzleWorkBuffer = new byte[pixelBufferVar.Length - 0];
            Array.Copy(pixelBufferVar, 0, swizzleWorkBuffer, 0, swizzleWorkBuffer.Length);

            for (int y = 0; y < ImgOptions.Height; y++)
            {
                for (int x = 0; x < ImgOptions.Width; x++)
                {
                    int blockLocation = (y & (~0xf)) * ImgOptions.Width + (x & (~0xf)) * 2;
                    int swapSelector = (((y + 2) >> 2) & 0x1) * 4;
                    int posY = (((y & (~3)) >> 1) + (y & 1)) & 0x7;
                    int columnLocation = posY * ImgOptions.Width * 2 + ((x + swapSelector) & 0x7) * 4;

                    int byteNum = ((y >> 1) & 1) + ((x >> 2) & 2);

                    pixelBufferVar[0 + (y * ImgOptions.Width) + x] = swizzleWorkBuffer[blockLocation + columnLocation + byteNum];
                }
            }

            byte[] unswizzledPixels = new byte[pixelBufferVar.Length];
            Array.Copy(pixelBufferVar, unswizzledPixels, unswizzledPixels.Length);

            return unswizzledPixels;
        }


        public static byte[] UnswizzlePalette(byte[] paletteBuffer)
        {
            byte[] newPaletteBuffer = new byte[1024];
            int copyIndex = 0;
            int p = 0;

            // Per iteration, 128 bytes will be 
            // unswizzled
            while (p < 1024)
            {
                // Copy this iteration's first 32 bytes
                Array.ConstrainedCopy(paletteBuffer, p, newPaletteBuffer, copyIndex, 32);
                copyIndex += 32;

                // From the 64th position after the first 32 bytes of
                // this iteration, copy 32 bytes
                p += 64;
                Array.ConstrainedCopy(paletteBuffer, p, newPaletteBuffer, copyIndex, 32);
                copyIndex += 32;

                // From the 32nd position after the first 32 bytes of
                // this iteration, copy 32 bytes
                p -= 32;
                Array.ConstrainedCopy(paletteBuffer, p, newPaletteBuffer, copyIndex, 32);
                copyIndex += 32;

                // From the 96th position after the first 32 bytes of
                // this iteration, copy 32 bytes
                p += 64;
                Array.ConstrainedCopy(paletteBuffer, p, newPaletteBuffer, copyIndex, 32);
                copyIndex += 32;

                p += 32;
            }

            return newPaletteBuffer;
        }
    }
}