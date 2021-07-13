using System.Windows;
using System.Windows.Controls;
using TempoIDE.Classes.ColorSchemes;

namespace TempoIDE.UserControls
{
    public partial class TopbarMenuItem : Button
    {
        public string Description
        {
            get => (string)GetValue(DescriptionProperty);
            set => SetValue(DescriptionProperty, value); 
        }
            
        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register(
                "Description", typeof(string),
                typeof(TopbarMenuItem)
            );

        public TopbarMenuItem()
        {
            InitializeComponent();
        }

        private void TopbarMenuItem_OnLoaded(object sender, RoutedEventArgs e)
        {
            DescriptionTextBlock.Text = Description;
        }
    }
}