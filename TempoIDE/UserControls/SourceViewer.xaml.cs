using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace TempoIDE.UserControls
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