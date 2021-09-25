using Jammo.TextAnalysis.DotNet.CSharp.Inspection;
using Jammo.TextAnalysis.DotNet.CSharp.Inspection.Rules;
using TempoControls.Core.CompletionProviders;
using TempoControls.Core.SyntaxSchemes;

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

        public static CSharpInspector CodeInspectorFromExtension(string extension)
        {
            switch (extension.Replace(".", string.Empty))
            {
                case "cs":
                    var inspector = new CSharpInspector();
                    inspector.AddRules(new CSharpInspectionRule[]
                    {
                        new UnusedFieldInspection()
                    });
                    
                    return inspector;
                default:
                    return null;
            }
        }
    }
}