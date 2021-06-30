using System;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace TempoIDE.Classes
{
    public static class Extensions
    {
        public static string GetPlainText(this RichTextBox textBox)
        {
            return new TextRange(textBox.Document.ContentStart, textBox.Document.ContentEnd).Text;
        }
        
        public static void AppendColoredText(this RichTextBox box, string text, string color)
        {
            var bc = new BrushConverter();
            var tr = new TextRange(box.Document.ContentEnd, box.Document.ContentEnd)
            {
                Text = text
            };

            try 
            { 
                tr.ApplyPropertyValue(TextElement.ForegroundProperty, 
                    bc.ConvertFromString(color)); 
            }
            catch (FormatException) { }
            catch (ArgumentNullException) { }
        }
    }
}