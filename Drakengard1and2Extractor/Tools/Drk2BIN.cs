using Drakengard1and2Extractor.Support;
using System;
using System.IO;
using System.Windows.Forms;

namespace Drakengard1and2Extractor.Tools
{
    internal class Drk2BIN
    {
        public static void ExtractBin(string mainBinFile)
        {
            try
            {
                var extractDir = Path.GetFullPath(mainBinFile) + "_extracted";
                CommonMethods.IfFileDirExistsDel(extractDir, CommonMethods.DelSwitch.folder);
                Directory.CreateDirectory(extractDir);

                using (FileStream mainBinStream = new FileStream(mainBinFile, FileMode.Open, FileAccess.Read))
                {
                    using (BinaryReader mainBinReader = new BinaryReader(mainBinStream))
                    {
                        mainBinReader.BaseStream.Position = 16;
                        var entries = mainBinReader.ReadUInt32();
                        var fileCount = 1;


                        uint intialOffset = 48;
                        var fname = "FILE_";
                        var rExtn = "";
                        for (int f = 0; f < entries; f++)
                        {
                            mainBinReader.BaseStream.Position = intialOffset;
                            var fileSize = mainBinReader.ReadUInt32();

                            mainBinReader.BaseStream.Position = intialOffset + 8;
                            var fileStart = mainBinReader.ReadUInt32();

                            var fExtn = "";

                            if (mainBinFile.Contains("D_BGM.BIN") || mainBinFile.Contains("D_VOICE.BIN") ||
                                mainBinFile.Contains("d_bgm.bin") || mainBinFile.Contains("d_voice.bin"))
                            {
                                var audioExtn = ".cads";
                                fExtn = audioExtn;
                            }
                            if (mainBinFile.Contains("D_MOVIE.BIN") || mainBinFile.Contains("d_movie.bin"))
                            {
                                var fmvExtn = ".pss";
                                fExtn = fmvExtn;
                            }

                            using (FileStream outFileStream = new FileStream(extractDir + "/" + fname + $"{fileCount}" + fExtn, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                            {
                                mainBinStream.Seek(fileStart, SeekOrigin.Begin);
                                mainBinStream.CopyStreamTo(outFileStream, fileSize, false);
                            }

                            if (mainBinFile.Contains("d_image.bin") || mainBinFile.Contains("D_IMAGE.BIN"))
                            {
                                var currentFile = extractDir + "/" + fname + $"{fileCount}" + fExtn;
                                using (FileStream extractedOutFileStream = new FileStream(currentFile, FileMode.Open, FileAccess.Read))
                                {
                                    using (BinaryReader extractedOutFileReader = new BinaryReader(extractedOutFileStream))
                                    {
                                        CommonMethods.GetFileHeader(extractedOutFileReader, ref rExtn);
                                    }
                                }
                                File.Move(currentFile, currentFile + rExtn);
                                rExtn = "";
                            }

                            intialOffset += 32;
                            fileCount++;
                        }
                    }
                }

                CommonMethods.AppMsgBox("Extracted " + Path.GetFileName(mainBinFile) + " file", "Success", MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                CommonMethods.AppMsgBox("" + ex, "Error", MessageBoxIcon.Error);
            }
        }
    }
}