using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Drakengard1and2Extractor.Support
{
    internal class LoggingMethods
    {
        private static TextBox _statusTxtBox { get; set; }

        public static void SetCoreFormStatusBox()
        {
            _statusTxtBox = Application.OpenForms["CoreForm"].Controls["StatusTextBox"] as TextBox;
        }

        public static void SetBatchFormStatusBox()
        {
            _statusTxtBox = Application.OpenForms["BatchForm"].Controls["BatchStatusTextBox"] as TextBox;
        }


        public static void LogMessage(string message)
        {
            _statusTxtBox.BeginInvoke((Action)(() =>
            {
                var lines = _statusTxtBox.Text.Split(CommonMethods.NewLineChara.ToCharArray());
                if (lines.Length > 700)
                {
                    _statusTxtBox.Text = string.Join(CommonMethods.NewLineChara, lines.Skip(700));
                }
                _statusTxtBox.AppendText(message + CommonMethods.NewLineChara);
            }));
        }


        public static void LogException(string exceptionMsg)
        {
            CommonMethods.AppMsgBox("Exception recorded in 'Exception.txt' file", "Exception", MessageBoxIcon.Warning);

            var newLineChars = CommonMethods.NewLineChara + CommonMethods.NewLineChara;
            File.AppendAllText("Exception.txt", newLineChars + exceptionMsg + newLineChars);
        }
    }
}