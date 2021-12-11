using System.ComponentModel;
using System.Runtime.CompilerServices;
using TempoIDE.Properties;

namespace TempoIDE.Controls.Explorer
{
    public partial class TitledExplorerItem : ExplorerItem, INotifyPropertyChanged
    {
        private string headerText;
        
        public string HeaderText
        {
            get => headerText;
            set
            {
                if (headerText == value)
                    return;
                
                headerText = value;
                
                OnPropertyChanged();
            } 
        }
        
        public TitledExplorerItem()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}