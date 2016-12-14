using System;
using System.Globalization;
using Xamarin.Forms;

namespace App2Night.ValueConverter
{
    public class PercentageValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var percentage = (double) value;
            return (percentage*100) + "%";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}