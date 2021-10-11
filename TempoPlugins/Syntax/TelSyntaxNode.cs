using System.Collections.Generic;
using System.Linq;
using TempoPlugins.Syntax.Nodes;

namespace TempoPlugins.Syntax
{
    public abstract class TelSyntaxNode
    {
        internal readonly List<TelSyntaxNode> nodes = new();
        internal readonly List<ParserError> errors = new();
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

                foreach (var nestedNode in node.Descendents())
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