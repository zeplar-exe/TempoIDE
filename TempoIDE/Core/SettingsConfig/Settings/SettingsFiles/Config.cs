using System;
using System.Collections.Generic;
using System.IO;
using Jammo.ParserTools;
using TempoIDE.Core.Helpers;
using TempoIDE.Core.Interfaces;
using TempoIDE.Core.SettingsConfig.Internal.Parser;

namespace TempoIDE.Core.SettingsConfig.Settings.SettingsFiles
{
    public abstract class Config : IDisposable, IParseWriteStream
    {
        protected Stream Stream { get; }

        private readonly List<ConfigDiagnostic> diagnostics = new();
        
        public IEnumerable<ConfigDiagnostic> Diagnostics => diagnostics.AsReadOnly();

        public string FilePath
        {
            get
            {
                if (Stream is FileStream fileStream)
                    return fileStream.Name;

                return string.Empty;
            }
        }

        public bool IsInitialized => Stream?.CanRead ?? false;
        
        public SettingsDocument Document => new SettingsParser(Stream).Parse();

        protected Config(FileInfo file)
        {
            Stream = file.OpenOrCreate(FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
        }
        
        protected Config(Stream stream)
        {
            _ = stream ?? throw new ArgumentNullException(nameof(stream));
            
            Stream = stream;
        }

        public abstract void Parse();
        public abstract void Write();

        protected StreamWriter CreateWriter() => new(Stream, leaveOpen: true);

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

        public void Dispose()
        {
            Stream.Dispose();
        }
    }
}