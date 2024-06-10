using System;
using System.Linq;
using System.Windows.Forms;

namespace Drakengard1and2Extractor.Support.LoggingHelpers
{
    internal class BatchFormLogHelper
    {
        private static TextBox _StatusTxtBox = Application.OpenForms["BatchForm"].Controls["BatchStatusTextBox"] as TextBox;

        public static void LogMessage(string message)
        {
            _StatusTxtBox.BeginInvoke((Action)(() =>
            {
                var lines = _StatusTxtBox.Text.Split(CoreForm.NewLineChara.ToCharArray());
                if (lines.Length > 700)
                {
                    _StatusTxtBox.Text = string.Join(CoreForm.NewLineChara, lines.Skip(700));
                }
                _StatusTxtBox.AppendText(message + CoreForm.NewLineChara);
            }));
        }
    }
}