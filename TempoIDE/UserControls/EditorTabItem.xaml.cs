using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TempoControls.Core.Static;
using TempoIDE.Core.Static;

namespace TempoIDE.UserControls
{
    public partial class EditorTabItem : UserControl
    {
        public Editor Editor;

        private FileInfo boundFile;
        public FileInfo BoundFile
        {
            get => boundFile;
            set
            {
                boundFile = value;
                Icon.Source = IconCache.ImageFromExtension(BoundFile?.Extension);
            }
        }

        public bool IsSelected;

        public event EventHandler Selected;
        public event EventHandler Closed;
        
        public EditorTabItem()
        {
            InitializeComponent();
        }

        private void EditorTabItem_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            IsSelected = true;
            Selected?.Invoke(this, new EventArgs());
        }

        private void Close_OnClick(object sender, RoutedEventArgs e)
        {
            Closed?.Invoke(this, new EventArgs());
        }
    }
}