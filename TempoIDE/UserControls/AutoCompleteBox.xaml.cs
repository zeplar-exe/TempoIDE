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

        private AutoCompletionSelection autoCompletions;
        private AutoCompletionSelection AutoCompletions
        {
            get => autoCompletions;
            set
            {
                Items.Clear();
                
                foreach (var completion in value.Choices)
                {
                    Items.Add(new ListBoxItem { Content = completion.Value });
                }
                
                autoCompletions = value;
            }
        }

        public AutoCompletion Selected => AutoCompletions.Selected;

        public AutoCompleteBox()
        {
            InitializeComponent();
        }

        public void Enable()
        {
            Enabled = true;
            Visibility = Visibility.Visible;
        }
        
        public void Disable()
        {
            Enabled = false;
            Visibility = Visibility.Collapsed;

            if (AutoCompletions != null)
                AutoCompletions.Index = 0;
        }

        public void Update(AutoCompletionSelection completions)
        {
            AutoCompletions = completions;
        }

        public void MoveSelectedIndex(LogicalDirection direction)
        {
            var newIndex = SelectedIndex;

            if (direction == LogicalDirection.Forward)
            {
                newIndex++;
                
                if (AutoCompletions != null)
                    AutoCompletions.Index++;
            }
            else
            {
                newIndex--;
                if (AutoCompletions != null)
                    AutoCompletions.Index--;
            }

            SelectedIndex = Math.Clamp(newIndex, 0, Items.Count);
        }
    }
}