using System;
using System.Windows;
using System.Windows.Input;
using TempoIDE.UserControls;

namespace TempoIDE.Classes.EditorCommands
{
    public class SelectAll : IEditorCommand
    {
        public Key[] Keybinds { get; } =
        {
            Key.LeftCtrl,
            Key.A
        };
        
        public void Execute(SyntaxTextBox textBox)
        {
            textBox.SelectionRange = new IntRange(0, textBox.TextArea.Text.Length);
        }
    }
}