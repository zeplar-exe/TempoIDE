using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TempoIDE.Classes.Types;

namespace TempoIDE.UserControls
{
    public partial class ExplorerPanel : Panel
    {
        // TODO: Make selected file persistent with refreshes
        // TODO: Handle deleted files accordingly
        private readonly SolidColorBrush unselectedFileBrush = Brushes.Transparent;
        private readonly SolidColorBrush selectedFileBrush = Brushes.Gray;
        private readonly SolidColorBrush openedFileBrush = Brushes.CornflowerBlue;
        
        private readonly string[] explorerExtensions =
        {
            ".txt", ".cs"
        };
        
        private const int IndentationSpace = 30;

        private FileInfo openFile;
        private FileInfo OpenFile
        {
            get => openFile;
            set
            {
                openFile = value;
                
                OpenFileEvent?.Invoke(this, new OpenFileEventArgs(value));
            }
        }

        public event OpenFileEventHandler OpenFileEvent;
        
        private ExplorerPanelElement selectedElement;

        public ExplorerPanel()
        {
            InitializeComponent();
        }

        public void AppendElement(ExplorerPanelElement element, ExplorerPanelExpander parent = null)
        {
            element.MouseLeftButtonDown += FileTextBlock_OnMouseUp;

            if (parent?.Content == null)
            {
                Children.Add(element);
            }
            else
            {
                element.Padding = new Thickness(IndentationSpace + parent.Padding.Left, 0, 0, 0);
                parent.Children.Add(element);
                Children.Add(element);
            }
        }

        public void AppendExpander(ExplorerPanelExpander expander, ExplorerPanelExpander parent = null)
        {
            if (parent?.Content == null)
            {
                Children.Add(expander);
            }
            else
            {
                expander.Padding = new Thickness(IndentationSpace + parent.Padding.Left, 0, 0, 0);

                parent.Children.Add(expander);
                Children.Add(expander);
            }
        }

        public void AppendDirectory(DirectoryInfo directory, ExplorerPanelExpander parent = null)
        {
            var root = AppendExpander(directory.FullName, parent);

            foreach (var filePath in Directory.GetFileSystemEntries(directory.FullName))
            {
                if (Directory.Exists(filePath))
                {
                    AppendDirectory(
                        new DirectoryInfo(filePath),
                        root
                    );
                }
                
                if (explorerExtensions.Contains(Path.GetExtension(filePath)))
                {
                    AppendElement(new DirectoryInfo(filePath), root);
                }
                
                UpdateLayout();
            }
        }

        private void AppendElement(DirectoryInfo directory, ExplorerPanelExpander parent)
        {
            var element = new ExplorerPanelElement
            {
                FilePath = directory.FullName
            };
            
            element.MouseLeftButtonDown += FileTextBlock_OnMouseUp;

            if (parent?.Content == null)
            {
                Children.Add(element);
            }
            else
            {
                element.Padding = new Thickness(IndentationSpace + parent.Padding.Left, 0, 0, 0);
                parent.Children.Add(element);
                Children.Add(element);
            }
        }

        private void FileTextBlock_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            var element = (ExplorerPanelElement) sender;

            if (e.ClickCount == 1)
            {
                if (selectedElement != null)
                {
                    selectedElement.Background = unselectedFileBrush;
                    selectedElement = null;
                }

                selectedElement = element;
                
                element.Background = selectedFileBrush;
            }
            else if (e.ClickCount == 2)
            {
                element.Background = openedFileBrush;
                OpenFile = new FileInfo(element.FilePath);
            }
        }
        
        private ExplorerPanelExpander AppendExpander(string path, ExplorerPanelExpander parent)
        {
            var expander = new ExplorerPanelExpander
            {
                ElementExpander =
                {
                    IsExpanded = true
                }
            };
            
            ((ExplorerPanelElement) expander.ElementExpander.Header).FilePath = path;

            if (parent?.Content == null)
            {
                Children.Add(expander);
            }
            else
            {
                expander.Padding = new Thickness(IndentationSpace + parent.Padding.Left, 0, 0, 0);
                parent.Children.Add(expander);
                Children.Add(expander);
            }
            
            return expander;
        }
        
        public void Clear() => Children.Clear();

        protected override Size MeasureOverride(Size availableSize)
        {
            var returnSize = new Size();
            
            foreach (UIElement child in Children)
            {
                child.Measure(availableSize);

                returnSize.Height += child.DesiredSize.Height;
            }
            
            return returnSize;
        }
        
        protected override Size ArrangeOverride(Size finalSize)
        {
            double yPos = 0;
            
            foreach (UIElement child in InternalChildren)
            {
                child.Arrange(new Rect(new Point(0, yPos), child.DesiredSize));
                
                yPos += child.DesiredSize.Height;
            }
            
            return finalSize;
        }
    }
}