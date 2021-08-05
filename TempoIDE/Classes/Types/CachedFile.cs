using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TempoIDE.Classes.Types
{
    public class CachedFile
    {
        public CompilationUnitSyntax Unit;

        public readonly FileInfo File;
        public string Content { get; private set; }

        public CachedFile(FileInfo file)
        {
            File = file;
            
            Update();
        }

        public void Update()
        {
            File.Refresh();

            if (!File.Exists)
                return;
            
            using var file = File.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var buffer = new BufferedStream(file);
            using var reader = new StreamReader(buffer);
            
            Content = reader.ReadToEnd();

            var syntaxTree = CSharpSyntaxTree.ParseText(Content);
            Unit = syntaxTree.GetCompilationUnitRoot();
        }
        
        public UsingDirectiveSyntax VerifyNamespace(string ns)
        {
            throw new NotImplementedException();
        }

        public UsingDirectiveSyntax VerifyType(string type)
        {
            throw new NotImplementedException();
        }
    }
}