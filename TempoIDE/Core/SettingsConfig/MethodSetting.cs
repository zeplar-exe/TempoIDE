using System.Collections.Generic;
using TempoIDE.Core.SettingsConfig.Internal.Parser;
using TempoIDE.Core.SettingsConfig.Internal.Parser.Nodes;

namespace TempoIDE.Core.SettingsConfig
{
    public abstract class MethodSetting : SettingValue
    {
        private readonly List<MethodOperation> operations = new();
        public IEnumerable<MethodOperation> Operations => operations;

        public readonly string Name;
    
        public MethodSetting(string name)
        {
            Name = name;
        }

        public void Invoke()
        {
            
        }

        public void AddOperation(MethodOperation operation)
        {
            operations.Add(operation);
        }

        public bool RemoveOperation(MethodOperation operation)
        {
            return operations.Remove(operation);
        }
    }

    public class MethodOperation
    {
        
    }
}