using System;
using System.Windows.Input;

namespace TempoIDE.Core.Commands;

public class RoutedCommandExt : RoutedCommand, ICommand
{
    private event EventHandler? CanExecuteChangedEv;

    public void RaiseCanExecuteChanged()
    {
        CanExecuteChangedEv?.Invoke(this, EventArgs.Empty);
    }

    public new event EventHandler CanExecuteChanged
    {
        add
        {
            CanExecuteChangedEv += value;
            base.CanExecuteChanged += value;
        }
        remove
        {
            CanExecuteChangedEv -= value;
            base.CanExecuteChanged -= value;
        }
    }
}