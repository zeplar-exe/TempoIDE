using Jammo.TextAnalysis.DotNet.CSharp;
using Jammo.TextAnalysis.DotNet.MsBuild;

namespace TempoIDE.Core.DataStructures
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
            Compilation.GenerateCompilation();
        }
    }
}