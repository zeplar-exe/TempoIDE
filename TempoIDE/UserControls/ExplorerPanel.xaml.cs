using System.ComponentModel;
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
        public SolidColorBrush UnselectedFileBrush { get; set; } = Brushes.Transparent;
        public SolidColorBrush SelectedFileBrush { get; set; } = Brushes.Gray;
        public SolidColorBrush OpenedFileBrush { get; set; } = Brushes.CornflowerBlue;
        
        private readonly string[] explorerExtensions =
        {
            ".txt", ".cs"
        };
        
        private const int IndentationSpace = 30;

        public event OpenFileEventHandler OpenElementEvent;
        
        private ExplorerPanelElement selectedElement;

        public ExplorerPanel()
        {
            InitializeComponent();
        }

        public ExplorerPanelElement AppendElement(ExplorerPanelElement element, ExplorerPanelElement parent = null)
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
                root = AppendElement(new ExplorerFileElement(directory.FullName), parent);
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
                        AppendElement(new ExplorerFileElement(filePath), root);
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
                    selectedElement.Background = UnselectedFileBrush;
                    selectedElement = null;
                }

                selectedElement = element;
                
                element.Background = SelectedFileBrush;
            }
            else if (e.ClickCount == 2)
            {
                element.Background = OpenedFileBrush;
                
                OpenElementEvent?.Invoke(this, new OpenExplorerElementArgs(element));
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