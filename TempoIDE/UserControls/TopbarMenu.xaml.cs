using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace TempoIDE.UserControls
{
    public partial class TopbarMenu : UserControl
    {
        public static TopbarMenu OpenMenu;
        
        public string MenuName
        {
            get => (string)GetValue(MenuNameProperty);
            set => SetValue(MenuNameProperty, value); 
        }
            
        public static readonly DependencyProperty MenuNameProperty =
            DependencyProperty.Register(
                "MenuName", typeof(string),
                typeof(TopbarMenu)
            );

        public List<UIElement> Items
        {
            get => (List<UIElement>) GetValue(ItemsProperty);
            set => SetValue(ItemsProperty, value);
        }

        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register(
                "Items", typeof(List<UIElement>),
                typeof(TopbarMenu)
            );

        public TopbarMenu()
        {
            InitializeComponent();
            Items = new List<UIElement>();
        }

        private void TopbarMenu_OnLoaded(object sender, RoutedEventArgs e)
        {
            MenuNameTextBlock.Text = MenuName;
            Menu.PlacementTarget = MenuNameTextBlock;

            foreach (var item in Items)
                Menu.Items.Add(item);
            
            Menu.Closed += OnMenuClosed;
        }

        private void TopbarMenu_OnPreviewMouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            // TODO: Clean this up
            
            if (OpenMenu != null)
            {
                if (OpenMenu == this)
                {
                    Menu.IsOpen = false;
                    Background = Brushes.Transparent;
                    
                    OpenMenu = null;
                    
                    return;
                }
                
                OpenMenu.Menu.IsOpen = false;
                OpenMenu.Background = Brushes.Transparent;
            }
            
            OpenMenu = this;

            OpenMenu.Background = Resources["TopbarMenuSelectedColor"] as SolidColorBrush;
            Menu.IsOpen = true;
        }

        private void OnMenuClosed(object sender, RoutedEventArgs e)
        {
            Background = Brushes.Transparent;
                    
            OpenMenu = null; // TODO: Placement is broken
        }
    }
}