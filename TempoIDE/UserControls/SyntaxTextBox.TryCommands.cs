using System;
using System.Windows;
using TempoIDE.Classes;

namespace TempoIDE.UserControls
{
    public partial class SyntaxTextBox
    {
        public bool TrySelectAll()
        {
            SelectionRange = new IntRange(0, TextArea.Text.Length);

            return true;
        }

        public bool TryCopyText()
        {
            var text = GetSelectedText();
            
            if (string.IsNullOrEmpty(text)) 
                return false;
            
            Clipboard.SetText(text, TextDataFormat.UnicodeText);
                
            return true;

        }

        public bool TryPasteText()
        {
            var text = Clipboard.GetText();

            if (string.IsNullOrEmpty(text))
                return false;
            
            if (GetSelectedText() == string.Empty)
            {
                AppendTextAtCaret(Clipboard.GetText(TextDataFormat.UnicodeText));   
            }
            else
            {
                Backspace(0);
                AppendTextAtCaret(Clipboard.GetText(TextDataFormat.UnicodeText));
            }
            
            return true;
        }

        public bool TryCutText()
        {
            var text = GetSelectedText();
            
            if (string.IsNullOrEmpty(text)) 
                return false;
            
            Clipboard.SetText(text, TextDataFormat.UnicodeText);
            Backspace(0);
        
            return true;
        }
    }
}