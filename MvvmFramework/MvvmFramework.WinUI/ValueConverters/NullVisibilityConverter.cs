using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using Toolbox;

namespace MvvmFramework.WinUI;

public class NullVisibilityConverter : IValueConverter
{
	public Visibility IsNull { get; set; } = Visibility.Collapsed;
	public Visibility IsNotNull { get; set; } = Visibility.Visible;

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
