using Drakengard1and2Extractor.Libraries;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Drakengard1and2Extractor.Tools
{
    internal class FileFPK
    {
        public static void ExtractFPK(string fpkFile, bool isSingleFile)
        {
            try
            {
                var extractDir = Path.GetFullPath(fpkFile) + "_extracted";
                CmnMethods.FileDirectoryExistsDel(extractDir, CmnMethods.DelSwitch.folder);
                Directory.CreateDirectory(extractDir);

                using (FileStream fpkStream = new FileStream(fpkFile, FileMode.Open, FileAccess.Read))
                {
                    using (BinaryReader fpkReader = new BinaryReader(fpkStream))
                    {
                        fpkReader.BaseStream.Position = 8;
                        var entries = fpkReader.ReadUInt32();

                        fpkReader.BaseStream.Position = 40;
                        var fpkDataStart = fpkReader.ReadUInt32();
                        var fpkDataSize = fpkReader.ReadUInt32();

                        CmnMethods.FileDirectoryExistsDel(extractDir + "/_.archive", CmnMethods.DelSwitch.file);

                        using (FileStream fpkDataStream = new FileStream(extractDir + "/_.archive", FileMode.OpenOrCreate, FileAccess.ReadWrite))
                        {
                            fpkStream.CopyTo(fpkDataStream, fpkDataStart, fpkDataSize);


                            uint intialOffset = 132;
                            var fName = "FILE_";
                            var rExtn = "";
                            var fileCount = 1;
                            for (int f = 0; f < entries; f++)
                            {
                                fpkReader.BaseStream.Position = intialOffset;
                                var outFileStart = fpkReader.ReadUInt32();
                                var outFileSize = fpkReader.ReadUInt32();
                                var extnChar = fpkReader.ReadChars(4);
                                Array.Reverse(extnChar);

                                var fileExtn = string.Join("", extnChar).Replace("\0", "");
                                var fExtn = "." + CmnMethods.ModifyString(fileExtn);

                                switch (fileExtn.StartsWith("/") || fileExtn.StartsWith("\\"))
                                {
                                    case true:
                                        break;

                                    case false:
                                        using (FileStream outFileStream = new FileStream(extractDir + "/" + fName + $"{fileCount}" + fExtn, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                                        {
                                            fpkDataStream.CopyTo(outFileStream, outFileStart, outFileSize);

                                            using (BinaryReader outFileReader = new BinaryReader(outFileStream))
                                            {
                                                CmnMethods.GetFileHeader(outFileReader, ref rExtn);
                                            }
                                        }

                                        File.Move(extractDir + "/" + fName + $"{fileCount}" + fExtn, extractDir + "/" + fName + $"{fileCount}" + fExtn + rExtn);

                                        string[] knownExtnArray = { ".fpk", ".dpk", ".zim", ".lz0", ".kps", ".kvm", ".spk0", ".emt", ".dcmr", ".dlgt", ".hi4", ".hd2" };

                                        if (knownExtnArray.Contains(rExtn))
                                        {
                                            if (!rExtn.Contains(".lz0"))
                                            {
                                                var outFileNameWithoutExtn = Path.GetFileNameWithoutExtension(extractDir + "/" + fName + $"{fileCount}" + fExtn + rExtn);
                                                File.Move(extractDir + "/" + fName + $"{fileCount}" + fExtn + rExtn, extractDir + "/" + outFileNameWithoutExtn);

                                                var outFileNameProperExtn = Path.GetFileNameWithoutExtension(extractDir + "/" + outFileNameWithoutExtn);
                                                var adjExtn = "";
                                                using (FileStream extnStream = new FileStream(extractDir + "/" + outFileNameWithoutExtn, FileMode.Open, FileAccess.Read))
                                                {
                                                    using (BinaryReader extnReader = new BinaryReader(extnStream))
                                                    {
                                                        CmnMethods.GetFileHeader(extnReader, ref adjExtn);
                                                    }
                                                }

                                                File.Move(extractDir + "/" + outFileNameWithoutExtn, extractDir + "/" + outFileNameProperExtn + adjExtn);
                                            }
                                            else
                                            {
                                                var outLzoFile = extractDir + "/" + fName + $"{fileCount}" + fExtn + rExtn;
                                                var outDcmpLzoFile = extractDir + "/" + Path.GetFileNameWithoutExtension(extractDir + "/" + fName + $"{fileCount}" + fExtn + rExtn);

                                                using (FileStream lzoStream = new FileStream(outLzoFile, FileMode.Open, FileAccess.Read))
                                                {
                                                    using (BinaryReader lzoReader = new BinaryReader(lzoStream))
                                                    {
                                                        lzoReader.BaseStream.Position = 24;
                                                        var lzoChunks = lzoReader.ReadUInt32();

                                                        uint lzoDataReadStart = 32;
                                                        for (int lz0 = 0; lz0 < lzoChunks; lz0++)
                                                        {
                                                            lzoReader.BaseStream.Position = lzoDataReadStart + 4;
                                                            var cmpChunkSize = lzoReader.ReadUInt32();
                                                            var uncmpChunkSize = lzoReader.ReadUInt32();

                                                            byte[] dcmpData = new byte[uncmpChunkSize];
                                                            using (MemoryStream lzoDataHolder = new MemoryStream())
                                                            {
                                                                lzoStream.CopyTo(lzoDataHolder, lzoDataReadStart + 12, cmpChunkSize);

                                                                byte[] cmpData = lzoDataHolder.ToArray();
                                                                MiniLz0Lib.Decompress(ref cmpData, uncmpChunkSize, ref dcmpData);

                                                                using (FileStream dcmpFileStream = new FileStream(outDcmpLzoFile, FileMode.Append, FileAccess.Write))
                                                                {
                                                                    dcmpFileStream.Write(dcmpData, 0, dcmpData.Length);
                                                                }
                                                            }

                                                            var seekLength = cmpChunkSize;
                                                            lzoReader.BaseStream.Seek(lzoDataReadStart + 12, SeekOrigin.Begin);
                                                            lzoReader.BaseStream.Seek(seekLength, SeekOrigin.Current);

                                                            var nextSeekVal = lzoReader.BaseStream;
                                                            var nextSeek = (uint)nextSeekVal.Position;

                                                            uint newLzoDataReadStart = nextSeek;
                                                            lzoDataReadStart = newLzoDataReadStart;
                                                        }
                                                    }
                                                }

                                                File.Delete(outLzoFile);

                                                using (FileStream dcmpLzoFile = new FileStream(outDcmpLzoFile, FileMode.Open, FileAccess.Read))
                                                {
                                                    using (BinaryReader dcmpFileReader = new BinaryReader(dcmpLzoFile))
                                                    {
                                                        CmnMethods.GetFileHeader(dcmpFileReader, ref rExtn);
                                                    }

                                                    File.Move(outDcmpLzoFile, extractDir + "/" + fName + $"{fileCount}" + rExtn);
                                                }
                                            }
                                        }
                                        break;
                                }

                                rExtn = "";

                                intialOffset += 16;
                                fileCount++;
                            }
                        }

                        File.Delete(extractDir + "/_.archive");
                    }
                }

                if (isSingleFile.Equals(true))
                {
                    CmnMethods.AppMsgBox("Extracted " + Path.GetFileName(fpkFile) + " file", "Success", MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                CmnMethods.AppMsgBox("" + ex, "Error", MessageBoxIcon.Error);
            }
        }
    }
}