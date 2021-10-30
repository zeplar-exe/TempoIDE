namespace TempoIDE.Core.SettingsConfig
{
    public readonly struct InvocationError
    {
        public readonly string Message;

        public InvocationError(string message)
        {
            Message = message;
        }
    }
}