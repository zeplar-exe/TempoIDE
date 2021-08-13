using Microsoft.Build.Construction;
using TempoAnalysis;

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
            var analysis = CSharpAnalysisWrapper.CreateAsync(Project.AbsolutePath, AnalysisType.Project).Result;

            Compilation = analysis.Compilation;
        }
    }
}