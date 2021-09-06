using System.Windows.Media;
using Jammo.CsAnalysis.Compilation;
using TempoControls.Core.IntTypes;
using TempoControls.Core.Types;
using TempoControls.Core.Types.Collections;

namespace TempoIDE.Core.Inspection
{
    public class CsFileInspector : IFileInspector
    {
        public void Inspect(SyntaxCharCollection characters, CompilationWrapper compilation)
        {
            foreach (var inspection in compilation.Inspections)
            {
                var range = new IntRange(inspection.Span.Start, inspection.Span.End);
                
                characters.UpdateUnderlineType(range, UnderlineType.Straight);
                characters.UpdateUnderline(range, Brushes.Yellow);
            }
        }
    }
}