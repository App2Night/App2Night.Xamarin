using System;
using System.Globalization;
using Xamarin.Forms;

namespace App2Night.Helper.ValueConverter
{
    public class StringNotEmptyConverter: IValueConverter

{
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !string.IsNullOrEmpty(value as string);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
}
}