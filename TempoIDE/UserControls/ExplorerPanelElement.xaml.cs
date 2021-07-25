using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TempoIDE.UserControls
{
    public partial class ExplorerPanelElement : UserControl
    {
        private string filePath;
        public string FilePath
        {
            get => filePath;
            set { filePath = value; Update(); }
        }
        private ExplorerPanelElementType type;
        
        public bool IsExpanded { get; private set; }
        public readonly ObservableCollection<ExplorerPanelElement> Children = new ObservableCollection<ExplorerPanelElement>();

        public ExplorerPanelElement()
        {
            InitializeComponent();

            Expand();
            RefreshChildren();
            Children.CollectionChanged += delegate { RefreshChildren(); };
        }
        
        public ExplorerPanelElement(string path)
        {
            InitializeComponent();

            FilePath = path;
            
            Expand();
            RefreshChildren();
            Children.CollectionChanged += delegate { RefreshChildren(); };
        }

        private void Update()
        {
            if (Directory.Exists(filePath))
            {
                type = ExplorerPanelElementType.Directory;
            }
            else if (File.Exists(filePath))
            {
                var file = new FileInfo(filePath);

                type = file.Extension switch
                {
                    ".csproj" => ExplorerPanelElementType.Project,
                    ".sln" => ExplorerPanelElementType.Solution,
                    _ => ExplorerPanelElementType.File
                };
            }

            Text.Text = Path.GetFileName(filePath);

            switch (type)
            {
                case ExplorerPanelElementType.Solution:
                {
                    Text.Text = Path.GetFileNameWithoutExtension(filePath);
                    
                    break;
                }
                case ExplorerPanelElementType.Project:
                {
                    Text.Text = Path.GetFileNameWithoutExtension(filePath);
                    
                    break;
                }
                case ExplorerPanelElementType.Directory:
                {
                    Text.Text = Path.GetFileNameWithoutExtension(filePath);
                    
                    break;
                }
                case ExplorerPanelElementType.File:
                {
                    Text.Text = Path.GetFileName(filePath);
                    
                    break;
                }
            }
        }
        
        private void ExpandButton_OnClick(object sender, RoutedEventArgs e)
        {
            IsExpanded = !IsExpanded;

            RefreshChildren();
        }

        public void RefreshChildren()
        {
            if (Children.Count == 0)
                ExpandButton.Visibility = Visibility.Collapsed;
            else
                ExpandButton.Visibility = Visibility.Visible;
            
            if (IsExpanded)
                Expand();
            else
                Collapse();
        }

        public void Expand()
        {
            IsExpanded = true;
            
            foreach (var element in Children)
            {
                element.Expand();
                element.Visibility = Visibility.Visible;
            }
        }

        public void Collapse()
        {
            IsExpanded = false;

            foreach (var element in Children)
            {
                element.Collapse();
                element.Visibility = Visibility.Collapsed;
            }
        }
    }
    
    public enum ExplorerPanelElementType { Solution, Project, Directory, File }
}