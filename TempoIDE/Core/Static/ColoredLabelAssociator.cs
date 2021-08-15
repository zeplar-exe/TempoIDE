using TempoControls;
using TempoIDE.CompletionProviders;
using TempoIDE.SyntaxSchemes;

namespace TempoIDE.Core.Static
{
    public static class ColoredLabelAssociator
    {
        public static ISyntaxScheme SchemeFromExtension(string extension)
        {
            switch (extension.Replace(".", string.Empty))
            {
                case "cs":
                    return new CsSyntaxScheme();
                default:
                    return new DefaultSyntaxScheme();
            }
        }
        
        public static ICompletionProvider CompletionProviderFromExtension(string extension)
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
}