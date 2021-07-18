using System;
using System.IO;
using System.Reflection;
using System.Windows.Input;
using Microsoft.Win32;
using TempoIDE.Classes.Types;
using TempoIDE.Windows;

namespace TempoIDE.Classes
{
    public static class AppCommands
    {
        public static object FromName(string commandName)
        {
            var type = typeof(AppCommands).GetNestedType(commandName.Replace(" ", "").Replace(".", ""), BindingFlags.Public | BindingFlags.IgnoreCase);

            return type == null ? null : Activator.CreateInstance(type);
        }

        public class OpenSolution : AppCommand
        {
            public override bool CanExecute(object parameter) => true;

            public override void Execute(object parameter)
            {
                var mainWindow = (MainWindow) parameter;
                var dialog = new OpenFileDialog { Filter = "Solution files|*.sln" };

                if (dialog.ShowDialog().ToRealValue())
                {
                    mainWindow.Explorer.UpdateDirectory(new DirectoryInfo(new FileInfo(dialog.FileName).Directory.FullName));
                }
            }
        }

        public class Copy : AppCommand
        {
            public override bool CanExecute(object parameter)
            {
                var window = parameter as MainWindow;
                
                return window.Editor.TextEditor.IsFocused || window.Explorer.IsFocused;
            }

            public override void Execute(object parameter)
            {
                var window = parameter as MainWindow;
                
                window.Editor.TextEditor.TryCopyText();
            }
        }
        
        public class Paste : AppCommand
        {
            public override bool CanExecute(object parameter)
            {
                var window = parameter as MainWindow;
                
                return window.Editor.TextEditor.IsFocused || window.Explorer.IsFocused;
            }

            public override void Execute(object parameter)
            {
                var window = parameter as MainWindow;
                
                window.Editor.TextEditor.TryPasteText();
            }
        }

        public class Cut : AppCommand
        {
            public override bool CanExecute(object parameter)
            {
                var window = parameter as MainWindow;
                
                return window.Editor.TextEditor.IsFocused || window.Editor.IsFocused;
            }

            public override void Execute(object parameter)
            {
                var window = parameter as MainWindow;
                
                window.Editor.TextEditor.TryCutText();
            }
        }

        public class SelectAll : AppCommand
        {
            public override bool CanExecute(object parameter)
            {
                var window = parameter as MainWindow;
                
                return window.Editor.TextEditor.IsFocused || window.Editor.IsFocused;
            }

            public override void Execute(object parameter)
            {
                var window = parameter as MainWindow;
                
                window.Editor.TextEditor.TrySelectAll();
            }
        }
    }
}