using TempoIDE.Classes.Types;
using TempoIDE.Windows;

namespace TempoIDE.Classes
{
    public static class AppCommands
    {
        public static void FromAppCommand(AppCommand command, MainWindow mainWindow)
        {
            typeof(AppCommands).GetMethod(command.Name.Replace(" ", string.Empty))?.Invoke(null, new object[] { mainWindow });
        }
        
        public static void Copy(MainWindow mainWindow)
        {
            mainWindow.Editor.TextEditor.TryCopyText();
        }

        public static void Paste(MainWindow mainWindow)
        {
            mainWindow.Editor.TextEditor.TryPasteText();
        }

        public static void Cut(MainWindow mainWindow)
        {
            mainWindow.Editor.TextEditor.TryCutText();
        }

        public static void SelectAll(MainWindow mainWindow)
        {
            mainWindow.Editor.TextEditor.TrySelectAll();
        }
    }
}