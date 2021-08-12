using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace TempoAnalysis
{
    public class CSharpAnalysis
    {
        public List<MergedCompilation> Compilations = new();
        
        public static CSharpAnalysis Create(string filePath, AnalysisType analysisType)
        {
            return new CSharpAnalysis(filePath, analysisType);
        }

        public static async Task<CSharpAnalysis> CreateAsync(string filePath, AnalysisType analysisType)
        {
            return await Task.Run(() => new CSharpAnalysis(filePath, analysisType));
        }
        

        private CSharpAnalysis(string filePath, AnalysisType analysisType)
        {
            switch (analysisType)
            {
                case AnalysisType.File:
                {
                    Compilations.Add(new MergedCompilation(new FileInfo(filePath)));
                    
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