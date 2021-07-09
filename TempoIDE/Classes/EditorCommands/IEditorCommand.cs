using System.Windows.Input;
using TempoIDE.UserControls;

namespace TempoIDE.Classes.EditorCommands
{
    public interface IEditorCommand
    {
        public Key[] Keybinds { get; }

        public void Execute(SyntaxTextBox textBox);
    }
}