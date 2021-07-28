using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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

        public void Open(FileInfo file)
        {
            if (!file.Exists)
                return;
            
            if (!Files.Contains(file.FullName))
                Files.Add(file.FullName);

            var tab = TabsPanel.Children
                .Cast<EditorTabItem>()
                .First(t => t.BoundFile.FullName == file.FullName);

            SelectedItem = tab;
            ContentDisplay.Child = tab.Editor;
        }

        public void Close(FileInfo file)
        {
            if (file.FullName == SelectedItem?.BoundFile.FullName)
            {
                var index = Files.IndexOf(file.FullName);
                var nextIndex = index + 1;
                var lastIndex = index - 1;

                if (index == 0)
                {
                    if (nextIndex < Files.Count)
                        Open(Files[nextIndex].ToFile());
                }
                else
                {
                    Open(Files[lastIndex].ToFile());
                }

                Files.Remove(file.FullName);
            }

            Files.Remove(file.FullName);
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
                
                fileInfo.Refresh();
                
                if (!fileInfo.Exists)
                {
                    Files.Remove(path);
                    continue;
                }

                var tab = new EditorTabItem
                {
                    Header = { Text = fileInfo.Name },
                    Editor = Editor.FromExtension(fileInfo.Extension),
                    BoundFile = fileInfo
                };
                
                tab.Editor.Update(fileInfo);
                tab.Selected += delegate { Open(tab.BoundFile); };
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