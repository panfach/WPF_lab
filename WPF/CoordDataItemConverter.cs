﻿using System;
using System.Windows.Data;
using System.Globalization;
using ViewModel;

namespace WPF
{
    class CoordDataItemConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DataConvertion.CoordDataItem(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
