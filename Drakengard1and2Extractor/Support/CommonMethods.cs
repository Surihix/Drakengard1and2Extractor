using System.IO;
using System.Windows.Forms;

namespace Drakengard1and2Extractor.Support
{
    internal class CommonMethods
    {
        public static void AppMsgBox(string msg, string msgHeader, MessageBoxIcon msgType)
        {
            MessageBox.Show(msg, msgHeader, MessageBoxButtons.OK, msgType);
        }

        public static void HeaderCheck(string fileName, ref string header)
        {
            using (FileStream extnCheck = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader extnCheckReader = new BinaryReader(extnCheck))
                {
                    extnCheckReader.BaseStream.Position = 0;
                    var headerChars = extnCheckReader.ReadChars(4);
                    header = string.Join("", headerChars);
                }

                extnCheck.Dispose();
            }
        }

        public static void IfFileDirExistsDel(string fileOrFolder, DelSwitch delSwitch)
        {
            switch (delSwitch)
            {
                case DelSwitch.file:
                    if (File.Exists(fileOrFolder))
                    {
                        File.Delete(fileOrFolder);
                    }
                    break;

                case DelSwitch.folder:
                    if (Directory.Exists(fileOrFolder))
                    {
                        Directory.Delete(fileOrFolder, true);
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

        public static void GetFileHeader(BinaryReader readerName, ref string rExtn)
        {
            readerName.BaseStream.Position = 0;
            var foundExtn = readerName.ReadChars(4);
            string realExtn = string.Join("", foundExtn).Replace("\0", "");

            switch (realExtn)
            {
                case "fpk":
                    rExtn = ".fpk";
                    break;
                case "dpk":
                    rExtn = ".dpk";
                    break;
                case "wZIM":
                    rExtn = ".zim";
                    break;
                case "V3a":
                    rExtn = ".lz0";
                    break;
                case "KPS_":
                    rExtn = ".kps";
                    break;
                case "kvm1":
                    rExtn = ".kvm";
                    break;
                case "SPK0":
                    rExtn = ".spk0";
                    break;
                case "EVMT":
                    rExtn = ".emt";
                    break;
                case "DCMR":
                    rExtn = ".dcmr";
                    break;
                case "DLGT":
                    rExtn = ".dlgt";
                    break;
                case "pBAX":
                    rExtn = ".hd2";
                    break;
            }
            if (realExtn.StartsWith("bh"))
            {
                rExtn = ".hi4";
            }
        }
    }
}