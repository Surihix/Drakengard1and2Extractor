using Drakengard1and2Extractor.Support;
using System;
using System.Windows.Forms;

namespace Drakengard1and2Extractor.BinRepack
{
    internal class RpkDrk2BIN
    {
        public static void RepackBin(string mbinFile, string unpackedMbinDir)
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