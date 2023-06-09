namespace XamlForms.WinUI.MultiBinding;

internal interface IOneWayToSourceMultibindingItem : IMultibindingItem
{
    Type SourcePropertyType { get; }

    void OnTargetPropertyValueChanged(object newSourcePropertyValue);
}
