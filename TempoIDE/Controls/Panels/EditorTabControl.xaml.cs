using System;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using TempoIDE.Controls.Editors;
using TempoIDE.Core;

namespace TempoIDE.Controls.Panels
{
    public partial class EditorTabControl : UserControl
    {
        public EditorTabItem SelectedItem { get; private set; }
        public int SelectedIndex => TabsPanel.Children.IndexOf(SelectedItem);
        public EditorTabItem[] Children => TabsPanel.Children.OfType<EditorTabItem>().ToArray();

        public EditorTabControl()
        {
            DataContext = this;
            
            InitializeComponent();
        }

        public Brush SelectedTabColor { get; set; }
        public Brush HoveredTabColor { get; set; }
        public Brush UnselectedTabColor { get; set; }

        public EditorTabItem OpenFile(FileInfo file)
        {
            switch (file.Extension)
            {
                case ".png":
                case ".jpg":
                {
                    return OpenImageFile(file);
                }
                default:
                {
                    return OpenTextFile(file);
                }
            }
        }

        public bool TryOpenNext()
        {
            if (SelectedItem == null)
                return false;

            if (SelectedIndex <= TabsPanel.Children.Count)
                return false;

            OpenEditor(Children[SelectedIndex + 1].Editor);

            return true;
        }

        public bool TryOpenPrevious()
        {
            if (SelectedItem == null)
                return false;
            
            if (SelectedIndex <= 0)
                return false;

            OpenEditor(Children[SelectedIndex - 1].Editor);

            return true;
        }

        public EditorTabItem OpenTextFile(FileInfo file)
        {
            if (file is not { Exists: true })
                return null;

            if (TryGetFileEditorTab(file, out var tab))
            {
                tab.Select();

                return tab;
            }

            var newTab = OpenEditor(TextFileEditor.FromFile(file));
            newTab.Header.Text = file.Name;
            
            return newTab;
        }

        public EditorTabItem OpenImageFile(FileInfo file)
        {
            if (TryGetFileEditorTab(file, out var tab))
            {
                tab.Select();

                return tab;
            }

            var newTab = OpenEditor(ImageFileEditor.FromFile(file));
            newTab.Header.Text = file.Name;

            return newTab;
        }

        public EditorTabItem OpenEditor(Editor editor)
        {
            if (editor == null)
                return null;
            
            foreach (var tab in Children)
            {
                if (tab.Editor == editor)
                {
                    tab.Select();

                    return tab;
                }
            }

            var newTab = CreateEditorTab(editor);
            OpenTab(newTab);

            return newTab;
        }

        private bool TryGetFileEditorTab(FileInfo file, out EditorTabItem editorTab)
        {
            editorTab = null;
            
            foreach (var tab in Children)
            {
                if (tab.Editor is not FileEditor fileEditor)
                    continue;
                
                if (fileEditor.BoundFile.EqualsOther(file))
                {
                    editorTab = tab;

                    return true;
                }
            }

            return false;
        }

        private EditorTabItem CreateEditorTab(Editor editor)
        {
            var tab = new EditorTabItem
            {
                Editor = editor,
                Background = UnselectedTabColor
            };

            tab.Selected += OnTabSelected;
            tab.MouseMove += OnTabMouseMove;
            tab.MouseLeave += OnTabMouseLeave;
            tab.Closed += OnTabClosed;

            return tab;
        }

        public void CloseFile(FileInfo file)
        {
            foreach (var tab in Children)
            {
                if (tab.Editor is not FileEditor fileEditor) 
                    continue;
                
                if (fileEditor.BoundFile.EqualsOther(file))
                {
                    CloseEditor(fileEditor);
                        
                    return;
                }
            }
        }
        
        public void OpenTab(EditorTabItem tabItem)
        {
            foreach (var tab in Children)
            {
                if (tab == tabItem)
                {
                    tab.Select();
                    return;
                }
            }

            TabsPanel.Children.Add(tabItem);
            tabItem.Select();
        }

        public void CloseEditor(Editor editor)
        {
            var tab = GetTabOfEditor(editor);
            
            if (tab == null)
                return;
            
            CloseTab(tab);
        }

        public void CloseTab(EditorTabItem tabItem)
        {
            if (tabItem == null)
                return;
            
            var closingSelected = SelectedItem == tabItem;

            if (tabItem.Editor is FileEditor fileEditor)
                fileEditor.UpdateFile();

            if (closingSelected)
            {
                SelectedItem = null;

                if (!TryOpenNext())
                {
                    if (!TryOpenPrevious())
                        ContentDisplay.Child = null;
                }
            }
            
            TabsPanel.Children.Remove(tabItem);
        }
        
        public void CloseAll()
        {
            foreach (var tab in Children)
                CloseTab(tab);
        }

        private EditorTabItem GetTabOfEditor(Editor editor)
        {
            return Children.FirstOrDefault(tab => tab.Editor == editor);
        }

        public void Refresh()
        {
            foreach (var tab in Children)
            {
                if (tab.Editor is FileEditor fileEditor)
                {
                    if (!fileEditor.BoundFile.Exists)
                        CloseEditor(fileEditor);

                    return;
                }
            }
        }

        private void OnTabSelected(object sender, EventArgs e)
        {
            if (SelectedItem != null)
                SelectedItem.IsSelected = false;
            
            SelectedItem = (EditorTabItem)sender;
            
            if (ContentDisplay.Child == SelectedItem)
                return;
            
            ContentDisplay.Child = SelectedItem.Editor;
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
            CloseTab((EditorTabItem)sender);
        }
    }
}