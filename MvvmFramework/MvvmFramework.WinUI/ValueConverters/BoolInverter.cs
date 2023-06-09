﻿using Microsoft.UI.Xaml.Data;

namespace MvvmFramework.WinUI;

public class BoolInverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if(value is bool @bool)
        {
            return !@bool;
        }

        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
