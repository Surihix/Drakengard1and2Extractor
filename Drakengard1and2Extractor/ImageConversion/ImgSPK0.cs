using Drakengard1and2Extractor.Support;
using Drakengard1and2Extractor.Support.ImageHelpers;
using System.IO;
using System.Windows.Forms;

namespace Drakengard1and2Extractor.ImageConversion
{
    internal class ImgSPK0
    {
        public static void ConvertSPK0(string spk0File, bool isSingleFile)
        {
            var extractDir = Path.Combine(Path.GetDirectoryName(spk0File), Path.GetFileName(spk0File) + "_extracted");

            CommonMethods.IfFileDirExistsDel(extractDir, CommonMethods.DelSwitch.directory);
            Directory.CreateDirectory(extractDir);

            using (FileStream spk0Stream = new FileStream(spk0File, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader spk0Reader = new BinaryReader(spk0Stream))
                {
                    spk0Reader.BaseStream.Position = 24;
                    var dls0SubChunkPos = spk0Reader.ReadUInt32();

                    spk0Reader.BaseStream.Position = 40;
                    var dmt0Size = spk0Reader.ReadUInt32();
                    var grf1SubChunkPos = dmt0Size + 32;

                    spk0Reader.BaseStream.Position = dls0SubChunkPos;
                    var grf1EndPos = spk0Reader.BaseStream.Position;
                    var grf1Size = grf1EndPos - dmt0Size - 32;
                    var dls0Size = spk0Stream.Length - grf1EndPos;


                    using (FileStream dmt0Stream = new FileStream(Path.Combine(extractDir, "DMT0"), FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        spk0Stream.Seek(32, SeekOrigin.Begin);
                        spk0Stream.CopyStreamTo(dmt0Stream, dmt0Size, false);
                    }


                    using (FileStream grf1Stream = new FileStream(Path.Combine(extractDir, "GRF1"), FileMode.OpenOrCreate, FileAccess.ReadWrite))
                    {
                        spk0Stream.Seek(grf1SubChunkPos, SeekOrigin.Begin);
                        spk0Stream.CopyStreamTo(grf1Stream, grf1Size, false);

                        using (BinaryReader grf1Reader = new BinaryReader(grf1Stream))
                        {
                            grf1Reader.BaseStream.Position = 4;
                            var totalImgCount = grf1Reader.ReadUInt32();

                            grf1Reader.BaseStream.Position = 24;
                            var imgPalPos = grf1Reader.ReadUInt32();

                            grf1Reader.BaseStream.Position = imgPalPos + 96;
                            var palDim1 = grf1Reader.ReadUInt32();
                            var palDim2 = grf1Reader.ReadUInt32();
                            var palSize = (palDim1 * 2) * (palDim2 * 2);

                            grf1Stream.Seek(imgPalPos + 144, SeekOrigin.Begin);
                            byte[] palBuffer = new byte[palSize];
                            grf1Stream.Read(palBuffer, 0, palBuffer.Length);

                            byte[] finalizedPalBuffer = PS2Unswizzlers.UnswizzlePalette(palBuffer);


                            uint imgReadPosStart = 20;
                            var imgFCount = 1;
                            for (int i = 0; i < totalImgCount; i++)
                            {
                                grf1Reader.BaseStream.Position = imgReadPosStart;
                                var imgStartPos = grf1Reader.ReadUInt32();

                                grf1Reader.BaseStream.Position = imgStartPos + 96;
                                ImgOptions.Width = (int)grf1Reader.ReadUInt32();
                                ImgOptions.Height = (int)grf1Reader.ReadUInt32();
                                var imgSize = ImgOptions.Width * ImgOptions.Height;

                                grf1Stream.Seek(imgStartPos + 144, SeekOrigin.Begin);
                                var pixelsBuffer = new byte[imgSize];
                                _ = grf1Stream.Read(pixelsBuffer, 0, imgSize);


                                string outImgPath = string.Empty;
                                switch (ImgOptions.SaveAsIndex)
                                {
                                    case 0:
                                        outImgPath = Path.Combine(extractDir, "GRF1_img_" + imgFCount + ".bmp");
                                        ImgOptions.ImageFormat = System.Drawing.Imaging.ImageFormat.Bmp;

                                        BmpPngExporter.CreateBmpPng(pixelsBuffer, palBuffer, outImgPath);
                                        break;

                                    case 1:
                                        outImgPath = Path.Combine(extractDir, "GRF1_img_" + imgFCount + ".dds");
                                        DDSimgExporter.CreateDDS(pixelsBuffer, palBuffer, outImgPath);
                                        break;

                                    case 2:
                                        outImgPath = Path.Combine(extractDir, "GRF1_img_" + imgFCount + ".png");
                                        ImgOptions.ImageFormat = System.Drawing.Imaging.ImageFormat.Png;

                                        BmpPngExporter.CreateBmpPng(pixelsBuffer, palBuffer, outImgPath);
                                        break;
                                }

                                if (isSingleFile)
                                {
                                    LoggingMethods.LogMessage($"Converted {Path.GetFileName(outImgPath)}");
                                }

                                imgFCount++;
                                imgReadPosStart += 32;
                            }
                        }
                    }


                    using (FileStream dls0Stream = new FileStream(Path.Combine(extractDir, "DLS0"), FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        spk0Stream.Seek(dls0SubChunkPos, SeekOrigin.Begin);
                        spk0Stream.CopyStreamTo(dls0Stream, dls0Size, false);
                    }
                }
            }

            if (isSingleFile)
            {
                LoggingMethods.LogMessage(CommonMethods.NewLineChara);
                LoggingMethods.LogMessage("Conversion has completed!");
                LoggingMethods.LogMessage(CommonMethods.NewLineChara);

                CommonMethods.AppMsgBox("Converted " + Path.GetFileName(spk0File) + " file", "Success", MessageBoxIcon.Information);
            }
        }
    }
}