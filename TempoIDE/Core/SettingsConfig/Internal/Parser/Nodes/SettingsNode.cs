using System.Collections.Generic;
using Jammo.ParserTools;

namespace TempoIDE.Core.SettingsConfig.Internal.Parser.Nodes
{
    public abstract class SettingsNode
    {
        private readonly List<ParserError> errors = new();

        public abstract IEnumerable<SettingsNode> Nodes { get; }
        public IEnumerable<ParserError> Errors => errors.AsReadOnly();

        public StringContext Context;

        protected SettingsNode(StringContext context)
        {
            Context = context;
        }

        public IEnumerable<SettingsNode> Descendents()
        {
            foreach (var child in Nodes)
            {
                yield return child;

                foreach (var descendent in child.Descendents())
                    yield return descendent;
            }
        }

        public void ReportError(string message)
        {
            errors.Add(new ParserError(message, Context));
        }
    }
}