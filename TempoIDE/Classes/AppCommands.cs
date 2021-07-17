using System;
using System.Windows.Input;
using TempoIDE.Classes.Types;
using TempoIDE.Windows;

namespace TempoIDE.Classes
{
    public static class AppCommands
    {
        public static object FromName(string commandName)
        {
            var type = typeof(AppCommands).GetNestedType(commandName.Replace(" ", ""));

            return type == null ? null : Activator.CreateInstance(type);
        }

        public class Copy : ICommand
        {
            public bool CanExecute(object parameter)
            {
                var window = parameter as MainWindow;
                
                return window.Editor.TextEditor.IsFocused || window.Explorer.IsFocused;
            }

            public void Execute(object parameter)
            {
                var window = parameter as MainWindow;
                
                window.Editor.TextEditor.TryCopyText();
            }

            public event EventHandler CanExecuteChanged
            {
                add => CommandManager.RequerySuggested += value;
                remove => CommandManager.RequerySuggested -= value;
            }
        }
        
        public class Paste : ICommand
        {
            public bool CanExecute(object parameter)
            {
                var window = parameter as MainWindow;
                
                return window.Editor.TextEditor.IsFocused || window.Explorer.IsFocused;
            }

            public void Execute(object parameter)
            {
                var window = parameter as MainWindow;
                
                window.Editor.TextEditor.TryPasteText();
            }

            public event EventHandler CanExecuteChanged
            {
                add => CommandManager.RequerySuggested += value;
                remove => CommandManager.RequerySuggested -= value;
            }
        }

        public class Cut : ICommand
        {
            public bool CanExecute(object parameter)
            {
                var window = parameter as MainWindow;
                
                return window.Editor.TextEditor.IsFocused || window.Editor.IsFocused;
            }

            public void Execute(object parameter)
            {
                var window = parameter as MainWindow;
                
                window.Editor.TextEditor.TryCutText();
            }

            public event EventHandler CanExecuteChanged
            {
                add => CommandManager.RequerySuggested += value;
                remove => CommandManager.RequerySuggested -= value;
            }
        }

        public class SelectAll : ICommand
        {
            public bool CanExecute(object parameter)
            {
                var window = parameter as MainWindow;
                
                return window.Editor.TextEditor.IsFocused || window.Editor.IsFocused;
            }

            public void Execute(object parameter)
            {
                var window = parameter as MainWindow;
                
                window.Editor.TextEditor.TrySelectAll();
            }

            public event EventHandler CanExecuteChanged
            {
                add => CommandManager.RequerySuggested += value;
                remove => CommandManager.RequerySuggested -= value;
            }
        }
    }
}