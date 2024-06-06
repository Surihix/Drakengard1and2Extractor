using Drakengard1and2Extractor.Support;
using System;
using System.IO;
using System.Windows.Forms;

namespace Drakengard1and2Extractor.Tools
{
    internal class Drk1BIN
    {
        public static void ExtractBin(string mainBinFile)
        {
            try
            {
                LoggingHelpers.LogMessage(CoreForm.NewLineChara);

                var extractDir = Path.GetFullPath(mainBinFile) + "_extracted";
                CommonMethods.IfFileDirExistsDel(extractDir, CommonMethods.DelSwitch.folder);
                Directory.CreateDirectory(extractDir);

                using (FileStream mainBinStream = new FileStream(mainBinFile, FileMode.Open, FileAccess.Read))
                {
                    using (BinaryReader mainBinReader = new BinaryReader(mainBinStream))
                    {
                        mainBinReader.BaseStream.Position = 8;
                        var entries = mainBinReader.ReadUInt32();

                        mainBinReader.BaseStream.Position = 40;
                        var binDataStart = mainBinReader.ReadUInt32();

                        CommonMethods.IfFileDirExistsDel(extractDir + "/_.archive", CommonMethods.DelSwitch.file);

                        using (FileStream binStream = new FileStream(extractDir + "/_.archive", FileMode.OpenOrCreate, FileAccess.ReadWrite))
                        {
                            mainBinStream.Seek(binDataStart, SeekOrigin.Begin);
                            mainBinStream.CopyTo(binStream);


                            uint intialOffset = 132;
                            var fname = "FILE_";
                            var rExtn = "";
                            var fileCount = 1;
                            for (int f = 0; f < entries; f++)
                            {
                                mainBinReader.BaseStream.Position = intialOffset;
                                var fileStart = mainBinReader.ReadUInt32();
                                var fileSize = mainBinReader.ReadUInt32();

                                var extnChar = mainBinReader.ReadChars(4);
                                Array.Reverse(extnChar);

                                var fileExtn = string.Join("", extnChar).Replace("\0", "");
                                var fExtn = "." + CommonMethods.ModifyString(fileExtn);

                                if (mainBinFile.Contains("image.bin") || mainBinFile.Contains("IMAGE.BIN"))
                                {
                                    string AdjExtn = "";
                                    fExtn = AdjExtn;
                                }

                                using (FileStream outFileStream = new FileStream(extractDir + "/" + fname + $"{fileCount}" + fExtn, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                                {
                                    binStream.Seek(fileStart, SeekOrigin.Begin);
                                    binStream.CopyStreamTo(outFileStream, fileSize, false);
                                    binStream.Seek(fileStart, SeekOrigin.Begin);

                                    if (mainBinFile.Contains("image.bin") || mainBinFile.Contains("IMAGE.BIN"))
                                    {
                                        using (BinaryReader outFileReader = new BinaryReader(outFileStream))
                                        {
                                            CommonMethods.GetFileHeader(outFileReader, ref rExtn);
                                        }
                                    }
                                }

                                if (mainBinFile.Contains("image.bin") || mainBinFile.Contains("IMAGE.BIN"))
                                {
                                    File.Move(extractDir + "/" + fname + $"{fileCount}" + fExtn, extractDir + "/" + fname + $"{fileCount}" + fExtn + rExtn);

                                    if (rExtn.Contains(".lz0"))
                                    {
                                        var currentLzoFile = extractDir + "/" + fname + $"{fileCount}" + fExtn + rExtn;
                                        var currentOutDecmpLzoFile = extractDir + "/" + Path.GetFileNameWithoutExtension(extractDir + "/" + fname + $"{fileCount}" + fExtn + rExtn);

                                        using (FileStream lzoStream = new FileStream(currentLzoFile, FileMode.Open, FileAccess.Read))
                                        {
                                            using (BinaryReader lzoReader = new BinaryReader(lzoStream))
                                            {
                                                lzoReader.BaseStream.Position = 24;
                                                var lzoChunks = lzoReader.ReadUInt32();


                                                uint lzoDataReadStart = 32;
                                                for (int lzo = 0; lzo < lzoChunks; lzo++)
                                                {
                                                    lzoReader.BaseStream.Position = lzoDataReadStart + 4;
                                                    var cmpChunkSize = lzoReader.ReadUInt32();
                                                    var uncmpChunkSize = lzoReader.ReadUInt32();

                                                    byte[] deCompressedData = new byte[0];
                                                    deCompressedData = new byte[uncmpChunkSize];
                                                    using (MemoryStream lzoDataHolder = new MemoryStream())
                                                    {
                                                        lzoStream.Seek(lzoDataReadStart + 12, SeekOrigin.Begin);
                                                        lzoStream.CopyStreamTo(lzoDataHolder, cmpChunkSize, false);

                                                        byte[] compressedData = lzoDataHolder.ToArray();
                                                        Minilz0Helpers.Decompress(ref compressedData, uncmpChunkSize, ref deCompressedData);

                                                        using (FileStream decompressedFileStream = new FileStream(currentOutDecmpLzoFile, FileMode.Append, FileAccess.Write))
                                                        {
                                                            decompressedFileStream.Write(deCompressedData, 0, deCompressedData.Length);
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

                                        File.Delete(currentLzoFile);

                                        using (FileStream dcmplzoFile = new FileStream(currentOutDecmpLzoFile, FileMode.Open, FileAccess.Read))
                                        {
                                            using (BinaryReader DcmpFileReader = new BinaryReader(dcmplzoFile))
                                            {
                                                CommonMethods.GetFileHeader(DcmpFileReader, ref rExtn);
                                            }
                                        }

                                        File.Move(currentOutDecmpLzoFile, currentOutDecmpLzoFile + rExtn);
                                    }

                                    rExtn = "";
                                }

                                LoggingHelpers.LogMessage($"Extracted '{fname}{fileCount}'");

                                intialOffset += 16;
                                fileCount++;
                            }
                        }

                        File.Delete(extractDir + "/_.archive");
                    }
                }

                LoggingHelpers.LogMessage(CoreForm.NewLineChara);
                CommonMethods.AppMsgBox("Extracted " + Path.GetFileName(mainBinFile) + " file", "Success", MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                CommonMethods.AppMsgBox("" + ex, "Error", MessageBoxIcon.Error);
            }
        }
    }
}