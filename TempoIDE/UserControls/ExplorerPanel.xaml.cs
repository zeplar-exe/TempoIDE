using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using TempoIDE.Classes;
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
        
        private const string SolutionExtension = ".sln";
        private readonly string[] explorerExtensions = new[]
        {
            ".txt", ".cs"
        };
        
        private const int IndentationSpace = 30;

        private DirectoryInfo currentDirectory = new DirectoryInfo(
            Directory.CreateDirectory(Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "ExplorerTest")).FullName);
        
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

        public void AppendElement(ExplorerPanelElement element, int indentationLevel, ExplorerPanelExpander parent = null)
        {
            element.SetValue(IndentationLevel, indentationLevel);
            element.MouseLeftButtonDown += FileTextBlock_OnMouseUp;

            if (parent?.Content == null)
            {
                Children.Add(element);
            }
            else
            {
                element.Padding = new Thickness(IndentationSpace, 0, 0, 0);
                parent.ExpanderContent.Children.Add(element);
            }
        }

        public void AppendExpander(ExplorerPanelExpander expander, int indentationLevel, ExplorerPanelExpander parent = null)
        {
            expander.SetValue(IndentationLevel, indentationLevel);

            if (parent?.Content == null)
            {
                Children.Add(expander);
            }
            else
            {
                expander.Padding = new Thickness(IndentationSpace, 0, 0, 0);
                // I have no idea why the padding here is needed but without it the panel breaks
                
                parent.ExpanderContent.Children.Add(expander);
            }
        }

        public void AppendDirectory(DirectoryInfo directory, int indentationLevel, ExplorerPanelExpander parent = null)
        {
            foreach (var filePath in Directory.GetFileSystemEntries(directory.FullName))
            {
                if (Directory.Exists(filePath))
                {
                    var expanderParent = AppendExpander(Path.GetFileName(filePath), indentationLevel, parent);
                    
                    AppendDirectory(
                        new DirectoryInfo(filePath), 
                        indentationLevel, 
                        expanderParent
                    );
                }
                
                if (explorerExtensions.Contains(Path.GetExtension(filePath)))
                {
                    AppendElement(Path.GetFileName(filePath), filePath, indentationLevel, parent);
                }
                
                UpdateLayout();
            }
        }

        private void AppendElement(string text, string directory, int indent, ExplorerPanelExpander parent)
        {
            var element = new ExplorerPanelElement
            {
                Text =
                {
                    Text = Path.GetFileName(text)
                },
                FilePath = directory
            };
            element.SetValue(IndentationLevel, indent);
            element.MouseLeftButtonDown += FileTextBlock_OnMouseUp;

            if (parent?.Content == null)
            {
                Children.Add(element);
            }
            else
            {
                element.Padding = new Thickness(IndentationSpace, 0, 0, 0);
                parent.ExpanderContent.Children.Add(element);
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

            if (e.ClickCount == 2)
            {
                element.Background = openedFileBrush;
                OpenFile = new FileInfo(element.FilePath);
            }
        }
        
        private ExplorerPanelExpander AppendExpander(string path, int indent, ExplorerPanelExpander parent)
        {
            var expander = new ExplorerPanelExpander
            {
                ElementExpander =
                {
                    IsExpanded = true
                }
            };

            ((ExplorerPanelElement) expander.ElementExpander.Header).FilePath = path;

            expander.SetValue(IndentationLevel, indent);

            if (parent?.Content == null)
            {
                Children.Add(expander);
            }
            else
            {
                expander.Padding = new Thickness(IndentationSpace, 0, 0, 0);
                // I have no idea why the padding here is needed but without it the panel breaks
                
                parent.ExpanderContent.Children.Add(expander);
            }
            
            return expander;
        }
        
        public void Clear() => Children.Clear();

        #region Attached Properties

        public static readonly DependencyProperty IndentationLevel = DependencyProperty.RegisterAttached(
            "IndendtationLevel",
            typeof(int),
            typeof(ExplorerPanel)
        );
        
        public static void SetIndentationLevel(UIElement element, int value)
        {
            element.SetValue(IndentationLevel, value);
        }
        public static int GetIndentationLevel(UIElement element)
        {
            return (int)element.GetValue(IndentationLevel);
        }
        
        #endregion

        protected override Size MeasureOverride(Size availableSize)
        {
            var returnSize = new Size();
            
            foreach (UIElement child in Children)
            {
                child.Measure(availableSize);

                returnSize.Height += child.DesiredSize.Height;
                returnSize.Width += child.DesiredSize.Width;
            }
            
            return returnSize;
        }
        
        protected override Size ArrangeOverride(Size finalSize)
        {
            double yPos = 0;
            
            foreach (UIElement child in InternalChildren)
            {
                child.Arrange(new Rect(new Point(IndentationSpace * GetIndentationLevel(child), yPos), child.DesiredSize));
                
                yPos += child.DesiredSize.Height;
            }
            
            return finalSize;
        }
    }
}