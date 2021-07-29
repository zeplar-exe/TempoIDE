using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using TempoIDE.Classes;

namespace TempoIDE.UserControls
{
    public partial class EditorTabControl : UserControl
    {
        public EditorTabItem SelectedItem { get; private set; }
        
        public ObservableCollection<string> Files { get; } = new ObservableCollection<string>(); 
        
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

            if (SelectedItem.NotNull())
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
            
            Files.Remove(file.FullName);
            
            if (closingSelected)
            {
                SelectedItem = null;

                if (index == 0)
                {
                    if (nextIndex < Files.Count)
                        Open(Files[nextIndex].ToFile());
                }
                else
                {
                    Open(Files[lastIndex].ToFile());
                }
            }
        }

        public void CloseAll()
        {
            foreach (var item in Files.ToArray())
                Close(item.ToFile());
        }

        public void Refresh()
        {
            TabsPanel.Children.Clear();

            foreach (var path in Files.ToArray())
            {
                var fileInfo = path.ToFile();

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
                tab.Selected += delegate { Open(tab.BoundFile); };
                tab.MouseMove += delegate
                {
                    if (!tab.IsSelected) 
                        tab.Background = HoveredTabColor;
                };
                tab.MouseLeave += delegate
                {
                    if (!tab.IsSelected) 
                        tab.Background = UnselectedTabColor;
                };
                tab.Closed += delegate { Close(tab.BoundFile);};

                TabsPanel.Children.Add(tab);
            }
        }

        public Editor GetFocusedEditor()
        {
            var selected = SelectedItem?.Editor;

            return selected?.IsFocused ?? false 
                ? selected : null;
        }
    }
}