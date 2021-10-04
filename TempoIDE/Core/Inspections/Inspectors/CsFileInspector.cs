using Jammo.TextAnalysis.DotNet.CSharp;
using Jammo.TextAnalysis.DotNet.CSharp.Inspection;
using TempoControls.Core.IntTypes;
using TempoControls.Core.Types;
using TempoControls.Core.Types.Collections;

namespace TempoIDE.Core.Inspections.Inspectors
{
    public class CsFileInspector
    {
        public void Inspect(SyntaxCharCollection characters, CSharpDocument document)
        {
            foreach (var diagnostic in document.Diagnostics)
            {
                var range = new IntRange(diagnostic.Span.Start, diagnostic.Span.End);
                var severity = InspectionSeverityAssociator.FromCode(diagnostic.Info.InspectionCode);
                var brush = InspectionSeverityAssociator.BrushFromSeverity(severity);
            
                characters.UpdateUnderlineType(range, UnderlineType.Straight);
                characters.UpdateUnderline(range, brush);
            }
        }
    }
}