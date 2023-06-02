using Drakengard1and2Extractor.Libraries;
using System;
using System.IO;
using System.Windows.Forms;

namespace Drakengard1and2Extractor.Tools
{
    internal class FileDPK
    {
        public static void ExtractDPK(string dpkFile, bool isSingleFile)
        {
            try
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
                        var fileCount = 1;


                        uint intialOffset = 48;
                        var fname = "FILE_";
                        var rExtn = "";
                        for (int f = 0; f < entries; f++)
                        {
                            dpkReader.BaseStream.Position = intialOffset;
                            var fileSize = dpkReader.ReadUInt32();

                            dpkReader.BaseStream.Position = intialOffset + 8;
                            var fileStart = dpkReader.ReadUInt32();

                            using (FileStream outFileStream = new FileStream(extractDir + "/" + fname + $"{fileCount}", FileMode.OpenOrCreate, FileAccess.ReadWrite))
                            {
                                dpkStream.CopyTo(outFileStream, fileStart, fileSize);
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
            catch (Exception ex)
            {
                CmnMethods.AppMsgBox("" + ex, "Error", MessageBoxIcon.Error);
            }
        }
    }
}