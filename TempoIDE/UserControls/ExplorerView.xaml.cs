using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TempoIDE.Classes;
using TempoIDE.Classes.Types;

namespace TempoIDE.UserControls
{
    public partial class ExplorerView : TreeView
    {
        public Color SelectedItemColor { get; set; } = (Color)ColorConverter.ConvertFromString("#4563d9");
        public Color UnfocusedItemColor { get; set; } = (Color)ColorConverter.ConvertFromString("#4a4d51");

        public static string[] SupportedExtensions =
        {
            ".txt", ".cs", ".xml", ".xaml"
        };
        
        public event OpenFileEventHandler OpenItemEvent;

        public ExplorerView()
        {
            InitializeComponent();
        }

        public void AppendElement(ExplorerViewItem element)
        {
            Items.Add(element);
        }

        public async void AppendDirectory(DirectoryInfo directory)
        {
            ExplorerViewItem root = new ExplorerFileItem(directory.FullName);

            Dispatcher.Invoke(delegate
            {
                AppendElement(root);
            });

            await Task.Run(delegate
            {
                foreach (var filePath in Directory.GetFileSystemEntries(directory.FullName))
                {
                    if (Directory.Exists(filePath))
                    {
                        Dispatcher.Invoke(delegate
                        {
                            root.AppendDirectory(new DirectoryInfo(filePath), true);
                        });
                    }
                    else if (SupportedExtensions.Contains(Path.GetExtension(filePath)))
                    {
                        Dispatcher.Invoke(delegate
                        {
                            root.AppendElement(new ExplorerFileItem(filePath));
                        });
                    }
                }
            });
        }
        
        public void Clear() => Items.Clear();

        private void ExplorerView_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount != 2)
                return;

            var clicked = e.OriginalSource as UIElement;
            
            if (clicked is null)
                return;

            var element = clicked.FindAncestorOfType<ExplorerViewItem>();

            OpenItemEvent?.Invoke(this, new OpenExplorerElementArgs(element));
        }
    }
}