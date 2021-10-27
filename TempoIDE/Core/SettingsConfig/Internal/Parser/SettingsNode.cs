using System.Collections.Generic;

namespace TempoIDE.Core.SettingsConfig.Internal.Parser
{
    public abstract class SettingsNode
    {
        private readonly List<ParserError> errors = new();

        public abstract IEnumerable<SettingsNode> Nodes { get; }
        public IEnumerable<ParserError> Errors => errors;

        public IEnumerable<SettingsNode> Descendents()
        {
            foreach (var child in Nodes)
            {
                yield return child;

                foreach (var descendent in child.Descendents())
                    yield return descendent;
            }
        }
    }
}