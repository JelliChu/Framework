using System.Runtime.InteropServices;

namespace MvvmFramework.WinUI;

public static class WindowInterop
{
	[DllImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

	[StructLayout(LayoutKind.Sequential)]
	public struct RECT
	{
		public int Left;
		public int Top;
		public int Right;
		public int Bottom;
	}

	public static RECT? GetWindowBounds(IntPtr windowHandle)
	{
		var success = GetWindowRect(windowHandle, out var rect);

		if(!success) return default;

		return rect;
	}


    [DllImport("user32.dll")]
    public static extern IntPtr GetDC(IntPtr hwnd);

    [DllImport("user32.dll")]
    public static extern void ReleaseDC(IntPtr hwnd, IntPtr dc);

    [DllImport("gdi32.dll")]
    public static extern uint GetPixel(IntPtr dc, int x, int y);
}
