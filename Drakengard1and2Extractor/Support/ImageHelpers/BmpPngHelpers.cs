using System.Drawing;

internal static class BmpPngHelpers
{
    public static void CreateBmpPng(this byte[] pixelData, byte[] paletteData, ImgOptions imgOptions, string outImgPath)
    {
        using (Bitmap finalImg = new Bitmap(imgOptions.Width, imgOptions.Height))
        {
            for (int y = 0; y < imgOptions.Height; y++)
            {
                for (int x = 0; x < imgOptions.Width; x++)
                {
                    var currentPixel = (y * imgOptions.Width) + x;
                    int palettePos = pixelData[currentPixel];
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

                    var pixelColor = Color.FromArgb(alpha, red, green, blue);
                    finalImg.SetPixel(x, y, pixelColor);
                }
            }

            finalImg.Save(outImgPath, imgOptions.ImageFormat);
        }
    }
}