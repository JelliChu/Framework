using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Toolbox.Reflection;
using MvvmFramework.WinUI.Controls;
using System.ComponentModel;

namespace MvvmFramework.WinUI.AttachedProperties;

public static class Validation
{
    #region Validate
    public static string GetValidate(DependencyObject obj) => (string)obj.GetValue(ValidateProperty);
    public static void SetValidate(DependencyObject obj, string value) => obj.SetValue(ValidateProperty, value);

    public static readonly DependencyProperty ValidateProperty = DependencyPropertyHelper.AutoRegisterAttached("Validate", new(default(string), OnValidateChanged));

	private static void OnValidateChanged(object sender, DependencyPropertyChangedEventArgs args)
	{
		var element = (FrameworkElement)sender;

		if(args.NewValue is not string propertyName) return;

		element.WhenLoaded(() =>
		{
			var page = FrameworkElementExtensions.FindLogicalAncestor<Page>(element);

			var dataEntityClass = GetDataEntityClass(page)
				?? page as INotifyDataErrorInfo
				?? page.TryGetProperty<INotifyDataErrorInfo>("ViewModel")
				?? throw new Exception("DataEntityClass needs to be specified.");

			var validationControl = new ValidationControl(element, dataEntityClass, propertyName);
			validationControl.Initialise();
		});
	}
    #endregion


    #region DataEntityClass
    public static INotifyDataErrorInfo GetDataEntityClass(DependencyObject obj) => (INotifyDataErrorInfo)obj.GetValue(DataEntityClassProperty);
    public static void SetDataEntityClass(DependencyObject obj, INotifyDataErrorInfo value) => obj.SetValue(DataEntityClassProperty, value);

    public static readonly DependencyProperty DataEntityClassProperty = DependencyPropertyHelper.AutoRegisterAttached("DataEntityClass", new(default(INotifyDataErrorInfo)));
    #endregion


    #region ErrorPlacement
    public static ErrorPlacement GetErrorPlacement(DependencyObject obj) => (ErrorPlacement)obj.GetValue(ErrorPlacementProperty);
    public static void SetErrorPlacement(DependencyObject obj, ErrorPlacement value) => obj.SetValue(ErrorPlacementProperty, value);

    public static readonly DependencyProperty ErrorPlacementProperty = DependencyPropertyHelper.AutoRegisterAttached("ErrorPlacement", new(default(ErrorPlacement)));
    #endregion


    #region DismissTrigger
    public static DismissTrigger GetDismissTrigger(DependencyObject obj) => (DismissTrigger)obj.GetValue(DismissTriggerProperty);
    public static void SetDismissTrigger(DependencyObject obj, DismissTrigger value) => obj.SetValue(DismissTriggerProperty, value);

    public static readonly DependencyProperty DismissTriggerProperty = DependencyPropertyHelper.AutoRegisterAttached("DismissTrigger", new PropertyMetadata(default(DismissTrigger)));
    #endregion


    #region ValueChangedPropertyName
    public static string GetValueChangedPropertyName(DependencyObject obj) => (string)obj.GetValue(ValueChangedPropertyNameProperty);
    public static void SetValueChangedPropertyName(DependencyObject obj, string value) => obj.SetValue(ValueChangedPropertyNameProperty, value);

    public static readonly DependencyProperty ValueChangedPropertyNameProperty = DependencyPropertyHelper.AutoRegisterAttached("ValueChangedPropertyName", new PropertyMetadata(default(string)));
	#endregion


	public enum ErrorPlacement { Bottom, Top, Inline }

	public enum DismissTrigger { LostFocus, GainFocus, ValueChanged, }
}
