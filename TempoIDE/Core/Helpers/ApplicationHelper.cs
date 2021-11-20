using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using CSharp_Logger;
using TempoIDE.Controls.Panels;
using TempoIDE.Windows;

namespace TempoIDE.Core.Helpers
{
    public static class ApplicationHelper
    {
        public static Encoding GlobalEncoding = Encoding.UTF8;
        public static TextDataFormat ClipboardEncoding = TextDataFormat.UnicodeText;
        
        public static Logger Logger;
        
        private static bool inspectionsEnabled = true;
        public static bool InspectionsEnabled
        {
            get => inspectionsEnabled;
            set { inspectionsEnabled = value; InspectionsEnabledChanged?.Invoke(value); }
        }
        
        public static MainWindow MainWindow
        {
            get
            {
                MainWindow window = null;

                AppDispatcher.Invoke(delegate
                {
                    window = Application.Current.MainWindow as MainWindow;
                });

                return window;
            }
        }

        public static Window ActiveWindow => Application.Current
            .Windows
            .OfType<Window>()
            .SingleOrDefault(x => x.IsActive);

        public static Dispatcher AppDispatcher => Application.Current.Dispatcher;

        public delegate void EnabledEventHandler(bool enabled);
        public static event EnabledEventHandler InspectionsEnabledChanged;

        public delegate void ErrorCodeEventHandler(ApplicationErrorCode code, string details);
        public static event ErrorCodeEventHandler ErrorCodeEmitted;
        
        public static bool EmitIfInvalidFile(string path)
        {
            if (File.Exists(path)) 
                return false;
            
            EmitErrorCode(ApplicationErrorCode.TI_INVALID_FILE, $"The file '{Path.GetFileName(path)}' does not exist.");
            
            return true;
        }
        
        
        public static bool EmitIfInvalidDirectory(string path)
        {
            if (Directory.Exists(path)) 
                return false;
            
            EmitErrorCode(ApplicationErrorCode.TI_INVALID_FILE, $"The directory '{Path.GetDirectoryName(path)}' does not exist.");
            
            return true;
        }
        
        public static void EmitErrorCode(ApplicationErrorCode code, string details)
        {
            var message =
                $"TempoIDE ran into an error: {code}\n" +
                "\n" +
                "Error details\n" +
                "---------------\n" +
                $"{details}";

            MainWindow.Notify(message, NotificationIcon.Error);
            
            ErrorCodeEmitted?.Invoke(code, details);
        }
    }
}