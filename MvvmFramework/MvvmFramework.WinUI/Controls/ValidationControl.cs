using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Toolbox.Reflection;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reflection;

namespace MvvmFramework.WinUI.Controls;

public class ValidationControl : UserControl
{
	private readonly FrameworkElement _element;
	private readonly INotifyDataErrorInfo _dataEntityClass;
	private readonly string _propertyName;

	private readonly ObservableCollection<string> _errors = new();

	private (DependencyObject, DependencyProperty, long)? _valuePropertyChangedCallbackToken;

	private Border _errorBorder;
	private Panel _errorMessagesPanel;
	private Button _errorMessagesFlyoutButton;
	private Panel _errorMessagesFlyoutPanel;

	public ValidationControl(FrameworkElement element, INotifyDataErrorInfo dataEntityClass, string propertyName)
	{
		_element = element;
		_dataEntityClass = dataEntityClass;
		_propertyName = propertyName;

		Unloaded += ValidationControl_Unloaded;
	}

	public void Initialise()
	{
		var elementParent = (FrameworkElement)_element.Parent;

		var index = RemoveParent(_element);

		_errorBorder = CreateErrorBorder();
		_errorMessagesPanel = CreateErrorMessagesPanel();
		_errorMessagesFlyoutPanel = CreateErrorMessagesFlyoutPanel();
		_errorMessagesFlyoutButton = CreateErrorMessagesFlyoutButton();

		Content = CreateContent();

		AddChild(elementParent, this, index);

		AssignDismissTrigger();

		_errors.CollectionChanged += Errors_CollectionChanged;
		_dataEntityClass.ErrorsChanged += DataEntityClass_ErrorsChanged;
	}


	protected virtual UIElement CreateContent()
	{
		var errorPlacement = AttachedProperties.Validation.GetErrorPlacement(_element);

		if(errorPlacement is AttachedProperties.Validation.ErrorPlacement.Top)
		{
			return new StackPanel
			{
				Children =
				{
					_errorMessagesPanel,
					new Grid
					{
						Children =
						{
							_element,
							_errorBorder,
						}
					},
				},
			};
		}

		if(errorPlacement is AttachedProperties.Validation.ErrorPlacement.Inline)
		{
			return new StackPanel
			{
				Children =
				{
					new Grid
					{
						Children =
						{
							_element,
							_errorBorder,
							_errorMessagesFlyoutButton,
						}
					},
				},
			};
		}

		return new StackPanel
		{
			Children =
			{
				new Grid
				{
					Children =
					{
						_element,
						_errorBorder,
					}
				},
				_errorMessagesPanel,
			},
		};
	}

	protected virtual Border CreateErrorBorder()
	{
		return new()
		{
			IsHitTestVisible = false,
			Visibility = Visibility.Collapsed,

			// Override border CornerRadius to match adorned element.
			CornerRadius = (_element as Control)?.CornerRadius ?? new(0),

			// ErrorPlacement / Template

			BorderBrush = ColorExtensions.CreateSolidColorBrush("#D64040"),
			BorderThickness = new(1),
		};
	}

	protected virtual Panel CreateErrorMessagesPanel()
	{
		return new StackPanel()
		{
			IsHitTestVisible = false,
			Visibility = Visibility.Collapsed,

			Margin = new(5, 5, 0, 0),

			ChildrenTransitions =
			{
				new EntranceThemeTransition
				{
					FromHorizontalOffset = -10,
					FromVerticalOffset = 0,
					IsStaggeringEnabled = true,
				},
			},
		};
	}

	protected virtual Panel CreateErrorMessagesFlyoutPanel()
	{
		return new StackPanel()
		{

		};
	}

	protected virtual Button CreateErrorMessagesFlyoutButton()
	{
		return new()
		{
			Visibility = Visibility.Collapsed,

			//CornerRadius = new(10),

			Content = new FontIcon()
			{
				Glyph = "\uE171",
				FontSize = 11,
				Foreground = ColorExtensions.CreateSolidColorBrush("#D64040"),
				VerticalAlignment = VerticalAlignment.Center,
			},

			Flyout = new Flyout()
			{
				Content = _errorMessagesFlyoutPanel,
			},

			HorizontalAlignment = HorizontalAlignment.Right,
			Margin = new(0, 0, 5, 0),
		};
	}

	protected virtual void AssignDismissTrigger()
	{
		var dismissTrigger = AttachedProperties.Validation.GetDismissTrigger(_element);

		if(dismissTrigger is AttachedProperties.Validation.DismissTrigger.GainFocus)
		{
			_element.GotFocus += Element_GotFocus;
		}
		else if(dismissTrigger is AttachedProperties.Validation.DismissTrigger.LostFocus)
		{
			_element.LostFocus += Element_LostFocus;
		}
		else if(dismissTrigger is AttachedProperties.Validation.DismissTrigger.ValueChanged)
		{
			var valueChangedPropertyName = AttachedProperties.Validation.GetValueChangedPropertyName(_element);
			if(string.IsNullOrWhiteSpace(valueChangedPropertyName))
			{
				throw new InvalidOperationException("ValueChangedPropertyName must be set on element.");
			}

			if(!valueChangedPropertyName.EndsWith("Property"))
			{
				valueChangedPropertyName += "Property";
			}

			if(_element is not DependencyObject dependencyObject)
			{
				throw new InvalidOperationException("Element must be a DependencyObject.");
			}

			var dependencyProperty = _element.TryGetProperty<DependencyProperty>(valueChangedPropertyName, BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Static);
			if(dependencyProperty is null)
			{
				throw new InvalidOperationException($@"'{valueChangedPropertyName}' is an invalid DependencyProperty name. ValueChangedPropertyName must be the name of a DependencyProperty.");
			}

			var token = dependencyObject.RegisterPropertyChangedCallback(dependencyProperty, Element_ValueChanged);

			_valuePropertyChangedCallbackToken = (dependencyObject, dependencyProperty, token);
		}
	}


	private void AddErrorMessage(string error)
	{
		_errors.Add(error);
	}

	private void ClearErrorMessages()
	{
		_errors.Clear();
	}


	private void Element_GotFocus(object sender, RoutedEventArgs e)
	{
		ClearErrorMessages();
	}

	private void Element_LostFocus(object sender, RoutedEventArgs e)
	{
		ClearErrorMessages();
	}

	private void Element_ValueChanged(DependencyObject dependencyObject, DependencyProperty dependencyProperty)
	{
		ClearErrorMessages();
	}

	private void Errors_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
	{
		if(_errors.Count > 0)
		{
			_errorBorder.Visibility = Visibility.Visible;
			_errorMessagesPanel.Visibility = Visibility.Visible;
			_errorMessagesFlyoutButton.Visibility = Visibility.Visible;
		}
		else
		{
			_errorBorder.Visibility = Visibility.Collapsed;
			_errorMessagesPanel.Visibility = Visibility.Collapsed;
			_errorMessagesFlyoutButton.Visibility = Visibility.Collapsed;
		}

		if(e.Action is NotifyCollectionChangedAction.Add)
		{
			e.NewItems
				?.Cast<string>()
				.ToList()
				.ForEach(error =>
				{
					_errorMessagesPanel.Children.Add(new TextBlock
					{
						Text = error,

						FontSize = 12,
						CharacterSpacing = 20,
						Foreground = ColorExtensions.CreateSolidColorBrush("#D64040"),
					});

					_errorMessagesFlyoutPanel.Children.Add(new TextBlock
					{
						Text = error,

						FontSize = 12,
						CharacterSpacing = 20,
						Foreground = ColorExtensions.CreateSolidColorBrush("#D64040"),
					});
				});
		}
		else if(e.Action is NotifyCollectionChangedAction.Remove)
		{
			e.OldItems
				?.Cast<string>()
				.ToList()
				.ForEach(error =>
				{
					var textBlock = _errorMessagesPanel.Children.OfType<TextBlock>().FirstOrDefault(t => t.Text == error);
					if(textBlock != null)
					{
						_errorMessagesPanel.Children.Remove(textBlock);
					}

					var textBlock2 = _errorMessagesFlyoutPanel.Children.OfType<TextBlock>().FirstOrDefault(t => t.Text == error);
					if(textBlock2 != null)
					{
						_errorMessagesFlyoutPanel.Children.Remove(textBlock);
					}
				});
		}
		else if(e.Action is NotifyCollectionChangedAction.Reset)
		{
			_errorMessagesPanel.Children.Clear();
			_errorMessagesFlyoutPanel.Children.Clear();
		}
	}

	private void DataEntityClass_ErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
	{
		if(e.PropertyName != _propertyName) return;

		ClearErrorMessages();

		var dataEntityClass = (INotifyDataErrorInfo)sender!;

		dataEntityClass
			.GetErrors(e.PropertyName)
			.Cast<string>()
			.ToList()
			.ForEach(error => AddErrorMessage(error));
	}

	private void ValidationControl_Unloaded(object sender, RoutedEventArgs e)
	{
		Unloaded -= ValidationControl_Unloaded;

		_element.GotFocus -= Element_GotFocus;
		_element.LostFocus -= Element_LostFocus;

		if(_valuePropertyChangedCallbackToken is not null)
		{
			_valuePropertyChangedCallbackToken.Value.Item1.UnregisterPropertyChangedCallback(
				_valuePropertyChangedCallbackToken.Value.Item2,
				_valuePropertyChangedCallbackToken.Value.Item3);
		}

		_errors.CollectionChanged -= Errors_CollectionChanged;
		_dataEntityClass.ErrorsChanged -= DataEntityClass_ErrorsChanged;
	}


	private int RemoveParent(FrameworkElement child)
	{
		if(child.Parent is Panel panel)
		{
			var index = panel.Children.IndexOf(child);
			panel.Children.RemoveAt(index);

			return index;
		}

		if(child.Parent is ContentControl contentControl)
		{
			contentControl.Content = null;
		}

		return 0;
	}

	private void AddChild(FrameworkElement parent, FrameworkElement child, int index = 0)
	{
		if(parent is Panel panel)
		{
			panel.Children.Insert(index, child);
		}

		if(parent is ContentControl contentControl)
		{
			contentControl.Content = child;
		}
	}
}
