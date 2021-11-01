using System;
using System.Globalization;
using System.Windows.Data;

namespace TempoIDE.Core.Wrappers
{
    public abstract class ValueConverter<TSource, TTarget> : IValueConverter
    {
        public Type SourceType => typeof(TSource);
        public Type TargetType => typeof(TTarget);

        public abstract TTarget Convert(TSource value, object parameter, CultureInfo culture);

        public abstract TSource ConvertBack(TTarget value, object parameter, CultureInfo culture);
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert((TSource)value, parameter, culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ConvertBack((TTarget)value, parameter, culture);
        }
    }
}