using Windows.ApplicationModel;

namespace MvvmFramework.WinUI;

public static class Storage
{
    public static string FromRootPath(params string[] paths)
	{
		try
		{
			return Path.Combine(new string[] { Package.Current.InstalledPath }.Concat(paths).ToArray());
		}
		catch
		{
			return Path.Combine(new string[] { "" }.Concat(paths).ToArray());
		}
	}

    public static string GetDataPath(params string[] paths)
    {
		return FromRootPath(new string[] { "Data" }.Concat(paths).ToArray());
    }
}
