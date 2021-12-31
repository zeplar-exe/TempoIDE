using System.IO;
using TempoIDE.Core.Environments;
using TempoIDE.Windows;

namespace TempoIDE.Core.Helpers;

public static class EnvironmentHelper
{
    public static DevelopmentEnvironment? Current;

    public static void LoadEnvironment(string path)
    {
        var env = EnvFromPath(path);

        if (env == null)
        {
            ApplicationHelper.EmitErrorCode(ApplicationErrorCode.TI_INVALID_FILE,
                $"The given file '{Path.GetFileName(path)}' does not exist.");

            return;
        }

        LoadEnvironment(env);
    }
        
    public static void LoadEnvironment(DevelopmentEnvironment environment)
    {
        ApplicationHelper.MainWindow?.v_Editor.v_Tabs.CloseAll();
            
        var progressDialog = new ProgressDialog
        {
            Title = "Preparing workspace",
            Owner = ApplicationHelper.MainWindow
        };
            
        Current = environment;

        progressDialog.Tasks.Enqueue(new ProgressTask("Caching files", environment.CacheFiles));
            
        progressDialog.Tasks.Enqueue(new ProgressTask("Loading files", 
            delegate { ApplicationHelper.AppDispatcher.Invoke(RefreshExplorer); }));

        progressDialog.Completed += delegate { progressDialog.Close(); };
        progressDialog.ShowDialog();

        environment.DirectoryWatcher.Changed += DirectoryChanged;
    }

    private static void DirectoryChanged(object sender, FileSystemEventArgs e)
    {
        if (e.ChangeType != WatcherChangeTypes.Changed)
        {
            ApplicationHelper.AppDispatcher.Invoke(RefreshExplorer);
            
            if (ApplicationHelper.MainWindow != null)
                ApplicationHelper.AppDispatcher.Invoke(ApplicationHelper.MainWindow.v_Editor.v_Tabs.Refresh);
        }
    }
        
    public static void CloseEnvironment()
    {
        Current?.Close();

        if (ApplicationHelper.MainWindow != null)
            ApplicationHelper.AppDispatcher.Invoke(ApplicationHelper.MainWindow.v_Editor.v_Tabs.CloseAll);
        ApplicationHelper.AppDispatcher.Invoke(RefreshExplorer);
    }
        
    public static void RefreshExplorer()
    {
        ApplicationHelper.MainWindow?.v_Explorer.Clear();
            
        if (ApplicationHelper.MainWindow != null) 
            Current?.LoadExplorer(ApplicationHelper.MainWindow.v_Explorer);
    }

    private static DevelopmentEnvironment? EnvFromPath(string path)
    {
        if (File.Exists(path))
        {
            if (Path.GetExtension(path) == ".sln")
                return new CSharpSolutionEnvironment(new FileInfo(path));

            return new FileEnvironment(new FileInfo(path));
        }
            
        if (Directory.Exists(path))
        {
            return new DirectoryEnvironment(new DirectoryInfo(path));
        }

        return null;
    }
}