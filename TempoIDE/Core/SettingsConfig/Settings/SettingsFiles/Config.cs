using System;
using System.Collections.Generic;
using System.IO;
using Jammo.ParserTools;
using TempoIDE.Core.Helpers;
using TempoIDE.Core.Interfaces;
using TempoIDE.Core.SettingsConfig.Internal.Parser;
using TempoIDE.Core.SettingsConfig.Settings.Exceptions;

namespace TempoIDE.Core.SettingsConfig.Settings.SettingsFiles
{
    public abstract class Config : IDisposable, IParseWriteStream
    {
        protected readonly Stream Stream;

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

        public IEnumerable<Setting> EnumerateSettings()
        {
            return new SettingsParser(Stream).ParseSettings();
        }

        protected void ReportUnexpectedSetting(Setting setting)
        {
            ReportWarning($"The setting '{setting.Key}' is never used.", setting.Context);
        }

        protected void ReportIfEmptySetting(Setting setting)
        {
            if (string.IsNullOrEmpty(setting.Value.ToString()))
                ReportError($"The setting {setting.Key} cannot be empty.", setting.Context);
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