using System.Collections.Generic;
using System.Linq;
using TempoPlugins.Syntax.Nodes;

namespace TempoPlugins.Syntax
{
    public abstract class TelSyntaxNode
    {
        private readonly List<TelSyntaxNode> nodes = new();
        private readonly List<ParserError> errors = new();
        
        public IEnumerable<TelSyntaxNode> Nodes => nodes.AsReadOnly();
        public IEnumerable<ParserError> Errors => errors.AsReadOnly();

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

        protected void ReportError(string message)
        {
            ReportError(new ParserError(message));
        }

        protected void ReportError(ParserError error)
        {
            errors.Add(error);
        }
    }
}