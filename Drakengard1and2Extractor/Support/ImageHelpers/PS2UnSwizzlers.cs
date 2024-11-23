using System;

namespace Drakengard1and2Extractor.Support.ImageHelpers
{
    internal static class PS2Unswizzlers
    {
        public static byte[] UnswizzlePixels8bpp(byte[] pixelBufferVar)
        {
            byte[] swizzleWorkBuffer = new byte[pixelBufferVar.Length];
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


        public static byte[] UnswizzlePixels4bpp(byte[] pixelBufferVar)
        {
            byte[] unswizzledPixels = new byte[pixelBufferVar.Length];

            for (int y = 0; y < ImgOptions.Height; y++)
            {
                for (int x = 0; x < ImgOptions.Width; x++)
                {
                    var index = y * ImgOptions.Width + x;

                    var pageX = x & (~0x7f);
                    var pageY = y & (~0x7f);

                    var pages_horz = (ImgOptions.Width + 127) / 128;
                    var pages_vert = (ImgOptions.Height + 127) / 128;

                    var page_number = (pageY / 128) * pages_horz + (pageX / 128);

                    var page32Y = (page_number / pages_vert) * 32;
                    var page32X = (page_number % pages_vert) * 64;

                    var page_location = page32Y * ImgOptions.Height * 2 + page32X * 4;

                    var locX = x & 0x7f;
                    var locY = y & 0x7f;

                    var block_location = ((locX & (~0x1f)) >> 1) * ImgOptions.Height + (locY & (~0xf)) * 2;
                    var swap_selector = (((y + 2) >> 2) & 0x1) * 4;
                    var posY = (((y & (~3)) >> 1) + (y & 1)) & 0x7;

                    var column_location = posY * ImgOptions.Height * 2 + ((x + swap_selector) & 0x7) * 4;

                    var byte_num = (x >> 3) & 3;
                    var bits_set = (y >> 1) & 1;
                    var pos = page_location + block_location + column_location + byte_num;

                    if ((bits_set & 1) != 0)
                    {
                        var uPen = (byte)((pixelBufferVar[pos] >> 4) & 0xf);
                        var pix = unswizzledPixels[index >> 1];

                        if ((index & 1) != 0)
                        {
                            unswizzledPixels[index >> 1] = (byte)(((uPen << 4) & 0xf0) | (pix & 0xf));
                        }
                        else
                        {
                            unswizzledPixels[index >> 1] = (byte)((pix & 0xf0) | (uPen & 0xf));
                        }
                    }
                    else
                    {
                        var uPen = (byte)(pixelBufferVar[pos] & 0xf);
                        var pix = unswizzledPixels[index >> 1];

                        if ((index & 1) != 0)
                        {
                            unswizzledPixels[index >> 1] = (byte)(((uPen << 4) & 0xf0) | (pix & 0xf));
                        }
                        else
                        {
                            unswizzledPixels[index >> 1] = (byte)((pix & 0xf0) | (uPen & 0xf));
                        }
                    }
                }
            }

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