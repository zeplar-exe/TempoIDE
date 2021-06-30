using System.Windows.Controls;
using System.Windows.Documents;

namespace TempoIDE.Classes
{
    public static class Extensions
    {
        public static string GetPlainText(this RichTextBox textBox)
        {
            return new TextRange(textBox.Document.ContentStart, textBox.Document.ContentEnd).Text;
        }
    }
}