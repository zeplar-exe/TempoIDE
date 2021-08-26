using JammaNalysis.Compilation;
using JammaNalysis.MsBuildAnalysis;

namespace TempoIDE.Core.Types.Wrappers
{
    public class CachedProjectCompilation
    {
        public readonly CsProjectFile Project;
        public MergeableCompilation Compilation;

        public CachedProjectCompilation(CsProjectFile project)
        {
            Project = project;
            
            Update();
        }

        public void Update()
        {
            var analysis = CSharpAnalysisWrapper.Create(Project.FilePath, AnalysisType.Project);
            
            Compilation = analysis.Compilation;
        }
    }
}