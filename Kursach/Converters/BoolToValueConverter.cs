using System;
using System.Globalization;
using System.Windows.Data;

namespace ISTraining_Part.Converters
{
    /// <summary>
    /// Конвертер bool -> object.
    /// </summary>
    [ValueConversion(typeof(bool), typeof(object))]
    class BoolToValueConverter : IValueConverter
    {
        public object TrueValue { get; set; }
        public object FalseValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? TrueValue : FalseValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
