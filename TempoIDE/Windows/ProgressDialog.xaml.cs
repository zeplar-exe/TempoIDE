using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace TempoIDE.Windows;

public partial class ProgressDialog : Window
{
    public readonly Queue<ProgressTask> Tasks = new();
    private double ProgressIncrement => 100d / Tasks.Count;

    public event EventHandler? Completed;

    public ProgressDialog()
    {
        InitializeComponent();
    }
        
    private async void ProgressDialog_OnActivated(object sender, EventArgs e)
    {
        foreach (var task in Tasks)
        {
            v_Header.Text = task.Header;
            await Task.Run(task.Method);
                
            Dispatcher.Invoke(delegate
            {
                v_ProgressBar.Value += ProgressIncrement;
            });
        }

        DialogResult = true;
        Completed?.Invoke(this, EventArgs.Empty);
    }
}

public readonly struct ProgressTask
{
    public readonly string Header;
    public readonly Action Method;

    public ProgressTask(string header, Action method)
    {
        Header = header;
        Method = method;
    }
}