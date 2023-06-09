using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using static CSharpMarkup.WinUI.Helpers;

namespace MvvmFramework.WinUI;

public class ImplicitDataTemplateSelector : DataTemplateSelector
{
    protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
    {
        if(item is null) return base.SelectTemplateCore(item, container);

        var type = item.GetType();

        var elementFactory = ImplicitDataTemplateRegister.TryFind(type);
        if(elementFactory is null) return base.SelectTemplateCore(item, container);

        var element = elementFactory(item);

        var dataTemplate = DataTemplate(() =>
        {
            var grid = Grid();
            ((Grid)grid).Children.Add(element);
            return grid;
        });

        return dataTemplate;
    }
}
