namespace MvvmFramework;

public interface INavigationService
{
	bool GoBack();
	bool GoForward();
	bool Navigate(Type source, object? parameter = null, INavigationOptions? navigationOptions = null);
	bool Navigate<T>(object? parameter = null, INavigationOptions? navigationOptions = null);
}
