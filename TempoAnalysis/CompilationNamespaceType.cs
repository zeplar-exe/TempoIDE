using System.Collections.Generic;

namespace TempoAnalysis
{
    public class CompilationNamespaceType
    {
        public readonly string Name;
        
        public readonly TypeModifier Modifier;
        public readonly CompilationNamespace Namespace;

        public readonly List<CompilationNamespaceType> Types = new();
        public readonly List<ICompilationTypeMember> Members = new();
        
        public bool Static;
        public bool Abstract;
        public bool Sealed;

        public CompilationNamespaceType(
            string name, 
            TypeModifier modifier, 
            CompilationNamespace @namespace,
            bool isStatic,
            bool isAbstract, 
            bool isSealed)
        {
            Name = name;
            Modifier = modifier;
            Static = isStatic;
            Abstract = isAbstract;
            Sealed = isSealed;
            Namespace = @namespace;
        }
    }
}