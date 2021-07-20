using System;
using System.IO;
using System.Reflection;
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

        public class OpenFile : AppCommand
        {
            public override bool CanExecute(object parameter) => true;

            public override void Execute(object parameter)
            {
                var dialog = new OpenFileDialog();

                if (dialog.ShowDialog().ToRealValue())
                {
                    EnvironmentManager.LoadEnvironment(dialog.FileName, EnvironmentFilterMode.Solution);   
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