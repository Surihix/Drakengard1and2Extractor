using System;
using System.IO;
using System.Windows.Forms;

namespace Drakengard1and2Extractor
{
    public class Drk2BIN
    {
        public static void ExtractBin(string MainBinFile)
        {
            try
            {
                var Extract_dir = Path.GetFullPath(MainBinFile) + "_extracted";
                if (Directory.Exists(Extract_dir))
                {
                    Directory.Delete(Extract_dir, true);
                }
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
                                        GetFileHeader(SplitFileReader, ref RExt);
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

                CoreForm.AppMsgBox("Extracted " + Path.GetFileName(MainBinFile) + " file", "Success", MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                CoreForm.AppMsgBox(ex.Message, "Error", MessageBoxIcon.Error);
            }
        }


        private static void GetFileHeader(BinaryReader ReaderName, ref string RExtVar)
        {
            ReaderName.BaseStream.Position = 0;
            var FoundExt = ReaderName.ReadChars(4);
            string RealExt = string.Join("", FoundExt).Replace("\0", "");

            switch (RealExt)
            {
                case "fpk":
                    RExtVar = ".fpk";
                    break;
                case "dpk":
                    RExtVar = ".dpk";
                    break;
                case "wZIM":
                    RExtVar = ".zim";
                    break;
                case "V3a":
                    RExtVar = ".lz0";
                    break;
                case "KPS_":
                    RExtVar = ".kps";
                    break;
                case "kvm1":
                    RExtVar = ".kvm";
                    break;
                case "SPK0":
                    RExtVar = ".spk0";
                    break;
            }
        }
    }
}