using System.Windows;
using System.Windows.Controls;

namespace TempoIDE.Controls.InputFields;

public abstract class InputField : UserControl
{
    public string FieldTitle
    {
        get => (string)GetValue(FieldTitleProperty);
        set => SetValue(FieldTitleProperty, value);
    }

    public static readonly DependencyProperty FieldTitleProperty =
        DependencyProperty.Register(
            "FieldTitle", typeof(string),
            typeof(InputField)
        );
        
    public string Value
    {
        get => (string)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public static readonly DependencyProperty ValueProperty =
        DependencyProperty.Register(
            "Value", typeof(string),
            typeof(InputField)
        );
}