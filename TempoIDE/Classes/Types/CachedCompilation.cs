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
        
        /* TODO: Fix Unhandled exception. System.TypeLoadException: Could not load type 'System.Runtime.Remoting.RemotingServices' from assembly 'mscorlib,
        Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'.
        at Microsoft.Build.BackEnd.TaskExecutionHost.Dispose(Boolean disposing)
        at Microsoft.Build.BackEnd.TaskExecutionHost.Finalize()*/

    }
}