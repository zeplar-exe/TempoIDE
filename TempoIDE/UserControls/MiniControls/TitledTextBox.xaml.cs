using System.Windows;
using System.Windows.Controls;

namespace TempoIDE.UserControls.MiniControls
{
    //[DependencyProperty("Header", typeof(string), typeof(TitledTextBox))]
    public partial class TitledTextBox : UserControl
    {
        public string Header
        {
            get => (string)GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register(
                "Header", typeof(string),
                typeof(TitledTextBox)
            );
        
        public TitledTextBox()
        {
            DataContext = this;
            
            InitializeComponent();
        }
    }
}