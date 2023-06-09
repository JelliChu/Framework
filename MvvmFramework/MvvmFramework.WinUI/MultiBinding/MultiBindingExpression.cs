using Microsoft.UI.Xaml.Data;

namespace XamlForms.WinUI.MultiBinding;

public class MultiBindingExpression
{
    private readonly Action _updateSource;


    public object[] DataItems { get; }

    public MultiBinding ParentMultiBinding { get; }


    private MultiBindingExpression(object[] dataItems, MultiBinding parentMultiBinding, Action updateSource)
    {
        DataItems = dataItems;
        ParentMultiBinding = parentMultiBinding;
        _updateSource = updateSource;
    }


    public void UpdateSource()
    {
        if(ParentMultiBinding.Mode > BindingMode.OneWay)
        {
            _updateSource();
        }
    }


    internal static MultiBindingExpression CreateFrom(MultiBinding parentMultiBinding, Action updateSource)
    {
        var dataItems = parentMultiBinding.Bindings.Select(binding => binding.Source).ToArray();

        return new MultiBindingExpression(dataItems, parentMultiBinding, updateSource);
    }
}
