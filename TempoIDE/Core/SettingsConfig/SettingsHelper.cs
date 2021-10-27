using System.IO;
using TempoIDE.Core.Helpers;
using TempoIDE.Core.SettingsConfig.Internal;
using TempoIDE.Core.SettingsConfig.Internal.Parser;
using TempoIDE.Properties;

namespace TempoIDE.Core.SettingsConfig
{
    public static class SettingsHelper
    {
        public static bool TryGet<T>(string path, out T result)
        {
            result = default;
            
            if (!AppDataHelper.TryGetDataDirectory(out var directory))
            {
                ApplicationHelper.EmitErrorCode(ApplicationErrorCode.TI_INVALID_DIRECTORY, ErrorResources.NoDataDirectory);
                
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

            using var reader = targetFile.OpenText();

            return true;
        }
    }

    public class DataPair
    {
        public readonly string Key;
        public readonly DataValue Value;

        public DataPair(string key, DataValue value)
        {
            Key = key;
            Value = value;
        }
    }

    public abstract class DataValue
    {
        public readonly string raw;

        protected DataValue(string raw)
        {
            this.raw = raw;
        }
    }

    public class TextData : DataValue
    {
        public TextData(string raw) : base(raw)
        {
        }
    }

    public class ExtendedData : DataValue
    {
        internal ExtendedData(SettingsNode node) : base(node.ToString())
        {
            
        }
    }
}