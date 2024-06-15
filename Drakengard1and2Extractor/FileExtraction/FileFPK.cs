using Drakengard1and2Extractor.Support;
using Drakengard1and2Extractor.Support.Lz0Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Drakengard1and2Extractor.FileExtraction
{
    internal class FileFPK
    {
        public static void ExtractFPK(string fpkFile, bool generateLstPaths, bool isSingleFile)
        {
            try
            {
                var extractDir = Path.GetFullPath(fpkFile) + "_extracted";
                SharedMethods.IfFileDirExistsDel(extractDir, SharedMethods.DelSwitch.directory);
                Directory.CreateDirectory(extractDir);

                var fpkStructure = new SharedStructures.FPK();
                var filesExtractedDict = new Dictionary<string, string>();

                using (FileStream fpkStream = new FileStream(fpkFile, FileMode.Open, FileAccess.Read))
                {
                    using (BinaryReader fpkReader = new BinaryReader(fpkStream))
                    {
                        fpkReader.BaseStream.Position = 8;
                        fpkStructure.EntryCount = fpkReader.ReadUInt32();

                        fpkReader.BaseStream.Position = 40;
                        fpkStructure.FPKbinDataOffset = fpkReader.ReadUInt32();
                        fpkStructure.FPKbinDataSize = fpkReader.ReadUInt32();

                        fpkReader.BaseStream.Position = 64;
                        var binNameInFile = fpkReader.ReadStringTillNull();
                        binNameInFile = binNameInFile.Replace("/", "").Replace("\\", "");
                        fpkStructure.FPKbinName = SharedMethods.ModifyString(binNameInFile);

                        if (fpkStructure.FPKbinName == "")
                        {
                            fpkStructure.FPKbinName = fpkStructure.FallBackName;
                        }

                        var fpkBinFile = Path.Combine(extractDir, "_.archive");

                        SharedMethods.IfFileDirExistsDel(fpkBinFile, SharedMethods.DelSwitch.file);

                        using (FileStream fpkDataStream = new FileStream(fpkBinFile, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                        {
                            fpkStream.Seek(fpkStructure.FPKbinDataOffset, SeekOrigin.Begin);
                            fpkStream.CopyStreamTo(fpkDataStream, fpkStructure.FPKbinDataSize, false);


                            uint intialOffset = 132;
                            var fName = "FILE_";
                            var fileExtn = string.Empty;
                            var fileExtnFixed = string.Empty;
                            var tmpExtn = string.Empty;
                            var realExtn = string.Empty;
                            var fileCount = 1;
                            for (int f = 0; f < fpkStructure.EntryCount; f++)
                            {
                                fpkReader.BaseStream.Position = intialOffset;
                                fpkStructure.EntryDataOffset = fpkReader.ReadUInt32();
                                fpkStructure.EntryDataSize = fpkReader.ReadUInt32();
                                fpkStructure.EntryExtnChars = fpkReader.ReadChars(4);
                                Array.Reverse(fpkStructure.EntryExtnChars);

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

                                var currentFile = Path.Combine(extractDir, fName + $"{fileCount}" + fileExtnFixed);

                                using (FileStream outFileStream = new FileStream(currentFile, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                                {
                                    fpkDataStream.Seek(fpkStructure.EntryDataOffset, SeekOrigin.Begin);
                                    fpkDataStream.CopyStreamTo(outFileStream, fpkStructure.EntryDataSize, false);

                                    tmpExtn = string.Empty;

                                    using (BinaryReader outFileReader = new BinaryReader(outFileStream))
                                    {
                                        tmpExtn = SharedMethods.GetFileHeader(outFileReader);
                                    }

                                    if (tmpExtn == "")
                                    {
                                        tmpExtn = fileExtnFixed;
                                    }
                                }

                                var currentTmpFile = Path.Combine(extractDir, fName + $"{fileCount}" + tmpExtn);
                                File.Move(currentFile, currentTmpFile);

                                if (tmpExtn == ".lz0")
                                {
                                    var dcmpLz0Data = Lz0Decompression.ProcessLz0Data(currentTmpFile);
                                    var outCurrentFile = Path.Combine(extractDir, fName + $"{fileCount}");

                                    File.WriteAllBytes(outCurrentFile, dcmpLz0Data);
                                    File.Delete(currentTmpFile);

                                    using (BinaryReader dcmpLz0Reader = new BinaryReader(File.Open(outCurrentFile, FileMode.Open, FileAccess.Read)))
                                    {
                                        realExtn = SharedMethods.GetFileHeader(dcmpLz0Reader);
                                    }

                                    File.Move(outCurrentFile, outCurrentFile + realExtn);

                                    filesExtractedDict.Add(fName + fileCount, outCurrentFile + realExtn);
                                }
                                else
                                {
                                    filesExtractedDict.Add(fName + fileCount, currentTmpFile);
                                }

                                intialOffset += 16;
                                fileCount++;
                            }
                        }
                    }
                }

                if (generateLstPaths && fpkStructure.HasLstFile)
                {
                    LstParser.ProcessLstFile(fpkStructure, isSingleFile, extractDir, filesExtractedDict);
                }

                if (isSingleFile)
                {
                    LoggingMethods.LogMessage(SharedMethods.NewLineChara);
                    LoggingMethods.LogMessage("Extraction has completed!");
                    LoggingMethods.LogMessage(SharedMethods.NewLineChara);

                    SharedMethods.AppMsgBox("Extracted " + Path.GetFileName(fpkFile) + " file", "Success", MessageBoxIcon.Information);
                }
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