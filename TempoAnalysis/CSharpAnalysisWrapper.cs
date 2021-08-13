using System.IO;
using System.Threading.Tasks;
using Microsoft.Build.Construction;

namespace TempoAnalysis
{
    public class CSharpAnalysisWrapper
    {
        public readonly MergeableCompilation Compilation;
        
        public static CSharpAnalysisWrapper Create(string filePath, AnalysisType analysisType)
        {
            return new CSharpAnalysisWrapper(filePath, analysisType);
        }

        public static async Task<CSharpAnalysisWrapper> CreateAsync(string filePath, AnalysisType analysisType)
        {
            return await Task.Run(() => new CSharpAnalysisWrapper(filePath, analysisType));
        }
        

        private CSharpAnalysisWrapper(string path, AnalysisType analysisType)
        {
            switch (analysisType)
            {
                case AnalysisType.File:
                {
                    Compilation = new MergeableCompilation(new FileInfo(path));
                    
                    break;
                }
                case AnalysisType.Directory:
                {
                    Compilation = MergeDirectory(new DirectoryInfo(path), SearchOption.AllDirectories);
                    
                    break;
                }
                case AnalysisType.Solution:
                {
                    Compilation = MergeSolution(new FileInfo(path));

                    break;
                }
                case AnalysisType.Project:
                {
                    Compilation = MergeProject(new FileInfo(path));
                    
                    break;
                }
            }
        }

        private MergeableCompilation MergeSolution(FileInfo file)
        {
            var solution = SolutionFile.Parse(file.FullName);
            var comp = MergeDirectory(file.Directory, SearchOption.TopDirectoryOnly);

            foreach (var project in solution.ProjectsInOrder)
                comp.Merge(MergeProject(new FileInfo(project.AbsolutePath)));

            return comp;
        }

        private MergeableCompilation MergeProject(FileInfo file)
        {
            return MergeDirectory(file.Directory, SearchOption.AllDirectories); 
            // TODO: Filter based on project config
        }
        
        private MergeableCompilation MergeDirectory(DirectoryInfo directory, SearchOption query)
        {
            MergeableCompilation comp = null;

            foreach (var file in directory.GetFiles("*.cs", query))
            {
                if (comp == null)
                {
                    comp = new MergeableCompilation(file);
                            
                    continue;
                }

                comp.Merge(new MergeableCompilation(file));
            }

            return comp;
        }
    }

    public enum AnalysisType
    {
        File,
        Solution,
        Project,
        Directory,
    }
}