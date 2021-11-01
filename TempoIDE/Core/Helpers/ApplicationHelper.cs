using System.Linq;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Threading;
using CSharp_Logger;
using TempoIDE.Controls.Panels;
using TempoIDE.Windows;

namespace TempoIDE.Core.Helpers
{
    public static class ApplicationHelper
    {
        private static readonly Timer TickTimer = new(3);
        private static ulong tick;

        public static Encoding GlobalEncoding = Encoding.UTF8;
        public static TextDataFormat ClipboardEncoding = TextDataFormat.UnicodeText;
        
        public static Logger Logger;
// TODO: Integrate settings helper object here
        public delegate void ApplicationTickHandler(ulong tick);
        public static event ApplicationTickHandler ApplicationTick;

        static ApplicationHelper()
        {
            TickTimer.Elapsed += delegate { ApplicationTick?.Invoke(++tick); };
            
            AwaitMainWindow();
        }

        private static async void AwaitMainWindow()
        {
            await AsyncHelper.WaitUntilNotNull(() => MainWindow);
            
            MainWindow.Activated += delegate { TickTimer.Start(); };
            MainWindow.Deactivated += delegate { TickTimer.Stop(); };
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

        public delegate void ErrorCodeEventHandler(ApplicationErrorCode code, string details);
        public static event ErrorCodeEventHandler ErrorCodeEmitted;
        
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
            LogError(message);
        }

        public static void LogError(string error)
        {
            Logger.Error(error);
        }
    }
}