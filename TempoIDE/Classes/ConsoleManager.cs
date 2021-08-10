using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;

namespace TempoIDE.Classes
{
    [SuppressUnmanagedCodeSecurity]
    public static class ConsoleManager
    {
        private const string Kernel32_DllName = "kernel32.dll";

        [DllImport(Kernel32_DllName)]
        private static extern bool AllocConsole();

        [DllImport(Kernel32_DllName)]
        private static extern bool FreeConsole();

        [DllImport(Kernel32_DllName)]
        private static extern IntPtr GetConsoleWindow();

        [DllImport(Kernel32_DllName)]
        private static extern int GetConsoleOutputCP();

        public static bool HasConsole => GetConsoleWindow() != IntPtr.Zero;

        public static ConsoleCommandErrorCode RunCommand(string exe, string directory, string args, int timeout = 10000)
        {
            var startInfo = new ProcessStartInfo
            {
                UseShellExecute = false,
                CreateNoWindow = false,
                FileName = exe,
                WorkingDirectory = directory,
                Arguments = args,
            };

            var process = Process.Start(startInfo);
            
            if (process == null)
                return ConsoleCommandErrorCode.InternalError;
            
            process.WaitForExit(timeout);

            try
            {
                return (ConsoleCommandErrorCode)process.ExitCode;
            }
            catch (InvalidOperationException)
            {
                return ConsoleCommandErrorCode.InternalError;
            }
        }

        /// <summary>
        /// Creates a new console instance if the process is not attached to a console already.
        /// </summary>
        public static void Show()
        {
            //#if DEBUG
            if (!HasConsole)
            {
                AllocConsole();
                //InvalidateOutAndError();
            }
            //#endif
        }

        /// <summary>
        /// If the process has a console attached to it, it will be detached and no longer visible. Writing to the System.Console is still possible, but no output will be shown.
        /// </summary>
        public static void Hide()
        {
            //#if DEBUG
            if (HasConsole)
            {
                SetOutAndErrorNull();
                FreeConsole();
            }
            //#endif
        }

        public static void Toggle()
        {
            if (HasConsole)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }

        static void InvalidateOutAndError()
        {
            var type = typeof(Console);

            var _out = type.GetField("_out",
                System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

            var _error = type.GetField("_error",
                System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

            var _InitializeStdOutError = type.GetMethod("InitializeStdOutError",
                System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

            Debug.Assert(_out != null);
            Debug.Assert(_error != null);

            Debug.Assert(_InitializeStdOutError != null);

            _out.SetValue(null, null);
            _error.SetValue(null, null);

            _InitializeStdOutError.Invoke(null, new object[] { true });
        }

        static void SetOutAndErrorNull()
        {
            Console.SetOut(TextWriter.Null);
            Console.SetError(TextWriter.Null);
        }
    }
    
    public enum ConsoleCommandErrorCode
    {
        Success = 0,
        InternalError = 256
    }
}  