using Drakengard1and2Extractor.Support;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Drakengard1and2Extractor.FileRepack
{
    internal class RpkDPK
    {
        public static void RepackDPK(string dpkFile, string unpackedDpkDir)
        {
            try
            {
                var dpkStructure = new SharedStructures.DPK();
                var hasRepacked = false;

                using (BinaryReader dpkReader = new BinaryReader(File.Open(dpkFile, FileMode.Open, FileAccess.Read)))
                {
                    dpkReader.BaseStream.Position = 4;
                    dpkStructure.DPKHeaderSize = dpkReader.ReadUInt32();
                    dpkStructure.UnkFlag = dpkReader.ReadUInt32();
                    dpkStructure.DPKDataOffset = dpkReader.ReadUInt32();
                    dpkStructure.EntryCount = dpkReader.ReadUInt32();
                    dpkReader.BaseStream.Position += 12;

                    var unpackedFilesInDir = Directory.GetFiles(unpackedDpkDir, "*.*", SearchOption.TopDirectoryOnly);
                    var unpackedFilesDict = SharedMethods.GetFilesInDirForRepack(unpackedFilesInDir, dpkStructure.EntryCount);

                    if (unpackedFilesDict.Keys.Count >= dpkStructure.EntryCount)
                    {
                        var fileOrderDict = new Dictionary<uint, (string, string, byte[], uint)>();
                        string currentKey;
                        string currentFile;
                        uint currentFileSize;

                        for (int e = 1; e < dpkStructure.EntryCount + 1; e++)
                        {
                            dpkStructure.EntryNameMD5Hash = dpkReader.ReadBytes(dpkStructure.EntryNameMD5Hash.Length);

                            dpkReader.BaseStream.Position += 8;
                            dpkStructure.EntryDataOffset = dpkReader.ReadUInt32();
                            dpkReader.BaseStream.Position += 4;

                            currentKey = $"FILE_{e}";
                            currentFile = unpackedFilesDict[currentKey];
                            currentFileSize = (uint)new FileInfo(currentFile).Length;
                            fileOrderDict.Add(dpkStructure.EntryDataOffset, (currentKey, unpackedFilesDict[currentKey], dpkStructure.EntryNameMD5Hash, currentFileSize));
                        }

                        var keysArranged = new List<uint>();
                        keysArranged.AddRange(fileOrderDict.Keys);
                        keysArranged.Sort();

                        var fileOrderRearrangedDict = new Dictionary<string, (string, byte[], uint)>();

                        foreach (var key in keysArranged)
                        {
                            currentKey = fileOrderDict[key].Item1;
                            currentFile = fileOrderDict[key].Item2;
                            byte[] currentHash = fileOrderDict[key].Item3;
                            currentFileSize = fileOrderDict[key].Item4;

                            fileOrderRearrangedDict.Add(currentKey, (currentFile, currentHash, currentFileSize));
                        }

                        using (MemoryStream newDpkHeaderStream = new MemoryStream())
                        {
                            using (BinaryWriter newDpkHeaderWriter = new BinaryWriter(newDpkHeaderStream))
                            {
                                newDpkHeaderWriter.BaseStream.Position = 0;
                                newDpkHeaderWriter.Write(System.Text.Encoding.UTF8.GetBytes(dpkStructure.DPKMagic));
                                newDpkHeaderWriter.Write(dpkStructure.DPKHeaderSize);
                                newDpkHeaderWriter.Write(dpkStructure.UnkFlag);
                                newDpkHeaderWriter.Write(dpkStructure.DPKDataOffset);
                                newDpkHeaderWriter.Write(dpkStructure.EntryCount);
                                newDpkHeaderWriter.Write((uint)0);
                                newDpkHeaderWriter.Write((uint)0);
                                newDpkHeaderWriter.Write((uint)0);

                                using (MemoryStream newDpkStream = new MemoryStream())
                                {
                                    newDpkStream.Seek(0, SeekOrigin.Begin);
                                    newDpkStream.PadNull(dpkStructure.DPKDataOffset);

                                    var entriesPacked = new Dictionary<uint, byte[]>();
                                    long currentOffset = dpkStructure.DPKDataOffset;
                                    var isFirstOne = true;
                                    uint entryKey = 0;

                                    foreach (var key in fileOrderRearrangedDict)
                                    {
                                        if (!isFirstOne)
                                        {
                                            SharedMethods.PadFixedAmountOfBytes(ref currentOffset, 16, newDpkStream);
                                            SharedMethods.PadFixedAmountOfBytes(ref currentOffset, 64, newDpkStream);
                                        }

                                        entryKey = uint.Parse(Path.GetFileNameWithoutExtension(key.Key).Split('_')[1]);
                                        currentFile = key.Value.Item1;
                                        byte[] currentHash = key.Value.Item2;
                                        currentFileSize = key.Value.Item3;

                                        byte[] currentFileSizeBytes = BitConverter.GetBytes(currentFileSize);

                                        var currentEntryData = new List<byte>();
                                        currentEntryData.AddRange(currentHash);
                                        currentEntryData.AddRange(currentFileSizeBytes);
                                        currentEntryData.AddRange(currentFileSizeBytes);
                                        currentEntryData.AddRange(BitConverter.GetBytes((uint)currentOffset));
                                        currentEntryData.AddRange(BitConverter.GetBytes((uint)0));

                                        entriesPacked.Add(entryKey, currentEntryData.ToArray());

                                        using (FileStream currentFileStream = new FileStream(currentFile, FileMode.Open, FileAccess.Read))
                                        {
                                            currentFileStream.Seek(0, SeekOrigin.Begin);
                                            currentFileStream.CopyTo(newDpkStream);
                                        }

                                        currentOffset = newDpkStream.Position;

                                        if (isFirstOne)
                                        {
                                            isFirstOne = false;
                                        }
                                    }

                                    keysArranged.Clear();
                                    keysArranged.AddRange(entriesPacked.Keys);
                                    keysArranged.Sort();

                                    foreach (var key in keysArranged)
                                    {
                                        newDpkHeaderWriter.Write(entriesPacked[key]);
                                    }

                                    newDpkHeaderWriter.BaseStream.PadNull(dpkStructure.DPKDataOffset - newDpkHeaderWriter.BaseStream.Position);

                                    SharedMethods.IfFileDirExistsDel(dpkFile + ".new", SharedMethods.DelSwitch.file);

                                    using (FileStream newDpkFileStream = new FileStream(dpkFile + ".new", FileMode.Append, FileAccess.Write))
                                    {
                                        newDpkHeaderStream.Seek(0, SeekOrigin.Begin);
                                        newDpkHeaderStream.CopyTo(newDpkFileStream);

                                        newDpkStream.Seek(dpkStructure.DPKDataOffset, SeekOrigin.Begin);
                                        newDpkStream.CopyTo(newDpkFileStream);
                                    }
                                }
                            }
                        }

                        hasRepacked = true;
                    }
                }

                if (hasRepacked)
                {
                    SharedMethods.IfFileDirExistsDel(dpkFile + ".old", SharedMethods.DelSwitch.file);
                    File.Move(dpkFile, dpkFile + ".old");
                    File.Move(dpkFile + ".new", dpkFile);

                    LoggingMethods.LogMessage(SharedMethods.NewLineChara);
                    LoggingMethods.LogMessage("Repacking has completed!");
                    LoggingMethods.LogMessage(SharedMethods.NewLineChara);

                    SharedMethods.AppMsgBox("Repacked " + Path.GetFileName(dpkFile) + " file", "Success", MessageBoxIcon.Information);
                }
                else
                {
                    LoggingMethods.LogMessage(SharedMethods.NewLineChara);
                    LoggingMethods.LogMessage("Missing files. Repacking failed!");
                    LoggingMethods.LogMessage(SharedMethods.NewLineChara);
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