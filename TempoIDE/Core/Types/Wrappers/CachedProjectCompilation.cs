using Jammo.TextAnalysis.DotNet.CSharp;
using Jammo.TextAnalysis.DotNet.MsBuild;
using TempoIDE.Core.Static;

namespace TempoIDE.Core.Types.Wrappers
{
    public class CachedProjectCompilation
    {
        public readonly JProjectFile Project;
        public CSharpAnalysisCompilation Compilation;

        public CachedProjectCompilation(JProjectFile project)
        {
            Project = project;
            
            Update();
        }

        public void Update()
        {
            Compilation = CSharpAnalysisCompilationHelper.Create(Project.FileInfo.FullName, AnalysisType.Project);
            
            Compilation.SetInspector(ExtensionAssociator.CodeInspectorFromExtension(".cs"));
            Compilation.GenerateInspections();
        }
    }
}