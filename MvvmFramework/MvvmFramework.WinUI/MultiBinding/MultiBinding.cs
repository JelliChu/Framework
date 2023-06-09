using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Markup;
using System.Reflection;
using Toolbox;
using System.Linq;

namespace XamlForms.WinUI.MultiBinding;

[ContentProperty(Name = nameof(Bindings))]
public class MultiBinding : BindingBase
{
    private const string TargetPropertyValuePropertyName = "TargetPropertyValue";


    private static readonly DependencyProperty TargetPropertyValueProperty = DependencyProperty.Register(TargetPropertyValuePropertyName, typeof(object), typeof(MultiBinding), new PropertyMetadata(default(object), OnTargetPropertyValueChanged));


    private static readonly DisableablePropertyChangedCallback DisableableTargetPropertyValueChangedCallback;
    private IDependencyPropertyDescriptor _dependencyPropertyDescriptor;
    private FrameworkElement _associatedObject;


    private IReadOnlyList<IMultibindingItem> MultibindingItems => Bindings.OfType<IMultibindingItem>().ToList();

    private IReadOnlyList<IOneWayMultibindingItem> OneWayMultibindingItems => Bindings.OfType<IOneWayMultibindingItem>().ToList();

    private IReadOnlyList<IOneWayToSourceMultibindingItem> OneWayToSourceMultibindingItems => Bindings.OfType<IOneWayToSourceMultibindingItem>().ToList();

    private bool CanUseStringFormat => StringFormat != null && _dependencyPropertyDescriptor.PropertyType == typeof(string);

    private bool CanUseConverter => Converter != null;


    internal bool IsBindingToAttachedProperty => AttachedPropertyOwnerTypeProvider != null;


    public string TargetProperty { get; set; }

    public BindingMode Mode { get; set; }

    public UpdateSourceTrigger UpdateSourceTrigger { get; set; }

    public string StringFormat { get; set; }

    public IMultiValueConverter Converter { get; set; }

    public object ConverterParameter { get; set; }

    public string ConverterLanguage { get; set; }

    public object TargetNullValue { get; set; }

    public object FallbackValue { get; set; }

    public ITypeProvider AttachedPropertyOwnerTypeProvider { get; set; }

    public List<Binding> Bindings { get; } = new();


    static MultiBinding()
    {
        DisableableTargetPropertyValueChangedCallback = new DisableablePropertyChangedCallback(NotifyOnTargetPropertyValueChanged);
    }

    public MultiBinding()
    {
        Mode = BindingMode.OneWay;
        UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
    }


    internal void OnAttached(FrameworkElement associatedObject)
    {
        if(_associatedObject != null)
        {
            throw new InvalidOperationException("There is already object associated with this Multibinding.");
        }

        _associatedObject = associatedObject;
        _associatedObject.Loaded += (s, e) => Initialize();
    }

    internal MultiBindingExpression GetExpression()
        => MultiBindingExpression.CreateFrom(this, AssociatedObjectOnTargetPropertyValueChanged);


    private void Initialize()
    {
        _dependencyPropertyDescriptor = GetTargetPropertyDescriptor();

        if(TargetNullValue != null)
        {
            TargetNullValue = ChangeType(TargetNullValue, _dependencyPropertyDescriptor.PropertyType);
        }
        FallbackValue = FallbackValue != null ? ChangeType(FallbackValue, _dependencyPropertyDescriptor.PropertyType) : GetDefaultValueForTargetProperty();

        if(!CanUseStringFormat && !CanUseConverter)
        {
            throw new InvalidOperationException("Unable to attach binding. Please specify StringFormat or Converter.");
        }

        Bindings
            .Where(binding => binding.Mode == default || binding.Mode > Mode)
            .ToList()
            .ForEach(binding => binding.Mode = Mode);
        Bindings.ForEach(binding => binding.UpdateSourceTrigger = UpdateSourceTrigger);
        MultibindingItems.ToList().ForEach(item => item.Initialize(_associatedObject));

        switch(Mode)
        {
            case BindingMode.OneTime:
            case BindingMode.OneWay:
                CreateOneWayBinding();
                break;
            case BindingMode.TwoWay:
                CreateOneWayBinding();
                CreateOneWayToSourceBinding();
                break;
        }

        TriggerInitialOneWayBinding();
    }

    private void CreateOneWayBinding()
    {
        OneWayMultibindingItems.ToList().ForEach(item => item.SourcePropertyValueChanged += OneWayMultibindingItemOnSourcePropertyValueChanged);
    }

    private void CreateOneWayToSourceBinding()
    {
        switch(UpdateSourceTrigger)
        {
            case UpdateSourceTrigger.Default:
            case UpdateSourceTrigger.PropertyChanged:
                using(DisableableTargetPropertyValueChangedCallback.Disable())
                {
                    var binding = new Binding { Source = this, Path = new PropertyPath(TargetPropertyValuePropertyName), Mode = BindingMode.TwoWay };
                    BindingOperations.SetBinding(_associatedObject, _dependencyPropertyDescriptor.DependencyProperty, binding);
                }
                break;
            case UpdateSourceTrigger.Explicit:
                break;
            default:
                throw new InvalidOperationException("Unknown UpdateSourceTrigger mode.");
        }
    }

    private void TriggerInitialOneWayBinding()
    {
        OneWayMultibindingItemOnSourcePropertyValueChanged(this, EventArgs.Empty);
    }

    private void OneWayMultibindingItemOnSourcePropertyValueChanged(object sender, EventArgs e)
    {
        if(CanUseStringFormat && CanUseConverter)
        {
            SetTargetPropertyValue(FallbackValue);
        }
        else
        {
            var values = OneWayMultibindingItems.Select(item => item.SourcePropertyValue).ToArray();
            if(CanUseStringFormat)
            {
                SetTargetPropertyValueUsingStringFormat(values);
            }
            else
            {
                SetTargetPropertyValueUsingConverter(values);
            }
        }
    }

    private void SetTargetPropertyValueUsingStringFormat(object[] args)
    {
        var formattedValue = string.Format(StringFormat, args);
        SetTargetPropertyValue(formattedValue);
    }

    private void SetTargetPropertyValueUsingConverter(object[] values)
    {
        var convertedValue = Converter.Convert(values, _dependencyPropertyDescriptor.PropertyType, ConverterParameter, ConverterLanguage);
        if(convertedValue == null)
        {
            convertedValue = TargetNullValue ?? FallbackValue;
        }
        else if(convertedValue == DependencyProperty.UnsetValue)
        {
            convertedValue = FallbackValue;
        }
        else
        {
            convertedValue = ChangeType(convertedValue, _dependencyPropertyDescriptor.PropertyType);
        }
        SetTargetPropertyValue(convertedValue);
    }

    private void SetTargetPropertyValue(object targetPropertyValue)
    {
        using(DisableableTargetPropertyValueChangedCallback.Disable())
        {
            _dependencyPropertyDescriptor.SetValue(_associatedObject, targetPropertyValue);
        }
    }

    private static object ChangeType(object value, Type conversionType)
    {
        var valueType = value.GetType();
        var valueTypeInfo = valueType.GetTypeInfo();
        var isCompatible = valueType == conversionType || valueTypeInfo.IsSubclassOf(conversionType);

        return isCompatible
            ? value
            : (conversionType == typeof(String) ? value.ToString() : Convert.ChangeType(value, conversionType));
    }

    private object GetDefaultValueForTargetProperty()
    {
        var propertyType = _dependencyPropertyDescriptor.PropertyType;
        var propertyTypeInfo = propertyType.GetTypeInfo();

        return propertyTypeInfo.IsValueType
            ? Activator.CreateInstance(propertyType)
            : (propertyType == typeof(string) ? string.Empty : null);
    }

    private static void OnTargetPropertyValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        DisableableTargetPropertyValueChangedCallback.OnPropertyChanged(d, e);
    }

    private static void NotifyOnTargetPropertyValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var multiBinding = (MultiBinding)d;
        multiBinding.AssociatedObjectOnTargetPropertyValueChanged();
    }

    private void AssociatedObjectOnTargetPropertyValueChanged()
    {
        if(!CanUseConverter)
        {
            return;
        }

        var value = _dependencyPropertyDescriptor.GetValue(_associatedObject);
        var targetTypes = OneWayToSourceMultibindingItems.Select(item => item.SourcePropertyType).ToArray();
        var values = Converter.ConvertBack(value, targetTypes, ConverterParameter, ConverterLanguage);
        for(var i  = 0; i < values.Length; i++)
        {
            var v = values[i];
            var item = OneWayToSourceMultibindingItems[i];

            if(item.Mode > BindingMode.OneWay)
            {
                item.OnTargetPropertyValueChanged(v);
            }
        }
    }

    private IDependencyPropertyDescriptor GetTargetPropertyDescriptor()
    {
        return IsBindingToAttachedProperty
            ? new AttachedPropertyDescriptor(AttachedPropertyOwnerTypeProvider.GetType(), TargetProperty)
            : new DependencyPropertyDescriptor(_associatedObject.GetType(), TargetProperty);
    }
}
