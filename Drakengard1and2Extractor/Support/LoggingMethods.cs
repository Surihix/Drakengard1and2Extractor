using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Drakengard1and2Extractor.Support
{
    internal class LoggingMethods
    {
        private static TextBox StatusTxtBox { get; set; }
        private static string LastMessage { get; set; }


        public static void SetCoreFormStatusBox()
        {
            StatusTxtBox = Application.OpenForms["CoreForm"].Controls["StatusTextBox"] as TextBox;
        }

        public static void SetBatchFormStatusBox()
        {
            StatusTxtBox = Application.OpenForms["BatchForm"].Controls["BatchStatusTextBox"] as TextBox;
        }


        public static void LogMessage(string message)
        {
            StatusTxtBox.Invoke((Action)(() =>
            {
                var lines = StatusTxtBox.Text.Split(SharedMethods.NewLineChara.ToCharArray());
                if (lines.Length > 700)
                {
                    StatusTxtBox.Text = string.Join(SharedMethods.NewLineChara, lines.Skip(700));
                }
                StatusTxtBox.AppendText(message + SharedMethods.NewLineChara);
                StatusTxtBox.ScrollToCaret();
            }));
        }


        public static void LogMessageConstant(string message)
        {
            StatusTxtBox.Invoke((Action)(() =>
            {
                var text = StatusTxtBox.Text;

                if (LastMessage != null)
                {
                    text = text.Replace(LastMessage, "");
                }

                LastMessage = message;
                text += message;

                StatusTxtBox.Text = text;
                StatusTxtBox.SelectionStart = StatusTxtBox.Text.Length;
                StatusTxtBox.ScrollToCaret();
            }));
        }


        public static void LogException(string exceptionMsg)
        {
            SharedMethods.AppMsgBox("Exception recorded in 'Exception.txt' file", "Exception", MessageBoxIcon.Warning);

            var newLineChars = SharedMethods.NewLineChara + SharedMethods.NewLineChara;
            File.AppendAllText("Exception.txt", newLineChars + exceptionMsg + newLineChars);
        }
    }
}