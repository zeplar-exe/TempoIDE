using System.IO;
using TempoIDE.Core.Helpers;

namespace TempoIDE.Core.SettingsConfig.Directories
{
    public class SettingsDirectory : SettingDirectoryWrapper
    {
        public AppSettings AppSettings { get; set; }
        public EditorSettings EditorSettings { get; set; }
        public ExplorerSettings ExplorerSettings { get; set; }

        public SettingsDirectory(DirectoryInfo root) : base(root)
        {
            AppSettings = new AppSettings(root.ToRelativeDirectory("app").CreateIfMissing());
            EditorSettings = new EditorSettings(root.ToRelativeDirectory("editor").CreateIfMissing());
            ExplorerSettings = new ExplorerSettings(root.ToRelativeDirectory("explorer").CreateIfMissing());
        }

        public override void Write()
        {
            AppSettings.Write();
            EditorSettings.Write();
            ExplorerSettings.Write();
        }

        public override void Dispose()
        {
            AppSettings.Dispose();
            EditorSettings.Dispose();
            ExplorerSettings.Dispose();
        }
    }
}