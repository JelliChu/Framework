using Microsoft.UI.Xaml.Media.Animation;

namespace MvvmFramework.WinUI;

public class NavigationOptions : INavigationOptions
{
	public NavigationOptions(NavigationTransitionInfo navigationTransitionInfo)
	{
		NavigationTransitionInfo = navigationTransitionInfo;
	}

	public NavigationTransitionInfo NavigationTransitionInfo { get; set; }
	public bool ReverseTransitionAnimation { get; set; }

	public static implicit operator NavigationOptions(NavigationTransitionInfo navigationTransitionInfo) => new(navigationTransitionInfo);
	public static implicit operator NavigationTransitionInfo?(NavigationOptions? navigationOptions) => navigationOptions?.NavigationTransitionInfo;
}

