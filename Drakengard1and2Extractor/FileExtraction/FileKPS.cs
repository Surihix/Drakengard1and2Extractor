using Drakengard1and2Extractor.Support;
using System.Windows.Forms;
using System;

namespace Drakengard1and2Extractor.FileExtraction
{
    internal class FileKPS
    {
        public static void ExtractKPS(string kpsFile, bool isSingleFile)
        {
            try
            {
                LoggingHelpers.LogMessage(CoreForm.NewLineChara);


            }
            catch (Exception ex)
            {
                CommonMethods.AppMsgBox("" + ex, "Error", MessageBoxIcon.Error);
                LoggingHelpers.LogMessage(CoreForm.NewLineChara);
                LoggingHelpers.LogException("Exception: " + ex);                       
            }
        }
    }
}