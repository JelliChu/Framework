﻿using Microsoft.UI.Xaml;

namespace XamlForms.WinUI.MultiBinding;

public static class MultiBindingHelper
{
    private static readonly IDictionary<MultiBindingTargetInfo, MultiBinding> MultiBindings;

    public static readonly DependencyProperty MultiBindingsProperty = DependencyProperty.RegisterAttached("MultiBindings", typeof(MultiBindingCollection), typeof(MultiBindingHelper), new PropertyMetadata(default(MultiBindingCollection), OnMultiBindingsChanged));

    static MultiBindingHelper()
    {
        MultiBindings = new Dictionary<MultiBindingTargetInfo, MultiBinding>(new MultiBindingTargetInfoEqualityComparer());
    }

    public static MultiBindingCollection GetMultiBindings(DependencyObject element)
    {
        return (MultiBindingCollection)element.GetValue(MultiBindingsProperty);
    }

    public static void SetMultiBindings(DependencyObject element, MultiBindingCollection value)
    {
        element.SetValue(MultiBindingsProperty, value);
    }

    internal static MultiBinding GetMultiBindingFor(FrameworkElement frameworkElement, DependencyProperty dependencyProperty)
    {
        var dependencyPropertyInfo = new MultiBindingTargetInfo(frameworkElement, dependencyProperty);

        return MultiBindings.ContainsKey(dependencyPropertyInfo) ? MultiBindings[dependencyPropertyInfo] : null;
    }

    private static void OnMultiBindingsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var associatedObject = (FrameworkElement)d;
        var multiBindings = (MultiBindingCollection)e.NewValue;

        multiBindings.ForEach(multiBinding =>
        {
            var targetPropertyName = multiBinding.TargetProperty;

            var dependencyPropertyOwnerType = multiBinding.IsBindingToAttachedProperty
                ? multiBinding.AttachedPropertyOwnerTypeProvider.GetType()
                : associatedObject.GetType();

            var targetDependencyProperty = 
                dependencyPropertyOwnerType.ExtractDependencyProperty(targetPropertyName)
                ?? throw new InvalidOperationException($"{targetPropertyName} is not a DependencyProperty.");

            var multiBindingTargetInfo = new MultiBindingTargetInfo(associatedObject, targetDependencyProperty);

            SaveMultibindingInfoForFutureAccess(multiBindingTargetInfo, multiBinding);
            multiBinding.OnAttached(associatedObject);
        });
    }

    private static void SaveMultibindingInfoForFutureAccess(MultiBindingTargetInfo multiBindingTargetInfo, MultiBinding multiBinding)
    {
        var frameworkElement = multiBindingTargetInfo.FrameworkElement;

        RoutedEventHandler unloadedEventHandler = null;

        unloadedEventHandler += (sender, args) =>
        {
            frameworkElement.Unloaded -= unloadedEventHandler;

            MultiBindings.Remove(multiBindingTargetInfo);
        };

        frameworkElement.Unloaded += unloadedEventHandler;

        MultiBindings[multiBindingTargetInfo] = multiBinding;
    }

    private class MultiBindingTargetInfo
    {
        public MultiBindingTargetInfo(FrameworkElement frameworkElement, DependencyProperty dependencyProperty)
        {
            FrameworkElement = frameworkElement;
            DependencyProperty = dependencyProperty;
        }

        public FrameworkElement FrameworkElement { get; }
        public DependencyProperty DependencyProperty { get; }
    }

    private class MultiBindingTargetInfoEqualityComparer : IEqualityComparer<MultiBindingTargetInfo>
    {
        public bool Equals(MultiBindingTargetInfo x, MultiBindingTargetInfo y)
            => x.FrameworkElement.Equals(y.FrameworkElement) && x.DependencyProperty.Equals(y.DependencyProperty);

        public int GetHashCode(MultiBindingTargetInfo obj)
            => obj.FrameworkElement.GetHashCode() ^ obj.DependencyProperty.GetHashCode();
    }
}
