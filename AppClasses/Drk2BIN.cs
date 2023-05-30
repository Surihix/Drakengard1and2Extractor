using System.IO;
using System.Windows.Forms;

namespace Drakengard1and2Extractor.AppClasses
{
    public class Drk2BIN
    {
        public static void ExtractBin(string MainBinFile)
        {
            var Extract_dir = Path.GetFullPath(MainBinFile) + "_extracted";
            CmnMethods.FileDirectoryExistsDel(Extract_dir, CmnMethods.DelSwitch.folder);
            Directory.CreateDirectory(Extract_dir);

            using (FileStream MainBIN = new FileStream(MainBinFile, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader MainBINReader = new BinaryReader(MainBIN))
                {
                    MainBINReader.BaseStream.Position = 16;
                    var Entries = MainBINReader.ReadUInt32();
                    int FileCount = 1;


                    int IntialOffsetPos = 48;
                    string fname = "FILE_";
                    string RExt = "";
                    for (int f = 0; f < Entries; f++)
                    {
                        MainBINReader.BaseStream.Position = IntialOffsetPos;
                        var fileSize = MainBINReader.ReadUInt32();
                        MainBINReader.BaseStream.Position = IntialOffsetPos + 8;
                        var fileStart = MainBINReader.ReadUInt32();

                        string fExt = "";

                        if (MainBinFile.Contains("D_BGM.BIN") || MainBinFile.Contains("D_VOICE.BIN") ||
                            MainBinFile.Contains("d_bgm.bin") || MainBinFile.Contains("d_voice.bin"))
                        {
                            string AudioExt = ".cads";
                            fExt = AudioExt;
                        }
                        if (MainBinFile.Contains("D_MOVIE.BIN") || MainBinFile.Contains("d_movie.bin"))
                        {
                            string FMVExt = ".pss";
                            fExt = FMVExt;
                        }

                        MainBIN.Seek(fileStart, SeekOrigin.Begin);
                        using (FileStream SplitFileOut = new FileStream(Extract_dir + "/" + fname + $"{FileCount}" + fExt,
                            FileMode.OpenOrCreate, FileAccess.ReadWrite))
                        {
                            byte[] SplitFileBuffer = new byte[fileSize];
                            var BytesToRead = MainBIN.Read(SplitFileBuffer, 0, SplitFileBuffer.Length);
                            SplitFileOut.Write(SplitFileBuffer, 0, BytesToRead);
                        }

                        if (MainBinFile.Contains("d_image.bin") || MainBinFile.Contains("D_IMAGE.BIN"))
                        {
                            var CurrentFile = Extract_dir + "/" + fname + $"{FileCount}" + fExt;
                            using (FileStream SplitFile = new FileStream(CurrentFile, FileMode.Open, FileAccess.Read))
                            {
                                using (BinaryReader SplitFileReader = new BinaryReader(SplitFile))
                                {
                                    CmnMethods.GetFileHeader(SplitFileReader, ref RExt);
                                }
                            }
                            File.Move(CurrentFile, CurrentFile + RExt);
                            RExt = "";
                        }

                        IntialOffsetPos += 32;
                        FileCount++;
                    }
                }
            }

            CmnMethods.AppMsgBox("Extracted " + Path.GetFileName(MainBinFile) + " file", "Success", MessageBoxIcon.Information);
        }
    }
}