using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using TempoIDE.UserControls.MiniControls;

namespace TempoIDE.UserControls.InputFields
{
    public partial class StringField : InputField
    {
        public event TextChangedEventHandler TextChanged;
        
        public int TextFontSize
        {
            get => (int)GetValue(TextFontSizeProperty);
            set => SetValue(TextFontSizeProperty, value);
        }
        
        public static readonly DependencyProperty TextFontSizeProperty =
            DependencyProperty.Register(
                "TextFontSize", typeof(int),
                typeof(StringField),
                new UIPropertyMetadata(15)
            );

        public Style TextBoxStyle
        {
            get => (Style)GetValue(TextBoxStyleProperty);
            set => SetValue(TextBoxStyleProperty, value);
        }

        public static readonly DependencyProperty TextBoxStyleProperty =
            DependencyProperty.Register(
                "TextBoxStyle", typeof(Style),
                typeof(StringField),
                new UIPropertyMetadata((Style)Application.Current.Resources["TextStyle"])
            );
        
        public TextBox InputElement => Input;
        
        public StringField()
        {
            InitializeComponent();
        }

        private void Input_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            TextChanged?.Invoke(sender, e);
        }
    }
}