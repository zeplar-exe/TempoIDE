using System.Windows;
using TempoIDE.Windows;

namespace TempoIDE.Plugins;

public abstract class WindowExtender : IExtender
{
    private readonly Window window;

    protected WindowExtender(Window window)
    {
        this.window = window;
    }
}

public class MainWindowExtender : WindowExtender
{
    internal MainWindowExtender(MainWindow window) : base(window)
    {
            
    }
}