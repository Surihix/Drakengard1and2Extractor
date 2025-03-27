using Drakengard1and2Extractor.Support;
using Drakengard1and2Extractor.Support.Lz0Helpers;
using System;
using System.IO;
using System.Windows.Forms;

namespace Drakengard1and2Extractor.FileExtraction
{
    internal class ExtDPK
    {
        public static void ExtractDPK(string dpkFile, bool isSingleFile)
        {
            try
            {
                var extractDir = Path.GetFullPath(dpkFile) + "_extracted";
                SharedMethods.IfFileDirExistsDel(extractDir, SharedMethods.DelSwitch.directory);
                Directory.CreateDirectory(extractDir);

                var dpkStructure = new SharedStructures.DPK();

                using (FileStream dpkStream = new FileStream(dpkFile, FileMode.Open, FileAccess.Read))
                {
                    using (BinaryReader dpkReader = new BinaryReader(dpkStream))
                    {
                        dpkReader.BaseStream.Position = 16;
                        dpkStructure.EntryCount = dpkReader.ReadUInt32();


                        uint intialOffset = 48;
                        var fname = "FILE_";
                        var realExtn = string.Empty;
                        var fileCount = 1;
                        for (int f = 0; f < dpkStructure.EntryCount; f++)
                        {
                            dpkReader.BaseStream.Position = intialOffset;
                            dpkStructure.EntryDataSize = dpkReader.ReadUInt32();

                            dpkReader.BaseStream.Position = intialOffset + 8;
                            dpkStructure.EntryDataOffset = dpkReader.ReadUInt32();

                            var currentFile = Path.Combine(extractDir, fname + $"{fileCount}");

                            using (FileStream outFileStream = new FileStream(currentFile, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                            {
                                dpkStream.Seek(dpkStructure.EntryDataOffset, SeekOrigin.Begin);
                                dpkStream.CopyStreamTo(outFileStream, dpkStructure.EntryDataSize, false);
                            }

                            using (BinaryReader outFileReader = new BinaryReader(File.Open(currentFile, FileMode.Open, FileAccess.Read)))
                            {
                                realExtn = SharedMethods.GetFileHeader(outFileReader);
                            }

                            File.Move(currentFile, currentFile + realExtn);

                            if (realExtn == ".lz0")
                            {
                                var dcmpLz0Data = Lz0Decompression.ProcessLz0Data(currentFile + realExtn);

                                File.WriteAllBytes(currentFile, dcmpLz0Data);
                                File.Delete(currentFile + realExtn);

                                using (BinaryReader dcmpLz0Reader = new BinaryReader(File.Open(currentFile, FileMode.Open, FileAccess.Read)))
                                {
                                    realExtn = SharedMethods.GetFileHeader(dcmpLz0Reader);
                                }

                                File.Move(currentFile, currentFile + realExtn);
                            }

                            LoggingMethods.LogMessage($"Extracted '{fname}{fileCount}'");

                            realExtn = string.Empty;

                            intialOffset += 32;
                            fileCount++;
                        }
                    }
                }

                if (isSingleFile)
                {
                    LoggingMethods.LogMessage(SharedMethods.NewLineChara);
                    LoggingMethods.LogMessage("Extraction has completed!");
                    LoggingMethods.LogMessage(SharedMethods.NewLineChara);

                    SharedMethods.AppMsgBox("Extracted " + Path.GetFileName(dpkFile) + " file", "Success", MessageBoxIcon.Information);
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