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
        
        public MergeableCompilation Compilation;

        public CachedProjectCompilation(ProjectInSolution project)
        {
            Project = project;
            
            Update();
        }

        public void Update()
        {
            var analysis = CSharpAnalysisWrapper.CreateAsync(Project.AbsolutePath, AnalysisType.Project).Result;

            Compilation = analysis.Compilations.FirstOrDefault()?.Merge(analysis.Compilations.Skip(1).ToArray());
        }
    }
}