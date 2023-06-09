using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace MvvmFramework.WinUI;

public class BoolVisibilityConverter : IValueConverter
{
    public Visibility IsTrue { get; set; } = Visibility.Visible;
    public Visibility IsFalse { get; set; } = Visibility.Collapsed;
    public Visibility IsNull { get; set; } = Visibility.Collapsed;

    public object Convert(object value, Type targetType, object parameter, string language)
    {
        return value switch
        {
            null => IsNull,
            bool b => b ? IsTrue : IsFalse,
            _ => value,
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        return value;
    }
}
