namespace TempoAnalysis
{
    public interface ICompilationTypeMember
    {
        public string Name { get; set; }

        public TypeModifier Modifier { get; set; }
        public CompilationNamespaceType ParentType { get; set; }

        public bool IsStatic { get; set; }
        public bool IsAbstract { get; set; }
        public bool IsVirtual { get; set; }
        public string Type { get; set; }
    }
}