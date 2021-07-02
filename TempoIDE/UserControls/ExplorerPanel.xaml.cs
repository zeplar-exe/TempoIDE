using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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
            ".txt"
        };
        
        private const int IndentationSpace = 30;

        private DirectoryInfo currentDirectory = new DirectoryInfo(@"C:\Users\zande\Code\C#\TempoIDE\TempoIDE\ExplorerTest");
        
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
        
        private TextBlock selectedTextBlock;
        private Thread updaterThread;
        
        public ExplorerPanel()
        {
            InitializeComponent();
        }
        
        private void ExplorerPanel_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
                return;

            updaterThread = new Thread(DirectoryUpdaterThread);
            updaterThread.Start();
            
            UpdateDirectory(currentDirectory); // TODO: Replace this with a file dialog
        }

        public void UpdateDirectory(DirectoryInfo newDirectory)
        {
            currentDirectory = newDirectory;

            FillFromDirectory(newDirectory);
        }

        private const int UpdaterCooldown = 3;
        private void DirectoryUpdaterThread()
        {
            while (true)
            {
                Thread.Sleep(UpdaterCooldown * 1000);

                try
                {
                    Dispatcher.Invoke(() =>
                    {
                        if (currentDirectory != null) FillFromDirectory(currentDirectory);
                    });
                }
                catch (TaskCanceledException e)
                {
                    // This is intended on application close
                    Console.WriteLine(@"There was a TaskCanceledException, may be important.");
                }
            }
        }

        public void FillFromDirectory(DirectoryInfo directoryPath)
        {
            Children.Clear();
            directoryPath.Refresh();
            
            var topLevelExpander = AddFileExpander(Path.GetFileName(directoryPath.Name.Trim('\\')), 0, null);
            AddDirectory(directoryPath, 1, topLevelExpander);
        }

        private void AddDirectory(DirectoryInfo directory, int indentationLevel, Expander parent = null)
        {
            foreach (string filePath in Directory.GetFileSystemEntries(directory.FullName))
            {
                if (Directory.Exists(filePath))
                {
                    var expanderParent = AddFileExpander(Path.GetFileName(filePath), indentationLevel, parent);
                    
                    AddDirectory(
                        new DirectoryInfo(filePath), 
                        indentationLevel, 
                        expanderParent
                    );
                }
                
                if (explorerExtensions.Contains(Path.GetExtension(filePath)))
                {
                    AddFileTextBlock(Path.GetFileName(filePath), filePath, indentationLevel, parent);
                }
                
                UpdateLayout();
            }
        }

        private void AddFileTextBlock(string text, string directory, int indent, Expander parent)
        {
            var textBlock = new TextBlock
            {
                Text = Path.GetFileName(text)
            };
            textBlock.SetValue(IndentationLevel, indent);
            textBlock.MouseLeftButtonDown += FileTextBlock_OnMouseUp;
            textBlock.Resources["BoundFile"] = directory;

            if (parent?.Content == null)
            {
                Children.Add(textBlock);
            }
            else
            {
                textBlock.Padding = new Thickness(IndentationSpace, 0, 0, 0);
                ((StackPanel)parent.Content).Children.Add(textBlock);
            }
        }

        private void FileTextBlock_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            var textBlock = (TextBlock) sender;

            if (e.ClickCount == 1)
            {
                if (selectedTextBlock != null)
                {
                    selectedTextBlock.Background = unselectedFileBrush;
                    selectedTextBlock = null;
                }

                selectedTextBlock = textBlock;
                
                textBlock.Background = selectedFileBrush;
            }

            if (e.ClickCount == 2)
            {
                textBlock.Background = openedFileBrush;
                OpenFile = new FileInfo((string)textBlock.Resources["BoundFile"]);
            }
        }
        
        private Expander AddFileExpander(string text, int indent, Expander parent)
        {
            var expander = new Expander
            {
                Header = Path.GetFileName(text),
                IsExpanded = true
            };
            expander.SetValue(IndentationLevel, indent);
            expander.Content = new StackPanel();

            if (parent?.Content == null)
            {
                Children.Add(expander);
            }
            else
            {
                expander.Padding = new Thickness(IndentationSpace, 0, 0, 0);
                ((StackPanel)parent.Content).Children.Add(expander);
            }
            
            return expander;
        }

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
            Size returnSize = new Size();
            
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