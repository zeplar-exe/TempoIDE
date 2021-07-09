using System.Windows;
using System.Windows.Input;
using TempoIDE.UserControls;

namespace TempoIDE.Classes.EditorCommands
{
    public class Copy : IEditorCommand
    {
        public Key[] Keybinds { get; } =
        {
            Key.LeftCtrl,
            Key.C
        };
        
        public void Execute(SyntaxTextBox textBox)
        {
            Clipboard.SetText(textBox.GetSelectedText(), TextDataFormat.UnicodeText);
        }
    }
}