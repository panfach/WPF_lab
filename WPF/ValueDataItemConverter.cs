using System;
using System.Windows.Data;
using System.Globalization;
using ViewModel;

namespace WPF
{
    class ValueDataItemConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DataConvertion.ValueDataItem(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
