using Jammo.CsAnalysis.Compilation;
using Jammo.CsAnalysis.MsBuildAnalysis;
using TempoIDE.Core.Static;

namespace TempoIDE.Core.Types.Wrappers
{
    public class CachedProjectCompilation
    {
        public readonly JProjectFile Project;
        public CompilationWrapper Compilation;

        public CachedProjectCompilation(JProjectFile project)
        {
            Project = project;
            
            Update();
        }

        public void Update()
        {
            var analysis = CSharpFileAnalysisWrapper.Create(Project.FileInfo.FullName, AnalysisType.Project);
            Compilation = analysis.CompilationWrapper;
            
            Compilation.SetInspector(ExtensionAssociator.CodeInspectorFromExtension(".cs"));
            //Compilation.GenerateCompilation(); Disabled for now for performance reasons
        }
    }
}