using TempoControls.Core.Types;

namespace TempoControls
{
    public interface ICompletionProvider
    {
        public AutoCompletion[] GetAutoCompletions(ColoredTextBox label);
    }
}