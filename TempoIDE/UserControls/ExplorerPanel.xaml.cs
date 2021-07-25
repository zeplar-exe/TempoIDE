using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TempoIDE.Classes.Types;
using TempoIDE.Windows;

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

        public ExplorerPanelElement AppendElement(string path, ExplorerPanelElement parent = null)
        {
            var element = new ExplorerPanelElement(path);
            
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

            return element;
        }

        public void AppendDirectory(DirectoryInfo directory, ExplorerPanelElement parent = null)
        {
            var worker = new BackgroundWorker
            {
                WorkerReportsProgress = true
            };
            
            worker.DoWork += delegate { AppendDirectoryThread(directory, parent); };

            worker.RunWorkerAsync();
        }

        private void AppendDirectoryThread(DirectoryInfo directory, ExplorerPanelElement parent = null)
        {
            ExplorerPanelElement root = null;
            
            Dispatcher.InvokeAsync(delegate
            {
                root = AppendElement(directory.FullName, parent);
            });
            
            foreach (var filePath in Directory.GetFileSystemEntries(directory.FullName))
            {
                if (Directory.Exists(filePath))
                {
                    Dispatcher.InvokeAsync(delegate
                    {
                        AppendDirectoryThread(
                            new DirectoryInfo(filePath),
                            root
                        );
                    });
                }
                
                if (explorerExtensions.Contains(Path.GetExtension(filePath)))
                {
                    Dispatcher.InvokeAsync(delegate
                    {
                        AppendElement(filePath, root);
                    });
                }
                
                Dispatcher.InvokeAsync(UpdateLayout);
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

        public void Clear() => Children.Clear();

        protected override Size MeasureOverride(Size availableSize)
        {
            var returnSize = new Size(0, 0); // TODO: Does not account for being given space
            
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