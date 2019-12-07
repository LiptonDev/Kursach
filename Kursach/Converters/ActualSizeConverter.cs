using System;
using System.Globalization;
using System.Windows.Data;

namespace Kursach.Converters
{

    [ValueConversion(typeof(double), typeof(double))]
    class ActualSizeConverter : IValueConverter
    {
        public double Add { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double s = (double)value;

            return s + Add;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
