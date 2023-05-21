using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace Drakengard1and2Extractor.AppClasses
{
    public partial class FileSPK0 : Form
    {
        private readonly string Spk0FileVar;
        public FileSPK0(string Spk0File)
        {
            try
            {
                Spk0FileVar = Spk0File;

                InitializeComponent();

                Spk0SaveAsComboBox.SelectedItem = "Bitmap (.bmp)";
                Spk0AlphaCompNumericUpDown.Enabled = false;
            }
            catch (Exception ex)
            {
                CmnMethods.AppMsgBox(ex.Message, "Error", MessageBoxIcon.Error);
            }
        }


        private void Spk0SaveAsComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            var SelectedFormat = Spk0SaveAsComboBox.SelectedItem.ToString();

            if (SelectedFormat.Contains("Bitmap (.bmp)"))
            {
                Spk0AlphaCompNumericUpDown.Enabled = false;
            }
            if (SelectedFormat.Contains("Portable Network Graphics (.png)"))
            {
                Spk0AlphaCompNumericUpDown.Enabled = true;
            }
        }


        private void ConvertSPK0ImgBtn_Click(object sender, EventArgs e)
        {
            try
            {
                ConvertSPK0ImgBtn.Text = "Converting...";
                var SaveAsFormat = Spk0SaveAsComboBox.SelectedItem.ToString();

                Spk0SaveAsComboBox.Enabled = false;
                ConvertSPK0ImgBtn.Enabled = false;

                if (SaveAsFormat.Contains("Portable Network Graphics (.png)"))
                {
                    Spk0AlphaCompNumericUpDown.Enabled = false;
                }

                var SPK0FileDir = Path.GetFullPath(Spk0FileVar);
                var ExtractDir = Path.GetDirectoryName(SPK0FileDir) + "/" + Path.GetFileName(Spk0FileVar) + "_extracted";

                if (Directory.Exists(ExtractDir))
                {
                    Directory.Delete(ExtractDir, true);
                }
                Directory.CreateDirectory(ExtractDir);


                using (FileStream SPKOStream = new FileStream(Spk0FileVar, FileMode.Open, FileAccess.Read))
                {
                    using (BinaryReader SPK0Reader = new BinaryReader(SPKOStream))
                    {
                        SPK0Reader.BaseStream.Position = 24;
                        var DLS0SubChunkPos = SPK0Reader.ReadUInt32();

                        SPK0Reader.BaseStream.Position = 40;
                        var DMT0Size = SPK0Reader.ReadUInt32();
                        var GRF1SubChunkPos = DMT0Size + 32;

                        SPK0Reader.BaseStream.Position = DLS0SubChunkPos;
                        var GRF1EndPos = SPK0Reader.BaseStream.Position;
                        var GRF1Size = GRF1EndPos - DMT0Size - 32;
                        var DLS0Size = SPKOStream.Length - GRF1EndPos;


                        using (FileStream DMT0Stream = new FileStream(ExtractDir + "/" + "DMT0", FileMode.OpenOrCreate,
                            FileAccess.Write))
                        {
                            SPKOStream.Seek(32, SeekOrigin.Begin);
                            byte[] DMT0Buffer = new byte[DMT0Size];
                            var DMT0DataRead = SPKOStream.Read(DMT0Buffer, 0, DMT0Buffer.Length);
                            DMT0Stream.Write(DMT0Buffer, 0, DMT0DataRead);
                        }


                        using (FileStream GRF1Stream = new FileStream(ExtractDir + "/" + "GRF1", FileMode.OpenOrCreate,
                            FileAccess.ReadWrite))
                        {
                            SPKOStream.Seek(GRF1SubChunkPos, SeekOrigin.Begin);
                            byte[] GRF1Buffer = new byte[GRF1Size];
                            var GRF1DataRead = SPKOStream.Read(GRF1Buffer, 0, GRF1Buffer.Length);
                            GRF1Stream.Write(GRF1Buffer, 0, GRF1DataRead);

                            using (BinaryReader GRF1Reader = new BinaryReader(GRF1Stream))
                            {
                                GRF1Reader.BaseStream.Position = 4;
                                var TotalImgCount = GRF1Reader.ReadUInt32();

                                GRF1Reader.BaseStream.Position = 24;
                                var ImgPalPos = GRF1Reader.ReadUInt32();

                                GRF1Reader.BaseStream.Position = ImgPalPos + 96;
                                var PalX = GRF1Reader.ReadUInt32();
                                var PalY = GRF1Reader.ReadUInt32();
                                var PalSize = (PalX * 2) * (PalY * 2);

                                GRF1Stream.Seek(ImgPalPos + 144, SeekOrigin.Begin);
                                byte[] PalBuffer = new byte[PalSize];
                                GRF1Stream.Read(PalBuffer, 0, PalBuffer.Length);

                                CmnMethods.MugiSwizzle(ref PalBuffer);

                                using (MemoryStream ProcessedPalette = new MemoryStream())
                                {
                                    ProcessedPalette.Write(PalBuffer, 0, PalBuffer.Length);

                                    using (BinaryReader ColorReader = new BinaryReader(ProcessedPalette))
                                    {

                                        var ReadImgPosStart = 20;
                                        var ImgCount = 1;
                                        for (int im = 0; im < TotalImgCount; im++)
                                        {
                                            GRF1Reader.BaseStream.Position = ReadImgPosStart;
                                            var ImgStartPos = GRF1Reader.ReadUInt32();

                                            GRF1Reader.BaseStream.Position = ImgStartPos + 96;
                                            var Width = GRF1Reader.ReadUInt32();
                                            var Height = GRF1Reader.ReadUInt32();
                                            var ImgSize = Width * Height;

                                            using (MemoryStream ImgStream = new MemoryStream())
                                            {
                                                GRF1Stream.Seek(ImgStartPos + 144, SeekOrigin.Begin);
                                                byte[] ImgBuffer = new byte[ImgSize];
                                                var ImgDataRead = GRF1Stream.Read(ImgBuffer, 0, ImgBuffer.Length);
                                                ImgStream.Write(ImgBuffer, 0, ImgDataRead);

                                                using (BinaryReader ImgReader = new BinaryReader(ImgStream))
                                                {


                                                    var CurrentPixel = 0;
                                                    var GetColorIndex = 0;
                                                    var red = 0;
                                                    var green = 0;
                                                    var blue = 0;
                                                    var alpha = 0;
                                                    var color = new Color();
                                                    var IncreaseAlphaVal = (int)Spk0AlphaCompNumericUpDown.Value;
                                                    using (Bitmap FinalImage = new Bitmap((int)Width, (int)Height))
                                                    {
                                                        for (int y = 0; y < Height; y++)
                                                            for (int x = 0; x < Width; x++)
                                                            {
                                                                CurrentPixel = (y * (int)Width) + x;
                                                                ImgReader.BaseStream.Position = CurrentPixel;
                                                                GetColorIndex = ImgReader.ReadByte();

                                                                var ColorIndex = GetColorIndex * 4;

                                                                ColorReader.BaseStream.Position = ColorIndex;
                                                                red = ColorReader.ReadByte();
                                                                green = ColorReader.ReadByte();
                                                                blue = ColorReader.ReadByte();
                                                                alpha = ColorReader.ReadByte();

                                                                if (SaveAsFormat.Contains("Portable Network Graphics (.png)"))
                                                                {
                                                                    alpha += IncreaseAlphaVal;

                                                                    if (IncreaseAlphaVal > 0)
                                                                    {
                                                                        if (alpha > 128)
                                                                        {
                                                                            alpha = 128;
                                                                        }
                                                                    }
                                                                }

                                                                color = Color.FromArgb(alpha, red, green, blue);
                                                                FinalImage.SetPixel(x, y, color);
                                                            }
                                                        switch (SaveAsFormat)
                                                        {
                                                            case "Bitmap (.bmp)":
                                                                FinalImage.Save(ExtractDir + "/" + "GRF1_img_" + ImgCount + 
                                                                    ".bmp", ImageFormat.Bmp);
                                                                break;

                                                            case "Portable Network Graphics (.png)":
                                                                FinalImage.Save(ExtractDir + "/" + "GRF1_img_" + ImgCount + 
                                                                    ".png", ImageFormat.Png);
                                                                break;
                                                        }
                                                    }
                                                }
                                            }
                                            ImgCount++;
                                            ReadImgPosStart += 32;
                                        }
                                    }
                                }
                            }
                        }


                        using (FileStream DLS0Stream = new FileStream(ExtractDir + "/" + "DLS0", FileMode.OpenOrCreate,
                            FileAccess.Write))
                        {
                            SPKOStream.Seek(DLS0SubChunkPos, SeekOrigin.Begin);
                            byte[] DLS0Buffer = new byte[DLS0Size];
                            var DLS0DataRead = SPKOStream.Read(DLS0Buffer, 0, DLS0Buffer.Length);
                            DLS0Stream.Write(DLS0Buffer, 0, DLS0DataRead);
                        }
                    }
                }

                CmnMethods.AppMsgBox("Converted " + Path.GetFileName(Spk0FileVar) + " file", "Success", MessageBoxIcon.Information);

                Spk0SaveAsComboBox.Enabled = true;
                ConvertSPK0ImgBtn.Enabled = true;

                if (SaveAsFormat.Contains("Portable Network Graphics (.png)"))
                {
                    Spk0AlphaCompNumericUpDown.Enabled = true;
                }

                ConvertSPK0ImgBtn.Text = "Convert";

            }
            catch (Exception ex)
            {
                CmnMethods.AppMsgBox(ex.Message, "Error", MessageBoxIcon.Error);
            }
        }
    }
}