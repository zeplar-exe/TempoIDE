using System.Windows;
using System.Windows.Input;
using TempoIDE.UserControls;

namespace TempoIDE.Classes.EditorCommands
{
    public class Cut : IEditorCommand
    {
        public Key[] Keybinds { get; } =
        {
            Key.LeftCtrl,
            Key.X
        };
        
        public void Execute(SyntaxTextBox textBox)
        {
            if (textBox.GetSelectedText() == string.Empty)
                return;
            
            Clipboard.SetText(textBox.GetSelectedText(), TextDataFormat.UnicodeText);
            
            textBox.Backspace(0);
        }
    }
}