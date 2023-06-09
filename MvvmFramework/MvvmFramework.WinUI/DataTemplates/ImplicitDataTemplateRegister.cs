using Microsoft.UI.Xaml;

namespace MvvmFramework.WinUI;

public class ImplicitDataTemplateRegister
{
    private static readonly Dictionary<Type, Func<object, FrameworkElement>> _dataTemplates = new();

    private ImplicitDataTemplateRegister() { }

    public static ImplicitDataTemplateRegister Register<TDataType>(Func<TDataType, FrameworkElement> elementFactory)
    {
        _dataTemplates[typeof(TDataType)] = vm => elementFactory((TDataType)vm);

        return new ImplicitDataTemplateRegister();
    }

    public static Func<object, FrameworkElement>? TryFind(Type dataType)
    {
        return _dataTemplates.ContainsKey(dataType)
            ? _dataTemplates[dataType]
            : default;
    }
}
