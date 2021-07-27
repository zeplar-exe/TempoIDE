using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TempoIDE.UserControls
{
    public partial class EditorTabControl : UserControl
    {
        public EditorTabItem LastSelected { get; private set; }
        
        public ObservableCollection<FileInfo> Items { get; } = new ObservableCollection<FileInfo>(); 
        
        public EditorTabControl()
        {
            DataContext = this;
            Items.CollectionChanged += delegate { Refresh(); };
            
            InitializeComponent();
        }

        #region Dependency Props

        public ItemsPanelTemplate ItemsPanelTemplate
        {
            get => (ItemsPanelTemplate) GetValue(ItemsPanelTemplateProperty);
            set => SetValue(ItemsPanelTemplateProperty, value);
        }

        public static readonly DependencyProperty ItemsPanelTemplateProperty =
            DependencyProperty.Register(
                "ItemsPanelTemplate", typeof(ItemsPanelTemplate),
                typeof(EditorTabControl)
            );
        
        public DataTemplate ItemsTemplate
        {
            get => (DataTemplate) GetValue(ItemsTemplateProperty);
            set => SetValue(ItemsTemplateProperty, value);
        }

        public static readonly DependencyProperty ItemsTemplateProperty =
            DependencyProperty.Register(
                "ItemTemplate", typeof(DataTemplate),
                typeof(EditorTabControl)
            );
        
        #endregion

        public void Open(FileInfo file)
        {
            if (Items.All(f => f.FullName != file.FullName))
                Items.Add(file);

            var tab = ListBox.Items
                .Cast<EditorTabItem>()
                .First(t => t.BoundFile.FullName == file.FullName);

            LastSelected = tab;
            ListBox.SelectedItem = tab;
            ContentDisplay.Child = tab.Editor;
        }

        public void Close(FileInfo file)
        {
            if (file.FullName == LastSelected?.BoundFile.FullName)
                ContentDisplay.Child = null;
            
            Items.Remove(file);
        }

        public void CloseAll()
        {
            foreach (var item in Items.ToArray())
                Close(item);
        }

        public void Refresh()
        {
            ListBox.Items.Clear();

            foreach (var file in Items)
            {
                file.Refresh();
                
                if (!file.Exists)
                {
                    Items.Remove(file);
                    continue;
                }

                var tab = new EditorTabItem
                {
                    Header = { Text = file.Name },
                    Editor = Editor.FromExtension(file.Extension),
                    BoundFile = file
                };
                
                tab.Editor.Update(file);

                ListBox.Items.Add(tab);
            }
        }

        public Editor GetFocusedEditor()
        {
            var selected = GetSelectedItem().Editor;

            return selected.IsFocused ? selected : null;
        }

        public EditorTabItem GetSelectedItem()
        {
            return ListBox.SelectedItem as EditorTabItem;
        }
        
        private void ListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Open(GetSelectedItem()?.BoundFile);
            
            LastSelected = GetSelectedItem();
        }
    }
}