using Jammo.TextAnalysis.DotNet.CSharp.Inspection;
using TempoControls.Core.IntTypes;
using TempoControls.Core.Types;
using TempoControls.Core.Types.Collections;
using TempoIDE.Core.Helpers;
using TempoIDE.Core.Inspections;

namespace TempoIDE.Controls.Editors
{
    public class CsFileEditor : TextFileEditor
    {
        public CsFileEditor()
        {
            ApplicationHelper.InspectionsEnabledChanged += _ => TextBox.TextArea.InvalidateTextChanged();
            TextBox.TextArea.AfterHighlight += Label_OnAfterHighlight;
        }
        
        private void Label_OnAfterHighlight(SyntaxCharCollection characters)
        {
            Inspect(characters);
        }

        private void Inspect(SyntaxCharCollection characters)
        {
            if (!ApplicationHelper.InspectionsEnabled)
                return;

            var inspector = new CSharpInspector(); // TODO: Assume C#, will change 
            inspector.Inspect(EnvironmentHelper.Current.GetRelevantCompilation(BoundFile));
            
            foreach (var diagnostic in inspector.Diagnostics)
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