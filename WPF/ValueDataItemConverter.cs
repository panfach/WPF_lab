using System;
using System.Windows.Data;
using System.Globalization;
using ClassLibrary;

namespace WPF
{
    class ValueDataItemConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DataItem)
                return $"Value: {((DataItem)value).Value.ToString("F2")}";
            else
                return " ";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
