using Microsoft.UI.Xaml.Markup;

namespace MvvmFramework.WinUI;

[MarkupExtensionReturnType(ReturnType = typeof(double))]
public sealed class Double : MarkupExtension
{
    public string Value { get; set; }

    protected override object ProvideValue()
    {
        if(double.TryParse(Value, out var value))
        {
            return value;
        }

        return default;
    }
}