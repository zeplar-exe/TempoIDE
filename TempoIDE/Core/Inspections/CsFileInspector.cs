using System.Linq;
using Jammo.TextAnalysis.DotNet.CSharp;
using TempoControls.Core.IntTypes;
using TempoControls.Core.Types;
using TempoControls.Core.Types.Collections;

namespace TempoIDE.Core.Inspections
{
    public class CsFileInspector : IFileInspector
    {
        public void Inspect(SyntaxCharCollection characters, CSharpAnalysisCompilation compilation)
        {
            if (compilation == null)
            {
                compilation = new CSharpAnalysisCompilation();
                compilation.AppendText(characters.ToString());
                
                compilation.GenerateInspections();
            }
            
            if (compilation.Inspections == null)
                return;

            foreach (var inspection in compilation.Inspections.ToArray())
            {
                var range = new IntRange(inspection.Span.Start, inspection.Span.End);
                var severity = InspectionSeverityAssociator.FromCode(inspection.Rule.GetInspectionInfo().InspectionCode);
                var brush = InspectionSeverityAssociator.BrushFromSeverity(severity);
            
                characters.UpdateUnderlineType(range, UnderlineType.Straight);
                characters.UpdateUnderline(range, brush);
            }
        }
    }
}