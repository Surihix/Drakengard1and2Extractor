using Drakengard1and2Extractor.Support;
using Drakengard1and2Extractor.Support.Lz0Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Drakengard1and2Extractor.BinExtraction
{
    internal class Drk2BIN
    {
        private static readonly Dictionary<string, string> _BinExtnsDict = new Dictionary<string, string>()
        {
            { "d_bgm.bin", ".cads" },
            { "d_movie.bin", ".pss" },
            { "d_voice.bin", ".cads" }
        };

        public static void ExtractBin(string mainBinFile)
        {
            try
            {
                LoggingMethods.LogMessage(CommonMethods.NewLineChara);

                var extractDir = Path.GetFullPath(mainBinFile) + "_extracted";
                CommonMethods.IfFileDirExistsDel(extractDir, CommonMethods.DelSwitch.directory);
                Directory.CreateDirectory(extractDir);

                var mainBinName = Path.GetFileName(mainBinFile);
                var isdImageBin = mainBinName == "d_image.bin" || mainBinName == "D_IMAGE.BIN";

                var fExtn = string.Empty;
                if (_BinExtnsDict.ContainsKey(mainBinName.ToLower()))
                {
                    fExtn = _BinExtnsDict[mainBinName.ToLower()];
                }

                var dpkStructure = new CommonStructures.DPK();

                using (FileStream mainBinStream = new FileStream(mainBinFile, FileMode.Open, FileAccess.Read))
                {
                    using (BinaryReader mainBinReader = new BinaryReader(mainBinStream))
                    {
                        mainBinReader.BaseStream.Position = 16;
                        dpkStructure.EntryCount = mainBinReader.ReadUInt32();


                        uint intialOffset = 48;
                        var fname = "FILE_";
                        var realExtn = string.Empty;
                        var fileCount = 1;
                        for (int f = 0; f < dpkStructure.EntryCount; f++)
                        {
                            mainBinReader.BaseStream.Position = intialOffset;
                            dpkStructure.EntryDataSize = mainBinReader.ReadUInt32();

                            mainBinReader.BaseStream.Position = intialOffset + 8;
                            dpkStructure.EntryDataOffset = mainBinReader.ReadUInt32();

                            var currentFile = Path.Combine(extractDir, fname + $"{fileCount}" + fExtn);

                            using (FileStream outFileStream = new FileStream(currentFile, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                            {
                                mainBinStream.Seek(dpkStructure.EntryDataOffset, SeekOrigin.Begin);
                                mainBinStream.CopyStreamTo(outFileStream, dpkStructure.EntryDataSize, false);
                            }

                            if (isdImageBin)
                            {
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

                                realExtn = string.Empty;
                            }

                            LoggingMethods.LogMessage($"Extracted '{fname}{fileCount}'");

                            intialOffset += 32;
                            fileCount++;
                        }
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