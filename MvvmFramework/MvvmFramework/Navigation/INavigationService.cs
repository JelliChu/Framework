namespace MvvmFramework;

public interface INavigationService
{
	void GoBack();
	void GoForward();
	bool Navigate(Type source, object? parameter = null, INavigationOptions? navigationOptions = null);
	bool Navigate<T>(object? parameter = null, INavigationOptions? navigationOptions = null);
}
