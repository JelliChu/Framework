using Microsoft.UI.Xaml;

namespace XamlForms.WinUI.MultiBinding;

internal interface IDependencyPropertyDescriptor
{
    DependencyProperty DependencyProperty { get; }
    Type PropertyType { get; }

    object GetValue(DependencyObject dependencyObject);
    void SetValue(DependencyObject dependencyObject, object value);
}
