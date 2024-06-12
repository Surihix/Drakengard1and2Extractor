public class ConverterWindow
{
    public static bool Is4BppImg { get; set; }
    public static int SaveAsIndex { get; set; }
    public static bool UnswizzlePixels { get; set; }
    public static int AlphaIncrease { get; set; }
    public static bool IsClosedByConvtBtn { get; set; }
}

public class ImgOptions
{
    public int Height { get; set; }
    public int Width { get; set; }
    public int AlphaIncrease { get; set; }
    public System.Drawing.Imaging.ImageFormat ImageFormat { get; set; }
}