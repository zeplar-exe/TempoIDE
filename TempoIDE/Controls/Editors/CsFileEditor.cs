using System.Windows.Media;
using TempoControls.Core.IntTypes;
using TempoIDE.Controls.CodeEditing.BlockElements;
using TempoIDE.Controls.CodeEditing.BlockElements.Common;
using TempoIDE.Core.Helpers;

namespace TempoIDE.Controls.Editors;

public class CsFileEditor : TextFileEditor
{
    public CsFileEditor()
    {
        ApplicationHelper.InspectionsEnabledChanged += _ => v_Codespace.InvalidateVisualChanged();
        v_Codespace.ModificationReady += CodespaceOnModificationReady;
        // TODO: Add OnInspectionReady
    }

    private void CodespaceOnModificationReady(object? sender, FormattedDocument document)
    {
        Inspect(document);
    }

    private void Inspect(FormattedDocument document)
    {
        if (!ApplicationHelper.InspectionsEnabled)
            return;
        
        var range = new IntRange(0, document.TextLength);

        foreach (var character in document.GetCharactersInRange(range))
        {
            var visual = new UnderlineVisual(UnderlineType.Straight, Brushes.Red, 1);
                
            var characterVisual = new FormattedCharacterVisual(FormattedVisualPosition.BottomLeft, visual);
            character.AddVisual(characterVisual);
        }
            
        // foreach (var diagnostic in EnvironmentHelper.Current.GetFileDiagnostics(BoundFile))
        // {
            // var range = new IntRange(diagnostic.Span.Start, diagnostic.Span.End);
            // var severity = InspectionSeverityAssociator.FromCode(diagnostic.Info.InspectionCode);
            // var brush = InspectionSeverityAssociator.BrushFromSeverity(severity);

            // foreach (var character in document.GetCharactersInRange(range))
            // {
                // var visual = new UnderlineVisual(UnderlineType.Straight, Brushes.Red, 3);
                
                // var characterVisual = new FormattedCharacterVisual(FormattedVisualPosition.BottomLeft, visual);
                // character.AddVisual(characterVisual);
            // }
        // }
    }
}