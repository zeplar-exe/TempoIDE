using TempoControls.Core.Types.Collections;
using TempoIDE.Core.Inspections;
using TempoIDE.Core.Static;

namespace TempoIDE.UserControls.Editors
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
            
            var inspector = IFileInspector.FromExtension(BoundFile?.Extension);
            var project = EnvironmentHelper.GetProjectOfFile(BoundFile);

            inspector?.Inspect(characters, project?.Compilation);
        }
    }
}