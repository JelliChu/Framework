using Microsoft.UI.Xaml.Data;
using Toolbox;

namespace MvvmFramework.WinUI;

public class StringFormatConverter : IValueConverter
{
    public string Format { get; set; }

    public object Convert(object value, Type targetType, object parameter, string language)
    {
        var format = parameter is string 
            ? parameter as string 
            : Format;

        if(format.IsNullOrWhitespace())
        {
            return $"{value}";
        }

        return string.Format(format, $"{value}");
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}

