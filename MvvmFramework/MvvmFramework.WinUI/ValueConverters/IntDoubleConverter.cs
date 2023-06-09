using Microsoft.UI.Xaml.Data;

namespace MvvmFramework.WinUI;

public class IntDoubleConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, string language)
	{
		if(value is null or int.MinValue)
		{
			if(targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(Nullable<>))
			{
				return null;
			}

			return double.NaN;
		}

		if(value is int @int)
		{
			if(targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(Nullable<>))
			{
				return (double?)@int;
			}

			return (double)@int;
		}

		return value;
	}

	public object ConvertBack(object value, Type targetType, object parameter, string language)
	{
		if(value is null or double.NaN)
		{
			if(targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(Nullable<>))
			{
				return null;
			}

			return int.MinValue;
		}

		if(value is double @double)
		{
			if(targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(Nullable<>))
			{
				return (int?)@double;
			}

			return (int)@double;
		}

		return value;
	}
}