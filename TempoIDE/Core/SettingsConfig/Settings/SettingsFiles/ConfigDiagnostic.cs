using Jammo.ParserTools;

namespace TempoIDE.Core.SettingsConfig.Settings.SettingsFiles
{
    public readonly struct ConfigDiagnostic
    {
        public readonly string Message;
        public readonly DiagnosticSeverity Severity;
        public readonly StringContext Context;

        public ConfigDiagnostic(string message, DiagnosticSeverity severity, StringContext context)
        {
            Message = message;
            Context = context;
            Severity = severity;
        }
    }

    public enum DiagnosticSeverity
    {
        Warning,
        Error
    }
}