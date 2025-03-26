using Drakengard1and2Extractor.Support;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Drakengard1and2Extractor.FileRepack
{
    internal class RpkKPS
    {
        private static readonly Encoding _ShiftJISEncoding = Encoding.GetEncoding(932);
        private static readonly Encoding _ANSIEncoding = Encoding.GetEncoding(1252);

        public static void RepackKPS(string txtFile, bool shiftJISParse)
        {
            try
            {
                var outKpsFile = Path.Combine(Path.GetDirectoryName(txtFile), Path.GetFileNameWithoutExtension(txtFile) + ".kps");

                using (var txtReader = new StreamReader(txtFile))
                {
                    var hasCoreInfo = false;
                    var kpsDateAndVersion = string.Empty;

                    if (uint.TryParse(txtReader.ReadLine().Split(':')[1].Replace(" ", ""), out uint linesCount) == true)
                    {
                        kpsDateAndVersion = txtReader.ReadLine().Split(':')[1].Substring(1);
                        hasCoreInfo = true;
                    }

                    var hasRepacked = false;

                    if (hasCoreInfo)
                    {
                        _ = txtReader.ReadLine();
                        _ = txtReader.ReadLine();

                        using (var outKpsOffsetStream = new MemoryStream())
                        {
                            using (var outKpsOffsetWriter = new BinaryWriter(outKpsOffsetStream))
                            {
                                outKpsOffsetWriter.BaseStream.Position = 0;
                                outKpsOffsetWriter.Write(_ANSIEncoding.GetBytes("KPS_"));
                                outKpsOffsetWriter.Write(_ANSIEncoding.GetBytes(kpsDateAndVersion + "\0"));
                                outKpsOffsetWriter.Write((uint)0);
                                outKpsOffsetWriter.Write((uint)64);
                                outKpsOffsetWriter.Write(linesCount);
                                outKpsOffsetWriter.BaseStream.PadNull(36);

                                var linesStartOffset = 64 + (linesCount * 4);

                                using (var outKpsStream = new MemoryStream())
                                {
                                    using (var outKpsWriter = new BinaryWriter(outKpsStream))
                                    {
                                        outKpsWriter.BaseStream.PadNull(linesStartOffset);

                                        string[] currentLineSplitData;
                                        string[] splitChara = new string[] { " |:| " };
                                        byte[] currentLineData;
                                        int currentLineLength;
                                        var currentLineDataList = new List<byte>();
                                        byte[] commandCodeRead = new byte[2];
                                        byte commandCode;
                                        long currentOffset = outKpsStream.Position;

                                        for (int l = 0; l < linesCount; l++)
                                        {
                                            outKpsOffsetWriter.Write((uint)currentOffset);

                                            currentLineSplitData = txtReader.ReadLine().Split(splitChara, StringSplitOptions.None);
                                            currentLineData = Encoding.UTF8.GetBytes(currentLineSplitData[1]);

                                            if (shiftJISParse)
                                            {
                                                currentLineData = Encoding.Convert(Encoding.UTF8, _ShiftJISEncoding, currentLineData);
                                            }

                                            currentLineLength = currentLineData.Length;

                                            for (int b = 0; b < currentLineLength; b++)
                                            {
                                                if (currentLineData[b] == 0x23 && b != currentLineLength - 2)
                                                {
                                                    if (currentLineData[b + 3] == 0x23)
                                                    {
                                                        commandCodeRead[0] = currentLineData[b + 1];
                                                        commandCodeRead[1] = currentLineData[b + 2];

                                                        commandCode = Convert.ToByte(_ANSIEncoding.GetString(commandCodeRead), 16);
                                                        currentLineDataList.Add(commandCode);
                                                        b += 3;
                                                    }
                                                    else
                                                    {
                                                        currentLineDataList.Add(currentLineData[b]);
                                                    }
                                                }
                                                else
                                                {
                                                    currentLineDataList.Add(currentLineData[b]);
                                                }
                                            }

                                            outKpsWriter.Write(currentLineDataList.ToArray());
                                            currentLineDataList.Clear();

                                            currentOffset = outKpsStream.Position;

                                            if (!txtReader.EndOfStream)
                                            {
                                                _ = txtReader.ReadLine();
                                            }
                                        }

                                        SharedMethods.IfFileDirExistsDel(outKpsFile + ".new", SharedMethods.DelSwitch.file);

                                        using (var outKpsFileStream = new FileStream(outKpsFile + ".new", FileMode.Append, FileAccess.Write))
                                        {
                                            outKpsOffsetStream.Seek(0, SeekOrigin.Begin);
                                            outKpsOffsetStream.CopyTo(outKpsFileStream);

                                            outKpsStream.Seek(linesStartOffset, SeekOrigin.Begin);
                                            outKpsStream.CopyTo(outKpsFileStream);
                                        }
                                    }
                                }
                            }
                        }

                        hasRepacked = true;
                    }

                    if (hasRepacked)
                    {
                        SharedMethods.IfFileDirExistsDel(outKpsFile + ".old", SharedMethods.DelSwitch.file);
                        File.Move(outKpsFile, outKpsFile + ".old");
                        File.Move(outKpsFile + ".new", outKpsFile);

                        LoggingMethods.LogMessage(SharedMethods.NewLineChara);
                        LoggingMethods.LogMessage("Repacking has completed!");
                        LoggingMethods.LogMessage(SharedMethods.NewLineChara);

                        SharedMethods.AppMsgBox("Repacked " + Path.GetFileName(txtFile) + " data to kps file", "Success", MessageBoxIcon.Information);
                    }
                    else
                    {
                        LoggingMethods.LogMessage(SharedMethods.NewLineChara);
                        LoggingMethods.LogMessage("Missing required info. Repacking failed!");
                        LoggingMethods.LogMessage(SharedMethods.NewLineChara);
                    }
                }
            }
            catch (Exception ex)
            {
                SharedMethods.AppMsgBox("" + ex, "Error", MessageBoxIcon.Error);
                LoggingMethods.LogMessage(SharedMethods.NewLineChara);
                LoggingMethods.LogException("Exception: " + ex);
            }
        }
    }
}