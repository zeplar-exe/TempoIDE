using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TempoAnalysis
{
    public class MergedCompilation
    {
        public readonly bool Success;

        public readonly CompilationNamespace GlobalNamespace;

        public readonly FileInfo Info;

        public MergedCompilation(FileInfo file, params FileInfo[] references)
        {
            Info = file;
            Info.Refresh();

            try
            {
                using var fileStream = file.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using var reader = new StreamReader(fileStream);
                
                var tree = CSharpSyntaxTree.ParseText(reader.ReadToEndAsync().Result);
                var root = (CompilationUnitSyntax)tree.GetRoot();
                
                var compilation = CSharpCompilation.Create("Compilation")
                    .AddReferences(references.Select(r => MetadataReference.CreateFromFile(r.FullName)))
                    .AddSyntaxTrees(tree);

                GlobalNamespace = ParseNamespace(compilation.GlobalNamespace);
                
                Success = true;
            }
            catch (IOException e)
            {
                Success = false;
            }
        }

        private CompilationNamespace ParseNamespace(INamespaceSymbol namespaceSymbol)
        {
            var compNamespace = new CompilationNamespace(namespaceSymbol.Name);

            foreach (var type in namespaceSymbol.GetTypeMembers())
                compNamespace.Types.Add(ParseType(type, compNamespace));

            foreach (var childNamespace in namespaceSymbol.GetNamespaceMembers())
                compNamespace.Namespaces.Add(ParseNamespace(childNamespace));
            
            return compNamespace;
        }

        private CompilationNamespaceType ParseType(INamedTypeSymbol type, CompilationNamespace compNamespace)
        {
            var compType = new CompilationNamespaceType(
                type.Name,
                TypeModifier.Private, // TODO: Get modifier
                compNamespace,
                type.IsStatic,
                type.IsAbstract,
                type.IsSealed);

            foreach (var member in type.GetMembers())
                compType.Members.Add(ParseMember(member, compType));

            foreach (var nestedType in type.GetTypeMembers())
                compType.Types.Add(ParseType(nestedType, compNamespace));

            return compType;
        }

        private ICompilationTypeMember ParseMember(ISymbol member, CompilationNamespaceType type)
        {
            if (member.Kind == SymbolKind.Method)
                return ParseMethod(member, type);

            var compMember = new CompilationTypeMember(
                member.Name,
                TypeModifier.Private,
                type,
                member.IsStatic,
                member.IsAbstract,
                member.IsVirtual,
                ((IFieldSymbol)member).Type.Name);

            return compMember;
        }

        private CompilationTypeMethod ParseMethod(ISymbol member, CompilationNamespaceType type)
        {
            var method = (IMethodSymbol)member;
            
            var compMethod = new CompilationTypeMethod(
                method.Name,
                TypeModifier.Private,
                type,
                member.IsStatic,
                member.IsAbstract,
                member.IsVirtual,
                method.ReturnType.Name);

            return compMethod;
        }

        public MergedCompilation Merge(params MergedCompilation[] other)
        {
            throw new NotImplementedException();
        }
    }
}