using System;
using System.Linq;
using System.Windows.Forms;

namespace Drakengard1and2Extractor.Support
{
    internal class LoggingHelpers
    {
        private static TextBox _StatusTxtBox = Application.OpenForms["CoreForm"].Controls["StatusTextBox"] as TextBox;
        private static TextBox _BatchModeStatusTxtBox = IsBatchModeFormOpen();


        private static TextBox IsBatchModeFormOpen()
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form.Name == "BatchMode")
                {
                    return form.Controls["BatchStatusTxtBox"] as TextBox;
                }
            }

            return null;
        }


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


        public static void LogMessageBatch(string message)
        {
            _BatchModeStatusTxtBox.BeginInvoke((Action)(() =>
            {
                var lines = _BatchModeStatusTxtBox.Text.Split(CoreForm.NewLineChara.ToCharArray());
                if (lines.Length > 700)
                {
                    _BatchModeStatusTxtBox.Text = string.Join(CoreForm.NewLineChara, lines.Skip(700));
                }
                _BatchModeStatusTxtBox.AppendText(message + CoreForm.NewLineChara);
            }));
        }
    }
}