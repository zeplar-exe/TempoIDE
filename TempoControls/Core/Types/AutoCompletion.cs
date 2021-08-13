using TempoControls.Controls;
using TempoControls.Core.Static;

namespace TempoControls.Core.Types
{
    public class AutoCompletion
    {
        public readonly string Value;

        public AutoCompletion(string value)
        {
            Value = value;
        }

        public virtual void Execute(SyntaxTextBox textBox)
        {
            textBox.AppendTextAtCaret(Value.ReplaceFirst(textBox.GetTypingWord(true), ""));
            
            textBox.ClearSelection();
            textBox.AutoComplete.Disable();
        }
    }
}