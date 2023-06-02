using Drakengard1and2Extractor.Libraries;
using System.IO;
using System.Windows.Forms;

namespace Drakengard1and2Extractor.Tools
{
    public class FileDPK
    {
        public static void ExtractDPK(string dpkFile, bool isSingleFile)
        {
            var extractDir = Path.GetFullPath(dpkFile) + "_extracted";
            CmnMethods.FileDirectoryExistsDel(extractDir, CmnMethods.DelSwitch.folder);
            Directory.CreateDirectory(extractDir);

            using (FileStream dpkStream = new FileStream(dpkFile, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader dpkReader = new BinaryReader(dpkStream))
                {
                    dpkReader.BaseStream.Position = 16;
                    var entries = dpkReader.ReadUInt32();
                    int fileCount = 1;


                    int intialOffset = 48;
                    string fname = "FILE_";
                    string rExtn = "";
                    for (int f = 0; f < entries; f++)
                    {
                        dpkReader.BaseStream.Position = intialOffset;
                        var fileSize = dpkReader.ReadUInt32();
                        dpkReader.BaseStream.Position = intialOffset + 8;
                        var fileStart = dpkReader.ReadUInt32();

                        dpkStream.Seek(fileStart, SeekOrigin.Begin);
                        using (FileStream outFileStream = new FileStream(extractDir + "/" + fname + $"{fileCount}", FileMode.OpenOrCreate, FileAccess.ReadWrite))
                        {
                            byte[] outFilebuffer = new byte[fileSize];
                            var outFileDataToCopy = dpkStream.Read(outFilebuffer, 0, outFilebuffer.Length);
                            outFileStream.Write(outFilebuffer, 0, outFileDataToCopy);
                        }

                        var currentFile = extractDir + "/" + fname + $"{fileCount}";
                        using (FileStream extractedOutFileStream = new FileStream(currentFile, FileMode.Open, FileAccess.Read))
                        {
                            using (BinaryReader extractedOutFileReader = new BinaryReader(extractedOutFileStream))
                            {
                                CmnMethods.GetFileHeader(extractedOutFileReader, ref rExtn);
                            }
                        }
                        File.Move(currentFile, currentFile + rExtn);

                        rExtn = "";

                        intialOffset += 32;
                        fileCount++;
                    }
                }
            }

            if (isSingleFile.Equals(true))
            {
                CmnMethods.AppMsgBox("Extracted " + Path.GetFileName(dpkFile) + " file", "Success", MessageBoxIcon.Information);
            }
        }
    }
}