using System;
using System.IO;
using System.Windows.Forms;

namespace Drakengard1and2Extractor.Support
{
    internal class SharedMethods
    {
        public static readonly string NewLineChara = Environment.NewLine;

        public static void AppMsgBox(string msg, string msgHeader, MessageBoxIcon msgType)
        {
            MessageBox.Show(msg, msgHeader, MessageBoxButtons.OK, msgType);
        }


        public static void CheckLzoDll(bool isJustLaunched)
        {
            if (!File.Exists("minilzo.dll"))
            {
                AppMsgBox("Missing minilz0.dll file.\nPlease check if this dll file is present next to the exe file.", "Error", MessageBoxIcon.Error);               
                if (!isJustLaunched)
                {
                    AppMsgBox("App will exit now", "Error", MessageBoxIcon.Error);
                }
                Environment.Exit(1);
            }

            var x86DllSha256 = "d414fad15b356f33bf02479bd417d2df767ee102180aae718ef1135146da2884";
            var x64DllSha256 = "ea006fafb08dd554657b1c81e45c92e88d663aca0c79c48ae1f3dca22e1e2314";
            string dllBuildHash;

            var appArchitecture = System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture;

            using (var dllStream = new FileStream("minilzo.dll", FileMode.Open, FileAccess.Read))
            {
                using (System.Security.Cryptography.SHA256 dllSHA256 = System.Security.Cryptography.SHA256.Create())
                {
                    dllBuildHash = BitConverter.ToString(dllSHA256.ComputeHash(dllStream)).Replace("-", "").ToLower();
                }
            }

            switch (appArchitecture)
            {
                case System.Runtime.InteropServices.Architecture.X86:
                    if (!dllBuildHash.Equals(x86DllSha256))
                    {
                        AppMsgBox("Detected incompatible minilz0.dll file.\nPlease check if the dll file included with this build of the app is the correct one.", "Error", MessageBoxIcon.Error);
                        if (!isJustLaunched)
                        {
                            AppMsgBox("App will exit now", "Error", MessageBoxIcon.Error);
                        }
                        Environment.Exit(1);
                    }
                    break;

                case System.Runtime.InteropServices.Architecture.X64:
                    if (!dllBuildHash.Equals(x64DllSha256))
                    {
                        AppMsgBox("Detected incompatible minilz0.dll file.\nPlease check if the dll file included with this build of the app is the correct one.", "Error", MessageBoxIcon.Error);
                        if (!isJustLaunched)
                        {
                            AppMsgBox("App will exit now", "Error", MessageBoxIcon.Error);
                        }
                        Environment.Exit(1);
                    }
                    break;
            }
        }


        public static string GetHeaderString(string fileName)
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


        public static bool SetBoolFromDlgResult(DialogResult dialogResult)
        {
            if (dialogResult == DialogResult.Yes)
            {
                return true;
            }
            else
            {
                return false;
            }
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


        public static string ModifyString(string readStringLetters)
        {
            var modifiedString = readStringLetters.Replace("|", "").Replace("?", "").Replace(":", "").Replace("<", "").
                Replace(">", "").Replace("*", "").Replace("0eng", "0eng.fpk").Replace("0jpn", "0jpn.fpk").
                Replace("1uk", "1uk.fpk").Replace("2fre", "2fre.fpk").Replace("3ger", "3ger.fpk").Replace("4ita", "4ita.fpk").
                Replace("5spa", "5spa.fpk");

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
                case "CHFh":
                    realExtn = ".chf";
                    break;
                case "CMFf":
                    realExtn = ".cmf";
                    break;
                case "CFFd":
                    realExtn = ".cff";
                    break;
                case "CJFg":
                    realExtn = ".cjf";
                    break;
                case "CEFh":
                    realExtn = ".cef";
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