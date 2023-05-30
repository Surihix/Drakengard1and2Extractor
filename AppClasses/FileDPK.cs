using System.IO;
using System.Windows.Forms;

namespace Drakengard1and2Extractor.AppClasses
{
    public class FileDPK
    {
        public static void ExtractDPK(string DpkFile)
        {
            var Extract_dir = Path.GetFullPath(DpkFile) + "_extracted";
            CmnMethods.FileDirectoryExistsDel(Extract_dir, CmnMethods.DelSwitch.folder);
            Directory.CreateDirectory(Extract_dir);

            using (FileStream DpkStream = new FileStream(DpkFile, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader DpkReader = new BinaryReader(DpkStream))
                {
                    DpkReader.BaseStream.Position = 16;
                    var Entries = DpkReader.ReadUInt32();
                    int FileCount = 1;


                    int IntialOffsetPos = 48;
                    string fname = "FILE_";
                    string RExt = "";
                    for (int f = 0; f < Entries; f++)
                    {
                        DpkReader.BaseStream.Position = IntialOffsetPos;
                        var fileSize = DpkReader.ReadUInt32();
                        DpkReader.BaseStream.Position = IntialOffsetPos + 8;
                        var fileStart = DpkReader.ReadUInt32();

                        DpkStream.Seek(fileStart, SeekOrigin.Begin);
                        using (FileStream SplitFileOut = new FileStream(Extract_dir + "/" + fname + $"{FileCount}", FileMode.OpenOrCreate, FileAccess.ReadWrite))
                        {
                            byte[] SplitFileBuffer = new byte[fileSize];
                            var BytesToRead = DpkStream.Read(SplitFileBuffer, 0, SplitFileBuffer.Length);
                            SplitFileOut.Write(SplitFileBuffer, 0, BytesToRead);
                        }

                        var CurrentFile = Extract_dir + "/" + fname + $"{FileCount}";
                        using (FileStream SplitFile = new FileStream(CurrentFile, FileMode.Open, FileAccess.Read))
                        {
                            using (BinaryReader SplitFileReader = new BinaryReader(SplitFile))
                            {
                                CmnMethods.GetFileHeader(SplitFileReader, ref RExt);
                            }
                        }
                        File.Move(CurrentFile, CurrentFile + RExt);

                        RExt = "";

                        IntialOffsetPos += 32;
                        FileCount++;
                    }
                }
            }

            CmnMethods.AppMsgBox("Extracted " + Path.GetFileName(DpkFile) + " file", "Success", MessageBoxIcon.Information);
        }
    }
}