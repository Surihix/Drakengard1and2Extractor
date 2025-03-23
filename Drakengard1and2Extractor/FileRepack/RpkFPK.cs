using Drakengard1and2Extractor.Support;
using System;
using System.IO;
using System.Windows.Forms;

namespace Drakengard1and2Extractor.FileRepack
{
    internal class RpkFPK
    {
        public static void RepackFPK(string fpkFile, string unpackedFpkDir)
        {
            try
            {
                var unpackedFilesInDir = Directory.GetFiles(unpackedFpkDir, "*.*", SearchOption.TopDirectoryOnly);
                var processLimit = unpackedFilesInDir.Length - 1;
                var unpackedFilesDict = SharedMethods.GetFilesInDirForRepack(unpackedFilesInDir, processLimit);

                var fpkStructure = new SharedStructures.FPK();
                var hasRepacked = false;

                using (BinaryReader fpkReader = new BinaryReader(File.Open(fpkFile, FileMode.Open, FileAccess.Read)))
                {
                    fpkReader.BaseStream.Position = 8;
                    fpkStructure.EntryCount = fpkReader.ReadUInt32();
                    fpkStructure.EntryAlignPosition = fpkReader.ReadUInt32();
                    fpkStructure.FpkHeaderSize = fpkReader.ReadUInt32();
                    fpkStructure.FpkHeaderSize2 = fpkReader.ReadUInt32();
                    fpkStructure.EntryTableSize = fpkReader.ReadUInt32();

                    fpkReader.BaseStream.Position += 8;
                    fpkStructure.FPKtypeFlag = fpkReader.ReadUInt32();
                    fpkStructure.FPKbinDataOffset = fpkReader.ReadUInt32();

                    fpkReader.BaseStream.Position += 20;
                    fpkStructure.FPKbinName = fpkReader.ReadStringTillNull();

                    var entryTableOffset = fpkStructure.FPKbinName == "image.bin" ? 160 : 128;

                    if (unpackedFilesDict.Keys.Count >= fpkStructure.EntryCount)
                    {
                        using (MemoryStream newFpkHeaderStream = new MemoryStream())
                        {
                            using (BinaryWriter newFpkHeaderWriter = new BinaryWriter(newFpkHeaderStream))
                            {
                                newFpkHeaderWriter.BaseStream.Position = 0;
                                newFpkHeaderWriter.Write(System.Text.Encoding.UTF8.GetBytes(fpkStructure.FPKMagic));
                                newFpkHeaderWriter.Write(fpkStructure.Reserved);
                                newFpkHeaderWriter.Write(fpkStructure.EntryCount);
                                newFpkHeaderWriter.Write(fpkStructure.EntryAlignPosition);
                                newFpkHeaderWriter.Write(fpkStructure.FpkHeaderSize);
                                newFpkHeaderWriter.Write(fpkStructure.FpkHeaderSize2);
                                newFpkHeaderWriter.Write(fpkStructure.EntryTableSize);
                                newFpkHeaderWriter.Write(fpkStructure.Reserved2);
                                newFpkHeaderWriter.Write(fpkStructure.FPKtypeFlag);
                                newFpkHeaderWriter.Write(fpkStructure.FPKbinDataOffset);
                                newFpkHeaderWriter.Write((uint)0);
                                newFpkHeaderWriter.Write(fpkStructure.Reserved3);
                                newFpkHeaderWriter.Write(System.Text.Encoding.UTF8.GetBytes(fpkStructure.FPKbinName));

                                newFpkHeaderWriter.BaseStream.PadNull(entryTableOffset - newFpkHeaderWriter.BaseStream.Position);

                                fpkReader.BaseStream.Position = entryTableOffset;

                                using (MemoryStream newFpkStream = new MemoryStream())
                                {
                                    newFpkHeaderWriter.BaseStream.Position = entryTableOffset;
                                    string currentKey;
                                    long currentOffset = 0;

                                    for (int e = 1; e < fpkStructure.EntryCount + 1; e++)
                                    {
                                        if (e != 1)
                                        {
                                            PadFixedAmountOfBytes(ref currentOffset, fpkStructure.EntryAlignPosition, newFpkStream);
                                        }

                                        currentKey = $"FILE_{e}";
                                        var currentFile = unpackedFilesDict[currentKey];

                                        fpkStructure.EntryDataOffset = (uint)currentOffset;
                                        fpkStructure.EntryDataSize = (uint)new FileInfo(currentFile).Length;

                                        using (var currentFileStream = new FileStream(currentFile, FileMode.Open, FileAccess.Read))
                                        {
                                            currentFileStream.Seek(0, SeekOrigin.Begin);
                                            currentFileStream.CopyStreamTo(newFpkStream, fpkStructure.EntryDataSize, false);
                                        }

                                        newFpkHeaderWriter.Write(fpkReader.ReadUInt32());
                                        newFpkHeaderWriter.Write(fpkStructure.EntryDataOffset);
                                        newFpkHeaderWriter.Write(fpkStructure.EntryDataSize);
                                        fpkReader.BaseStream.Position += 8;

                                        if (entryTableOffset == 128)
                                        {
                                            newFpkHeaderWriter.Write(fpkReader.ReadUInt32());
                                        }
                                        else
                                        {
                                            _ = fpkReader.ReadUInt32();
                                            newFpkHeaderWriter.Write((uint)0);
                                        }

                                        currentOffset = newFpkStream.Position;
                                    }

                                    PadFixedAmountOfBytes(ref currentOffset, fpkStructure.EntryAlignPosition, newFpkStream);

                                    newFpkHeaderWriter.BaseStream.PadNull(fpkStructure.FPKbinDataOffset - newFpkHeaderWriter.BaseStream.Position);

                                    newFpkHeaderWriter.BaseStream.Position = 44;
                                    newFpkHeaderWriter.Write((uint)newFpkStream.Length);

                                    using (var newFpkFileStream = new FileStream(fpkFile + ".new", FileMode.Append, FileAccess.Write))
                                    {
                                        newFpkHeaderStream.Seek(0, SeekOrigin.Begin);
                                        newFpkHeaderStream.CopyTo(newFpkFileStream);

                                        newFpkStream.Seek(0, SeekOrigin.Begin);
                                        newFpkStream.CopyTo(newFpkFileStream);
                                    }
                                }
                            }
                        }

                        hasRepacked = true;
                    }
                }

                if (hasRepacked)
                {
                    SharedMethods.IfFileDirExistsDel(fpkFile + ".old", SharedMethods.DelSwitch.file);
                    File.Move(fpkFile, fpkFile + ".old");
                    File.Move(fpkFile + ".new", fpkFile);

                    LoggingMethods.LogMessage(SharedMethods.NewLineChara);
                    LoggingMethods.LogMessage("Repacking has completed!");
                    LoggingMethods.LogMessage(SharedMethods.NewLineChara);

                    SharedMethods.AppMsgBox("Repacked " + Path.GetFileName(fpkFile) + " file", "Success", MessageBoxIcon.Information);
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


        private static void PadFixedAmountOfBytes(ref long offset, uint padValue, Stream streamToPad)
        {
            if (offset % padValue != 0)
            {
                var remainder = offset % padValue;
                var increaseBytes = padValue - remainder;
                var newPos = offset + increaseBytes;
                var nullBytesAmount = newPos - offset;

                streamToPad.Seek(offset, SeekOrigin.Begin);
                streamToPad.PadNull(nullBytesAmount);

                offset = streamToPad.Position;
            }
        }
    }
}