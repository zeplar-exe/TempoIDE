using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TempoIDE.UserControls.Panels;

namespace TempoIDE.Windows.SolutionCreation
{
    public partial class CreateSolutionWindow : Window
    {
        public CreateSolutionWindow()
        {
            InitializeComponent();
        }
        
        private void CreateSolutionWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            SetDisplayFromControl(Explorer.SelectedItem as UIElement);
        }
        
        public static readonly DependencyProperty CreationControlProperty = DependencyProperty.RegisterAttached(
            "CreationControl",
            typeof(UserControl),
            typeof(ExplorerViewItem)
        );
        
        public static void SetCreationControl(UIElement element, UserControl value)
        {
            element.SetValue(CreationControlProperty, value);
        }
        public static UserControl GetCreationControl(UIElement element)
        {
            return (UserControl)element?.GetValue(CreationControlProperty);
        }

        private void Explorer_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SetDisplayFromControl(e.NewValue as UIElement);
        }

        private void SetDisplayFromControl(UIElement element)
        {
            ContentDisplay.Child = GetCreationControl(element);
        }

        public static readonly RoutedCommandExt CreateRoutedCommand = new();
        public static readonly RoutedCommandExt CancelRoutedCommand = new();

        private void ExecuteCreateCommand(object sender, ExecutedRoutedEventArgs executedRoutedEventArgs)
        {
            ((SolutionCreationPanel)ContentDisplay.Child).Create();
            
            Close();
        }

        private void CanExecuteCreateCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ((SolutionCreationPanel)ContentDisplay.Child).CanCreate();
        }
        
        private void ExecuteCancelCommand(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }
    }
}