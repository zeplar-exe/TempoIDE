using System.Windows;

namespace TempoIDE.UserControls.Editors
{
    public partial class SourceViewer : Editor
    {
        public override bool IsFocused { get; }
        
        public SourceViewer()
        {
            InitializeComponent();
        }
        
        private void Label_OnLoaded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}