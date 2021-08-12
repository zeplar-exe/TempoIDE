using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Build.Construction;
using Microsoft.CodeAnalysis;
using TempoAnalysis;

namespace TempoIDE.Classes.Types
{
    public class CachedProjectCompilation
    {
        public readonly ProjectInSolution Project;
        
        public MergedCompilation Compilation;

        public CachedProjectCompilation(ProjectInSolution project)
        {
            Project = project;
            
            Update();
        }

        public void Update()
        {
            var analysis = CSharpAnalysis.CreateAsync(Project.AbsolutePath, AnalysisType.Project).Result;

            if (analysis.Compilations.Count == 0)
                return;

            Compilation = analysis.Compilations.First().Merge(analysis.Compilations.Skip(1).ToArray());
        }
    }
}