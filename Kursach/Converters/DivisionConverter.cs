using System;
using System.Globalization;
using System.Windows.Data;

namespace ISTraining_Part.Converters
{
    /// <summary>
    /// Конвертер подразделения.
    /// </summary>
    class DivisionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return $"{(int)value + 1} подразделение";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
