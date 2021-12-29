using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TempoIDE.Core;
using TempoIDE.Core.CustomEventArgs;
using TempoIDE.Core.DataStructures;
using TempoIDE.Core.Environments;
using TempoIDE.Core.Helpers;

namespace TempoIDE.Controls.Panels;

public partial class ExplorerView : TreeView
{
    public Color SelectedItemColor { get; set; } = Brushes.Blue.Color;
    public Color UnfocusedItemColor { get; set; } = Brushes.CadetBlue.Color;

    public static readonly string[] SupportedExtensions =
    {
        ".txt", ".cs", ".xml", ".xaml"
    };
        
    public event OpenFileEventHandler OpenItemEvent;

    public ExplorerView()
    {
        InitializeComponent();
    }

    public BitTree GetExpansionState()
    {
        var trees = Items.OfType<ExplorerViewItem>()
            .Select(item => 
                BitTree.FromTree(item, 
                    i => i.Items.OfType<ExplorerViewItem>(), 
                    i => i.IsExpanded).Root);

        var root = new Bit(true);
        root.Children.AddRange(trees);

        return new BitTree(root);
    }

    public void AppendElement(ExplorerViewItem element)
    {
        Items.Add(element);
    }

    public async void AppendDirectory(DirectoryInfo directory)
    {
        ExplorerViewItem root = new ExplorerFileSystemItem(directory.FullName);

        Dispatcher.Invoke(delegate
        {
            AppendElement(root);
        });

        await Task.Run(delegate
        {
            foreach (var filePath in Directory.GetFileSystemEntries(directory.FullName))
            {
                if (EnvironmentHelper.Current is CSharpSolutionEnvironment solutionEnv)
                {
                    if (solutionEnv.ConfigStream.QueryExcluded(filePath))
                        continue;
                }

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
                        root.AppendElement(new ExplorerFileSystemItem(filePath));
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

        var clicked = (UIElement)e.OriginalSource;
            
        if (clicked is null)
            return;

        var element = clicked.FindAncestorOfType<ExplorerViewItem>();

        OpenItemEvent?.Invoke(this, new OpenExplorerElementArgs(element));
    }

    private void ExplorerView_OnPreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
    {
        var clicked = (UIElement)e.OriginalSource;
            
        if (clicked is null)
            return;

        clicked.FindAncestorOfType<TreeViewItem>().IsSelected = true;
    }
}