using System.ComponentModel;
using System.IO;
using TempoIDE.Core.Helpers;

namespace TempoIDE.Controls.Explorer
{
    public class ExplorerFile : TitledExplorerItem, INotifyPropertyChanged
    {
        private string filePath;
        
        public string FilePath
        {
            get => filePath;
            set
            {
                if (filePath == value)
                    return;

                filePath = value;

                HeaderText = FileName;
                
                OnPropertyChanged();
                OnPropertyChanged(nameof(FileName));
            } 
        }
        
        public string FileName => Path.GetFileName(FilePath);
        public bool IsDirectory => Directory.Exists(FilePath);

        public static ExplorerFile FromFile(string path)
        {
            var item = new ExplorerFile();
            
            if (ApplicationHelper.EmitIfInvalidFile(path))
                return item;
            
            item.FilePath = path;

            return item;
        }

        public static ExplorerFile FromDirectory(string path)
        {
            var item = new ExplorerFile();
            
            if (ApplicationHelper.EmitIfInvalidDirectory(path))
                return item;

            item.FilePath = path;
            
            foreach (var entry in Directory.EnumerateFileSystemEntries(path))
            {
                if (File.Exists(entry))
                {
                    item.Add(FromFile(entry));
                }
                else if (Directory.Exists(entry))
                {
                    item.Add(FromDirectory(entry));
                }
            }
            
            return item;
        }
    }
}