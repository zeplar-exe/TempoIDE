using System.Windows.Media;
using TempoControls.Core.IntTypes;
using TempoControls.Core.Types;
using TempoControls.Core.Types.Collections;
using TempoIDE.Core.Helpers;

namespace TempoIDE.Controls.Editors
{
    public class CsFileEditor : TextFileEditor
    {
        public CsFileEditor()
        {
            TextBox.TextArea.AfterHighlight += Label_OnAfterHighlight;
            // TODO: Improve ColoredTextBox a tonne
        }
        
        private void Label_OnAfterHighlight(SyntaxCharCollection characters)
        {
            Inspect(characters);
        }

        private void Inspect(SyntaxCharCollection characters)
        {
            foreach (var diagnostic in EnvironmentHelper.Current.GetFileDiagnostics(BoundFile))
            {
                var range = new IntRange(diagnostic.Span.Start, diagnostic.Span.End);
                var brush = Brushes.Orange; // TODO: Implement InspectionSeverity from SharpEye

                characters.UpdateUnderlineType(range, UnderlineType.Straight);
                characters.UpdateUnderline(range, brush);
            }
        }
    }
}