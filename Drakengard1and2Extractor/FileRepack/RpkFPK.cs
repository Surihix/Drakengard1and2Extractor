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
                var fpkStructure = new SharedStructures.FPK();

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

                    using (MemoryStream newFpkStream = new MemoryStream())
                    {
                        using (BinaryWriter newFpkWriter = new BinaryWriter(newFpkStream))
                        {
                            newFpkWriter.BaseStream.Position = 0;
                            newFpkWriter.Write(System.Text.Encoding.UTF8.GetBytes(fpkStructure.FPKMagic));
                            newFpkWriter.Write(fpkStructure.Reserved);
                            newFpkWriter.Write(fpkStructure.EntryCount);
                            newFpkWriter.Write(fpkStructure.EntryAlignPosition);
                            newFpkWriter.Write(fpkStructure.FpkHeaderSize);
                            newFpkWriter.Write(fpkStructure.FpkHeaderSize);
                            newFpkWriter.Write(fpkStructure.EntryTableSize);
                            newFpkWriter.Write(fpkStructure.Reserved2);
                            newFpkWriter.Write(fpkStructure.FPKtypeFlag);
                            newFpkWriter.Write(fpkStructure.FPKbinDataOffset);
                            newFpkWriter.Write((uint)0);
                            newFpkWriter.Write(fpkStructure.Reserved3);
                            newFpkWriter.Write(System.Text.Encoding.UTF8.GetBytes(fpkStructure.FPKbinName));

                            newFpkWriter.BaseStream.PadNull(entryTableOffset - newFpkWriter.BaseStream.Position);
                            
                            fpkReader.BaseStream.Position = entryTableOffset;
                            string fileExtn;
                            string fileExtnFixed;
                            var fName = "FILE_";

                            for (int e = 1; e < fpkStructure.EntryCount + 1; e++)
                            {
                                fpkReader.BaseStream.Position += 12;
                                fpkStructure.EntryExtnChars = fpkReader.ReadChars(4);
                                Array.Reverse(fpkStructure.EntryExtnChars);

                                fileExtn = string.Join("", fpkStructure.EntryExtnChars).Replace("\0", "");
                                fileExtnFixed = "." + SharedMethods.ModifyString(fileExtn);

                                if (fileExtn.StartsWith("\\") || fileExtn.StartsWith("/"))
                                {
                                    fileExtnFixed = "";
                                }

                                var currentFile = Path.Combine(unpackedFpkDir, fName + $"{e}" + fileExtnFixed);

                                if (!File.Exists(currentFile))
                                {
                                    
                                }
                                else
                                {

                                }
                            }
                        }
                    }
                }

                LoggingMethods.LogMessage(SharedMethods.NewLineChara);
                LoggingMethods.LogMessage("Repacking has completed!");
                LoggingMethods.LogMessage(SharedMethods.NewLineChara);

                SharedMethods.AppMsgBox("Repacked " + Path.GetFileName(fpkFile) + " file", "Success", MessageBoxIcon.Information);
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