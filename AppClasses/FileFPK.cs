using MiniLz0;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Drakengard1and2Extractor.AppClasses
{
    public class FileFPK
    {
        public static void ExtractFPK(string fpkFile)
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

                    CmnMethods.FileDirectoryExistsDel(extractDir + "/_", CmnMethods.DelSwitch.file);

                    using (FileStream fpkDataStream = new FileStream(extractDir + "/_", FileMode.OpenOrCreate, FileAccess.ReadWrite))
                    {
                        fpkStream.Seek(fpkDataStart, SeekOrigin.Begin);
                        byte[] fpkDataBuffer = new byte[fpkDataSize];
                        var fpkDataToCopy = fpkStream.Read(fpkDataBuffer, 0, fpkDataBuffer.Length);
                        fpkDataStream.Write(fpkDataBuffer, 0, fpkDataToCopy);


                        uint intialOffset = 132;
                        var fName = "FILE_";
                        var rExt = "";
                        int fileCount = 1;
                        for (int f = 0; f < entries; f++)
                        {
                            fpkReader.BaseStream.Position = intialOffset;
                            var outFileStart = fpkReader.ReadUInt32();

                            fpkReader.BaseStream.Position = intialOffset + 4;
                            var outFileSize = fpkReader.ReadUInt32();

                            fpkReader.BaseStream.Position = intialOffset + 8;
                            var extnChar = fpkReader.ReadChars(4);
                            Array.Reverse(extnChar);

                            string fileExt = string.Join("", extnChar).Replace("\0", "");
                            CmnMethods.ModifyString(ref fileExt);

                            switch (fileExt.StartsWith("/") || fileExt.StartsWith("\\"))
                            {
                                case true:
                                    break;

                                case false:
                                    string fExt = "." + fileExt;

                                    using (FileStream outFileStream = new FileStream(extractDir + "/" + fName + $"{fileCount}" + fExt, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                                    {
                                        fpkDataStream.Seek(outFileStart, SeekOrigin.Begin);
                                        byte[] outFilebuffer = new byte[outFileSize];
                                        var outFileDataToCopy = fpkDataStream.Read(outFilebuffer, 0, outFilebuffer.Length);
                                        outFileStream.Write(outFilebuffer, 0, outFileDataToCopy);

                                        using (BinaryReader outFileReader = new BinaryReader(outFileStream))
                                        {
                                            CmnMethods.GetFileHeader(outFileReader, ref rExt);
                                        }
                                    }

                                    File.Move(extractDir + "/" + fName + $"{fileCount}" + fExt, extractDir + "/" + fName + $"{fileCount}" + fExt + rExt);

                                    string[] knownExtArray = { ".fpk", ".dpk", ".zim", ".lz0", ".kps", ".kvm", ".spk0", ".emt", ".dcmr", ".dlgt", ".hi4" };

                                    if (knownExtArray.Contains(rExt))
                                    {
                                        if (!rExt.Contains(".lz0"))
                                        {
                                            var outFileNameWithoutExtn = Path.GetFileNameWithoutExtension(extractDir + "/" + fName + $"{fileCount}" + fExt + rExt);
                                            File.Move(extractDir + "/" + fName + $"{fileCount}" + fExt + rExt, extractDir + "/" + outFileNameWithoutExtn);

                                            var outFileNameProperExt = Path.GetFileNameWithoutExtension(extractDir + "/" + outFileNameWithoutExtn);
                                            var adjExt = "";
                                            using (FileStream extnStream = new FileStream(extractDir + "/" + outFileNameWithoutExtn, FileMode.Open, FileAccess.Read))
                                            {
                                                using (BinaryReader extnReader = new BinaryReader(extnStream))
                                                {
                                                    CmnMethods.GetFileHeader(extnReader, ref adjExt);
                                                }
                                            }

                                            File.Move(extractDir + "/" + outFileNameWithoutExtn, extractDir + "/" + outFileNameProperExt + adjExt);
                                        }
                                        else
                                        {
                                            var outLzoFile = extractDir + "/" + fName + $"{fileCount}" + fExt + rExt;
                                            var outDcmpLzoFile = extractDir + "/" + Path.GetFileNameWithoutExtension(extractDir + "/" + fName + $"{fileCount}" + fExt + rExt);

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

                                                        lzoReader.BaseStream.Position = lzoDataReadStart + 8;
                                                        var uncmpChunkSize = lzoReader.ReadUInt32();

                                                        byte[] dcmpData = new byte[uncmpChunkSize];
                                                        using (MemoryStream lzoDataHolder = new MemoryStream())
                                                        {
                                                            lzoStream.Seek(lzoDataReadStart + 12, SeekOrigin.Begin);
                                                            byte[] cmpBuffer = new byte[cmpChunkSize];
                                                            var CmpDataToCopy = lzoStream.Read(cmpBuffer, 0, cmpBuffer.Length);
                                                            lzoDataHolder.Write(cmpBuffer, 0, CmpDataToCopy);

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

                                            using (FileStream dcmplzoFile = new FileStream(outDcmpLzoFile, FileMode.Open, FileAccess.Read))
                                            {
                                                using (BinaryReader dcmpFileReader = new BinaryReader(dcmplzoFile))
                                                {
                                                    CmnMethods.GetFileHeader(dcmpFileReader, ref rExt);
                                                }

                                                File.Move(outDcmpLzoFile, extractDir + "/" + fName + $"{fileCount}" + rExt);
                                            }
                                        }
                                    }
                                    break;
                            }

                            rExt = "";

                            intialOffset += 16;
                            fileCount++;
                        }
                    }

                    File.Delete(extractDir + "/_");
                }
            }

            CmnMethods.AppMsgBox("Extracted " + Path.GetFileName(fpkFile) + " file", "Success", MessageBoxIcon.Information);
        }
    }
}