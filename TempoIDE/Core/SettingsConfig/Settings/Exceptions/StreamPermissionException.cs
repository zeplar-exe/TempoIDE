using System;
using System.Text;

namespace TempoIDE.Core.SettingsConfig.Settings.Exceptions
{
    public class StreamPermissionException : Exception
    {
        public StreamPermissionException(StreamPermission permission) : base(CreateMessage(permission))
        {
            
        }

        private static string CreateMessage(StreamPermission permission)
        {
            var builder = new StringBuilder();

            builder.Append("Expected ");

            if (permission.HasFlag(StreamPermission.CanRead))
                builder.Append("read ");

            if (permission.HasFlag(StreamPermission.CanWrite))
                builder.Append("and write ");

            builder.Append("permissions.");

            return builder.ToString();
        }
    }

    [Flags]
    public enum StreamPermission
    {
        CanRead = 1<<1,
        CanWrite = 1<<2,
        CanReadAndWrite = CanRead | CanWrite
    }
}