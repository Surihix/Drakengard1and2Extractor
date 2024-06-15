using System.IO;
using System.Text;

public static class BinaryReaderHelpers
{
    public static string ReadStringTillNull(this BinaryReader reader)
    {
        var sb = new StringBuilder();
        char chars;
        while ((chars = reader.ReadChar()) != default)
        {
            sb.Append(chars);
        }
        return sb.ToString();
    }
}