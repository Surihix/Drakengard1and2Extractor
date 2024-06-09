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
                LoggingHelpers.LogMessage(CoreForm.NewLineChara);

                LoggingHelpers.LogMessage("Preparing bin file....");
                LoggingHelpers.LogMessage(CoreForm.NewLineChara);

                var extractDir = Path.GetFullPath(mainBinFile) + "_extracted";
                CommonMethods.IfFileDirExistsDel(extractDir, CommonMethods.DelSwitch.folder);
                Directory.CreateDirectory(extractDir);

                var mainBinName = Path.GetFileName(mainBinFile);
                var isImageBinFile = mainBinName == "image.bin" || mainBinName == "IMAGE.BIN";

                using (FileStream mainBinStream = new FileStream(mainBinFile, FileMode.Open, FileAccess.Read))
                {
                    using (BinaryReader mainBinReader = new BinaryReader(mainBinStream))
                    {
                        mainBinReader.BaseStream.Position = 8;
                        var entries = mainBinReader.ReadUInt32();

                        mainBinReader.BaseStream.Position = 40;
                        var binDataStart = mainBinReader.ReadUInt32();

                        var tmpArchiveFile = Path.Combine(extractDir, "_.archive");

                        CommonMethods.IfFileDirExistsDel(tmpArchiveFile, CommonMethods.DelSwitch.file);

                        using (FileStream binStream = new FileStream(tmpArchiveFile, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                        {
                            mainBinStream.Seek(binDataStart, SeekOrigin.Begin);
                            mainBinStream.CopyTo(binStream);


                            uint intialOffset = 132;
                            var fname = "FILE_";
                            var fileExtn = string.Empty;
                            var fileExtnFixed = string.Empty;
                            var tmpExtn = string.Empty;
                            var realExtn = string.Empty;
                            var fileCount = 1;
                            for (int f = 0; f < entries; f++)
                            {
                                mainBinReader.BaseStream.Position = intialOffset;
                                var fileStart = mainBinReader.ReadUInt32();
                                var fileSize = mainBinReader.ReadUInt32();

                                if (fileStart == 0 && fileSize == 0)
                                {
                                    intialOffset += 16;
                                    continue;
                                }

                                var extnChar = mainBinReader.ReadChars(4);
                                Array.Reverse(extnChar);

                                if (isImageBinFile)
                                {
                                    fileExtnFixed = string.Empty;
                                }
                                else
                                {
                                    fileExtn = string.Join("", extnChar).Replace("\0", "");
                                    fileExtnFixed = "." + CommonMethods.ModifyExtnString(fileExtn);
                                }

                                var currentFile = Path.Combine(extractDir, fname + $"{fileCount}" + fileExtnFixed);

                                using (FileStream outFileStream = new FileStream(currentFile, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                                {
                                    binStream.Seek(fileStart, SeekOrigin.Begin);
                                    binStream.CopyStreamTo(outFileStream, fileSize, false);

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

                                    realExtn = string.Empty;
                                }

                                LoggingHelpers.LogMessage($"Extracted '{fname}{fileCount}'");

                                intialOffset += 16;
                                fileCount++;
                            }
                        }

                        File.Delete(tmpArchiveFile);
                    }
                }

                LoggingHelpers.LogMessage(CoreForm.NewLineChara);
                LoggingHelpers.LogMessage("Extraction has completed!");

                CommonMethods.AppMsgBox("Extracted " + Path.GetFileName(mainBinFile) + " file", "Success", MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                CommonMethods.AppMsgBox("" + ex, "Error", MessageBoxIcon.Error);
                LoggingHelpers.LogMessage(CoreForm.NewLineChara);
                LoggingHelpers.LogException("Exception: " + ex);
            }
        }
    }
}