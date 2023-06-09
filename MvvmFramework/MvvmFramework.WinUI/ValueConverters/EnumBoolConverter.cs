using Microsoft.UI.Xaml.Data;

namespace MvvmFramework.WinUI;

public class EnumBoolConverter : IValueConverter
{
    public bool IsTrue { get; set; } = true;
    public bool IsFalse { get; set; } = false;

    public object Convert(object value, Type targetType, object parameter, string language)
    {
        return value == parameter ? IsTrue : IsFalse;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        return value;
    }
}