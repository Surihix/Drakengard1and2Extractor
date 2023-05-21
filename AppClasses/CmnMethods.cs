using System;
using System.IO;
using System.Windows.Forms;

namespace Drakengard1and2Extractor.AppClasses
{
    internal class CmnMethods
    {
        public static void AppMsgBox(string Msg, string MsgHeader, MessageBoxIcon MsgType)
        {
            MessageBox.Show(Msg, MsgHeader, MessageBoxButtons.OK, MsgType);
        }

        public static void FileDirectoryExistsDel(string fileOrFolderVar, DelSwitch delSwitchVar)
        {
            switch (delSwitchVar)
            {
                case DelSwitch.file:
                    if (File.Exists(fileOrFolderVar))
                    {
                        File.Delete(fileOrFolderVar);
                    }
                    break;

                case DelSwitch.folder:
                    if (Directory.Exists(fileOrFolderVar))
                    {
                        Directory.Delete(fileOrFolderVar, true);
                    }
                    break;
            }
        }
        public enum DelSwitch
        {
            folder,
            file
        }

        public static void MugiSwizzle(ref byte[] palBufferVar)
        {
            uint[] newPalette = new uint[256];
            uint[] origPalette = new uint[256];
            Buffer.BlockCopy(palBufferVar, 0, origPalette, 0, palBufferVar.Length);

            for (uint k = 0; k < 8; k++)
            {
                for (uint j = 0; j < 2; j++)
                {
                    for (uint i = 0; i < 8; i++)
                    {
                        newPalette[k * 32 + j * 16 + i] = origPalette[k * 32 + 8 * j + i];
                        newPalette[k * 32 + j * 16 + i + 8] = origPalette[k * 32 + 8 * j + 16 + i];
                    }
                }
            }

            Buffer.BlockCopy(newPalette, 0, palBufferVar, 0, palBufferVar.Length);
        }
    }
}