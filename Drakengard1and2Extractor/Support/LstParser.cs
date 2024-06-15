using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Drakengard1and2Extractor.Support
{
    internal class LstParser
    {
        public static void ProcessLstFile(SharedStructures.FPK fpkStructure, bool isSingleFile, string extractDir, Dictionary<string, string> filesExtractedDict)
        {
            try
            {
                var lstFile = Path.Combine(extractDir, "FILE_1.lst");

                if (File.Exists(lstFile))
                {
                    var linesBuffer = File.ReadAllLines(lstFile);
                    var lineCount = linesBuffer.Length;

                    if (lineCount == fpkStructure.EntryCount)
                    {
                        if (isSingleFile)
                        {
                            LoggingMethods.LogMessage(SharedMethods.NewLineChara);
                            LoggingMethods.LogMessage("Generating paths....");
                        }

                        var generatedPathsFolder = Path.Combine(extractDir, "#Generated_Paths");
                        SharedMethods.IfFileDirExistsDel(generatedPathsFolder, SharedMethods.DelSwitch.directory);
                        Directory.CreateDirectory(generatedPathsFolder);

                        var separatorSymbols = new string[]
                        {
                            "/", "\\", "/..", "\\..", "../", "..\\", "./", ".\\", "/.", "\\."
                        };

                        var fileCounter = 1;
                        for (int l = 0; l < lineCount; l++)
                        {
                            var currentLine = linesBuffer[l];

                            if (currentLine == "" || currentLine == " " || separatorSymbols.Contains(currentLine))
                            {
                                continue;
                            }

                            currentLine = currentLine.Replace
                                (separatorSymbols[2], "").Replace(separatorSymbols[3], "").
                                Replace(separatorSymbols[4], "").Replace(separatorSymbols[5], "").
                                Replace(separatorSymbols[6], "").Replace(separatorSymbols[7], "").
                                Replace(separatorSymbols[8], "").Replace(separatorSymbols[9], "");

                            var generatedFPath = Path.Combine(generatedPathsFolder, Path.GetDirectoryName(currentLine), Path.GetFileName(currentLine));

                            if (!Directory.Exists(Path.GetDirectoryName(generatedFPath)))
                            {
                                Directory.CreateDirectory(Path.GetDirectoryName(generatedFPath));
                            }

                            File.Copy(filesExtractedDict[$"FILE_{fileCounter}"], generatedFPath, true);
                            fileCounter++;
                        }
                    }
                    else
                    {
                        LoggingMethods.LogMessage(SharedMethods.NewLineChara);
                        LoggingMethods.LogMessage("Mismatch detected in lst file. Path generation failed!");
                    }
                }
                else
                {
                    LoggingMethods.LogMessage(SharedMethods.NewLineChara);
                    LoggingMethods.LogMessage("Unable to find lst file. Path generation failed!");
                }
            }
            catch (Exception ex)
            {
                SharedMethods.AppMsgBox("" + ex, "Error", MessageBoxIcon.Error);
                return;
            }
        }
    }
}