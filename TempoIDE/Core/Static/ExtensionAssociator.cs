using Jammo.CsAnalysis.CodeInspection;
using Jammo.CsAnalysis.CodeInspection.Rules;
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

        public static CodeInspector CodeInspectorFromExtension(string extension)
        {
            switch (extension.Replace(".", string.Empty))
            {
                case "cs":
                    var inspector = new CodeInspector();
                    inspector.WithRules(new[] { new UnusedFieldInspection() });
                    
                    return inspector;
            }

            return new CodeInspector();
        }
    }
}