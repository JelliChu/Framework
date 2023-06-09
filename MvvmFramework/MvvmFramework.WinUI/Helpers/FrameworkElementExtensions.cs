using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Windows.Foundation;

namespace MvvmFramework.WinUI;

public static class FrameworkElementExtensions
{
	public static TAncestorType FindVisualAncestor<TAncestorType>(FrameworkElement frameworkElement) where TAncestorType : FrameworkElement
	{
		var currentItem = frameworkElement;

		var depth = 10;

		for(var i = 0; i < depth; i++)
		{
			var parent = (FrameworkElement)VisualTreeHelper.GetParent(currentItem);

			if(parent is null)
			{
				return null;
			}

			if(parent is TAncestorType ancestorType)
			{
				return ancestorType;
			}

			currentItem = parent;
		}

		return null;
	}

	public static TAncestorType FindLogicalAncestor<TAncestorType>(FrameworkElement frameworkElement) where TAncestorType : FrameworkElement
	{
		var currentItem = frameworkElement;

		var depth = 10;

		for(var i = 0; i < depth; i++)
		{
			var parent = (FrameworkElement)currentItem.Parent;

			if(parent is null)
			{
				return null;
			}

			if(parent is TAncestorType ancestorType)
			{
				return ancestorType;
			}

			currentItem = parent;
		}

		return null;
	}

	public static TChildType FindVisualChild<TChildType>(FrameworkElement frameworkElement, Func<TChildType, bool>? predicate = default) where TChildType : FrameworkElement
	{
		var children = new List<FrameworkElement>();

		var childrenCount = VisualTreeHelper.GetChildrenCount(frameworkElement);

		for(var childIndex = 0; childIndex < childrenCount; childIndex++)
		{
			var child = (FrameworkElement)VisualTreeHelper.GetChild(frameworkElement, childIndex);

			if(child is TChildType foundChild)
			{
				if(predicate is null)
				{
					return foundChild;
				}
				else if(predicate(foundChild))
				{
					return foundChild;
				}
			}

			children.Add(child);
		}

		return
			children
			.Select(i => FindVisualChild(i, predicate))
			.Where(i => i is not null)
			.FirstOrDefault();
	}

	public static IEnumerable<TChildType> FindVisualChildren<TChildType>(FrameworkElement frameworkElement) where TChildType : FrameworkElement
	{
		var children = new List<FrameworkElement>();

		var childrenCount = VisualTreeHelper.GetChildrenCount(frameworkElement);

		for(var childIndex = 0; childIndex < childrenCount; childIndex++)
		{
			var child = (FrameworkElement)VisualTreeHelper.GetChild(frameworkElement, childIndex);

			//if(child is TChildType foundChild)
			//{
			//    return foundChild;
			//}

			children.Add(child);
		}

		return children.Select(i => FindVisualChild<TChildType>(i)).ToList();
	}

	public static IEnumerable<FrameworkElement> FindChildren(FrameworkElement frameworkElement)
	{
		var children = new List<FrameworkElement>();

		var childrenCount = VisualTreeHelper.GetChildrenCount(frameworkElement);

		for(var childIndex = 0; childIndex < childrenCount; childIndex++)
		{
			var child = (FrameworkElement)VisualTreeHelper.GetChild(frameworkElement, childIndex);

			children.Add(child);
		}

		return children;
	}

	public static bool IsVisibleToUser<TFrameworkElement>(this TFrameworkElement frameworkElement, FrameworkElement? container = default) where TFrameworkElement : FrameworkElement
	{
		if(frameworkElement.Visibility == Visibility.Collapsed)
		{
			return false;
		}

		if(container is null)
		{
			container = (FrameworkElement)frameworkElement.XamlRoot.Content;
		}

		var elementTransform = frameworkElement.TransformToVisual(container);
		var elementBounds = elementTransform.TransformBounds(new Rect(0.0, 0.0, frameworkElement.ActualWidth, frameworkElement.ActualHeight));

		var isVisible =
			VisualTreeHelper
			.FindElementsInHostCoordinates(elementBounds, container)
			.OfType<TFrameworkElement>()
			.Any(i => i == frameworkElement);

		return isVisible;
	}

	/// <summary>
	/// If the framework element is loaded, the action will run immediately.
	/// Otherwise, the action will run when the framework element loads.
	/// </summary>
	/// <param name="frameworkElement"></param>
	/// <param name="action"></param>
	public static void WhenLoaded(this FrameworkElement frameworkElement, Action action)
	{
		if(frameworkElement.IsLoaded)
		{
			action?.Invoke();
			return;
		}

		frameworkElement.Loaded += FrameworkElement_Loaded;
		void FrameworkElement_Loaded(object sender, RoutedEventArgs e)
		{
			frameworkElement.Loaded -= FrameworkElement_Loaded;

			action?.Invoke();
		}
	}
}
