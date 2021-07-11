using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TempoIDE.Classes.Types
{
    public class CaretContext
    {
        public readonly CaretContextType ContextType;
        public readonly CaretContext Parent;
        public List<CaretContext> Children = new List<CaretContext>();

        public CaretContext(CaretContextType contextType, CaretContext parent)
        {
            ContextType = contextType;
            Parent = parent;
        }

        public static CaretContext FromSyntaxTree(CompilationUnitSyntax syntaxTree)
        {
            var rootContext = new CaretContext(CaretContextType.Document, null);
            
                    
    
            return rootContext;
        }
    }

    public enum CaretContextType
    {
        Document,
        Namespace,
        ForLoop,
        WhileLoop,
        IfStatement,
        Method,
        Class,
        Interface,
        Struct
    }
}