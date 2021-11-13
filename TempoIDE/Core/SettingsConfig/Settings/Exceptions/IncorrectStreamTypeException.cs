using System;
using System.Text;

namespace TempoIDE.Core.SettingsConfig.Settings.Exceptions
{
    public class IncorrectStreamTypeException : Exception
    {
        public IncorrectStreamTypeException(Type type, Type expected) : base(CreateMessage(type, expected)) { }

        private static string CreateMessage(Type type, Type expected)
        {
            var builder = new StringBuilder();

            builder.Append($"Cannot use a stream of type '{type.Name}'");

            if (expected != null)
                builder.Append($", expected '{expected.Name}'");

            return builder.ToString();
        }
    }
}