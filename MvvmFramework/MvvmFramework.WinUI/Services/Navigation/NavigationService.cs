using Microsoft.UI.Xaml.Controls;

namespace MvvmFramework.WinUI;

public class NavigationService : INavigationService
{
	private readonly Frame _frame;

	public NavigationService(Frame frame)
	{
		_frame = frame;
	}

	public void GoBack()
	{
		_frame.GoBack();
	}

	public void GoForward()
	{
		_frame.GoForward();
	}

	public bool Navigate<T>(object? parameter = default, NavigationOptions? navigationOptions = default)
	{
		return Navigate(typeof(T), parameter, navigationOptions);
	}

	public bool Navigate(Type source, object? parameter = default, NavigationOptions? navigationOptions = default)
	{
		if(navigationOptions?.ReverseTransitionAnimation == true)
		{
			// To force reverse transition, add this request to the back stack and navigate back.
			_frame.BackStack.Add(new(source, parameter, navigationOptions));
			_frame.GoBack();

			return true;
		}

		return _frame.Navigate(source, parameter, navigationOptions);
	}

	bool INavigationService.Navigate<T>(object? parameter, INavigationOptions? navigationOptions) => Navigate<T>(parameter, (NavigationOptions?)navigationOptions);

	bool INavigationService.Navigate(Type source, object? parameter, INavigationOptions? navigationOptions) => Navigate(source, parameter, (NavigationOptions?)navigationOptions);
}







//public class MsixUpdateService : IUpdateService
//{
//	public async Task<MsixUpdateInfo> GetUpdateInfoAsync()
//	{
//		var result = await Package.Current.CheckUpdateAvailabilityAsync();

//		var isUpdateAvailable = result.Availability is PackageUpdateAvailability.Required or PackageUpdateAvailability.Available;

//		return new(isUpdateAvailable);
//	}

//	public async Task DownloadUpdateAsync(Action<int>? onProgress = default)
//	{
//		var packageManager = new PackageManager();

//	NOTE: Requires.appinstaller file
//		var appInstallerInfo = Package.Current.GetAppInstallerInfo();
//		var uri = appInstallerInfo.Uri;

//		var deploymentTask =
//			packageManager.RequestAddPackageByAppInstallerFileAsync(
//				uri,
//				AddPackageByAppInstallerOptions.None,
//				packageManager.GetDefaultPackageVolume());

//		deploymentTask.Progress = (task, progress) => onProgress?.Invoke((int)progress.percentage);

//		var result = await deploymentTask;

//		if(result.ExtendedErrorCode is not null)
//		{
//			throw result.ExtendedErrorCode;
//		}


//	}

//	async Task<IUpdateInfo> IUpdateService.GetUpdateInfoAsync() => await GetUpdateInfoAsync();
//}

//public class MsixUpdateInfo : IUpdateInfo
//{
//	public MsixUpdateInfo(bool isUpdateAvailable)
//	{
//		IsUpdateAvailable = isUpdateAvailable;
//	}

//	public bool IsUpdateAvailable { get; }
//}

