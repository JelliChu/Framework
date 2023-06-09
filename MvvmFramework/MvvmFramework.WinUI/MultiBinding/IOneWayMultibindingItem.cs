namespace XamlForms.WinUI.MultiBinding;

internal interface IOneWayMultibindingItem : IMultibindingItem
{
    object SourcePropertyValue { get; }

    event EventHandler SourcePropertyValueChanged;
}
