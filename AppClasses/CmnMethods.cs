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

        public static void HeaderCheck(string FileName, ref string HeaderVar)
        {
            using (FileStream ExtnCheck = new FileStream(FileName, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader ExtnCheckReader = new BinaryReader(ExtnCheck))
                {
                    ExtnCheckReader.BaseStream.Position = 0;
                    var HeaderChars = ExtnCheckReader.ReadChars(4);
                    HeaderVar = string.Join("", HeaderChars);
                }

                ExtnCheck.Dispose();
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

        public static void ModifyString(ref string readStringLetters)
        {
            readStringLetters.Replace("\0", "").Replace("|", "").Replace("?", "").Replace(":", "").
                Replace("<", "").Replace(">", "").Replace("*", "").Replace("0eng", "0eng.fpk").
                Replace("0jpn", "0jpn.fpk").Replace("1uk", "1uk.fpk").Replace("2fre", "2fre.fpk").Replace("3ger", "3ger.fpk").
                Replace("4ita", "4ita.fpk").Replace("5spa", "5spa.fpk");
        }

        public static void GetFileHeader(BinaryReader ReaderName, ref string RExtVar)
        {
            ReaderName.BaseStream.Position = 0;
            var FoundExt = ReaderName.ReadChars(4);
            string RealExt = string.Join("", FoundExt).Replace("\0", "");

            switch (RealExt)
            {
                case "fpk":
                    RExtVar = ".fpk";
                    break;
                case "dpk":
                    RExtVar = ".dpk";
                    break;
                case "wZIM":
                    RExtVar = ".zim";
                    break;
                case "V3a":
                    RExtVar = ".lz0";
                    break;
                case "KPS_":
                    RExtVar = ".kps";
                    break;
                case "kvm1":
                    RExtVar = ".kvm";
                    break;
                case "SPK0":
                    RExtVar = ".spk0";
                    break;
                case "EVMT":
                    RExtVar = ".emt";
                    break;
                case "DCMR":
                    RExtVar = ".dcmr";
                    break;
                case "DLGT":
                    RExtVar = ".dlgt";
                    break;
                case "pBAX":
                    RExtVar = ".hd2";
                    break;
            }
            if (RealExt.StartsWith("bh"))
            {
                RExtVar = ".hi4";
            }
        }
    }
}