using System;
using System.Globalization;
using Xamarin.Forms;

namespace App2Night.ValueConverter
{
    public class DateInFutureConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var date = (DateTime) value;
            return date.Date >= DateTime.Today;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}