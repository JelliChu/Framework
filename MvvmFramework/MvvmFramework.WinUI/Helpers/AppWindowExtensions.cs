using Microsoft.UI.Windowing;

namespace MvvmFramework.WinUI;

public static class AppWindowExtensions
{
	public static TPresenter? TryGetPresenter<TPresenter>(this AppWindow appWindow)
		where TPresenter : AppWindowPresenter
		=> appWindow.Presenter as TPresenter;
}
