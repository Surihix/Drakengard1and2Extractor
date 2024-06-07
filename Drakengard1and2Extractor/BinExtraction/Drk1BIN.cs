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

                var extractDir = Path.GetFullPath(mainBinFile) + "_extracted";
                CommonMethods.IfFileDirExistsDel(extractDir, CommonMethods.DelSwitch.folder);
                Directory.CreateDirectory(extractDir);

                var mainBinName = Path.GetFileName(mainBinFile);

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

                                if (mainBinName == "image.bin" || mainBinFile == "IMAGE.BIN")
                                {
                                    fExtn = "";
                                }

                                using (FileStream outFileStream = new FileStream(extractDir + "/" + fname + $"{fileCount}" + fExtn, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                                {
                                    binStream.Seek(fileStart, SeekOrigin.Begin);
                                    binStream.CopyStreamTo(outFileStream, fileSize, false);
                                    binStream.Seek(fileStart, SeekOrigin.Begin);

                                    if (mainBinName == "image.bin" || mainBinFile == "IMAGE.BIN")
                                    {
                                        using (BinaryReader outFileReader = new BinaryReader(outFileStream))
                                        {
                                            CommonMethods.GetFileHeader(outFileReader, ref rExtn);
                                        }
                                    }
                                }

                                if (mainBinName == "image.bin" || mainBinFile == "IMAGE.BIN")
                                {
                                    File.Move(extractDir + "/" + fname + $"{fileCount}" + fExtn, extractDir + "/" + fname + $"{fileCount}" + fExtn + rExtn);

                                    if (rExtn == ".lz0")
                                    {
                                        var currentLz0File = extractDir + "/" + fname + $"{fileCount}" + fExtn + rExtn;
                                        var currentDcmpLz0File = extractDir + "/" + Path.GetFileNameWithoutExtension(extractDir + "/" + fname + $"{fileCount}" + fExtn + rExtn);

                                        var dcmpLz0Data = Lz0Decompression.ProcessLz0Data(currentLz0File);
                                        using (FileStream dcmpLz0Stream = new FileStream(currentDcmpLz0File, FileMode.Append, FileAccess.Write))
                                        {
                                            dcmpLz0Stream.Write(dcmpLz0Data, 0, dcmpLz0Data.Length);
                                        }

                                        File.Delete(currentLz0File);

                                        using (BinaryReader dcmpLz0Reader = new BinaryReader(File.Open(currentDcmpLz0File, FileMode.Open, FileAccess.Read)))
                                        {
                                            CommonMethods.GetFileHeader(dcmpLz0Reader, ref rExtn);
                                        }

                                        File.Move(currentDcmpLz0File, currentDcmpLz0File + rExtn);
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