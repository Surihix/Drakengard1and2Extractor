using MiniLz0;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Drakengard1and2Extractor
{
    public class SideFPK
    {
        public static void ExtractFPK(string FpkFile)
        {
            try
            {
                var Extract_dir = Path.GetFullPath(FpkFile) + "_extracted";
                if (Directory.Exists(Extract_dir))
                {
                    Directory.Delete(Extract_dir, true);
                }
                Directory.CreateDirectory(Extract_dir);

                using (FileStream Fpk = new FileStream(FpkFile, FileMode.Open, FileAccess.Read))
                {
                    using (BinaryReader FpkReader = new BinaryReader(Fpk))
                    {
                        FpkReader.BaseStream.Position = 8;
                        var Entries = FpkReader.ReadUInt32();

                        FpkReader.BaseStream.Position = 40;
                        var SubBinDataStart = FpkReader.ReadUInt32();

                        FpkReader.BaseStream.Position = 44;
                        var SubBinSize = FpkReader.ReadUInt32();

                        FpkReader.BaseStream.Position = 64;
                        StringBuilder SubBinName = new StringBuilder();
                        char BinNameChars;
                        while ((BinNameChars = FpkReader.ReadChar()) != default)
                        {
                            SubBinName.Append(BinNameChars);
                        }
                        SubBinName.ToString();

                        if (File.Exists(Extract_dir + "/" + SubBinName))
                        {
                            File.Delete(Extract_dir + "/" + SubBinName);
                        }
                        SubBinName.Replace("\0", "").Replace("/", "").Replace("|", "").Replace("?", "").Replace(":", "").
                            Replace("<", "").Replace(">", "").Replace("*", "").Replace("\\", "");

                        using (FileStream SubBIN = new FileStream(Extract_dir + "/" + SubBinName, FileMode.OpenOrCreate,
                            FileAccess.ReadWrite))
                        {
                            Fpk.Seek(SubBinDataStart, SeekOrigin.Begin);
                            byte[] BinBuffer = new byte[SubBinSize];
                            var BytesToRead = Fpk.Read(BinBuffer, 0, BinBuffer.Length);
                            SubBIN.Write(BinBuffer, 0, BytesToRead);


                            uint IntialOffsetPos = 132;
                            var fname = "FILE_";
                            var RExt = "";
                            int FileCount = 1;
                            for (int f = 0; f < Entries; f++)
                            {
                                FpkReader.BaseStream.Position = IntialOffsetPos;
                                var fileStart = FpkReader.ReadUInt32();

                                FpkReader.BaseStream.Position = IntialOffsetPos + 4;
                                var fileSize = FpkReader.ReadUInt32();

                                FpkReader.BaseStream.Position = IntialOffsetPos + 8;
                                var extnChar = FpkReader.ReadChars(4);
                                Array.Reverse(extnChar);
                                string fileExt = string.Join("", extnChar).Replace("\0", "").Replace("/", "").Replace("|", "").
                                    Replace("?", "").Replace(":", "").Replace("<", "").Replace(">", "").Replace("*", "").
                                    Replace("\\", "");
                                string fExt = "." + fileExt;

                                using (FileStream SplitFile = new FileStream(Extract_dir + "/" + fname + $"{FileCount}" + fExt,
                                    FileMode.OpenOrCreate, FileAccess.ReadWrite))
                                {
                                    SubBIN.Seek(fileStart, SeekOrigin.Begin);
                                    byte[] SplitFilebuffer = new byte[fileSize];
                                    var SplitFileBytesToRead = SubBIN.Read(SplitFilebuffer, 0, SplitFilebuffer.Length);
                                    SplitFile.Write(SplitFilebuffer, 0, SplitFileBytesToRead);

                                    using (BinaryReader SplitFileReader = new BinaryReader(SplitFile))
                                    {
                                        GetFileHeader(SplitFileReader, ref RExt);
                                    }
                                }

                                File.Move(Extract_dir + "/" + fname + $"{FileCount}" + fExt,
                                    Extract_dir + "/" + fname + $"{FileCount}" + fExt + RExt);

                                string[] KnownExtArray = { ".fpk", ".dpk", ".zim", ".lz0", ".kps", ".kvm", ".spk0", ".emt", ".dcmr",
                                ".dlgt", ".hi4"};

                                if (KnownExtArray.Contains(RExt))
                                {
                                    if (!RExt.Contains(".lz0"))
                                    {
                                        var CurrentFileNameNoExtn =
                                            Path.GetFileNameWithoutExtension(Extract_dir + "/" + fname + $"{FileCount}" + fExt + RExt);
                                        File.Move(Extract_dir + "/" + fname + $"{FileCount}" + fExt + RExt, Extract_dir + "/" +
                                            CurrentFileNameNoExtn);

                                        var ToRenameFileWithProperExt = Path.GetFileNameWithoutExtension(Extract_dir + "/" +
                                            CurrentFileNameNoExtn);
                                        var AdjExt = "";
                                        using (FileStream ExtnStream = new FileStream(Extract_dir + "/" + CurrentFileNameNoExtn,
                                            FileMode.Open, FileAccess.Read))
                                        {
                                            using (BinaryReader ExtnReader = new BinaryReader(ExtnStream))
                                            {
                                                GetFileHeader(ExtnReader, ref AdjExt);
                                            }
                                        }

                                        File.Move(Extract_dir + "/" + CurrentFileNameNoExtn,
                                            Extract_dir + "/" + ToRenameFileWithProperExt + AdjExt);
                                    }
                                    else
                                    {
                                        var CurrentLz0File = Extract_dir + "/" + fname + $"{FileCount}" + fExt + RExt;
                                        var CurrentOutDecmpLz0File = Extract_dir + "/" +
                                            Path.GetFileNameWithoutExtension(Extract_dir + "/" + fname + $"{FileCount}" + fExt + RExt);
                                        var CurrentProperFileExtn = Extract_dir + "/" + fname + $"{FileCount}";

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

                                                    byte[] DeCompressedData = new byte[UncmpChunkSize];
                                                    using (MemoryStream Lz0DataHolder = new MemoryStream())
                                                    {
                                                        Lz0Stream.Seek(Lz0DataReadStart + 12, SeekOrigin.Begin);
                                                        byte[] CmpBuffer = new byte[CmpChunkSize];
                                                        var CmpBytesToRead = Lz0Stream.Read(CmpBuffer, 0, CmpBuffer.Length);
                                                        Lz0DataHolder.Write(CmpBuffer, 0, CmpBytesToRead);

                                                        byte[] CompressedData = Lz0DataHolder.ToArray();

                                                        MiniLz0Lib.Decompress(ref CompressedData, UncmpChunkSize,
                                                            ref DeCompressedData);

                                                        using (FileStream DecompressedFileStream = new FileStream(
                                                            CurrentOutDecmpLz0File, FileMode.Append, FileAccess.Write))
                                                        {
                                                            DecompressedFileStream.Write(DeCompressedData, 0,
                                                                DeCompressedData.Length);
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

                                            File.Move(CurrentOutDecmpLz0File, CurrentProperFileExtn + RExt);
                                        }
                                    }
                                }

                                RExt = "";

                                IntialOffsetPos += 16;
                                FileCount++;
                            }
                        }

                        File.Delete(Extract_dir + "/" + SubBinName);
                    }
                }

                CoreForm.AppMsgBox("Extracted " + Path.GetFileName(FpkFile) + " file", "Success", MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                CoreForm.AppMsgBox(ex.Message, "Error", MessageBoxIcon.Error);
            }
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
                case "dpk":
                    RExtVar = ".dpk";
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
                case "kvm1":
                    RExtVar = ".kvm";
                    break;
                case "SPK0":
                    RExtVar = ".spk0";
                    break;
                case "EVMT":
                    RExtVar = ".emt";
                    break;
                case "DCMR":
                    RExtVar = ".dcmr";
                    break;
                case "DLGT":
                    RExtVar = ".dlgt";
                    break;
            }
            if (RealExt.StartsWith("bh"))
            {
                RExtVar = ".hi4";
            }
        }
    }
}