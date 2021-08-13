using TempoIDE.Core.Types;
using TempoIDE.UserControls;

namespace TempoIDE.Core.CompletionProviders
{
    public static class CompletionProviderFactory
    {
        public static ICompletionProvider FromExtension(string extension)
        {
            switch (extension.Replace(".", string.Empty))
            {
                case "cs":
                    return new CsCompletionProvider();
                default:
                    return new DefaultCompletionProvider();
            }
        }
    }

    public interface ICompletionProvider
    {
        public AutoCompletion[] GetAutoCompletions(SyntaxTextBox label);
    }
}