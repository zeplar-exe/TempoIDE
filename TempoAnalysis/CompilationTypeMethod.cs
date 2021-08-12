namespace TempoAnalysis
{
    public class CompilationTypeMethod : ICompilationTypeMember
    {
        public string Name { get; set; }
        
        public TypeModifier Modifier { get; set; }
        public CompilationNamespaceType ParentType { get; set; }

        public bool IsStatic { get; set; }
        public bool IsAbstract { get; set; }
        public bool IsVirtual { get; set; }
        public string Type { get; set; }
        
        public CompilationTypeMethod(
            string name,
            TypeModifier modifier,
            CompilationNamespaceType type, 
            bool isStatic,
            bool isAbstract,
            bool isVirtual,
            string returnType)
        {
            Name = name;
            Modifier = modifier;
            ParentType = type;
            IsStatic = isStatic;
            IsAbstract = isAbstract;
            IsVirtual = isVirtual;
            Type = returnType;
        }
    }
}