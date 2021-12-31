using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TempoIDE.Controls.Panels;
using TempoIDE.Core.Commands;

namespace TempoIDE.Windows.SolutionCreation;

public partial class CreateSolutionWindow : ModifiedWindow
{
    public CreateSolutionWindow()
    {
        InitializeComponent();
    }

    public UserControl? SelectedControl
    {
        get => (UserControl)GetValue(SelectedControlProperty);
        set => SetValue(SelectedControlProperty, value);
    }

    public static readonly DependencyProperty SelectedControlProperty =
        DependencyProperty.Register(
            "SelectedControl", typeof(UserControl),
            typeof(CreateSolutionWindow)
        );
        
    public static readonly DependencyProperty CreationControlProperty = DependencyProperty.RegisterAttached(
        "CreationControl",
        typeof(UserControl),
        typeof(ExplorerViewItem)
    );
        
    public static void SetCreationControl(UIElement element, UserControl value)
    {
        element.SetValue(CreationControlProperty, value);
    }
    public static UserControl? GetCreationControl(UIElement? element)
    {
        return (UserControl?)element?.GetValue(CreationControlProperty);
    }

    private void Explorer_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
        SelectedControl = GetCreationControl(e.NewValue as UIElement);
    }

    public static readonly RoutedCommandExt CreateRoutedCommand = new();
    public static readonly RoutedCommandExt CancelRoutedCommand = new();

    private void ExecuteCreateCommand(object sender, ExecutedRoutedEventArgs e)
    {
        (SelectedControl as SolutionCreationPanel)?.Create();
            
        Close();
    }

    private void CanExecuteCreateCommand(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = (SelectedControl as SolutionCreationPanel)?.CanCreate() ?? false;
    }
        
    private void ExecuteCancelCommand(object sender, ExecutedRoutedEventArgs e)
    {
        Close();
    }

    private void CreateSolutionWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
        SelectedControl = GetCreationControl(v_Explorer?.SelectedItem as UIElement);
    }
}