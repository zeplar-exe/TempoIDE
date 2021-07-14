using System;
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
                typeof(TopbarControl)
            );

        public List<TopbarMenuItem> Items
        {
            get => (List<TopbarMenuItem>) GetValue(ItemsProperty);
            set => SetValue(ItemsProperty, value);
        }

        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register(
                "Items", typeof(List<TopbarMenuItem>),
                typeof(TopbarMenu)
            );

        public TopbarMenu()
        {
            InitializeComponent();
            Items = new List<TopbarMenuItem>();
        }

        private void TopbarMenu_OnLoaded(object sender, RoutedEventArgs e)
        {
            MenuNameTextBlock.Text = MenuName;

            foreach (var item in Items)
                ItemsPanel.Children.Add(item);
        }

        private void TopbarMenu_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // TODO: Clean this up
            
            if (OpenMenu != null)
            {
                if (OpenMenu == this)
                {
                    ItemsPanel.Visibility = Visibility.Collapsed;
                    Background = Brushes.Transparent;
                    
                    OpenMenu = null;
                    
                    return;
                }
                
                OpenMenu.ItemsPanel.Visibility = Visibility.Collapsed;
                OpenMenu.Background = Brushes.Transparent;
            }
            
            OpenMenu = this;

            OpenMenu.Background = Brushes.Aquamarine;
            ItemsPanel.Visibility = Visibility.Visible;
        }
    }
}