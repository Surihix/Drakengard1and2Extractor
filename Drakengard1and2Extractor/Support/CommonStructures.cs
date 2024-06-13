namespace Drakengard1and2Extractor.Support
{
    internal class CommonStructures
    {
        public class FPK
        {
            public uint EntryCount;
            public uint FPKbinDataOffset;
            public uint FPKbinDataSize;
            public uint EntryDataOffset;
            public uint EntryDataSize;
            public char[] EntryExtnChars;
            public bool HasLstFile;
        }

        public class DPK
        {
            public uint EntryCount;
            public uint EntryDataSize;
            public uint EntryDataOffset;
        }
    }
}