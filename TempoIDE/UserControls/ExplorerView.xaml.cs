using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TempoIDE.Classes.Types;

namespace TempoIDE.UserControls
{
    public partial class ExplorerView : TreeView
    {
        private readonly string[] explorerExtensions =
        {
            ".txt", ".cs"
        };
        
        public event OpenFileEventHandler OpenItemEvent;

        public ExplorerView()
        {
            InitializeComponent();
        }

        public ExplorerViewItem AppendElement(ExplorerViewItem element, ExplorerViewItem parent = null)
        {
            element.PreviewMouseDoubleClick += ExplorerViewItem_PreviewMouseDoubleClick;

            if (parent == null)
            {
                Items.Add(element);
            }
            else
            {
                parent.Items.Add(element);
            }

            return element;
        }

        public void AppendDirectory(DirectoryInfo directory, ExplorerViewItem parent = null)
        {
            var worker = new BackgroundWorker
            {
                WorkerReportsProgress = true
            };
            
            worker.DoWork += delegate { AppendDirectoryThread(directory, parent); };

            worker.RunWorkerAsync();
        }

        private void AppendDirectoryThread(DirectoryInfo directory, ExplorerViewItem parent = null)
        {
            ExplorerViewItem root = null;
            
            Dispatcher.InvokeAsync(delegate
            {
                root = AppendElement(new ExplorerFileItem(directory.FullName), parent);
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
                        AppendElement(new ExplorerFileItem(filePath), root);
                    });
                }
                
                Dispatcher.InvokeAsync(UpdateLayout);
            }
        }

        private void ExplorerViewItem_PreviewMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            
            var element = (ExplorerViewItem) sender;
            
            OpenItemEvent?.Invoke(this, new OpenExplorerElementArgs(element));
        }

        public void Clear() => Items.Clear();
    }
}