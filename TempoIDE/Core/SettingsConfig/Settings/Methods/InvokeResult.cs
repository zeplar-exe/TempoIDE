using System.Collections.Generic;

namespace TempoIDE.Core.SettingsConfig.Settings.Methods
{
    public readonly struct InvokeResult
    {
        public readonly bool Success;
        public readonly IEnumerable<InvocationError> Errors;

        public InvokeResult(bool success, IEnumerable<InvocationError> errors)
        {
            Success = success;
            Errors = errors;
        }
    }
}