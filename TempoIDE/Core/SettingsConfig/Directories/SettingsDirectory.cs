using System.IO;
using TempoIDE.Core.Helpers;

namespace TempoIDE.Core.SettingsConfig.Directories
{
    public class SettingsDirectory : SettingDirectoryWrapper
    {
        public AppSettings AppSettings { get; }
        public EditorSettings EditorSettings { get; }
        public ExplorerSettings ExplorerSettings { get; }

        public SettingsDirectory(DirectoryInfo root) : base(root)
        {
            AppSettings = new AppSettings(root.ToRelativeDirectory("app").CreateIfMissing());
            EditorSettings = new EditorSettings(root.ToRelativeDirectory("editor").CreateIfMissing());
            ExplorerSettings = new ExplorerSettings(root.ToRelativeDirectory("explorer").CreateIfMissing());
        }

        public override void Parse()
        {
            AppSettings.Parse();
            EditorSettings.Parse();
            ExplorerSettings.Parse();
        }

        public override void Write()
        {
            AppSettings.Write();
            EditorSettings.Write();
            ExplorerSettings.Write();
        }
    }
}