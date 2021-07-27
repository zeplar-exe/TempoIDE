using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using TempoIDE.Classes.Types;

namespace TempoIDE.UserControls
{
    public partial class AutoCompleteBox : ListBox
    {
        public bool Enabled { get; private set; }
        
        public AutoCompleteBox()
        {
            InitializeComponent();
        }

        public void Enable()
        {
            Enabled = true;
            Visibility = Visibility.Visible;
        }

        public void Update(AutoCompletion[] completions)
        {
            Items.Clear();
            
            foreach (var completion in completions)
            {
                Items.Add(new ListBoxItem { Content = completion.Value });
            }
        }

        public void Disable()
        {
            Enabled = false;
            Visibility = Visibility.Collapsed;
        }

        public void MoveSelectedIndex(LogicalDirection direction)
        {
            int newIndex = SelectedIndex;

            if (direction == LogicalDirection.Forward)
                newIndex++;
            else
                newIndex--;

            SelectedIndex = Math.Clamp(newIndex, 0, Items.Count);
        }
    }
}