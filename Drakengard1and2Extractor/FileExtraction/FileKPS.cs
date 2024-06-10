using Drakengard1and2Extractor.Support;
using Drakengard1and2Extractor.Support.LoggingHelpers;
using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Drakengard1and2Extractor.FileExtraction
{
    internal class FileKPS
    {
        public static void ExtractKPS(string kpsFile, bool shiftJISParse, bool isSingleFile)
        {
            try
            {
                var outTxtFile = Path.Combine(Path.GetDirectoryName(kpsFile), Path.GetFileNameWithoutExtension(kpsFile) + ".txt");

                using (var kpsReader = new BinaryReader(File.Open(kpsFile, FileMode.Open, FileAccess.Read)))
                {
                    kpsReader.BaseStream.Position = 20;
                    var linesOffsetTable = kpsReader.ReadUInt32();
                    var linesCount = kpsReader.ReadUInt32();

                    using (var outTxtStream = new MemoryStream())
                    {
                        using (var outTxtBinWriter = new BinaryWriter(outTxtStream, Encoding.UTF8))
                        {
                            outTxtBinWriter.Write(Encoding.UTF8.GetBytes($"Lines: {linesCount}"));
                            outTxtBinWriter.Write(BitConverter.GetBytes((ushort)2573));
                            outTxtBinWriter.Write(BitConverter.GetBytes((ushort)2573));

                            var readPos = linesOffsetTable;
                            var lineCounter = 1;
                            var currentLineLength = 0;
                            var writePos = (int)outTxtBinWriter.BaseStream.Length;
                            for (int l = 0; l < linesCount; l++)
                            {
                                kpsReader.BaseStream.Position = readPos;
                                var currentLineOffset = kpsReader.ReadUInt32();
                                var currentLineNoData = Encoding.UTF8.GetBytes($"Line {lineCounter} |:| ");

                                if (l == linesCount - 1)
                                {
                                    currentLineLength = (int)kpsReader.BaseStream.Length - (int)currentLineOffset;
                                }
                                else
                                {
                                    var nextLineStart = kpsReader.ReadUInt32();
                                    currentLineLength = (int)nextLineStart - (int)currentLineOffset;
                                }


                                kpsReader.BaseStream.Position = currentLineOffset;
                                var currentLineData = kpsReader.ReadBytes(currentLineLength);

                                var outLineData = new byte[] { };
                                if (shiftJISParse)
                                {
                                    var shiftJISData = Encoding.Convert(Encoding.GetEncoding(1252), Encoding.GetEncoding(932), currentLineData);
                                    outLineData = Encoding.Convert(Encoding.GetEncoding(932), Encoding.UTF8, shiftJISData);
                                }
                                else
                                {
                                    outLineData = Encoding.Convert(Encoding.GetEncoding(1252), Encoding.UTF8, currentLineData);
                                }

                                outTxtBinWriter.BaseStream.Position = writePos;
                                outTxtBinWriter.Write(currentLineNoData);
                                outTxtBinWriter.Write(outLineData);
                                outTxtBinWriter.Write(BitConverter.GetBytes((ushort)2573));

                                readPos += 4;
                                lineCounter++;
                                writePos += outLineData.Length + 2 + currentLineNoData.Length;
                            }

                            CommonMethods.IfFileDirExistsDel(outTxtFile, CommonMethods.DelSwitch.file);

                            outTxtStream.Seek(0, SeekOrigin.Begin);
                            File.WriteAllBytes(outTxtFile, outTxtStream.ToArray());
                        }
                    }
                }

                if (isSingleFile)
                {
                    CoreFormLogHelpers.LogMessage(CoreForm.NewLineChara);
                    CoreFormLogHelpers.LogMessage("Extraction has completed!");
                    CoreFormLogHelpers.LogMessage(CoreForm.NewLineChara);

                    CommonMethods.AppMsgBox("Extracted " + Path.GetFileName(kpsFile) + " file", "Success", MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                CommonMethods.AppMsgBox("" + ex, "Error", MessageBoxIcon.Error);
                CoreFormLogHelpers.LogMessage(CoreForm.NewLineChara);
                CoreFormLogHelpers.LogException("Exception: " + ex);
            }
        }
    }
}