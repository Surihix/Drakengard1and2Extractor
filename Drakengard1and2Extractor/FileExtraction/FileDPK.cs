using Drakengard1and2Extractor.Support;
using Drakengard1and2Extractor.Support.Lz0Helpers;
using System;
using System.IO;
using System.Windows.Forms;

namespace Drakengard1and2Extractor.FileExtraction
{
    internal class FileDPK
    {
        public static void ExtractDPK(string dpkFile, bool isSingleFile)
        {
            try
            {
                var extractDir = Path.GetFullPath(dpkFile) + "_extracted";
                CommonMethods.IfFileDirExistsDel(extractDir, CommonMethods.DelSwitch.folder);
                Directory.CreateDirectory(extractDir);

                using (FileStream dpkStream = new FileStream(dpkFile, FileMode.Open, FileAccess.Read))
                {
                    using (BinaryReader dpkReader = new BinaryReader(dpkStream))
                    {
                        dpkReader.BaseStream.Position = 16;
                        var entries = dpkReader.ReadUInt32();


                        uint intialOffset = 48;
                        var fname = "FILE_";
                        var realExtn = string.Empty;
                        var fileCount = 1;
                        for (int f = 0; f < entries; f++)
                        {
                            dpkReader.BaseStream.Position = intialOffset;
                            var fileSize = dpkReader.ReadUInt32();

                            dpkReader.BaseStream.Position = intialOffset + 8;
                            var fileStart = dpkReader.ReadUInt32();

                            var currentFile = Path.Combine(extractDir, fname + $"{fileCount}");

                            using (FileStream outFileStream = new FileStream(currentFile, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                            {
                                dpkStream.Seek(fileStart, SeekOrigin.Begin);
                                dpkStream.CopyStreamTo(outFileStream, fileSize, false);
                            }

                            using (BinaryReader outFileReader = new BinaryReader(File.Open(currentFile, FileMode.Open, FileAccess.Read)))
                            {
                                realExtn = CommonMethods.GetFileHeader(outFileReader);
                            }

                            File.Move(currentFile, currentFile + realExtn);

                            if (realExtn == ".lz0")
                            {
                                var dcmpLz0Data = Lz0Decompression.ProcessLz0Data(currentFile + realExtn);
                                
                                File.WriteAllBytes(currentFile, dcmpLz0Data);
                                File.Delete(currentFile + realExtn);

                                using (BinaryReader dcmpLz0Reader = new BinaryReader(File.Open(currentFile, FileMode.Open, FileAccess.Read)))
                                {
                                    realExtn = CommonMethods.GetFileHeader(dcmpLz0Reader);
                                }

                                File.Move(currentFile, currentFile + realExtn);
                            }

                            LoggingHelpers.LogMessage($"Extracted '{fname}{fileCount}'");

                            realExtn = string.Empty;

                            intialOffset += 32;
                            fileCount++;
                        }
                    }
                }

                if (isSingleFile)
                {
                    LoggingHelpers.LogMessage(CoreForm.NewLineChara);
                    LoggingHelpers.LogMessage("Extraction has completed!");
                    LoggingHelpers.LogMessage(CoreForm.NewLineChara);

                    CommonMethods.AppMsgBox("Extracted " + Path.GetFileName(dpkFile) + " file", "Success", MessageBoxIcon.Information);
                }
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