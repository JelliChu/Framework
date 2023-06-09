using Microsoft.UI.Xaml.Data;

namespace MvvmFramework.WinUI;

public class DebugConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        return value;
    }
}