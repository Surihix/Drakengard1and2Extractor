using Drakengard1and2Extractor.Support;
using System;
using System.IO;
using System.Windows.Forms;

namespace Drakengard1and2Extractor.FileRepack
{
    internal class RpkFPK
    {
        public static void RepackFPK(string fpkFile, string unpackedFpkDir)
        {
            try
            {
                var fpkStructure = new SharedStructures.FPK();

                using (BinaryReader fpkReader = new BinaryReader(File.Open(fpkFile, FileMode.Open, FileAccess.Read)))
                {


                    using (MemoryStream newFpkStream = new MemoryStream())
                    {
                        using (BinaryWriter newFpkWriter = new BinaryWriter(newFpkStream))
                        {

                        }
                    }
                }

                LoggingMethods.LogMessage(SharedMethods.NewLineChara);
                LoggingMethods.LogMessage("Repacking has completed!");
                LoggingMethods.LogMessage(SharedMethods.NewLineChara);

                SharedMethods.AppMsgBox("Repacked " + Path.GetFileName(fpkFile) + " file", "Success", MessageBoxIcon.Information);
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