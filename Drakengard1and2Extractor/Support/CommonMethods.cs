using System;
using System.IO;
using System.Windows.Forms;

namespace Drakengard1and2Extractor.Support
{
    internal class CommonMethods
    {
        public static readonly string NewLineChara = Environment.NewLine;


        public static void AppMsgBox(string msg, string msgHeader, MessageBoxIcon msgType)
        {
            MessageBox.Show(msg, msgHeader, MessageBoxButtons.OK, msgType);
        }


        public static string HeaderCheck(string fileName)
        {
            string header = string.Empty;

            using (BinaryReader extnCheckReader = new BinaryReader(File.Open(fileName, FileMode.Open, FileAccess.Read)))
            {
                extnCheckReader.BaseStream.Position = 0;
                var headerChars = extnCheckReader.ReadChars(4);
                header = string.Join("", headerChars);
            }

            header = header.Replace("\0", "");

            return header;
        }


        public static void IfFileDirExistsDel(string fileOrDir, DelSwitch delSwitch)
        {
            switch (delSwitch)
            {
                case DelSwitch.file:
                    if (File.Exists(fileOrDir))
                    {
                        File.Delete(fileOrDir);
                    }
                    break;

                case DelSwitch.directory:
                    if (Directory.Exists(fileOrDir))
                    {
                        Directory.Delete(fileOrDir, true);
                    }
                    break;
            }
        }
        public enum DelSwitch
        {
            directory,
            file
        }


        public static string ModifyExtnString(string readStringLetters)
        {
            var modifiedString = readStringLetters.Replace("|", "").Replace("?", "").Replace(":", "").
                Replace("<", "").Replace(">", "").Replace("*", "").Replace("0eng", "0eng.fpk").
                Replace("0jpn", "0jpn.fpk").Replace("1uk", "1uk.fpk").Replace("2fre", "2fre.fpk").Replace("3ger", "3ger.fpk").
                Replace("4ita", "4ita.fpk").Replace("5spa", "5spa.fpk");

            return modifiedString;
        }


        public static string GetFileHeader(BinaryReader readerName)
        {
            readerName.BaseStream.Position = 0;
            var foundExtnChars = readerName.ReadChars(4);
            var foundExtn = string.Join("", foundExtnChars).Replace("\0", "");

            string realExtn = string.Empty;

            switch (foundExtn)
            {
                case "fpk":
                    realExtn = ".fpk";
                    break;
                case "dpk":
                    realExtn = ".dpk";
                    break;
                case "wZIM":
                    realExtn = ".zim";
                    break;
                case "V3a":
                    realExtn = ".lz0";
                    break;
                case "KPS_":
                    realExtn = ".kps";
                    break;
                case "kvm1":
                    realExtn = ".kvm";
                    break;
                case "SPK0":
                    realExtn = ".spk0";
                    break;
                case "EVMT":
                    realExtn = ".emt";
                    break;
                case "DCMR":
                    realExtn = ".dcmr";
                    break;
                case "DLGT":
                    realExtn = ".dlgt";
                    break;
                case "pBAX":
                    realExtn = ".hd2";
                    break;
                case "SPFn":
                    realExtn = ".spf";
                    break;
            }
            if (foundExtn.StartsWith("bh"))
            {
                realExtn = ".hi4";
            }

            return realExtn;
        }
    }
}