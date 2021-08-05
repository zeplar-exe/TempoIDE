using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using TempoIDE.Windows;

namespace TempoIDE.Classes.Types
{
    public class EnvironmentCache
    {
        public List<SemanticModel> Models = new List<SemanticModel>();
        private readonly Dictionary<string, CachedFile> fileData = new Dictionary<string, CachedFile>();
        
        public void UpdateModels()
        {
            if (EnvironmentHelper.Mode == EnvironmentMode.Solution)
            {
                var workspace = MSBuildWorkspace.Create();
                var solution = workspace.OpenSolutionAsync(EnvironmentHelper.EnvironmentPath.FullName).Result;

                foreach (var project in solution.Projects)
                {
                    var compilation = project.GetCompilationAsync().Result;

                    if (compilation is null)
                        return;

                    var model = compilation.GetSemanticModel(compilation.SyntaxTrees.First());

                    Models.Add(model);
                }
            }
            else if (EnvironmentHelper.Mode == EnvironmentMode.Directory)
            {
                // This
            }
            else if (EnvironmentHelper.Mode == EnvironmentMode.File)
            {
                // This
            }
        }

        public CachedFile GetFile(FileInfo file)
        {
            return fileData.TryGetValue(file.FullName, out var cached) ? cached : null;
        }

        public void AddFile(FileInfo file)
        {
            if (file.Exists)
            {
                if (fileData.TryGetValue(file.FullName, out var cached))
                    cached.Update();
                else
                    fileData.Add(file.FullName, new CachedFile(file));
            }
        }

        public void RemoveFile(FileInfo file)
        {
            if (fileData.ContainsKey(file.FullName))
                fileData.Remove(file.FullName);
        }
    }
}