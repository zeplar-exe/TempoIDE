using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace TempoAnalysis
{
    public class CSharpAnalysisWrapper
    {
        public List<MergeableCompilation> Compilations = new();
        
        public static CSharpAnalysisWrapper Create(string filePath, AnalysisType analysisType)
        {
            return new CSharpAnalysisWrapper(filePath, analysisType);
        }

        public static async Task<CSharpAnalysisWrapper> CreateAsync(string filePath, AnalysisType analysisType)
        {
            return await Task.Run(() => new CSharpAnalysisWrapper(filePath, analysisType));
        }
        

        private CSharpAnalysisWrapper(string filePath, AnalysisType analysisType)
        {
            switch (analysisType)
            {
                case AnalysisType.File:
                {
                    Compilations.Add(new MergeableCompilation(new FileInfo(filePath)));
                    
                    break;
                }
            }
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