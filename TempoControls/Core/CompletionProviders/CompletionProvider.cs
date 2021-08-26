using TempoControls.Core.Types;

namespace TempoControls.Core.CompletionProviders
{
    public interface ICompletionProvider
    {
        public AutoCompletion[] GetAutoCompletions(ColoredTextBox label);
    }
    
    public class DefaultCompletionProvider : ICompletionProvider
    {
        public AutoCompletion[] GetAutoCompletions(ColoredTextBox label)
        {
            return null;
        }
    }
}