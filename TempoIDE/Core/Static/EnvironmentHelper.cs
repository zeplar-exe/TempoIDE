using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Jammo.TextAnalysis.DotNet.MsBuild;
using TempoIDE.Core.Environments;
using TempoIDE.Core.Types;
using TempoIDE.Windows;

namespace TempoIDE.Core.Static
{
    public static class EnvironmentHelper
    {
        public static DevelopmentEnvironment Current;
        
        public static void LoadEnvironment(string path)
        {
            ApplicationHelper.MainWindow.Editor.Tabs.CloseAll();
            
            var env = EnvFromPath(path);

            if (env == null)
            {
                // TODO: Notify user
                
                return;
            }

            var progressDialog = new ProgressDialog
            {
                Title = "Preparing workspace",
                Owner = ApplicationHelper.MainWindow
            };

            progressDialog.Tasks.Enqueue(new ProgressTask("Caching files", env.CacheFiles));
            
            progressDialog.Tasks.Enqueue(new ProgressTask("Reading semantics", env.Cache.UpdateModels));
            
            progressDialog.Tasks.Enqueue(new ProgressTask(
                "Loading files", delegate { ApplicationHelper.AppDispatcher.Invoke(RefreshExplorer); }));

            progressDialog.Completed += delegate { progressDialog.Close(); };
            progressDialog.StartAsync();

            env.DirectoryWatcher.Changed += DirectoryChanged;
            
            Current = env;
        }

        private static void DirectoryChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                ApplicationHelper.AppDispatcher.Invoke(RefreshExplorer);
                ApplicationHelper.AppDispatcher.Invoke(ApplicationHelper.MainWindow.Editor.Tabs.Refresh);
            }

            Current.Cache.UpdateModels();
        }
        
        public static void CloseEnvironment()
        {
            Current.Close();

            ApplicationHelper.AppDispatcher.Invoke(ApplicationHelper.MainWindow.Editor.Tabs.CloseAll);
            ApplicationHelper.AppDispatcher.Invoke(RefreshExplorer);
        }
        
        public static void RefreshExplorer()
        {
            ApplicationHelper.MainWindow.Explorer.Clear();
            
            Current.LoadExplorer(ApplicationHelper.MainWindow.Explorer);
        }

        private static DevelopmentEnvironment EnvFromPath(string path)
        {
            if (File.Exists(path))
            {
                if (Path.GetExtension(path) == ".sln")
                {
                    return new SolutionEnvironment(new FileInfo(path));
                }
                
                return new FileEnvironment(new FileInfo(path));
            }
            
            if (Directory.Exists(path))
            {
                return new DirectoryEnvironment(new DirectoryInfo(path));
            }

            return null;
        }
    }
}