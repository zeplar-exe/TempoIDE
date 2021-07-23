using TempoIDE.UserControls;

namespace TempoIDE.Classes.Types
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
        }
    }
}