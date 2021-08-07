using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace TempoIDE.Classes.Types
{
    public class CachedCompilation
    {
        public readonly Project Project;

        public Compilation Compilation;
        public IEnumerable<INamedTypeSymbol> Types;

        public CachedCompilation(Project project)
        {
            Project = project;
            
            Update();
        }

        public void Update()
        {
            Compilation = Project.GetCompilationAsync().Result;
            
            if (Compilation is null)
                throw new Exception("Project compilation failed.");
            
            Types = Compilation.GlobalNamespace.GetTypeMembers();
        }
    }
}