using System;
using System.Linq;
using System.Runtime.InteropServices;

unsafe class Minilz0Helpers
{
    [DllImport("minilzo.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern int lzo1x_decompress(IntPtr src, uint src_len, IntPtr dst, ref uint dst_len);
    public static void Decompress(ref byte[] compressedArray, uint ogSize, ref byte[] decompressedArray)
    {
        // This will be the uncompressed data
        byte[] outData = new byte[0];

        // Decompress from inData and set the uncompressed data to outData
        fixed (byte* ptr = compressedArray)
        {
            // Get the pointer at the start of inData
            IntPtr dataPtr = (IntPtr)ptr;

            // Prepare the output byte data that minilzo will write the uncompressed data to. There will be unused bytes
            byte[] outBytes = new byte[ogSize];

            // The actual length of the uncompressed data. This is set by minilzo after lzo1x_decompress is called
            uint outLength = 0;

            fixed (byte* ptr2 = outBytes)
            {
                // Get the pointer at the start of outBytes
                IntPtr outPtr = (IntPtr)ptr2;

                // Call the decompress code from inData to outBytes
                lzo1x_decompress(dataPtr, (uint)compressedArray.Length, outPtr, ref outLength);
            }

            // Copy 'outLength' number of bytes to outData. Gets all the bytes from outBytes[0] to outBytes[outLength - 1]
            outData = outBytes.Take((int)outLength).ToArray();

            // Copy the decompressed data from the outData array into the DecompressedArray
            outData.CopyTo(decompressedArray, 0);
        }
    }
}