using Microsoft.UI.Xaml.Media;

namespace MvvmFramework.WinUI;

public static class ColorExtensions
{
	public static Windows.UI.Color ToWindowsColor(this System.Drawing.Color color)
	{
		return new Windows.UI.Color
		{
			A = color.A,
			R = color.R,
			G = color.G,
			B = color.B,
		};
	}

	public static SolidColorBrush CreateSolidColorBrush(string hex)
	{
		var (r, g, b) = HexToRgb(hex);
		return new SolidColorBrush(Windows.UI.Color.FromArgb(255, r, g, b));
	}

	public static (byte r, byte g, byte b) HexToRgb(string hex)
	{
		hex = hex.Replace("#", string.Empty);
		var r = (byte)Convert.ToUInt32(hex.Substring(0, 2), 16);
		var g = (byte)Convert.ToUInt32(hex.Substring(2, 2), 16);
		var b = (byte)Convert.ToUInt32(hex.Substring(4, 2), 16);

		return (r, g, b);
	}

    public static Windows.UI.Color HexToColor(string hex)
    {
		var (r, g, b) = HexToRgb(hex);

		return Windows.UI.Color.FromArgb(255, r, g, b);
    }
}
