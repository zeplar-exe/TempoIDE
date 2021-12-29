using System;
using System.Windows.Input;

namespace TempoIDE.Core.Commands;

public abstract class AppCommand : ICommand
{
    public abstract bool CanExecute(object parameter);

    public abstract void Execute(object parameter);

    public event EventHandler CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }
}