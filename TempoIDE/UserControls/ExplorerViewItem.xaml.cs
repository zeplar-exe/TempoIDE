using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TempoIDE.UserControls
{
    public partial class ExplorerViewItem : TreeViewItem
    {
        public string HeaderText { get; set; }
        
        public ExplorerViewItem()
        {
            InitializeComponent();
        }

        public void AppendElement(ExplorerViewItem item)
        {
            Items.Add(item);
        }

        public async void AppendDirectory(DirectoryInfo directory, bool includeRoot)
        {
            ExplorerViewItem root;

            if (includeRoot)
            {
                root = new ExplorerFileItem(directory.FullName);

                Dispatcher.Invoke(delegate
                {
                    AppendElement(root);
                });
            }
            else
            {
                root = this;
            }

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

                    if (ExplorerView.SupportedExtensions.Contains(Path.GetExtension(filePath)))
                    {
                        Dispatcher.Invoke(delegate
                        {
                            root.AppendElement(new ExplorerFileItem(filePath));
                        });
                    }
                }
            });
        }
    }
}