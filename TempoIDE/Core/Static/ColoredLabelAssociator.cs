using TempoControls;
using TempoIDE.Core.CompletionProviders;
using TempoIDE.Core.SyntaxSchemes;

namespace TempoIDE.Core.Static
{
    public static class ColoredLabelAssociator
    {
        public static ISyntaxScheme SchemeFromExtension(string extension)
        {
            return extension.Replace(".", string.Empty) switch
            {
                "cs" => new CsSyntaxScheme(),
                _ => new DefaultSyntaxScheme()
            };
        }
        
        public static ICompletionProvider CompletionProviderFromExtension(string extension)
        {
            return extension.Replace(".", string.Empty) switch
            {
                "cs" => new CsCompletionProvider(),
                _ => new DefaultCompletionProvider()
            };
        }
    }
}