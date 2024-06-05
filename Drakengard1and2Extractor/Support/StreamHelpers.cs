using System;
using System.IO;

internal static class StreamHelpers
{
    public static void CopyStreamTo(this Stream inStream, Stream outStream, long size, bool showProgress)
    {
        int bufferSize = 81920;
        long amountRemaining = size;
        long amountCopied = 0;
        decimal currentAmount;

        while (amountRemaining > 0)
        {
            long arraySize = Math.Min(bufferSize, amountRemaining);
            var copyArray = new byte[arraySize];

            _ = inStream.Read(copyArray, 0, (int)arraySize);
            outStream.Write(copyArray, 0, (int)arraySize);

            amountRemaining -= arraySize;

            amountCopied += arraySize;

            if (showProgress)
            {
                currentAmount = Math.Round(((decimal)amountCopied / size) * 100);
                Console.Write("\r{0}", "Copied " + currentAmount + "%");
            }
        }
    }

    public static void PadNull(this Stream stream, long padAmount)
    {
        for (long p = 0; p < padAmount; p++)
        {
            stream.WriteByte(0);
        }
    }
}