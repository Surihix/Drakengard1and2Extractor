using MiniLz0;
using System;
using System.IO;
using System.Windows.Forms;

namespace Drakengard1and2Extractor.AppClasses
{
    public class Drk1BIN
    {
        public static void ExtractBin(string MainBinFile)
        {
            var Extract_dir = Path.GetFullPath(MainBinFile) + "_extracted";
            CmnMethods.FileDirectoryExistsDel(Extract_dir, CmnMethods.DelSwitch.folder);
            Directory.CreateDirectory(Extract_dir);

            using (FileStream MainBIN = new FileStream(MainBinFile, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader MainBINReader = new BinaryReader(MainBIN))
                {
                    MainBINReader.BaseStream.Position = 8;
                    var Entries = MainBINReader.ReadUInt32();

                    MainBINReader.BaseStream.Position = 40;
                    var BinDataStart = MainBINReader.ReadUInt32();

                    MainBINReader.BaseStream.Position = 64;
                    var MainBinNameChars = MainBINReader.ReadChars(12);
                    string MainBinName = string.Join("", MainBinNameChars).Replace("\0", "").Replace("/", "").
                        Replace("|", "").Replace("?", "").Replace(":", "").Replace("<", "").Replace(">", "").
                        Replace("*", "").Replace("\\", "");

                    CmnMethods.FileDirectoryExistsDel(Extract_dir + "/" + MainBinName, CmnMethods.DelSwitch.file);

                    using (FileStream BIN = new FileStream(Extract_dir + "/" + MainBinName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                    {
                        MainBIN.Seek(BinDataStart, SeekOrigin.Begin);
                        MainBIN.CopyTo(BIN);


                        uint IntialOffsetPos = 132;
                        var fname = "FILE_";
                        var RExt = "";
                        int FileCount = 1;
                        for (int f = 0; f < Entries; f++)
                        {
                            MainBINReader.BaseStream.Position = IntialOffsetPos;
                            var fileStart = MainBINReader.ReadUInt32();
                            MainBINReader.BaseStream.Position = IntialOffsetPos + 4;
                            var fileSize = MainBINReader.ReadUInt32();

                            MainBINReader.BaseStream.Position = IntialOffsetPos + 8;
                            var extnChar = MainBINReader.ReadChars(4);
                            Array.Reverse(extnChar);
                            string fileExt = string.Join("", extnChar).Replace("\0", "").Replace("/", "").Replace("|", "").
                                Replace("?", "").Replace(":", "").Replace("<", "").Replace(">", "").Replace("*", "").
                                Replace("\\", "").Replace("0eng", "0eng.fpk").Replace("0jpn", "0jpn.fpk").
                                Replace("1uk", "1uk.fpk").Replace("2fre", "2fre.fpk").Replace("3ger", "3ger.fpk").
                                Replace("4ita", "4ita.fpk").Replace("5spa", "5spa.fpk");
                            string fExt = "." + fileExt;

                            if (MainBinFile.Contains("image.bin") || MainBinFile.Contains("IMAGE.BIN"))
                            {
                                string AdjExt = "";
                                fExt = AdjExt;
                            }

                            using (FileStream SplitFile = new FileStream(Extract_dir + "/" + fname + $"{FileCount}" + fExt, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                            {
                                BIN.Seek(fileStart, SeekOrigin.Begin);
                                byte[] SplitFilebuffer = new byte[0];
                                SplitFilebuffer = new byte[fileSize];
                                var SplitFileBytesToRead = BIN.Read(SplitFilebuffer, 0, SplitFilebuffer.Length);
                                SplitFile.Write(SplitFilebuffer, 0, SplitFileBytesToRead);

                                if (MainBinFile.Contains("image.bin") || MainBinFile.Contains("IMAGE.BIN"))
                                {
                                    using (BinaryReader SplitFileReader = new BinaryReader(SplitFile))
                                    {
                                        GetFileHeader(SplitFileReader, ref RExt);
                                    }
                                }
                            }

                            if (MainBinFile.Contains("image.bin") || MainBinFile.Contains("IMAGE.BIN"))
                            {
                                File.Move(Extract_dir + "/" + fname + $"{FileCount}" + fExt, Extract_dir + "/" + fname + $"{FileCount}" + fExt + RExt);

                                if (RExt.Contains(".lz0"))
                                {
                                    var CurrentLz0File = Extract_dir + "/" + fname + $"{FileCount}" + fExt + RExt;
                                    var CurrentOutDecmpLz0File = Extract_dir + "/" + Path.GetFileNameWithoutExtension(Extract_dir + "/" + fname + $"{FileCount}" + fExt + RExt);
                                    
                                    using (FileStream Lz0Stream = new FileStream(CurrentLz0File, FileMode.Open, FileAccess.Read))
                                    {
                                        using (BinaryReader Lz0Reader = new BinaryReader(Lz0Stream))
                                        {
                                            Lz0Reader.BaseStream.Position = 24;
                                            var Lz0Chunks = Lz0Reader.ReadUInt32();


                                            uint Lz0DataReadStart = 32;
                                            for (int lz0 = 0; lz0 < Lz0Chunks; lz0++)
                                            {
                                                Lz0Reader.BaseStream.Position = Lz0DataReadStart + 4;
                                                var CmpChunkSize = Lz0Reader.ReadUInt32();

                                                Lz0Reader.BaseStream.Position = Lz0DataReadStart + 8;
                                                var UncmpChunkSize = Lz0Reader.ReadUInt32();

                                                byte[] DeCompressedData = new byte[0];
                                                DeCompressedData = new byte[UncmpChunkSize];
                                                using (MemoryStream Lz0DataHolder = new MemoryStream())
                                                {
                                                    Lz0Stream.Seek(Lz0DataReadStart + 12, SeekOrigin.Begin);
                                                    byte[] CmpBuffer = new byte[0];
                                                    CmpBuffer = new byte[CmpChunkSize];
                                                    var CmpBytesToRead = Lz0Stream.Read(CmpBuffer, 0, CmpBuffer.Length);
                                                    Lz0DataHolder.Write(CmpBuffer, 0, CmpBytesToRead);


                                                    byte[] CompressedData = Lz0DataHolder.ToArray();
                                                    MiniLz0Lib.Decompress(ref CompressedData, UncmpChunkSize, ref DeCompressedData);

                                                    using (FileStream DecompressedFileStream = new FileStream(CurrentOutDecmpLz0File, FileMode.Append, FileAccess.Write))
                                                    {
                                                        DecompressedFileStream.Write(DeCompressedData, 0, DeCompressedData.Length);
                                                    }
                                                }

                                                var SeekLength = CmpChunkSize;
                                                Lz0Reader.BaseStream.Seek(Lz0DataReadStart + 12, SeekOrigin.Begin);
                                                Lz0Reader.BaseStream.Seek(SeekLength, SeekOrigin.Current);

                                                var NextSeekVal = Lz0Reader.BaseStream;
                                                var NextSeek = (uint)NextSeekVal.Position;

                                                uint newLz0DataReadStart = NextSeek;
                                                Lz0DataReadStart = newLz0DataReadStart;
                                            }
                                        }
                                    }

                                    File.Delete(CurrentLz0File);

                                    using (FileStream Dcmplz0File = new FileStream(CurrentOutDecmpLz0File, FileMode.Open,
                                        FileAccess.Read))
                                    {
                                        using (BinaryReader DcmpFileReader = new BinaryReader(Dcmplz0File))
                                        {
                                            GetFileHeader(DcmpFileReader, ref RExt);
                                        }
                                    }

                                    File.Move(CurrentOutDecmpLz0File, CurrentOutDecmpLz0File + RExt);
                                }

                                RExt = "";
                            }

                            IntialOffsetPos += 16;
                            FileCount++;
                        }
                    }

                    File.Delete(Extract_dir + "/" + MainBinName);
                }
            }

            CmnMethods.AppMsgBox("Extracted " + Path.GetFileName(MainBinFile) + " file", "Success", MessageBoxIcon.Information);
        }


        private static void GetFileHeader(BinaryReader ReaderName, ref string RExtVar)
        {
            ReaderName.BaseStream.Position = 0;
            var FoundExt = ReaderName.ReadChars(4);
            string RealExt = string.Join("", FoundExt).Replace("\0", "");

            switch (RealExt)
            {
                case "fpk":
                    RExtVar = ".fpk";
                    break;
                case "wZIM":
                    RExtVar = ".zim";
                    break;
                case "V3a":
                    RExtVar = ".lz0";
                    break;
                case "KPS_":
                    RExtVar = ".kps";
                    break;
                case "SPK0":
                    RExtVar = ".spk0";
                    break;
            }
        }
    }
}