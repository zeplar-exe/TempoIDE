using TempoControls.Core.CompletionProviders;
using TempoControls.Core.SyntaxSchemes;
using TempoIDE.Core.Inspection;

namespace TempoIDE.Core.Static
{
    public static class ExtensionAssociator
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
        
        public static IFileInspector InspectorFromExtension(string extension)
        {
            return extension.Replace(".", string.Empty) switch
            {
                "cs" => new CsFileInspector(),
                _ => new DefaultFileInspector()
            };
        }
    }
}