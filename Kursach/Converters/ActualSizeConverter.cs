using System;
using System.Globalization;
using System.Windows.Data;

namespace ISTraining_Part.Converters
{
    /// <summary>
    /// Конвертер актуального размера.
    /// </summary>

    [ValueConversion(typeof(double), typeof(double))]
    class ActualSizeConverter : IValueConverter
    {
        public object Add { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Add == null)
                return ((double)value) / 2;
            return (double)value + System.Convert.ToDouble(Add);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
