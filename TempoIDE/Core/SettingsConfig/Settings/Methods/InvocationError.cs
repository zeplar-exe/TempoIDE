namespace TempoIDE.Core.SettingsConfig.Settings.Methods
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