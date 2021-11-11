using System;
using System.Collections.Generic;
using System.IO;
using Jammo.ParserTools;
using TempoIDE.Core.SettingsConfig.Internal.Parser;

namespace TempoIDE.Core.SettingsConfig.Settings.SettingsFiles
{
    public abstract class Config : IDisposable
    {
        private readonly List<ConfigDiagnostic> diagnostics = new();
        
        public IEnumerable<ConfigDiagnostic> Diagnostics => diagnostics.AsReadOnly();

        public string FileContents { get; }
        public string FilePath { get; }
        
        public SettingsDocument Document => new SettingsParser(FileContents).Parse();

        protected Config(FileInfo file)
        {
            FilePath = file.FullName;
            
            using var reader = new StreamReader(file.OpenRead());
            FileContents = reader.ReadToEnd();
        }
        
        protected Config(Stream stream)
        {
            _ = stream ?? throw new ArgumentNullException(nameof(stream));
            
            using var reader = new StreamReader(stream);
            FileContents = reader.ReadToEnd();
        }
        
        public abstract void Write();

        protected StreamWriter CreateWriter() => new(new FileInfo(FilePath).OpenWrite());

        protected void ReportUnexpectedSetting(Setting setting)
        {
            ReportWarning($"The setting '{setting.Key}' is never used.", setting.Context);
        }
        
        protected bool ReportIfUnexpectedSettingType<TType>(Setting setting, out TType expected) where TType : class
        {
            expected = default;
            
            if (setting.Value.GetType() == typeof(TType))
            {
                expected = setting.Value as TType;
                return false;
            }

            ReportWarning($"The setting '{setting.Key}' is of the wrong type. Expected {expected}.", setting.Context);

            return true;
        }

        protected bool ReportIfEmptySetting(Setting setting)
        {
            if (!string.IsNullOrEmpty(setting.Value.ToString()))
                return false;
            
            ReportError($"The setting {setting.Key} cannot be empty.", setting.Context);
            return true;

        }

        protected void ReportWarning(string message, StringContext context)
        {
            Report(message, DiagnosticSeverity.Warning, context);
        }

        protected void ReportError(string message, StringContext context)
        {
            Report(message, DiagnosticSeverity.Error, context);
        }

        protected void Report(string message, DiagnosticSeverity severity, StringContext context)
        {
            diagnostics.Add(new ConfigDiagnostic(message, severity, context));
        }

        public virtual void Dispose()
        {
            
        }
    }
}