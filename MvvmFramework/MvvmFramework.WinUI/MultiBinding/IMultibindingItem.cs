using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace XamlForms.WinUI.MultiBinding;

internal interface IMultibindingItem
{
    BindingMode Mode { get; }

    void Initialize(FrameworkElement targetElement);
}
