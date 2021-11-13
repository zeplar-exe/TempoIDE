using System;
using System.Collections;
using System.Linq;

namespace TempoIDE.Core.SettingsConfig.Internal.Parser
{
    public static class Truthy
    {
        public static bool IsTruthy(this object value)
        {
            switch (value)
            {
                case ITruthy truthy:
                    return truthy.IsTruthy();
                case IEnumerable enumerable and not string:
                    return enumerable.Cast<object>().Any();
                case bool boolean:
                    return boolean;
                case string @string:
                    return !string.IsNullOrEmpty(@string);
                case int integer:
                    return integer != 0;
                case long @long:
                    return @long != 0;
                case short @short:
                    return @short != 0;
                case uint @uint:
                    return @uint != 0;
                case ulong @ulong:
                    return @ulong != 0;
                case ushort @ushort:
                    return @ushort != 0;
                case byte @byte:
                    return @byte != 0;
                case sbyte @sbyte:
                    return @sbyte != 0;
                case double @double:
                    return Math.Round(@double) != 0;
                case float @float:
                    return Math.Round(@float) != 0;
                case decimal @decimal:
                    return Math.Round(@decimal) != 0;
                default:
                    return value != null;
            }
        }
    }
}