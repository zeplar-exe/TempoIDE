using System.IO;
using TempoIDE.Core.Helpers;
using TempoIDE.Core.SettingsConfig.Internal;
using TempoIDE.Core.SettingsConfig.Internal.Parser;
using TempoIDE.Properties;

namespace TempoIDE.Core.SettingsConfig
{
    public static class SettingsHelper
    {
        public static bool TryGet<T>(string path, out T result) where T : SettingValue
        {
            result = default;

            if (!AppDataHelper.TryGetDataDirectory(out var directory))
            {
                ApplicationHelper.EmitErrorCode(ApplicationErrorCode.TI_INVALID_DIRECTORY,
                    ErrorResources.NoDataDirectory);

                return false;
            }

            var target = new Target<string>(path.Split('.'));

            FileInfo targetFile = null;

            while (target.TryMoveNext(out var part))
            {
                var newPath = Path.Join(directory.FullName, part);

                if (Directory.Exists(newPath))
                {
                    directory = new DirectoryInfo(newPath);
                }
                else if (File.Exists(newPath))
                {
                    targetFile = new FileInfo(newPath);
                    break;
                }
                else
                {
                    return false;
                }
            }

            if (targetFile == null)
                return false;

            if (!target.TryMoveNext(out var targetKey))
                return false;

            using var reader = targetFile.OpenText();

            foreach (var pair in new SettingsParser(reader.ReadToEnd()).Parse())
            {
                if (pair.Key == targetKey)
                    result = (T)pair.Value;
            }

            return true;
        }
    }
}