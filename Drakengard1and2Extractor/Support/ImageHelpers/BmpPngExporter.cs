using System.Drawing;

namespace Drakengard1and2Extractor.Support.ImageHelpers
{
    internal class BmpPngExporter
    {
        public static void CreateBmpPng(byte[] pixelData, byte[] paletteData, string outImgPath)
        {
            using (Bitmap finalImg = new Bitmap(ImgOptions.Width, ImgOptions.Height))
            {
                for (int y = 0; y < ImgOptions.Height; y++)
                {
                    for (int x = 0; x < ImgOptions.Width; x++)
                    {
                        var currentPixel = (y * ImgOptions.Width) + x;
                        int palettePos = pixelData[currentPixel];
                        palettePos *= 4;

                        byte red = paletteData[palettePos];
                        byte green = paletteData[palettePos + 1];
                        byte blue = paletteData[palettePos + 2];
                        int alpha = paletteData[palettePos + 3];

                        alpha += ImgOptions.AlphaIncrease;
                        if (alpha > 255)
                        {
                            alpha = 255;
                        }

                        var pixelColor = Color.FromArgb(alpha, red, green, blue);
                        finalImg.SetPixel(x, y, pixelColor);
                    }
                }

                finalImg.Save(outImgPath, ImgOptions.ImageFormat);
            }
        }
    }
}