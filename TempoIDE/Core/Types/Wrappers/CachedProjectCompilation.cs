using JammaNalysis.Compilation;
using Microsoft.Build.Construction;

namespace TempoIDE.Core.Types.Wrappers
{
    public class CachedProjectCompilation
    {
        public readonly ProjectInSolution Project;
        
        public MergeableCompilation Compilation;

        public CachedProjectCompilation(ProjectInSolution project)
        {
            Project = project;
            
            Update();
        }

        public void Update()
        {
            var analysis = CSharpAnalysisWrapper.Create(Project.AbsolutePath, AnalysisType.Project);

            Compilation = analysis.Compilation;
        }
    }
}