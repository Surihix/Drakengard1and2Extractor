using Drakengard1and2Extractor.Support;
using Drakengard1and2Extractor.Support.Lz0Helpers;
using System;
using System.IO;
using System.Windows.Forms;

namespace Drakengard1and2Extractor.FileExtraction
{
    internal class FileFPK
    {
        public static void ExtractFPK(string fpkFile, bool isSingleFile)
        {
            try
            {
                var extractDir = Path.GetFullPath(fpkFile) + "_extracted";
                CommonMethods.IfFileDirExistsDel(extractDir, CommonMethods.DelSwitch.directory);
                Directory.CreateDirectory(extractDir);

                var fpkStructure = new CommonStructures.FPK();

                using (FileStream fpkStream = new FileStream(fpkFile, FileMode.Open, FileAccess.Read))
                {
                    using (BinaryReader fpkReader = new BinaryReader(fpkStream))
                    {
                        fpkReader.BaseStream.Position = 8;
                        fpkStructure.EntryCount = fpkReader.ReadUInt32();

                        fpkReader.BaseStream.Position = 40;
                        fpkStructure.FPKbinDataOffset = fpkReader.ReadUInt32();
                        fpkStructure.FPKbinDataSize = fpkReader.ReadUInt32();

                        var tmpArchiveFile = Path.Combine(extractDir, "_.archive");

                        CommonMethods.IfFileDirExistsDel(tmpArchiveFile, CommonMethods.DelSwitch.file);

                        using (FileStream fpkDataStream = new FileStream(tmpArchiveFile, FileMode.OpenOrCreate, FileAccess.ReadWrite))
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
                                fileExtnFixed = "." + CommonMethods.ModifyExtnString(fileExtn);

                                var currentFile = Path.Combine(extractDir, fName + $"{fileCount}" + fileExtnFixed);

                                if (fileExtnFixed[1] == '/' || fileExtnFixed[1] == '\\')
                                {
                                    intialOffset += 16;
                                    continue;
                                }

                                using (FileStream outFileStream = new FileStream(currentFile, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                                {
                                    fpkDataStream.Seek(fpkStructure.EntryDataOffset, SeekOrigin.Begin);
                                    fpkDataStream.CopyStreamTo(outFileStream, fpkStructure.EntryDataSize, false);

                                    using (BinaryReader outFileReader = new BinaryReader(outFileStream))
                                    {
                                        tmpExtn = CommonMethods.GetFileHeader(outFileReader);
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
                                        realExtn = CommonMethods.GetFileHeader(dcmpLz0Reader);
                                    }

                                    File.Move(outCurrentFile, outCurrentFile + realExtn);
                                }

                                intialOffset += 16;
                                fileCount++;
                            }
                        }

                        File.Delete(tmpArchiveFile);
                    }
                }

                if (isSingleFile)
                {
                    LoggingMethods.LogMessage(CommonMethods.NewLineChara);
                    LoggingMethods.LogMessage("Extraction has completed!");
                    LoggingMethods.LogMessage(CommonMethods.NewLineChara);

                    CommonMethods.AppMsgBox("Extracted " + Path.GetFileName(fpkFile) + " file", "Success", MessageBoxIcon.Information);
                }
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