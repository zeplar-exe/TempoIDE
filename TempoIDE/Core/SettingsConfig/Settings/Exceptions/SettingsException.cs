using System;

namespace TempoIDE.Core.SettingsConfig.Settings.Exceptions
{
    public class SettingsException : Exception
    {
        public SettingsException(string message) : base(message) { }
    }
}