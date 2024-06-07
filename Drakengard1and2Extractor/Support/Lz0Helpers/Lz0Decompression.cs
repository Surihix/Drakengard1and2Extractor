using System.Collections.Generic;
using System.IO;

namespace Drakengard1and2Extractor.Support.Lz0Helpers
{
    internal class Lz0Decompression
    {
        public static byte[] ProcessLz0Data(string lz0File)
        {
            var processedDataList = new List<byte>();

            using (var lz0Stream = new FileStream(lz0File, FileMode.Open, FileAccess.Read))
            {
                lz0Stream.Seek(0, SeekOrigin.Begin);

                using (var lz0Reader = new BinaryReader(lz0Stream))
                {

                    lz0Reader.BaseStream.Position = 24;
                    var lz0Chunks = lz0Reader.ReadUInt32();
                    uint lz0DataReadStart = 32;

                    for (int l = 0; l < lz0Chunks; l++)
                    {
                        lz0Reader.BaseStream.Position = lz0DataReadStart + 4;
                        var cmpChunkSize = lz0Reader.ReadUInt32();
                        var uncmpChunkSize = lz0Reader.ReadUInt32();

                        lz0Stream.Seek(lz0DataReadStart + 12, SeekOrigin.Begin);

                        byte[] compressedData = new byte[cmpChunkSize];
                        _ = lz0Stream.Read(compressedData, 0, (int)cmpChunkSize);

                        byte[] deCompressedData = new byte[uncmpChunkSize];
                        Minilz0Function.Decompress(ref compressedData, uncmpChunkSize, ref deCompressedData);

                        processedDataList.AddRange(deCompressedData);

                        var seekLength = cmpChunkSize;
                        lz0Reader.BaseStream.Seek(lz0DataReadStart + 12, SeekOrigin.Begin);
                        lz0Reader.BaseStream.Seek(seekLength, SeekOrigin.Current);

                        lz0DataReadStart = (uint)lz0Reader.BaseStream.Position;
                    }
                }
            }

            return processedDataList.ToArray();
        }
    }
}