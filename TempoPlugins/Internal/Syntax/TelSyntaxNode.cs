using System.Collections.Generic;
using System.Linq;
using Jammo.ParserTools;
using TempoPlugins.Internal.Syntax.Nodes;

namespace TempoPlugins.Internal.Syntax
{
    public abstract class TelSyntaxNode
    {
        private readonly List<TelSyntaxNode> nodes = new();
        private readonly List<ParserError> errors = new();
        
        public IEnumerable<TelSyntaxNode> Nodes => nodes;
        public IEnumerable<ParserError> Errors => errors;

        public bool HasError => errors.Any();

        public void AddNode(TelSyntaxNode node)
        {
            nodes.Add(node);
        }
        
        public IEnumerable<TelSyntaxNode> Descendents()
        {
            foreach (var node in Nodes)
            {
                yield return node;

                foreach (var nestedNode in node.Nodes)
                    yield return nestedNode;
            }
        }

        protected void ReportError(string message, StringContext context)
        {
            ReportError(new ParserError(message, context));
        }

        protected void ReportError(ParserError error)
        {
            errors.Add(error);
        }
    }
}