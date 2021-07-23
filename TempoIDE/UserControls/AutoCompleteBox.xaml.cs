using System;
using System.Windows.Controls;
using System.Windows.Documents;
using TempoIDE.Classes.Types;

namespace TempoIDE.UserControls
{
    public partial class AutoCompleteBox : ListBox
    {
        public AutoCompleteBox()
        {
            InitializeComponent();
        }

        public void SupplyAutoCompletions(AutoCompletion[] completions)
        {
            Items.Clear();

            foreach (var completion in completions)
            {
                Items.Add(new ListBoxItem {Content = completion.Value});
            }
        }
    }
}