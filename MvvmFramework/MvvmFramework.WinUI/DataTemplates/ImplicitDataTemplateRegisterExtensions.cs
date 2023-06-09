using Microsoft.UI.Xaml;

namespace MvvmFramework.WinUI;

public static class ImplicitDataTemplateRegisterExtensions
{
    // For method chaining.
    public static ImplicitDataTemplateRegister Register<TDataType>(this ImplicitDataTemplateRegister implicitDataTemplateRegister, Func<TDataType, FrameworkElement> elementFactory)
    {
        ImplicitDataTemplateRegister.Register(elementFactory);
        return implicitDataTemplateRegister;
    }
}
