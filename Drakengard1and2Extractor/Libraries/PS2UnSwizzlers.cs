using System;

namespace Drakengard1and2Extractor.Libraries
{
    internal class PS2UnSwizzlers
    {
        public static void UnSwizzlePixels(ref byte[] pixelBufferVar, ushort widthVar, ushort heightVar)
        {
            byte[] swizzledBuffer = new byte[pixelBufferVar.Length - 0];
            Array.Copy(pixelBufferVar, 0, swizzledBuffer, 0, swizzledBuffer.Length);

            for (int y = 0; y < heightVar; y++)
            {
                for (int x = 0; x < widthVar; x++)
                {
                    int blockLocation = (y & (~0xf)) * widthVar + (x & (~0xf)) * 2;
                    int swapSelector = (((y + 2) >> 2) & 0x1) * 4;
                    int posY = (((y & (~3)) >> 1) + (y & 1)) & 0x7;
                    int columnLocation = posY * widthVar * 2 + ((x + swapSelector) & 0x7) * 4;

                    int byteNum = ((y >> 1) & 1) + ((x >> 2) & 2);

                    pixelBufferVar[0 + (y * widthVar) + x] = swizzledBuffer[blockLocation + columnLocation + byteNum];
                }
            }
        }


        public static void UnSwizzlePalette(ref byte[] palBufferVar)
        {
            uint[] newPalette = new uint[256];
            uint[] origPalette = new uint[256];
            Buffer.BlockCopy(palBufferVar, 0, origPalette, 0, 1024);

            for (uint k = 0; k < 8; k++)
            {
                for (uint j = 0; j < 2; j++)
                {
                    for (uint i = 0; i < 8; i++)
                    {
                        newPalette[k * 32 + j * 16 + i] = origPalette[k * 32 + 8 * j + i];
                        newPalette[k * 32 + j * 16 + i + 8] = origPalette[k * 32 + 8 * j + 16 + i];
                    }
                }
            }

            Buffer.BlockCopy(newPalette, 0, palBufferVar, 0, palBufferVar.Length);
        }
    }
}