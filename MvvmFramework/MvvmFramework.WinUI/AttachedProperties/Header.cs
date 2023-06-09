using Microsoft.UI.Xaml;

namespace MvvmFramework.WinUI.AttachedProperties;

public static class Header
{
    #region Placement
    public static HeaderPlacement GetPlacement(DependencyObject obj) => (HeaderPlacement)obj.GetValue(PlacementProperty);
    public static void SetPlacement(DependencyObject obj, HeaderPlacement value) => obj.SetValue(PlacementProperty, value);

    public static readonly DependencyProperty PlacementProperty = DependencyPropertyHelper.AutoRegisterAttached("Placement", new(HeaderPlacement.Top));
    #endregion


    #region Style
    public static Style GetStyle(DependencyObject obj) => (Style)obj.GetValue(StyleProperty);
    public static void SetStyle(DependencyObject obj, Style value) => obj.SetValue(StyleProperty, value);

    public static readonly DependencyProperty StyleProperty = DependencyPropertyHelper.AutoRegister("Style");
	#endregion
}

public enum HeaderPlacement
{
    Top,
    Bottom,
    Left,
    Right,
}
