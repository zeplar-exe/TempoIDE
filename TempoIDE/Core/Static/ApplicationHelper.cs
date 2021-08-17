using System.Text;
using TempoIDE.Windows;

namespace TempoIDE.Core.Static
{
    public static class ApplicationHelper
    {
        public static void ThrowErrorCode(ApplicationErrorCode code, string details)
        {
            var message = new StringBuilder();
            
            message.AppendLine($"TempoIDE ran into an error: {code}");
            message.AppendLine();
            message.AppendLine("Error details");
            message.AppendLine(details);
            
            var dialog = new UserDialog(message.ToString(), UserResult.Ok);

            dialog.ShowDialog();
        }
    }
}