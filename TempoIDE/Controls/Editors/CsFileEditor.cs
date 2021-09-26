using Jammo.TextAnalysis.DotNet.CSharp;
using TempoControls.Core.Types.Collections;
using TempoIDE.Core.Environments;
using TempoIDE.Core.Helpers;
using TempoIDE.Core.Inspections.Inspectors;

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
            
            var inspector = IFileInspector.FromExtension(BoundFile?.Extension);

            var compilation = EnvironmentHelper.Current switch
            {
                SolutionEnvironment solutionEnv => solutionEnv.GetProjectOfFile(BoundFile)?.Compilation,
                
                DirectoryEnvironment directoryEnv => CSharpAnalysisCompilationHelper.Create(
                    directoryEnv.EnvironmentPath.FullName, AnalysisType.Directory),
                
                FileEnvironment fileEnv => CSharpAnalysisCompilationHelper.Create(fileEnv.EnvironmentPath.FullName,
                    AnalysisType.File),
                
                _ => null
            };

            inspector?.Inspect(characters, compilation);
        }
    }
}