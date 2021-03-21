using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Data;
using System.Globalization;
using ClassLibrary;

namespace WPF
{
    class CoordDataItemConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DataItem)
                return $"X: {((DataItem)value).Coord.X.ToString("F2")}  Y: {((DataItem)value).Coord.Y.ToString("F2")}";
            else
                return " ";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
