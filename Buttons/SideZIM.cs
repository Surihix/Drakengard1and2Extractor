using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace Drakengard1and2Extractor
{
    public partial class SideZIM : Form
    {
        private readonly string ZimFileVar;
        public SideZIM(string ZimFile)
        {
            try
            {
                this.ZimFileVar = ZimFile;

                var BppValue = 0;
                using (FileStream GetBppStream = new FileStream(ZimFile, FileMode.Open, FileAccess.Read))
                {
                    using (BinaryReader GetBppReader = new BinaryReader(GetBppStream))
                    {
                        GetBppReader.BaseStream.Position = 82;
                        BppValue = GetBppReader.ReadByte();
                    }
                }
                if (BppValue == 64)
                {
                    CoreForm.AppMsgBox("Detected 4bpp image\nDo not use the Alpha Compensation setting when saving the image " +
                        "in png format.", "Warning", MessageBoxIcon.Warning);
                }

                InitializeComponent();

                ZimSaveAsComboBox.SelectedItem = "Bitmap (.bmp)";
                ZimAlphaCompNumericUpDown.Enabled = false;
            }
            catch (Exception ex)
            {
                CoreForm.AppMsgBox(ex.Message, "Error", MessageBoxIcon.Error);
            }
        }

        private void ZimSaveAsComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            var SelectedFormat = ZimSaveAsComboBox.SelectedItem.ToString();

            if (SelectedFormat.Contains("Bitmap (.bmp)"))
            {
                ZimAlphaCompNumericUpDown.Enabled = false;
            }
            if (SelectedFormat.Contains("Portable Network Graphics (.png)"))
            {
                ZimAlphaCompNumericUpDown.Enabled = true;
            }
        }

        private void ConvertZIMImgBtn_Click(object sender, EventArgs e)
        {
            try
            {
                var SaveAsFormat = ZimSaveAsComboBox.SelectedItem.ToString();

                ZimSaveAsComboBox.Enabled = false;
                ConvertZIMImgBtn.Enabled = false;
                if (SaveAsFormat.Contains("Portable Network Graphics (.png)"))
                {
                    ZimAlphaCompNumericUpDown.Enabled = false;
                }

                var ZIMFileDir = Path.GetFullPath(ZimFileVar);
                var Extract_dir = Path.GetDirectoryName(ZIMFileDir) + "/";

                using (FileStream ZIMStream = new FileStream(ZimFileVar, FileMode.Open, FileAccess.Read))
                {
                    using (BinaryReader ZIMReader = new BinaryReader(ZIMStream))
                    {
                        ZIMReader.BaseStream.Position = 44;
                        var Width = ZIMReader.ReadUInt16();
                        var Height = ZIMReader.ReadUInt16();

                        ZIMReader.BaseStream.Position = 72;
                        var PaletteSection = ZIMReader.ReadUInt32();

                        ZIMReader.BaseStream.Position = 82;
                        var BppFlag = ZIMReader.ReadByte();

                        ZIMReader.BaseStream.Position = PaletteSection + 96;
                        var PalDimW = ZIMReader.ReadUInt32();
                        var PalDimH = ZIMReader.ReadUInt32();

                        PalDimW *= 2;
                        PalDimH *= 2;
                        var PalSize = PalDimW * PalDimH;

                        if (BppFlag == 64)
                        {
                            Width /= 2;
                        }
                        var ImageSize = Width * Height;

                        ZIMStream.Seek(352, SeekOrigin.Begin);


                        using (MemoryStream ImgStream = new MemoryStream())
                        {
                            byte[] IMG_buffer = new byte[ImageSize];
                            var IMGbytesToRead = ZIMStream.Read(IMG_buffer, 0, IMG_buffer.Length);
                            ImgStream.Write(IMG_buffer, 0, IMGbytesToRead);

                            ZIMStream.Seek(PaletteSection + 160, SeekOrigin.Begin);
                            byte[] Pal_Buffer = new byte[PalSize];
                            var PalBytesToRead = ZIMStream.Read(Pal_Buffer, 0, Pal_Buffer.Length);

                            if (BppFlag == 48)
                            {
                                uint[] newPalette = new uint[256];
                                uint[] origPalette = new uint[256];
                                Buffer.BlockCopy(Pal_Buffer, 0, origPalette, 0, Pal_Buffer.Length);
                                for (uint k = 0; k < 8; k++)
                                    for (uint j = 0; j < 2; j++)
                                        for (uint i = 0; i < 8; i++)
                                        {
                                            newPalette[k * 32 + j * 16 + i] = origPalette[k * 32 + 8 * j + i];
                                            newPalette[k * 32 + j * 16 + i + 8] = origPalette[k * 32 + 8 * j + 16 + i];
                                        }
                                Buffer.BlockCopy(newPalette, 0, Pal_Buffer, 0, Pal_Buffer.Length);
                            }


                            using (MemoryStream ProcessedPalette = new MemoryStream())
                            {
                                ProcessedPalette.Write(Pal_Buffer, 0, Pal_Buffer.Length);

                                using (BinaryReader ColorReader = new BinaryReader(ProcessedPalette))
                                {
                                    using (BinaryReader ImgReader = new BinaryReader(ImgStream))
                                    {
                                        var CurrentPixel = 0;
                                        var GetColorIndex = 0;
                                        var Pixel1 = 0;
                                        var Pixel2 = 0;
                                        var red = 0;
                                        var green = 0;
                                        var blue = 0;
                                        var alpha = 0;
                                        var color = new Color();
                                        var IncreaseAlphaVal = (int)ZimAlphaCompNumericUpDown.Value;
                                        Bitmap FinalImage = new Bitmap(Width, Height);
                                        switch (BppFlag)
                                        {
                                            case 48:
                                                for (int y = 0; y < Height; y++)
                                                    for (int x = 0; x < Width; x++)
                                                    {
                                                        CurrentPixel = (y * Width) + x;
                                                        ImgReader.BaseStream.Position = CurrentPixel;
                                                        GetColorIndex = ImgReader.ReadByte();

                                                        var ColorIndex = GetColorIndex * 4;

                                                        ColorReader.BaseStream.Position = ColorIndex;
                                                        red = ColorReader.ReadByte();
                                                        green = ColorReader.ReadByte();
                                                        blue = ColorReader.ReadByte();
                                                        alpha = ColorReader.ReadByte();

                                                        switch (SaveAsFormat)
                                                        {
                                                            case "Portable Network Graphics (.png)":

                                                                alpha += IncreaseAlphaVal;

                                                                if (IncreaseAlphaVal > 0)
                                                                {
                                                                    if (alpha > 255)
                                                                    {
                                                                        alpha = 255;
                                                                    }
                                                                }

                                                                color = Color.FromArgb(alpha, red, green, blue);
                                                                FinalImage.SetPixel(x, y, color);
                                                                break;

                                                            case "Bitmap (.bmp)":

                                                                color = Color.FromArgb(red, green, blue);
                                                                FinalImage.SetPixel(x, y, color);
                                                                break;
                                                        }
                                                    }
                                                if (SaveAsFormat.Contains("Portable Network Graphics (.png)"))
                                                {
                                                    FinalImage.Save(Extract_dir + Path.GetFileNameWithoutExtension(ZimFileVar) +
                                                        ".png", ImageFormat.Png);
                                                }
                                                else
                                                {
                                                    FinalImage.Save(Extract_dir + Path.GetFileNameWithoutExtension(ZimFileVar) +
                                                        ".bmp", ImageFormat.Bmp);
                                                }
                                                break;


                                            case 64:
                                                for (int y = 0; y < Height; y++)
                                                    for (int x = 0; x < Width; x++)
                                                    {
                                                        CurrentPixel = (y * Width) + x;
                                                        ImgReader.BaseStream.Position = CurrentPixel;
                                                        GetColorIndex = ImgReader.ReadByte();

                                                        Pixel1 = GetColorIndex >> 4;
                                                        Pixel2 = GetColorIndex & 0xF;

                                                        Pixel1 *= 4;
                                                        Pixel2 *= 4;

                                                        ColorReader.BaseStream.Position = Pixel1;
                                                        red = ColorReader.ReadByte();
                                                        green = ColorReader.ReadByte();
                                                        blue = ColorReader.ReadByte();
                                                        alpha = ColorReader.ReadByte();

                                                        switch (SaveAsFormat)
                                                        {
                                                            case "Portable Network Graphics (.png)":

                                                                alpha += IncreaseAlphaVal;

                                                                if (IncreaseAlphaVal > 0)
                                                                {
                                                                    if (alpha > 255)
                                                                    {
                                                                        alpha = 255;
                                                                    }
                                                                }

                                                                color = Color.FromArgb(alpha, red, green, blue);
                                                                FinalImage.SetPixel(x, y, color);

                                                                CurrentPixel += 1;
                                                                ImgReader.BaseStream.Position = CurrentPixel;

                                                                ColorReader.BaseStream.Position = Pixel2;
                                                                red = ColorReader.ReadByte();
                                                                green = ColorReader.ReadByte();
                                                                blue = ColorReader.ReadByte();
                                                                alpha = ColorReader.ReadByte();

                                                                color = Color.FromArgb(alpha, red, green, blue);
                                                                FinalImage.SetPixel(x, y, color);
                                                                break;

                                                            case "Bitmap (.bmp)":

                                                                color = Color.FromArgb(red, green, blue);
                                                                FinalImage.SetPixel(x, y, color);

                                                                CurrentPixel += 1;
                                                                ImgReader.BaseStream.Position = CurrentPixel;

                                                                ColorReader.BaseStream.Position = Pixel2;
                                                                red = ColorReader.ReadByte();
                                                                green = ColorReader.ReadByte();
                                                                blue = ColorReader.ReadByte();

                                                                color = Color.FromArgb(red, green, blue);
                                                                FinalImage.SetPixel(x, y, color);
                                                                break;
                                                        }
                                                    }
                                                if (SaveAsFormat.Contains("Portable Network Graphics (.png)"))
                                                {
                                                    FinalImage.Save(Extract_dir + Path.GetFileNameWithoutExtension(ZimFileVar) +
                                                        ".png", ImageFormat.Png);
                                                }
                                                else
                                                {
                                                    FinalImage.Save(Extract_dir + Path.GetFileNameWithoutExtension(ZimFileVar) +
                                                        ".bmp", ImageFormat.Bmp);
                                                }
                                                break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                CoreForm.AppMsgBox("Converted " + Path.GetFileName(ZimFileVar) + " file", "Success", MessageBoxIcon.Information);

                ZimSaveAsComboBox.Enabled = true;
                ConvertZIMImgBtn.Enabled = true;

                if (SaveAsFormat.Contains("Portable Network Graphics (.png)"))
                {
                    ZimAlphaCompNumericUpDown.Enabled = true;
                }

            }
            catch (Exception ex)
            {
                CoreForm.AppMsgBox(ex.Message, "Error", MessageBoxIcon.Error);
                Close();
            }
        }
    }
}