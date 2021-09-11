using Jammo.CsAnalysis.CodeInspection;
using Jammo.CsAnalysis.CodeInspection.Rules;
using TempoControls.Core.CompletionProviders;
using TempoControls.Core.SyntaxSchemes;
using TempoIDE.Core.Inspections.TempoRules;

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

        public static CodeInspector CodeInspectorFromExtension(string extension)
        {
            switch (extension.Replace(".", string.Empty))
            {
                case "cs":
                    var inspector = new CodeInspector();
                    inspector.AddRules(new InspectionRule[]
                    {
                        new UnusedFieldInspection(), 
                        new MisspelledWordInspection()
                    });
                    
                    return inspector;
                default:
                    return new CodeInspector();
            }
        }
    }
}