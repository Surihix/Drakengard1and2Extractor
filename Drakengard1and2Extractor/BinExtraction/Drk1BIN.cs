using Drakengard1and2Extractor.Support;
using Drakengard1and2Extractor.Support.Lz0Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Drakengard1and2Extractor.BinExtraction
{
    internal class Drk1BIN
    {
        public static void ExtractBin(string mainBinFile, bool generateLstPaths)
        {
            try
            {
                LoggingMethods.LogMessage(SharedMethods.NewLineChara);
                LoggingMethods.LogMessage("Preparing bin file....");
                LoggingMethods.LogMessage(SharedMethods.NewLineChara);

                var extractDir = Path.GetFullPath(mainBinFile) + "_extracted";
                SharedMethods.IfFileDirExistsDel(extractDir, SharedMethods.DelSwitch.directory);
                Directory.CreateDirectory(extractDir);

                var mainBinName = Path.GetFileName(mainBinFile);
                var isImageBinFile = mainBinName == "image.bin" || mainBinName == "IMAGE.BIN";

                var fpkStructure = new SharedStructures.FPK();
                var filesExtractedDict = new Dictionary<string, string>();

                using (FileStream mainBinStream = new FileStream(mainBinFile, FileMode.Open, FileAccess.Read))
                {
                    using (BinaryReader mainBinReader = new BinaryReader(mainBinStream))
                    {
                        mainBinReader.BaseStream.Position = 8;
                        fpkStructure.EntryCount = mainBinReader.ReadUInt32();

                        mainBinReader.BaseStream.Position = 40;
                        fpkStructure.FPKbinDataOffset = mainBinReader.ReadUInt32();
                        fpkStructure.FPKbinDataSize = mainBinReader.ReadUInt32();

                        mainBinReader.BaseStream.Position = 64;
                        var binNameInFile = mainBinReader.ReadStringTillNull();
                        binNameInFile = binNameInFile.Replace("/", "").Replace("\\", "");
                        fpkStructure.FPKbinName = SharedMethods.ModifyString(binNameInFile);

                        if (fpkStructure.FPKbinName == "")
                        {
                            fpkStructure.FPKbinName = fpkStructure.FallBackName;
                        }

                        var subBinFile = Path.Combine(extractDir, fpkStructure.FPKbinName);

                        SharedMethods.IfFileDirExistsDel(subBinFile, SharedMethods.DelSwitch.file);

                        using (FileStream binStream = new FileStream(subBinFile, FileMode.OpenOrCreate, FileAccess.ReadWrite))
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
                                    fileExtnFixed = "." + SharedMethods.ModifyString(fileExtn);

                                    if (fileExtnFixed == ".lst")
                                    {
                                        fpkStructure.HasLstFile = true;
                                    }

                                    if (fileExtn.StartsWith("\\") || fileExtn.StartsWith("/"))
                                    {
                                        fileExtnFixed = "";
                                    }
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
                                            tmpExtn = SharedMethods.GetFileHeader(outFileReader);
                                        }
                                    }
                                    else
                                    {
                                        filesExtractedDict.Add(fname + fileCount, currentFile);
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
                                            realExtn = SharedMethods.GetFileHeader(dcmpLz0Reader);
                                        }

                                        File.Move(currentFile, currentFile + realExtn);

                                        filesExtractedDict.Add(fname + fileCount, currentFile + realExtn);
                                    }
                                    else
                                    {
                                        filesExtractedDict.Add(fname + fileCount, currentTmpFile);
                                    }
                                }

                                LoggingMethods.LogMessage($"Extracted '{fname}{fileCount}'");

                                intialOffset += 16;
                                fileCount++;
                            }
                        }
                    }
                }

                if (generateLstPaths && fpkStructure.HasLstFile)
                {
                    LstParser.ProcessLstFile(fpkStructure, true, extractDir, filesExtractedDict);
                }

                LoggingMethods.LogMessage(SharedMethods.NewLineChara);
                LoggingMethods.LogMessage("Extraction has completed!");

                SharedMethods.AppMsgBox("Extracted " + Path.GetFileName(mainBinFile) + " file", "Success", MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                SharedMethods.AppMsgBox("" + ex, "Error", MessageBoxIcon.Error);
                LoggingMethods.LogMessage(SharedMethods.NewLineChara);
                LoggingMethods.LogException("Exception: " + ex);
            }
        }
    }
}