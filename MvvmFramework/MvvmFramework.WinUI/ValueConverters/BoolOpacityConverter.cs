using Microsoft.UI.Xaml.Data;

namespace MvvmFramework.WinUI;

public class BoolOpacityConverter : IValueConverter
{
	public double IsTrue { get; set; } = 1;
	public double IsFalse { get; set; } = 0;

	public object Convert(object value, Type targetType, object parameter, string language)
	{
		if(value is bool @bool)
		{
			return @bool ? IsTrue : IsFalse;
		}

		return value;
	}

	public object ConvertBack(object value, Type targetType, object parameter, string language)
	{
		throw new NotImplementedException();
	}
}
