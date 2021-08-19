using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Build.Construction;

namespace TempoAnalysis.MsBuildAnalysis
{
    public class CsSolutionFile
    {
        private SolutionFile file;

        public readonly IEnumerable<CsProjectFile> Projects;
        
        public CsSolutionFile(string path)
        {
            var info = new FileInfo(path);

            if (!info.Exists)
                throw new ArgumentException($"File '{info.Name}' does not exist.");

            if (info.Extension != ".sln")
                throw new ArgumentException("Expected a .sln file.");

            Projects = SolutionFile.Parse(path).ProjectsInOrder
                .Select(p => new CsProjectFile(p.AbsolutePath));
        }
    }
}