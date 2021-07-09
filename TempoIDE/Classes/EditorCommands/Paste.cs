using System.Windows;
using System.Windows.Input;
using TempoIDE.UserControls;

namespace TempoIDE.Classes.EditorCommands
{
    public class Paste : IEditorCommand
    {
        public Key[] Keybinds { get; } =
        {
            Key.LeftCtrl,
            Key.V
        };
        
        public void Execute(SyntaxTextBox textBox)
        {
            if (textBox.GetSelectedText() == string.Empty)
            {
                // TODO: This appends after the newline if the caret is at that point
                
                textBox.AppendTextAtCaret(Clipboard.GetText(TextDataFormat.UnicodeText));   
            }
            else
            {
                return; 
                // TODO: Fix this, it removes characters when it shouldn't

                foreach (int index in textBox.SelectionRange)
                {
                    textBox.RemoveIndex(index);
                }
            }
        }
    }
}