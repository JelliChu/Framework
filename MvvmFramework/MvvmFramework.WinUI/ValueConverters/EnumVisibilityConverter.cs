using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace MvvmFramework.WinUI;

public class EnumVisibilityConverter : IValueConverter
{
    public Visibility IsTrue { get; set; } = Visibility.Visible;
    public Visibility IsFalse { get; set; } = Visibility.Collapsed;

    public object Convert(object value, Type targetType, object parameter, string language)
    {
		return value == parameter ? IsTrue : IsFalse;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        return value;
    }
}
