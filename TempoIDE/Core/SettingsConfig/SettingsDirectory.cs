using System.IO;
using TempoIDE.Core.Helpers;

namespace TempoIDE.Core.SettingsConfig
{
    public class SettingsDirectory : SettingDirectoryWrapper
    {
        public readonly AppSettings AppSettings;
        public readonly EditorSettings EditorSettings;
        public readonly ExplorerSettings ExplorerSettings;

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