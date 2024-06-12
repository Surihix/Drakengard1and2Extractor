using Drakengard1and2Extractor.Support;
using Drakengard1and2Extractor.Support.Lz0Helpers;
using System;
using System.IO;
using System.Windows.Forms;

namespace Drakengard1and2Extractor.BinExtraction
{
    internal class Drk1BIN
    {
        public static void ExtractBin(string mainBinFile)
        {
            try
            {
                LoggingMethods.LogMessage(CommonMethods.NewLineChara);

                LoggingMethods.LogMessage("Preparing bin file....");
                LoggingMethods.LogMessage(CommonMethods.NewLineChara);

                var extractDir = Path.GetFullPath(mainBinFile) + "_extracted";
                CommonMethods.IfFileDirExistsDel(extractDir, CommonMethods.DelSwitch.directory);
                Directory.CreateDirectory(extractDir);

                var mainBinName = Path.GetFileName(mainBinFile);
                var isImageBinFile = mainBinName == "image.bin" || mainBinName == "IMAGE.BIN";

                var fpkStructure = new CommonStructures.FPK();

                using (FileStream mainBinStream = new FileStream(mainBinFile, FileMode.Open, FileAccess.Read))
                {
                    using (BinaryReader mainBinReader = new BinaryReader(mainBinStream))
                    {
                        mainBinReader.BaseStream.Position = 8;
                        fpkStructure.EntryCount = mainBinReader.ReadUInt32();

                        mainBinReader.BaseStream.Position = 40;
                        fpkStructure.FPKbinDataOffset = mainBinReader.ReadUInt32();
                        fpkStructure.FPKbinDataSize = mainBinReader.ReadUInt32();

                        var tmpArchiveFile = Path.Combine(extractDir, "_.archive");

                        CommonMethods.IfFileDirExistsDel(tmpArchiveFile, CommonMethods.DelSwitch.file);

                        using (FileStream binStream = new FileStream(tmpArchiveFile, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                        {
                            mainBinStream.Seek(fpkStructure.FPKbinDataOffset, SeekOrigin.Begin);
                            mainBinStream.CopyStreamTo(binStream, fpkStructure.FPKbinDataSize, false);


                            uint intialOffset = 132;
                            var fname = "FILE_";
                            var fileExtn = string.Empty;
                            var fileExtnFixed = string.Empty;
                            var tmpExtn = string.Empty;
                            var realExtn = string.Empty;
                            var fileCount = 1;
                            for (int f = 0; f < fpkStructure.EntryCount; f++)
                            {
                                mainBinReader.BaseStream.Position = intialOffset;
                                fpkStructure.EntryDataOffset = mainBinReader.ReadUInt32();
                                fpkStructure.EntryDataSize = mainBinReader.ReadUInt32();

                                if (fpkStructure.EntryDataOffset == 0 && fpkStructure.EntryDataSize == 0)
                                {
                                    intialOffset += 16;
                                    continue;
                                }

                                fpkStructure.EntryExtnChars = mainBinReader.ReadChars(4);
                                Array.Reverse(fpkStructure.EntryExtnChars);

                                if (isImageBinFile)
                                {
                                    fileExtnFixed = string.Empty;
                                }
                                else
                                {
                                    fileExtn = string.Join("", fpkStructure.EntryExtnChars).Replace("\0", "");
                                    fileExtnFixed = "." + CommonMethods.ModifyExtnString(fileExtn);
                                }

                                var currentFile = Path.Combine(extractDir, fname + $"{fileCount}" + fileExtnFixed);

                                using (FileStream outFileStream = new FileStream(currentFile, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                                {
                                    binStream.Seek(fpkStructure.EntryDataOffset, SeekOrigin.Begin);
                                    binStream.CopyStreamTo(outFileStream, fpkStructure.EntryDataSize, false);

                                    if (isImageBinFile)
                                    {
                                        using (BinaryReader outFileReader = new BinaryReader(outFileStream))
                                        {
                                            tmpExtn = CommonMethods.GetFileHeader(outFileReader);
                                        }
                                    }
                                }

                                if (isImageBinFile)
                                {
                                    var currentTmpFile = Path.Combine(extractDir, fname + $"{fileCount}" + fileExtnFixed + tmpExtn);

                                    File.Move(currentFile, currentTmpFile);

                                    if (tmpExtn == ".lz0")
                                    {
                                        var dcmpLz0Data = Lz0Decompression.ProcessLz0Data(currentTmpFile);

                                        File.WriteAllBytes(currentFile, dcmpLz0Data);
                                        File.Delete(currentTmpFile);

                                        using (BinaryReader dcmpLz0Reader = new BinaryReader(File.Open(currentFile, FileMode.Open, FileAccess.Read)))
                                        {
                                            realExtn = CommonMethods.GetFileHeader(dcmpLz0Reader);
                                        }

                                        File.Move(currentFile, currentFile + realExtn);
                                    }
                                }

                                LoggingMethods.LogMessage($"Extracted '{fname}{fileCount}'");

                                intialOffset += 16;
                                fileCount++;
                            }
                        }

                        File.Delete(tmpArchiveFile);
                    }
                }

                LoggingMethods.LogMessage(CommonMethods.NewLineChara);
                LoggingMethods.LogMessage("Extraction has completed!");

                CommonMethods.AppMsgBox("Extracted " + Path.GetFileName(mainBinFile) + " file", "Success", MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                CommonMethods.AppMsgBox("" + ex, "Error", MessageBoxIcon.Error);
                LoggingMethods.LogMessage(CommonMethods.NewLineChara);
                LoggingMethods.LogException("Exception: " + ex);
            }
        }
    }
}