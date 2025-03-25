using Drakengard1and2Extractor.Support;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Drakengard1and2Extractor.FileExtraction
{
    internal class ExtKPS
    {
        private static readonly byte[] _NewLineBytes = new byte[] { 0x0D, 0x0A };

        private static readonly Encoding _ShiftJISEncoding = Encoding.GetEncoding(932);
        private static readonly Encoding _ANSIEncoding = Encoding.GetEncoding(1252);

        public static void ExtractKPS(string kpsFile, bool shiftJISParse, bool isSingleFile)
        {
            try
            {
                var outTxtFile = Path.Combine(Path.GetDirectoryName(kpsFile), Path.GetFileNameWithoutExtension(kpsFile) + ".txt");

                using (var kpsReader = new BinaryReader(File.Open(kpsFile, FileMode.Open, FileAccess.Read)))
                {
                    kpsReader.BaseStream.Position = 4;
                    var kpsDateVersion = kpsReader.ReadStringTillNull();

                    kpsReader.BaseStream.Position = 20;
                    var linesOffsetTable = kpsReader.ReadUInt32();
                    var linesCount = kpsReader.ReadUInt32();

                    using (var outTxtStream = new MemoryStream())
                    {
                        using (var outTxtBinWriter = new BinaryWriter(outTxtStream))
                        {
                            outTxtBinWriter.Write(Encoding.UTF8.GetBytes($"Lines: {linesCount}"));
                            outTxtBinWriter.Write(_NewLineBytes);
                            outTxtBinWriter.Write(Encoding.UTF8.GetBytes($"Date and Version: {kpsDateVersion}"));
                            outTxtBinWriter.Write(_NewLineBytes);
                            outTxtBinWriter.Write(_NewLineBytes);
                            outTxtBinWriter.Write(_NewLineBytes);


                            var readPos = linesOffsetTable;
                            var lineCounter = 1;
                            var penUltimateLine = linesCount - 1;
                            var currentLineLength = 0;
                            var writePos = (int)outTxtBinWriter.BaseStream.Length;
                            for (int l = 0; l < linesCount; l++)
                            {
                                kpsReader.BaseStream.Position = readPos;
                                var currentLineOffset = kpsReader.ReadUInt32();
                                var currentLineNoData = Encoding.UTF8.GetBytes($"Line {lineCounter} |:| ");

                                if (l == penUltimateLine)
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

                                var outLineData = ParseLineData(shiftJISParse, currentLineData);

                                outTxtBinWriter.BaseStream.Position = writePos;
                                outTxtBinWriter.Write(currentLineNoData);
                                outTxtBinWriter.Write(outLineData);
                                outTxtBinWriter.Write(_NewLineBytes);
                                outTxtBinWriter.Write(_NewLineBytes);

                                readPos += 4;
                                lineCounter++;
                                writePos += outLineData.Length + 4 + currentLineNoData.Length;
                            }

                            SharedMethods.IfFileDirExistsDel(outTxtFile, SharedMethods.DelSwitch.file);

                            outTxtStream.Seek(0, SeekOrigin.Begin);
                            File.WriteAllBytes(outTxtFile, outTxtStream.ToArray());
                        }
                    }
                }

                if (isSingleFile)
                {
                    LoggingMethods.LogMessage(SharedMethods.NewLineChara);
                    LoggingMethods.LogMessage("Extraction has completed!");
                    LoggingMethods.LogMessage(SharedMethods.NewLineChara);

                    SharedMethods.AppMsgBox("Extracted " + Path.GetFileName(kpsFile) + " file", "Success", MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                SharedMethods.AppMsgBox("" + ex, "Error", MessageBoxIcon.Error);
                LoggingMethods.LogMessage(SharedMethods.NewLineChara);
                LoggingMethods.LogException("Exception: " + ex);
            }
        }


        private static byte[] ParseLineData(bool shiftJISParse, byte[] currentLineData)
        {
            byte[] outLineData;
            var processedLineData = new List<byte>();

            if (shiftJISParse)
            {
                // Check and decode unreadable
                // bytes in shift-jis encoding
                for (int d = 0; d < currentLineData.Length; d++)
                {
                    if (d != currentLineData.Length - 1)
                    {
                        var b1 = currentLineData[d];
                        var b2 = currentLineData[d + 1];

                        var isShiftJIS2Chara = Check2CharaShiftJIS(b1, b2);

                        if (isShiftJIS2Chara)
                        {
                            processedLineData.Add(b1);
                            processedLineData.Add(b2);
                            d++;
                        }
                        else
                        {
                            Process1CharaShiftJIS(b1, ref processedLineData);
                        }
                    }
                    else
                    {
                        Process1CharaShiftJIS(currentLineData[d], ref processedLineData);
                    }
                }

                currentLineData = processedLineData.ToArray();
                outLineData = Encoding.Convert(_ShiftJISEncoding, Encoding.UTF8, currentLineData);
            }
            else
            {
                // Check and decode unreadable
                // bytes in ANSI encoding
                for (int d = 0; d < currentLineData.Length; d++)
                {
                    var b = currentLineData[d];
                    var isAnsiChara = CheckAnsiChara(b);
                    if (isAnsiChara)
                    {
                        processedLineData.Add(b);
                    }
                    else
                    {
                        processedLineData.AddRange(GetNonStringByteInHex(_ANSIEncoding, b));
                    }
                }

                currentLineData = processedLineData.ToArray();
                outLineData = Encoding.Convert(_ANSIEncoding, Encoding.UTF8, currentLineData);
            }

            return outLineData;
        }

        private static void Process1CharaShiftJIS(byte b, ref List<byte> processedLineData)
        {
            var isShiftJIS1Chara = Check1CharaShiftJIS(b);

            if (isShiftJIS1Chara)
            {
                processedLineData.Add(b);
            }
            else
            {
                processedLineData.AddRange(GetNonStringByteInHex(_ShiftJISEncoding, b));
            }
        }

        private static byte[] GetNonStringByteInHex(Encoding encodingToUse, byte b)
        {
            return encodingToUse.GetBytes("#" + b.ToString("X2") + "# ");
        }


        #region Character Checks
        private static bool Check2CharaShiftJIS(byte b1, byte b2)
        {
            var isChara = new bool();
            var isChecked = new bool();

            // Range 1
            switch (b1)
            {
                case 0x81:
                    if (b2 >= 0x40 && b2 <= 0x7E && !isChecked)
                    {
                        isChara = true;
                        isChecked = true;
                    }
                    if (b2 >= 0x80 && b2 <= 0xAC && !isChecked)
                    {
                        isChara = true;
                        isChecked = true;
                    }
                    if (b2 >= 0xB8 && b2 <= 0xBF && !isChecked)
                    {
                        isChara = true;
                        isChecked = true;
                    }
                    if (b2 >= 0xC8 && b2 <= 0xCE && !isChecked)
                    {
                        isChara = true;
                        isChecked = true;
                    }
                    if (b2 >= 0xDA && b2 <= 0xE8 && !isChecked)
                    {
                        isChara = true;
                        isChecked = true;
                    }
                    if (b2 >= 0xF0 && b2 <= 0xF7 && !isChecked)
                    {
                        isChara = true;
                        isChecked = true;
                    }
                    if (b2 == 0xFC && !isChecked)
                    {
                        isChara = true;
                        isChecked = true;
                    }
                    break;

                case 0x82:
                    if (b2 >= 0x4F && b2 <= 0x58 && !isChecked)
                    {
                        isChara = true;
                        isChecked = true;
                    }
                    if (b2 >= 0x60 && b2 <= 0x79 && !isChecked)
                    {
                        isChara = true;
                        isChecked = true;
                    }
                    if (b2 >= 0x81 && b2 <= 0x9A && !isChecked)
                    {
                        isChara = true;
                        isChecked = true;
                    }
                    if (b2 >= 0x9F && b2 <= 0xF1 && !isChecked)
                    {
                        isChara = true;
                        isChecked = true;
                    }
                    break;

                case 0x83:
                    if (b2 >= 0x40 && b2 <= 0x7E && !isChecked)
                    {
                        isChara = true;
                        isChecked = true;
                    }
                    if (b2 >= 0x80 && b2 <= 0x96 && !isChecked)
                    {
                        isChara = true;
                        isChecked = true;
                    }
                    if (b2 >= 0x9F && b2 <= 0xB6 && !isChecked)
                    {
                        isChara = true;
                        isChecked = true;
                    }
                    if (b2 >= 0xBF && b2 <= 0xD6 && !isChecked)
                    {
                        isChara = true;
                        isChecked = true;
                    }
                    break;

                case 0x84:
                    if (b2 >= 0x40 && b2 <= 0x60 && !isChecked)
                    {
                        isChara = true;
                        isChecked = true;
                    }
                    if (b2 >= 0x70 && b2 <= 0x7E && !isChecked)
                    {
                        isChara = true;
                        isChecked = true;
                    }
                    if (b2 >= 0x80 && b2 <= 0x91 && !isChecked)
                    {
                        isChara = true;
                        isChecked = true;
                    }
                    if (b2 >= 0x9F && b2 <= 0xBE && !isChecked)
                    {
                        isChara = true;
                        isChecked = true;
                    }
                    break;

                case 0x87:
                    if (b2 >= 0x40 && b2 <= 0x5D && !isChecked)
                    {
                        isChara = true;
                        isChecked = true;
                    }
                    if (b2 >= 0x5F && b2 <= 0x75 && !isChecked)
                    {
                        isChara = true;
                        isChecked = true;
                    }
                    if (b2 == 0x7E && !isChecked)
                    {
                        isChara = true;
                        isChecked = true;
                    }
                    if (b2 >= 0x80 && b2 <= 0x8F && !isChecked)
                    {
                        isChara = true;
                        isChecked = true;
                    }
                    if (b2 >= 0x93 && b2 <= 0x94 && !isChecked)
                    {
                        isChara = true;
                        isChecked = true;
                    }
                    if (b2 >= 0x98 && b2 <= 0x99 && !isChecked)
                    {
                        isChara = true;
                        isChecked = true;
                    }
                    break;

                case 0x88:
                    if (b2 >= 0x9F && b2 <= 0xFC && !isChecked)
                    {
                        isChara = true;
                        isChecked = true;
                    }
                    break;

                case 0x98:
                    if (b2 >= 0x40 && b2 <= 0x72 && !isChecked)
                    {
                        isChara = true;
                        isChecked = true;
                    }
                    if (b2 >= 0x9F && b2 <= 0xFC && !isChecked)
                    {
                        isChara = true;
                        isChecked = true;
                    }
                    break;

                case 0xEA:
                    if (b2 >= 0x40 && b2 <= 0x7E && !isChecked)
                    {
                        isChara = true;
                        isChecked = true;
                    }
                    if (b2 >= 0x80 && b2 <= 0xA4 && !isChecked)
                    {
                        isChara = true;
                        isChecked = true;
                    }
                    break;

                case 0xFA:
                    if (b2 >= 0x40 && b2 <= 0x49 && !isChecked)
                    {
                        isChara = true;
                        isChecked = true;
                    }
                    if (b2 >= 0x55 && b2 <= 0x57 && !isChecked)
                    {
                        isChara = true;
                        isChecked = true;
                    }
                    if (b2 >= 0x5C && b2 <= 0x7E && !isChecked)
                    {
                        isChara = true;
                        isChecked = true;
                    }
                    if (b2 >= 0x80 && b2 <= 0xFC && !isChecked)
                    {
                        isChara = true;
                        isChecked = true;
                    }
                    break;

                case 0xFB:
                    if (b2 >= 0x40 && b2 <= 0x7E && !isChecked)
                    {
                        isChara = true;
                        isChecked = true;
                    }
                    if (b2 >= 0x80 && b2 <= 0xFC && !isChecked)
                    {
                        isChara = true;
                        isChecked = true;
                    }
                    break;

                case 0xFC:
                    if (b2 >= 0x40 && b2 <= 0x4B && !isChecked)
                    {
                        isChara = true;
                        isChecked = true;
                    }
                    break;
            }

            // Range 2
            if (b1 >= 0x89 && b1 <= 0x97 && !isChecked)
            {
                if (b2 >= 0x40 && b2 <= 0x7E && !isChecked)
                {
                    isChara = true;
                    isChecked = true;
                }
                if (b2 >= 0x80 && b2 <= 0xFC && !isChecked)
                {
                    isChara = true;
                    isChecked = true;
                }
            }

            // Range 3
            if (b1 >= 0x99 && b1 <= 0x9F && !isChecked)
            {
                if (b2 >= 0x40 && b2 <= 0x7E && !isChecked)
                {
                    isChara = true;
                    isChecked = true;
                }
                if (b2 >= 0x80 && b2 <= 0xFC && !isChecked)
                {
                    isChara = true;
                    isChecked = true;
                }
            }

            // Range 4
            if (b1 >= 0xE0 && b1 <= 0xE9 && !isChecked)
            {
                if (b2 >= 0x40 && b2 <= 0x7E && !isChecked)
                {
                    isChara = true;
                    isChecked = true;
                }
                if (b2 >= 0x80 && b2 <= 0xFC && !isChecked)
                {
                    isChara = true;
                }
            }

            return isChara;
        }

        private static bool Check1CharaShiftJIS(byte b)
        {
            var isChara = new bool();
            var isChecked = new bool();

            if (b >= 0x20 && b <= 0x7E)
            {
                isChara = true;
                isChecked = true;
            }

            if (b >= 0xA1 && b <= 0xDF && !isChecked)
            {
                isChara = true;
            }

            return isChara;
        }

        private static bool CheckAnsiChara(byte b)
        {
            var isChara = new bool();
            var isChecked = new bool();

            if (b >= 0x20 && b <= 0x7E)
            {
                isChara = true;
                isChecked = true;
            }

            if (b == 0x80 && !isChecked)
            {
                isChara = true;
                isChecked = true;
            }

            if (b >= 0x82 && b <= 0x8C && !isChecked)
            {
                isChara = true;
                isChecked = true;
            }

            if (b == 0x8E && !isChecked)
            {
                isChara = true;
                isChecked = true;
            }

            if (b >= 0x91 && b <= 0x9C && !isChecked)
            {
                isChara = true;
                isChecked = true;
            }

            if (b >= 0x9E && b <= 0xFF && !isChecked)
            {
                isChara = true;
            }

            return isChara;
        }
        #endregion
    }
}