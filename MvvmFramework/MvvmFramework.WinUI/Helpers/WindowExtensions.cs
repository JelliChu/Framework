using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using System.Runtime.InteropServices;
using Windows.Foundation;
using Windows.Storage;
using WinRT;
using WinRT.Interop;
using static PInvoke.User32;

namespace MvvmFramework.WinUI;

public static class WindowExtensions
{
	public static IntPtr GetWindowHandle(this Window window)
	{
		return WindowNative.GetWindowHandle(window);
	}

	public static AppWindow GetAppWindow(this Window window)
	{
		var windowHandle = WindowNative.GetWindowHandle(window);
		var windowId = Win32Interop.GetWindowIdFromWindow(windowHandle);
		return AppWindow.GetFromWindowId(windowId);
	}

	public static ApplicationTheme GetRootTheme(this Window window)
	{
		if(window.Content is FrameworkElement element)
		{
			return element.RequestedTheme switch
            {
                ElementTheme.Default => ApplicationTheme.Light,
                ElementTheme.Light => ApplicationTheme.Light,
                ElementTheme.Dark => ApplicationTheme.Dark,
                _ => throw new NotImplementedException(),
            };

			//return (ApplicationTheme)((int)element.RequestedTheme - 1);
		}

		return default;
	}

	public static void SetRootTheme(this Window window, ApplicationTheme theme)
	{
		// TODO: Change colour of system buttons

		if(window.Content is FrameworkElement element)
		{
            element.RequestedTheme = theme switch
            {
                ApplicationTheme.Light => ElementTheme.Light,
                ApplicationTheme.Dark => ElementTheme.Dark,
                _ => ElementTheme.Default,
            };

            //element.RequestedTheme = (ElementTheme)((int)theme + 1);
        }
    }

	public static void RestoreLastWindowSize(this Window window)
	{
		var localSettings = ApplicationData.Current.LocalSettings;

		var bounds = (Rect?)localSettings.Values[$"{window.Title}_WindowBounds"];
		if(bounds is not null)
		{
			window.GetAppWindow().MoveAndResize(new Windows.Graphics.RectInt32
			{
				X = (int)bounds.Value.X,
				Y = (int)bounds.Value.Y,
				Width = (int)bounds.Value.Width,
				Height = (int)bounds.Value.Height,
			});
		}

		window.Closed += Window_Closed;
	}

	private static void Window_Closed(object sender, WindowEventArgs args)
	{
		var window = (Window)sender;
		var localSettings = ApplicationData.Current.LocalSettings;

		var position = window.GetAppWindow().Position;

		var bounds = new Rect
		{
			X = position.X,
			Y = position.Y,
			Width = window.Bounds.Width,
			Height = window.Bounds.Height,
		};

		localSettings.Values[$"{window.Title}_WindowBounds"] = bounds;

		window.Closed -= Window_Closed;
	}

    public static void Activate(this Window window, bool showInTaskbar = true)
    {
        if(!showInTaskbar)
        {
            const int GWL_EX_STYLE = -20;
            const int WS_EX_APPWINDOW = 0x00040000;
            const int WS_EX_TOOLWINDOW = 0x00000080;

            window.SetWindowLong(GWL_EX_STYLE, (window.GetWindowLong(GWL_EX_STYLE) | WS_EX_TOOLWINDOW) & ~WS_EX_APPWINDOW);
        }

        window.Activate();
    }

    public static void SetWindowSize(this Window window, int width, int height, bool keepTopMost = false)
    {
        var hWnd = window.As<IWindowNative>().WindowHandle;

        var dpi = PInvoke.User32.GetDpiForWindow(hWnd);
        float scalingFactor = (float)dpi / 96;
        width = (int)(width * scalingFactor);
        height = (int)(height * scalingFactor);

        var hWndInsertAfter = keepTopMost
                            ? PInvoke.User32.SpecialWindowHandles.HWND_TOPMOST
                            : PInvoke.User32.SpecialWindowHandles.HWND_TOP;

        PInvoke.User32.SetWindowPos(
            hWnd,
            hWndInsertAfter,
            0, 0,
            width, height,
            PInvoke.User32.SetWindowPosFlags.SWP_NOMOVE);
    }

    public static void SetWindowPosition(this Window window, int x, int y, bool keepTopMost = false)
    {
        var hWnd = window.As<IWindowNative>().WindowHandle;

        var hWndInsertAfter = keepTopMost
                            ? PInvoke.User32.SpecialWindowHandles.HWND_TOPMOST
                            : PInvoke.User32.SpecialWindowHandles.HWND_TOP;

        PInvoke.User32.SetWindowPos(
            hWnd,
            hWndInsertAfter,
            x, y,
            0, 0,
            PInvoke.User32.SetWindowPosFlags.SWP_NOSIZE
            );
    }

    public static Rect GetWindowRect(this Window window)
    {
        var hWnd = window.As<IWindowNative>().WindowHandle;

        PInvoke.User32.GetWindowRect(hWnd, out PInvoke.RECT rect);

        return new()
        {
            Height = rect.bottom - rect.top,
            Width = rect.right - rect.left,
            X = rect.left,
            Y = rect.top,
        };
    }

    public static void Maximise(this Window window)
    {
        var hWnd = window.As<IWindowNative>().WindowHandle;
        PInvoke.User32.ShowWindow(hWnd, PInvoke.User32.WindowShowStyle.SW_MAXIMIZE);
    }

    public static void Minimise(this Window window)
    {
        var hWnd = window.As<IWindowNative>().WindowHandle;
        PInvoke.User32.ShowWindow(hWnd, PInvoke.User32.WindowShowStyle.SW_MINIMIZE);
    }

    public static void Restore(this Window window)
    {
        var hWnd = window.As<IWindowNative>().WindowHandle;
        PInvoke.User32.ShowWindow(hWnd, PInvoke.User32.WindowShowStyle.SW_RESTORE);
    }

    public static int GetWindowLong(this Window window, int nIndex)
    {
        var hWnd = window.As<IWindowNative>().WindowHandle;
        return GetWindowLong(hWnd, nIndex);
    }

    public static int SetWindowLong(this Window window, int nIndex, int dwNewLong)
    {
        var hWnd = window.As<IWindowNative>().WindowHandle;
        return SetWindowLong(hWnd, nIndex, dwNewLong);
    }

    public static void SetWindowParent(this Window window, IntPtr parentWindowHandle)
    {
        var windowHandle = window.As<IWindowNative>().WindowHandle;

        PInvoke.User32.SetParent(windowHandle, parentWindowHandle);

        PInvoke.User32.GetWindowRect(parentWindowHandle, out var r);
        var height = r.bottom - r.top;

        window.SetWindowLong(
            (int)WindowLongIndexFlags.GWL_STYLE,
            window.GetWindowLong((int)WindowLongIndexFlags.GWL_STYLE)
            & ~((int)WindowStyles.WS_CAPTION
            | (int)WindowStyles.WS_SIZEFRAME
            | (int)WindowStyles.WS_MINIMIZEBOX
            | (int)WindowStyles.WS_MAXIMIZEBOX
            | (int)WindowStyles.WS_SYSMENU)
            );

        PInvoke.User32.SetWindowPos(windowHandle, IntPtr.Zero, 0, 0, 0, 0, SetWindowPosFlags.SWP_SHOWWINDOW);
    }


    [DllImport("user32.dll", SetLastError = true)]
    static extern int GetWindowLong(IntPtr hWnd, int nIndex);
    [DllImport("user32.dll")]
    static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
}
