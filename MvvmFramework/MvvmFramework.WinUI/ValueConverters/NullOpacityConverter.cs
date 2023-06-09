using Microsoft.UI.Xaml.Data;
using Toolbox;

namespace MvvmFramework.WinUI;

public class NullOpacityConverter : IValueConverter
{
    public double IsNull { get; set; } = 0;
    public double IsNotNull { get; set; } = 1;

    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if(value is string @string)
        {
            return @string.IsNullOrWhitespace() ? IsNull : IsNotNull;
        }

        return value is null ? IsNull : IsNotNull;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        return value;
    }
}