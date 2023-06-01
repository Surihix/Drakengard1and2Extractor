using System.IO;
using System.Windows.Forms;

namespace Drakengard1and2Extractor.AppClasses
{
    public class Drk2BIN
    {
        public static void ExtractBin(string mainBinFile)
        {
            var extractDir = Path.GetFullPath(mainBinFile) + "_extracted";
            CmnMethods.FileDirectoryExistsDel(extractDir, CmnMethods.DelSwitch.folder);
            Directory.CreateDirectory(extractDir);

            using (FileStream mainBinStream = new FileStream(mainBinFile, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader mainBinReader = new BinaryReader(mainBinStream))
                {
                    mainBinReader.BaseStream.Position = 16;
                    var entries = mainBinReader.ReadUInt32();
                    int fileCount = 1;


                    int intialOffset = 48;
                    string fname = "FILE_";
                    string rExtn = "";
                    for (int f = 0; f < entries; f++)
                    {
                        mainBinReader.BaseStream.Position = intialOffset;
                        var fileSize = mainBinReader.ReadUInt32();
                        mainBinReader.BaseStream.Position = intialOffset + 8;
                        var fileStart = mainBinReader.ReadUInt32();

                        string fExtn = "";

                        if (mainBinFile.Contains("D_BGM.BIN") || mainBinFile.Contains("D_VOICE.BIN") ||
                            mainBinFile.Contains("d_bgm.bin") || mainBinFile.Contains("d_voice.bin"))
                        {
                            string audioExtn = ".cads";
                            fExtn = audioExtn;
                        }
                        if (mainBinFile.Contains("D_MOVIE.BIN") || mainBinFile.Contains("d_movie.bin"))
                        {
                            string fmvExtn = ".pss";
                            fExtn = fmvExtn;
                        }

                        mainBinStream.Seek(fileStart, SeekOrigin.Begin);
                        using (FileStream outFileStream = new FileStream(extractDir + "/" + fname + $"{fileCount}" + fExtn, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                        {
                            byte[] outFilebuffer = new byte[fileSize];
                            var outFileDataToCopy = mainBinStream.Read(outFilebuffer, 0, outFilebuffer.Length);
                            outFileStream.Write(outFilebuffer, 0, outFileDataToCopy);
                        }

                        if (mainBinFile.Contains("d_image.bin") || mainBinFile.Contains("D_IMAGE.BIN"))
                        {
                            var currentFile = extractDir + "/" + fname + $"{fileCount}" + fExtn;
                            using (FileStream extractedOutFileStream = new FileStream(currentFile, FileMode.Open, FileAccess.Read))
                            {
                                using (BinaryReader extractedOutFileReader = new BinaryReader(extractedOutFileStream))
                                {
                                    CmnMethods.GetFileHeader(extractedOutFileReader, ref rExtn);
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

            CmnMethods.AppMsgBox("Extracted " + Path.GetFileName(mainBinFile) + " file", "Success", MessageBoxIcon.Information);
        }
    }
}