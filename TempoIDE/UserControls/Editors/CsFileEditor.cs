using TempoControls.Core.Types.Collections;
using TempoIDE.Core.Environments;
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

            if (EnvironmentHelper.Current is SolutionEnvironment solutionEnv) 
            { // TODO: Use compilation of all relevant files if false
                var project = solutionEnv.GetProjectOfFile(BoundFile);

                inspector?.Inspect(characters, project?.Compilation);   
            }
        }
    }
}