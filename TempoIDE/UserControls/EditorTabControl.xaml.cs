using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;

namespace TempoIDE.UserControls
{
    public partial class EditorTabControl : UserControl
    {
        public EditorTabItem SelectedItem { get; private set; }
        
        public ObservableCollection<string> Files { get; } = new(); 
        
        public EditorTabControl()
        {
            DataContext = this;
            Files.CollectionChanged += delegate { Refresh(); };
            
            InitializeComponent();
        }

        public Brush SelectedTabColor { get; set; }
        public Brush HoveredTabColor { get; set; }
        public Brush UnselectedTabColor { get; set; }

        public void Open(FileInfo file)
        {
            if (!file.Exists)
                return;
            
            if (!Files.Contains(file.FullName))
                Files.Add(file.FullName);

            var tab = TabsPanel.Children
                .OfType<EditorTabItem>()
                .First(t => t.BoundFile.FullName == file.FullName);
            
            tab.Background = SelectedTabColor;
            tab.IsSelected = true;

            if (SelectedItem != null)
            {
                SelectedItem.IsSelected = false;
                SelectedItem.Background = UnselectedTabColor;
            }

            SelectedItem = tab;
            ContentDisplay.Child = tab.Editor;
        }

        public void Close(FileInfo file)
        {
            var closingSelected = file.FullName == SelectedItem?.BoundFile.FullName;
            
            var index = Files.IndexOf(file.FullName);
            var nextIndex = index + 1;
            var lastIndex = index - 1;
            
            var tab = TabsPanel.Children
                .OfType<EditorTabItem>()
                .First(t => t.BoundFile.FullName == file.FullName);
            
            tab.Editor.UpdateFile();
            
            Files.Remove(file.FullName);
            
            if (closingSelected)
            {
                SelectedItem = null;

                if (index == 0)
                {
                    if (nextIndex < Files.Count)
                        Open(new FileInfo(Files[nextIndex]));
                    else
                        ContentDisplay.Child = null;
                }
                else
                {
                    Open(new FileInfo(Files[lastIndex]));
                }
            }
        }

        public void CloseAll()
        {
            foreach (var item in Files.ToArray())
                Close(new FileInfo(item));
        }

        public void Refresh()
        {
            var selectedFile = SelectedItem?.BoundFile;
            
            TabsPanel.Children.Clear();

            foreach (var path in Files.ToArray())
            {
                var fileInfo = new FileInfo(path);

                if (!fileInfo.Exists)
                {
                    Files.Remove(path);
                    continue;
                }

                var tab = new EditorTabItem
                {
                    Header = { Text = fileInfo.Name },
                    Editor = Editor.FromExtension(fileInfo.Extension),
                    BoundFile = fileInfo,
                    Background = UnselectedTabColor
                };
                
                tab.Editor.Update(fileInfo);
                
                tab.Selected += OnTabSelected;
                tab.MouseMove += OnTabMouseMove;
                tab.MouseLeave += OnTabMouseLeave;
                tab.Closed += OnTabClosed;

                TabsPanel.Children.Add(tab);
                
                if (selectedFile?.FullName == tab.BoundFile.FullName)
                    tab.Select();
            }
        }

        private void OnTabSelected(object sender, EventArgs e)
        {
            Open(((EditorTabItem)sender).BoundFile);
        }

        private void OnTabMouseMove(object sender, EventArgs e)
        {
            var tab = (EditorTabItem)sender;
            
            if (!tab.IsSelected) 
                tab.Background = HoveredTabColor;
        }

        private void OnTabMouseLeave(object sender, EventArgs e)
        {
            var tab = (EditorTabItem)sender;
            
            if (!tab.IsSelected) 
                tab.Background = UnselectedTabColor;
        }

        private void OnTabClosed(object sender, EventArgs e)
        {
            Close(((EditorTabItem)sender).BoundFile);
        }

        public Editor GetFocusedEditor()
        {
            var selected = SelectedItem?.Editor;

            return selected?.IsFocused ?? false 
                ? selected : null;
        }
    }
}