using System;
using System.IO;
using System.Linq;
using TempoIDE.Core.SettingsConfig.Internal.Parser;
using TempoIDE.Core.SettingsConfig.Settings.Exceptions;

namespace TempoIDE.Core.SettingsConfig.Settings.SettingsFiles
{
    public class SkinConfig : Config
    {
        public SkinConfigSetting CurrentSkin;
        public SkinConfigSetting PreviousSkin;
        
        public SkinConfig(Stream stream) : base(stream)
        {
            
        }

        public SkinConfig(FileStream stream) : base(stream)
        {
            
        }

        public override async void Parse()
        {
            if (FilePath == null)
                throw new IOException("Cannot correctly parse a non-file stream.");
            
            using var reader = new StreamReader(Stream, leaveOpen: true);
            var settings = new SettingsParser(await reader.ReadToEndAsync()).Parse();

            var previous = "";
            var current = "";
            
            foreach (var setting in settings)
            {
                switch (setting.Key)
                {
                    case "previous_skin":
                        previous = setting.Value.ToString();
                        break;
                    case "current_skin":
                        current = setting.Value.ToString();
                        break;
                }
            }

            if (string.IsNullOrEmpty(current))
                throw new MissingSettingException("current_skin");

            var file = new FileInfo(FilePath);
            var skinDirectory = new DirectoryInfo(Path.Join(file.DirectoryName, "skins"));

            if (!skinDirectory.Exists)
                throw new DirectoryNotFoundException("Expected a 'skins' directory adjacent to 'skins.txt'");

            var targetPreviousFile = new FileInfo(Path.Join(skinDirectory.FullName, previous));
            var targetCurrentFile = new FileInfo(Path.Join(skinDirectory.FullName, current));

            if (!targetCurrentFile.Exists)
                throw new FileNotFoundException("The target 'current_file' does not exist.");

            CurrentSkin = new SkinConfigSetting(current, targetCurrentFile);

            if (targetPreviousFile.Exists)
                PreviousSkin = new SkinConfigSetting(previous, targetPreviousFile);
        }

        public override void Write()
        {
            Stream.Seek(0, SeekOrigin.Begin);

            using var writer = new StreamWriter(Stream, leaveOpen: true);
            
            if (!string.IsNullOrEmpty(PreviousSkin.GivenName))
                writer.WriteLine($"previous_skin={PreviousSkin.GivenName}");
            
            writer.Write($"current_skin={CurrentSkin.GivenName}");
        }
    }
}