using System.IO;
using System;

public static class StreamExtensions
{
    /// <summary>
    /// Copies bytes from the source stream to the destination stream
    /// </summary>
    /// <param name="source">The stream to copy bytes from</param>
    /// <param name="destination">The stream to write the copied bytes to</param>
    /// <param name="offset">The position in the source stream to begin copying from</param>
    /// <param name="count">The number of bytes to copy from the source stream</param>
    /// <param name="bufferSize">The size of the temporary buffer the bytes are copied to (default size taken from <see cref="FileStream"/>)</param>
    public static void CopyTo(this Stream source, Stream destination, long offset, long count, int bufferSize = 81920)
    {
        // Seek to the given offset of the source stream
        var returnAddress = source.Position;
        source.Seek(offset, SeekOrigin.Begin);

        // Copy the data in chunks of bufferSize bytes until all are done
        var bytesRemaining = count;
        while (bytesRemaining > 0)
        {
            var readSize = Math.Min(bufferSize, bytesRemaining);
            var buffer = new byte[readSize];
            _ = source.Read(buffer, 0, (int)readSize);

            destination.Write(buffer, 0, (int)readSize);
            bytesRemaining -= readSize;
        }

        // Seek the source stream back to where it was
        source.Seek(returnAddress, SeekOrigin.Begin);
    }
}