using System;
using System.Globalization;
using System.Windows.Data;

namespace Kursach.Converters
{
    [ValueConversion(typeof(bool), typeof(string))]
    class IsBudgetConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool bv = (bool)value;

            return bv ? "Бюджетная группа" : "Коммерческая группа";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
