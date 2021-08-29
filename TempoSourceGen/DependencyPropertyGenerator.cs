using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace TempoSourceGen
{
    [Generator]
    public class DependencyPropertyGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            // Nothing to do here
        }
        
        public void Execute(GeneratorExecutionContext context)
        {
            // TODO: Not called
            var attributeType = typeof(DependencyPropertyAttribute);
            var visitor = new AttributedTypesVisitor(attributeType);
            visitor.VisitNamespace(context.Compilation.GlobalNamespace);

            foreach (var type in visitor.TypeSymbols)
            {
                var attributeIndex = 0;
                
                foreach (var attribute in type
                    .GetAttributes()
                    .Where(a => a.AttributeClass?.Name == attributeType.Name))
                {
                    context.AddSource(
                        $"{type.Name}_{attributeIndex++}",
                        SourceText.From(GenerateDependencyProperty(
                            new DependencyPropertyAttribute(
                                attribute.ConstructorArguments[0].Value as string,
                                attribute.ConstructorArguments[1].Value as Type,
                                attribute.ConstructorArguments[2].Value as Type)), Encoding.UTF8));
                }
            }
        }

        private string GenerateDependencyProperty(DependencyPropertyAttribute attribute)
        {
            return $"public partial class {attribute.OwnerType.Name}" +
                   "{" +
                   $"   public {attribute.Type.Name} {attribute.Name}" +
                   @"   {" +
                   $"       get => ({attribute.Type.Name})GetValue({attribute.Name}Property);" +
                   $"       set => SetValue({attribute.Name}Property, value);" +
                   @"   }" +
                   $"   public static readonly DependencyProperty {attribute.Name}Property =" +
                   $"      DependencyProperty.Register(" +
                   $"          \"{attribute.Name}\", typeof({attribute.Type.Name}), " +
                   $"          typeof({attribute.OwnerType.Name}));" +
                   "}";
        }

        public class AttributedTypesVisitor : SymbolVisitor
        {
            private List<INamedTypeSymbol> typeSymbols = new();
            public INamedTypeSymbol[] TypeSymbols => typeSymbols.ToArray();

            public readonly Type AttributeType;

            public AttributedTypesVisitor(Type attributeType)
            {
                AttributeType = attributeType;
            }

            public override void VisitNamespace(INamespaceSymbol symbol)
            {
                typeSymbols.Clear();

                Parallel.ForEach(symbol.GetTypeMembers(), s => s.Accept(this));
            }

            public override void VisitNamedType(INamedTypeSymbol symbol)
            {
                if (symbol.GetAttributes().Any(s => s.AttributeClass?.Name == AttributeType.Name))
                    typeSymbols.Add(symbol);
            }
        }
    }
}