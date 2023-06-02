using System.IO;
using System.Windows.Forms;

namespace Drakengard1and2Extractor.Libraries
{
    internal class CmnMethods
    {
        public static void AppMsgBox(string msg, string msgHeader, MessageBoxIcon msgType)
        {
            MessageBox.Show(msg, msgHeader, MessageBoxButtons.OK, msgType);
        }

        public static void HeaderCheck(string fileName, ref string headerVar)
        {
            using (FileStream extnCheck = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader extnCheckReader = new BinaryReader(extnCheck))
                {
                    extnCheckReader.BaseStream.Position = 0;
                    var headerChars = extnCheckReader.ReadChars(4);
                    headerVar = string.Join("", headerChars);
                }

                extnCheck.Dispose();
            }
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

        public static string ModifyString(string readStringLetters)
        {
            var modifiedString = readStringLetters.Replace("|", "").Replace("?", "").Replace(":", "").
                Replace("<", "").Replace(">", "").Replace("*", "").Replace("0eng", "0eng.fpk").
                Replace("0jpn", "0jpn.fpk").Replace("1uk", "1uk.fpk").Replace("2fre", "2fre.fpk").Replace("3ger", "3ger.fpk").
                Replace("4ita", "4ita.fpk").Replace("5spa", "5spa.fpk");

            return modifiedString;
        }

        public static void GetFileHeader(BinaryReader readerName, ref string rExtnVar)
        {
            readerName.BaseStream.Position = 0;
            var foundExtn = readerName.ReadChars(4);
            string realExtn = string.Join("", foundExtn).Replace("\0", "");

            switch (realExtn)
            {
                case "fpk":
                    rExtnVar = ".fpk";
                    break;
                case "dpk":
                    rExtnVar = ".dpk";
                    break;
                case "wZIM":
                    rExtnVar = ".zim";
                    break;
                case "V3a":
                    rExtnVar = ".lz0";
                    break;
                case "KPS_":
                    rExtnVar = ".kps";
                    break;
                case "kvm1":
                    rExtnVar = ".kvm";
                    break;
                case "SPK0":
                    rExtnVar = ".spk0";
                    break;
                case "EVMT":
                    rExtnVar = ".emt";
                    break;
                case "DCMR":
                    rExtnVar = ".dcmr";
                    break;
                case "DLGT":
                    rExtnVar = ".dlgt";
                    break;
                case "pBAX":
                    rExtnVar = ".hd2";
                    break;
            }
            if (realExtn.StartsWith("bh"))
            {
                rExtnVar = ".hi4";
            }
        }
    }
}