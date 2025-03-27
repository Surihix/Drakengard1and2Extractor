namespace Drakengard1and2Extractor.Support
{
    internal class SharedStructures
    {
        public class FPK
        {
            public string FPKMagic = "fpk\0";
            public byte[] Reserved = new byte[4] { 0x00, 0x00, 0x00, 0x00 };
            public uint EntryCount;
            public uint EntryAlignPosition;
            public uint FPKHeaderSize;
            public uint FPKHeaderSize2;
            public uint EntryTableSize;
            public byte[] Reserved2 = new byte[8] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            public uint FPKtypeFlag;
            public uint FPKbinDataOffset;
            public uint FPKbinDataSize;
            public byte[] Reserved3 = new byte[16] 
            { 
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 
            };
            public string FPKbinName;
            public string FallBackName = "fpkDataBin";

            public uint EntryDataOffset;
            public uint EntryDataSize;
            public char[] EntryExtnChars;
            public bool HasLstFile;
        }

        public class DPK
        {
            public string DPKMagic = "dpk\0";
            public uint DPKHeaderSize;
            public uint UnkFlag = 1;
            public uint DPKDataOffset;
            public uint EntryCount;
            public byte[] Reserved = new byte[12]
            {
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            };

            public byte[] EntryNameMD5Hash = new byte[16];
            public uint EntryDataSize;
            public uint EntryDataOffset;
        }
    }
}