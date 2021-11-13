using System;
using System.Collections.Generic;

namespace TempoIDE.Core.SettingsConfig.Settings.Methods
{
    public class MethodSetting : SettingValue, IInvokable
    {
        private readonly List<MethodOperation> operations = new();
        public IEnumerable<MethodOperation> Operations => operations.AsReadOnly();
    
        public MethodSetting()
        {
            
        }

        public InvokeResult Invoke()
        {
            throw new NotImplementedException();
        }

        public void AddOperation(MethodOperation operation)
        {
            operations.Add(operation);
        }

        public bool RemoveOperation(MethodOperation operation)
        {
            return operations.Remove(operation);
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }
    }

    public class MethodOperation : IInvokable
    {
        public InvokeResult Invoke()
        {
            throw new NotImplementedException();
        }
    }
}