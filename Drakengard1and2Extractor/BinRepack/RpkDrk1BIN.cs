using Drakengard1and2Extractor.Support;
using System;
using System.IO;
using System.Windows.Forms;

namespace Drakengard1and2Extractor.BinRepack
{
    internal class RpkDrk1BIN
    {
        public static void RepackBin(string mbinFile, string unpackedMbinDir)
        {
            try
            {
                LoggingMethods.LogMessage(SharedMethods.NewLineChara);

                var fpkStructure = new SharedStructures.FPK();
                var hasRepacked = false;

                using (BinaryReader mbinReader = new BinaryReader(File.Open(mbinFile, FileMode.Open, FileAccess.Read)))
                {
                    mbinReader.BaseStream.Position = 8;
                    fpkStructure.EntryCount = mbinReader.ReadUInt32();
                    fpkStructure.EntryAlignPosition = mbinReader.ReadUInt32();
                    fpkStructure.FPKHeaderSize = mbinReader.ReadUInt32();
                    fpkStructure.FPKHeaderSize2 = mbinReader.ReadUInt32();
                    fpkStructure.EntryTableSize = mbinReader.ReadUInt32();

                    mbinReader.BaseStream.Position += 8;
                    fpkStructure.FPKtypeFlag = mbinReader.ReadUInt32();
                    fpkStructure.FPKbinDataOffset = mbinReader.ReadUInt32();

                    mbinReader.BaseStream.Position += 20;
                    fpkStructure.FPKbinName = mbinReader.ReadStringTillNull();

                    var entryTableOffset = fpkStructure.FPKbinName == "image.bin" ? 160 : 128;

                    var unpackedFilesInDir = Directory.GetFiles(unpackedMbinDir, "*.*", SearchOption.TopDirectoryOnly);
                    var unpackedFilesDict = SharedMethods.GetFilesInDirForRepack(unpackedFilesInDir, fpkStructure.EntryCount);

                    if (unpackedFilesDict.Keys.Count >= fpkStructure.EntryCount)
                    {
                        using (MemoryStream newMbinHeaderStream = new MemoryStream())
                        {
                            using (BinaryWriter newMbinHeaderWriter = new BinaryWriter(newMbinHeaderStream))
                            {
                                newMbinHeaderWriter.BaseStream.Position = 0;
                                newMbinHeaderWriter.Write(System.Text.Encoding.UTF8.GetBytes(fpkStructure.FPKMagic));
                                newMbinHeaderWriter.Write(fpkStructure.Reserved);
                                newMbinHeaderWriter.Write(fpkStructure.EntryCount);
                                newMbinHeaderWriter.Write(fpkStructure.EntryAlignPosition);
                                newMbinHeaderWriter.Write(fpkStructure.FPKHeaderSize);
                                newMbinHeaderWriter.Write(fpkStructure.FPKHeaderSize2);
                                newMbinHeaderWriter.Write(fpkStructure.EntryTableSize);
                                newMbinHeaderWriter.Write(fpkStructure.Reserved2);
                                newMbinHeaderWriter.Write(fpkStructure.FPKtypeFlag);
                                newMbinHeaderWriter.Write(fpkStructure.FPKbinDataOffset);
                                newMbinHeaderWriter.Write((uint)0);
                                newMbinHeaderWriter.Write(fpkStructure.Reserved3);
                                newMbinHeaderWriter.Write(System.Text.Encoding.UTF8.GetBytes(fpkStructure.FPKbinName));

                                newMbinHeaderWriter.BaseStream.PadNull(entryTableOffset - newMbinHeaderWriter.BaseStream.Position);

                                mbinReader.BaseStream.Position = entryTableOffset;

                                var subBinFile = Path.Combine(Path.GetDirectoryName(unpackedMbinDir), $"for_packing_{fpkStructure.FPKbinName}");
                                SharedMethods.IfFileDirExistsDel(subBinFile, SharedMethods.DelSwitch.file);

                                using (FileStream newMbinStream = new FileStream(subBinFile, FileMode.Append, FileAccess.Write))
                                {
                                    newMbinHeaderWriter.BaseStream.Position = entryTableOffset;
                                    string currentKey;
                                    long currentOffset = 0;
                                    long readOffset = 0;

                                    for (int e = 1; e < fpkStructure.EntryCount + 1; e++)
                                    {
                                        if (e != 1)
                                        {
                                            SharedMethods.PadFixedAmountOfBytes(ref currentOffset, fpkStructure.EntryAlignPosition, newMbinStream);
                                        }

                                        currentKey = $"FILE_{e}";
                                        var currentFile = unpackedFilesDict[currentKey];

                                        fpkStructure.EntryDataOffset = (uint)currentOffset;
                                        fpkStructure.EntryDataSize = (uint)new FileInfo(currentFile).Length;

                                        using (FileStream currentFileStream = new FileStream(currentFile, FileMode.Open, FileAccess.Read))
                                        {
                                            currentFileStream.Seek(0, SeekOrigin.Begin);
                                            currentFileStream.CopyTo(newMbinStream);
                                        }

                                        newMbinHeaderWriter.Write(mbinReader.ReadUInt32());
                                        readOffset = mbinReader.ReadUInt32();

                                        if (e != 1 && readOffset == 0)
                                        {
                                            newMbinHeaderWriter.Write((uint)0);
                                            newMbinHeaderWriter.Write((uint)0);
                                        }
                                        else
                                        {
                                            newMbinHeaderWriter.Write(fpkStructure.EntryDataOffset);
                                            newMbinHeaderWriter.Write(fpkStructure.EntryDataSize);
                                        }

                                        mbinReader.BaseStream.Position += 4;

                                        if (fpkStructure.FPKbinName == "image.bin")
                                        {
                                            _ = mbinReader.ReadUInt32();
                                            newMbinHeaderWriter.Write((uint)0);
                                        }
                                        else
                                        {
                                            newMbinHeaderWriter.Write(mbinReader.ReadUInt32());
                                        }

                                        currentOffset = newMbinStream.Position;

                                        LoggingMethods.LogMessageConstant($"Repacked '{currentKey}'");
                                    }

                                    SharedMethods.PadFixedAmountOfBytes(ref currentOffset, fpkStructure.EntryAlignPosition, newMbinStream);

                                    newMbinHeaderWriter.BaseStream.PadNull(fpkStructure.FPKbinDataOffset - newMbinHeaderWriter.BaseStream.Position);

                                    newMbinHeaderWriter.BaseStream.Position = 44;
                                    newMbinHeaderWriter.Write((uint)newMbinStream.Length);
                                }

                                SharedMethods.IfFileDirExistsDel(mbinFile + ".new", SharedMethods.DelSwitch.file);

                                LoggingMethods.LogMessage(SharedMethods.NewLineChara);
                                LoggingMethods.LogMessageConstant($"Building archive....");

                                using (FileStream newMbinFileStream = new FileStream(mbinFile + ".new", FileMode.Append, FileAccess.Write))
                                {
                                    using (FileStream newMbinStreamPacked = new FileStream(subBinFile, FileMode.Open, FileAccess.Read))
                                    {
                                        newMbinHeaderStream.Seek(0, SeekOrigin.Begin);
                                        newMbinHeaderStream.CopyTo(newMbinFileStream);

                                        newMbinStreamPacked.Seek(0, SeekOrigin.Begin);
                                        newMbinStreamPacked.CopyTo(newMbinFileStream);
                                    }
                                }

                                File.Delete(subBinFile);
                            }
                        }

                        hasRepacked = true;
                    }
                }

                if (hasRepacked)
                {
                    SharedMethods.IfFileDirExistsDel(mbinFile + ".old", SharedMethods.DelSwitch.file);
                    File.Move(mbinFile, mbinFile + ".old");
                    File.Move(mbinFile + ".new", mbinFile);

                    LoggingMethods.LogMessage(SharedMethods.NewLineChara);
                    LoggingMethods.LogMessage("Repacking has completed!");
                    LoggingMethods.LogMessage(SharedMethods.NewLineChara);

                    SharedMethods.AppMsgBox("Repacked " + Path.GetFileName(mbinFile) + " file", "Success", MessageBoxIcon.Information);
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