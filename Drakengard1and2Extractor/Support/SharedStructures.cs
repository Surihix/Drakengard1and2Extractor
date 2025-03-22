namespace Drakengard1and2Extractor.Support
{
    internal class SharedStructures
    {
        public class FPK
        {
            public uint EntryCount;
            public uint HeaderSize;
            public uint EntryTableSize;
            public uint FPKbinDataOffset;
            public uint FPKbinDataSize;
            public string FPKbinName;
            public string FallBackName = "fpkDataBin";
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