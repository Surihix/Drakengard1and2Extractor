using Drakengard1and2Extractor.AppClasses;
using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace MiniLz0
{
    unsafe class MiniLz0Lib
    {
        public static void ChecklzoDll()
        {
            var x86DllSha256 = "d414fad15b356f33bf02479bd417d2df767ee102180aae718ef1135146da2884";
            var x64DllSha256 = "ea006fafb08dd554657b1c81e45c92e88d663aca0c79c48ae1f3dca22e1e2314";
            string dllBuildHash;

            var appArchitecture = RuntimeInformation.ProcessArchitecture;
            using (FileStream lzoDllStream = new FileStream("minilzo.dll", FileMode.Open, FileAccess.Read))
            {
                using (SHA256 dllSHA256 = SHA256.Create())
                {
                    var hashBuffer = dllSHA256.ComputeHash(lzoDllStream);
                    dllBuildHash = BitConverter.ToString(hashBuffer).Replace("-", "").ToLower();
                }
            }

            switch (appArchitecture)
            {
                case Architecture.X86:
                    CheckHashResult(x86DllSha256, dllBuildHash);
                    break;

                case Architecture.X64:
                    CheckHashResult(x64DllSha256, dllBuildHash);
                    break;
            }
        }
        private static void CheckHashResult(string dllHashVar, string dllBuildHashVar)
        {
            if (!dllHashVar.Equals(dllBuildHashVar))
            {
                CmnMethods.AppMsgBox("Detected incompatible minilz0.dll file.\nPlease check if the dll file included with this build of the app is the correct one.", "Error", MessageBoxIcon.Error);
                Environment.Exit(0);
            }
        }


        // Copy minizlo.dll to the same location of your C# exe to use it.
        [DllImport("minilzo.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int lzo1x_decompress(IntPtr src, uint src_len, IntPtr dst, ref uint dst_len);
        public static void Decompress(ref byte[] CompressedArray, uint OgSize, ref byte[] DecompressedArray)
        {
            byte[] outData = new byte[0]; // This will be the uncompressed data

            // Decompress from inData and set the uncompressed data to outData
            fixed (byte* ptr = CompressedArray)
            {
                // Get the pointer at the start of inData
                IntPtr dataPtr = (IntPtr)ptr;

                // Prepare the output byte data that minilzo will write the uncompressed data to. There will be unused bytes
                byte[] outBytes = new byte[OgSize];

                // The actual length of the uncompressed data. This is set by minilzo after lzo1x_decompress is called
                uint outLength = 0;

                fixed (byte* ptr2 = outBytes)
                {
                    // Get the pointer at the start of outBytes
                    IntPtr outPtr = (IntPtr)ptr2;

                    // Call the decompress code from inData to outBytes
                    lzo1x_decompress(dataPtr, (uint)CompressedArray.Length, outPtr, ref outLength);
                }

                // Copy 'outLength' number of bytes to outData. Gets all the bytes from outBytes[0] to outBytes[outLength - 1]
                outData = outBytes.Take((int)outLength).ToArray();

                // Copy the decompressed data from the outData array into the DecompressedArray
                outData.CopyTo(DecompressedArray, 0);
            }
        }
    }
}