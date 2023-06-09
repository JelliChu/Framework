using Microsoft.UI.Xaml;

namespace MvvmFramework.WinUI.AttachedProperties;

public static class Opacity
{
    #region CollapseOnOpacityValue
    public static int GetOpacityCollapseThreshold(DependencyObject obj) => (int)obj.GetValue(OpacityCollapseThresholdProperty);
    public static void SetOpacityCollapseThreshold(DependencyObject obj, int value) => obj.SetValue(OpacityCollapseThresholdProperty, value);

    public static readonly DependencyProperty OpacityCollapseThresholdProperty = DependencyPropertyHelper.AutoRegisterAttached("OpacityCollapseThreshold", new(-1, OnOpacityCollapseThresholdChanged));

    private static void OnOpacityCollapseThresholdChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        if(sender is not FrameworkElement element)
        {
            return;
        }

        if(e.NewValue is not int value)
        {
            return;
        }

        if(value < 0)
        {
            return;
        }

        element.RegisterPropertyChangedCallback(UIElement.OpacityProperty, OnOpacityChanged);
    }

    private static void OnOpacityChanged(DependencyObject sender, DependencyProperty property)
    {
        if(sender is not FrameworkElement element)
        {
            return;
        }

        var opacityCollapseThreshold = GetOpacityCollapseThreshold(element);

        element.Visibility = 
            element.Opacity > opacityCollapseThreshold 
            ? Visibility.Visible 
            : Visibility.Collapsed;
    }
    #endregion
}