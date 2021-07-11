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
                textBox.AppendTextAtCaret(Clipboard.GetText(TextDataFormat.UnicodeText));   
            }
            else
            {
                textBox.Backspace(0);
                textBox.AppendTextAtCaret(Clipboard.GetText(TextDataFormat.UnicodeText));
            }
        }
    }
}