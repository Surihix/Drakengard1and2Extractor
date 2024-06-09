using Drakengard1and2Extractor.Support;
using Drakengard1and2Extractor.Support.Lz0Helpers;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Drakengard1and2Extractor.FileExtraction
{
    internal class FileFPK
    {
        private static readonly string[] _KnownExtns = { ".fpk", ".dpk", ".zim", ".lz0", ".kps", ".kvm", ".spk0", ".emt", ".dcmr", ".dlgt", ".hi4", ".hd2" };
        public static void ExtractFPK(string fpkFile, bool isSingleFile)
        {
            try
            {
                var extractDir = Path.GetFullPath(fpkFile) + "_extracted";
                CommonMethods.IfFileDirExistsDel(extractDir, CommonMethods.DelSwitch.folder);
                Directory.CreateDirectory(extractDir);

                using (FileStream fpkStream = new FileStream(fpkFile, FileMode.Open, FileAccess.Read))
                {
                    using (BinaryReader fpkReader = new BinaryReader(fpkStream))
                    {
                        fpkReader.BaseStream.Position = 8;
                        var entries = fpkReader.ReadUInt32();

                        fpkReader.BaseStream.Position = 40;
                        var fpkDataStart = fpkReader.ReadUInt32();
                        var fpkDataSize = fpkReader.ReadUInt32();

                        CommonMethods.IfFileDirExistsDel(extractDir + "/_.archive", CommonMethods.DelSwitch.file);

                        using (FileStream fpkDataStream = new FileStream(extractDir + "/_.archive", FileMode.OpenOrCreate, FileAccess.ReadWrite))
                        {
                            fpkStream.Seek(fpkDataStart, SeekOrigin.Begin);
                            fpkStream.CopyStreamTo(fpkDataStream, fpkDataSize, false);


                            uint intialOffset = 132;
                            var fName = "FILE_";
                            var rExtn = "";
                            var fileCount = 1;
                            for (int f = 0; f < entries; f++)
                            {
                                fpkReader.BaseStream.Position = intialOffset;
                                var outFileStart = fpkReader.ReadUInt32();
                                var outFileSize = fpkReader.ReadUInt32();
                                var extnChar = fpkReader.ReadChars(4);
                                Array.Reverse(extnChar);

                                var fileExtn = string.Join("", extnChar).Replace("\0", "");
                                var fExtn = "." + CommonMethods.ModifyExtnString(fileExtn);

                                switch (fileExtn.StartsWith("/") || fileExtn.StartsWith("\\"))
                                {
                                    case true:
                                        break;

                                    case false:
                                        using (FileStream outFileStream = new FileStream(extractDir + "/" + fName + $"{fileCount}" + fExtn, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                                        {
                                            fpkDataStream.Seek(outFileStart, SeekOrigin.Begin);
                                            fpkDataStream.CopyStreamTo(outFileStream, outFileSize, false);

                                            using (BinaryReader outFileReader = new BinaryReader(outFileStream))
                                            {
                                                rExtn = CommonMethods.GetFileHeader(outFileReader);
                                            }
                                        }

                                        File.Move(extractDir + "/" + fName + $"{fileCount}" + fExtn, extractDir + "/" + fName + $"{fileCount}" + fExtn + rExtn);

                                        if (_KnownExtns.Contains(rExtn))
                                        {
                                            if (rExtn == ".lz0")
                                            {
                                                var currentLz0File = extractDir + "/" + fName + $"{fileCount}" + fExtn + rExtn;
                                                var currentDcmpLz0File = extractDir + "/" + Path.GetFileNameWithoutExtension(extractDir + "/" + fName + $"{fileCount}" + fExtn + rExtn);

                                                var dcmpLz0Data = Lz0Decompression.ProcessLz0Data(currentLz0File);
                                                using (FileStream dcmpLz0Stream = new FileStream(currentDcmpLz0File, FileMode.Append, FileAccess.Write))
                                                {
                                                    dcmpLz0Stream.Write(dcmpLz0Data, 0, dcmpLz0Data.Length);
                                                }

                                                File.Delete(currentLz0File);

                                                using (BinaryReader dcmpLz0Reader = new BinaryReader(File.Open(currentDcmpLz0File, FileMode.Open, FileAccess.Read)))
                                                {
                                                    rExtn = CommonMethods.GetFileHeader(dcmpLz0Reader);
                                                }

                                                File.Move(currentDcmpLz0File, extractDir + "/" + fName + $"{fileCount}" + rExtn);
                                            }
                                            else
                                            {
                                                var outFileNameWithoutExtn = Path.GetFileNameWithoutExtension(extractDir + "/" + fName + $"{fileCount}" + fExtn + rExtn);
                                                File.Move(extractDir + "/" + fName + $"{fileCount}" + fExtn + rExtn, extractDir + "/" + outFileNameWithoutExtn);

                                                var outFileNameProperExtn = Path.GetFileNameWithoutExtension(extractDir + "/" + outFileNameWithoutExtn);
                                                var adjExtn = "";
                                                using (FileStream extnStream = new FileStream(extractDir + "/" + outFileNameWithoutExtn, FileMode.Open, FileAccess.Read))
                                                {
                                                    using (BinaryReader extnReader = new BinaryReader(extnStream))
                                                    {
                                                        adjExtn = CommonMethods.GetFileHeader(extnReader);
                                                    }
                                                }

                                                File.Move(extractDir + "/" + outFileNameWithoutExtn, extractDir + "/" + outFileNameProperExtn + adjExtn);
                                            }
                                        }
                                        break;
                                }

                                rExtn = "";

                                intialOffset += 16;
                                fileCount++;
                            }
                        }

                        File.Delete(extractDir + "/_.archive");
                    }
                }

                if (isSingleFile)
                {
                    LoggingHelpers.LogMessage(CoreForm.NewLineChara);
                    LoggingHelpers.LogMessage("Extraction has completed!");
                    LoggingHelpers.LogMessage(CoreForm.NewLineChara);

                    CommonMethods.AppMsgBox("Extracted " + Path.GetFileName(fpkFile) + " file", "Success", MessageBoxIcon.Information);
                }
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