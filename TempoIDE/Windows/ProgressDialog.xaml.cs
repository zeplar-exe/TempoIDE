using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace TempoIDE.Windows
{
    public partial class ProgressDialog : Window
    {
        public Queue<ProgressTask> Tasks = new Queue<ProgressTask>();

        private double ProgressIncrement => 100d / Tasks.Count;

        public event EventHandler Completed; 

        public ProgressDialog()
        {
            InitializeComponent();
        }

        public async void StartAsync()
        {
            foreach (var task in Tasks)
            {
                Header.Text = task.Header;
                await Task.Run(task.Method);
                
                Dispatcher.Invoke(delegate
                {
                    ProgressBar.Value += ProgressIncrement;
                });
            }
            
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
}