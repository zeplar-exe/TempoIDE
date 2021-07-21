using System;
using System.IO;
using System.Reflection;
using Microsoft.Win32;
using TempoIDE.Classes.Types;
using TempoIDE.Windows;
using Ookii.Dialogs.Wpf;

namespace TempoIDE.Classes
{
    public static class AppCommands
    {
        public static object FromName(string commandName)
        {
            if (string.IsNullOrWhiteSpace(commandName))
                return null;
            
            var type = typeof(AppCommands).GetNestedType(commandName.Replace(" ", "").Replace(".", ""), BindingFlags.Public | BindingFlags.IgnoreCase);

            return type == null ? null : Activator.CreateInstance(type);
        }

        public class OpenDropdown : AppCommand
        {
            public override bool CanExecute(object parameter) => true;

            public override void Execute(object parameter) { }
        }

        public class OpenFileCommand : AppCommand
        {
            public override bool CanExecute(object parameter) => true;

            public override void Execute(object parameter)
            {
                var dialog = new OpenFileDialog();

                if (dialog.ShowDialog().ToRealValue())
                {
                    EnvironmentHelper.LoadEnvironment(dialog.FileName, EnvironmentFilterMode.File);   
                }
            }
        }

        public class OpenFolderCommand : AppCommand
        {
            public override bool CanExecute(object parameter) => true;

            public override void Execute(object parameter)
            {
                var dialog = new VistaFolderBrowserDialog();
                
                if (dialog.ShowDialog().ToRealValue())
                {
                    EnvironmentHelper.LoadEnvironment(dialog.SelectedPath, EnvironmentFilterMode.Directory);   
                }
            }
        }

        public class CopyTextCommand : AppCommand
        {
            public override bool CanExecute(object parameter)
            {
                var window = EnvironmentHelper.MainWindow;
                
                return window.Editor.TextEditor.IsFocused || window.Explorer.IsFocused;
            }

            public override void Execute(object parameter)
            {
                var window = EnvironmentHelper.MainWindow;
                
                window.Editor.TextEditor.TryCopyText();
            }
        }
        
        public class PasteTextCommand : AppCommand
        {
            public override bool CanExecute(object parameter)
            {
                var window = EnvironmentHelper.MainWindow;
                
                return window.Editor.TextEditor.IsFocused || window.Explorer.IsFocused;
            }

            public override void Execute(object parameter)
            {
                var window = EnvironmentHelper.MainWindow;
                
                window.Editor.TextEditor.TryPasteText();
            }
        }

        public class CutTextCommand : AppCommand
        {
            public override bool CanExecute(object parameter)
            {
                var window = EnvironmentHelper.MainWindow;
                
                return window.Editor.TextEditor.IsFocused || window.Editor.IsFocused;
            }

            public override void Execute(object parameter)
            {
                var window = EnvironmentHelper.MainWindow;
                
                window.Editor.TextEditor.TryCutText();
            }
        }

        public class SelectAllTextCommand : AppCommand
        {
            public override bool CanExecute(object parameter)
            {
                var window = EnvironmentHelper.MainWindow;
                
                return window.Editor.TextEditor.IsFocused || window.Editor.IsFocused;
            }

            public override void Execute(object parameter)
            {
                var window = EnvironmentHelper.MainWindow;
                
                window.Editor.TextEditor.TrySelectAll();
            }
        }
    }
}