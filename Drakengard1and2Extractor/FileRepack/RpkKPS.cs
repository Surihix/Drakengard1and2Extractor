using Drakengard1and2Extractor.Support;
using System;
using System.Text;
using System.Windows.Forms;

namespace Drakengard1and2Extractor.FileRepack
{
    internal class RpkKPS
    {
        private static readonly Encoding _ShiftJISEncoding = Encoding.GetEncoding(932);
        private static readonly Encoding _ANSIEncoding = Encoding.GetEncoding(1252);

        public static void RepackKPS(string txtFile, bool shiftJISParse)
        {
            try
            {

            }
            catch (Exception ex)
            {
                SharedMethods.AppMsgBox("" + ex, "Error", MessageBoxIcon.Error);
                LoggingMethods.LogMessage(SharedMethods.NewLineChara);
                LoggingMethods.LogException("Exception: " + ex);
            }
        }
    }
}