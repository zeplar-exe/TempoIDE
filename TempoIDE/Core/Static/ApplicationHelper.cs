using System.Text;
using TempoIDE.Windows;

namespace TempoIDE.Core.Static
{
    public static class ApplicationHelper
    {
        public static Encoding GlobalEncoding = Encoding.UTF8;
        
        private static bool inspectionsEnabled = true;
        public static bool InspectionsEnabled
        {
            get => inspectionsEnabled;
            private set
            {
                inspectionsEnabled = value;
                InspectionsEnabledChanged?.Invoke(value);
            }
        }

        public static void EnableInspections() => InspectionsEnabled = false;
        public static void DisableInspections() => InspectionsEnabled = false;

        public delegate void EnabledEventHandler(bool enabled);
        public static event EnabledEventHandler InspectionsEnabledChanged;

        public delegate void ErrorCodeEventHandler(ApplicationErrorCode code, string details);
        public static event ErrorCodeEventHandler ErrorCodeEmitted;
        
        public static void EmitErrorCode(ApplicationErrorCode code, string details)
        {
            var message =
                $"TempoIDE ran into an error: {code}" +
                "" +
                "Error details" +
                "---------------" +
                $"{details}";

            var dialog = new UserDialog(message, UserResult.Ok);
            
            dialog.ShowDialog();
            ErrorCodeEmitted?.Invoke(code, details);
        }
    }
}