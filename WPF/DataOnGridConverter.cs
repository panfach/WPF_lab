using System;
using System.Windows.Data;
using System.Globalization;
using ClassLibrary;

namespace WPF
{
    class DataOnGridConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            V3DataOnGrid item = (V3DataOnGrid)value;

            if (item != null)
                return $"   SizeX: {item.XGrid.Size}   SizeY: {item.YGrid.Size}";
            else 
                return " ";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
