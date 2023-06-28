using System.Runtime.InteropServices;

namespace Toolbox;

public static partial class Win32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int X;
        public int Y;
    }

    // TODO: Look into LibraryImport https://github.com/dotnet/runtime/issues/46822

    [DllImport("shell32.dll")]
    public static extern bool Shell_NotifyIcon(uint dwMessage, [In] ref NotifyIconData pnid);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll")]
    public static extern uint CheckMenuItem(IntPtr hmenu, uint uIDCheckItem, uint uCheck);

    // Import the GetCursorPos function from user32.dll
    [DllImport("user32.dll")]
    public static extern bool GetCursorPos(out POINT lpPoint);

    //[DllImport("user32.dll")]
    //public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

    //[DllImport("user32.dll")]
    //public static extern uint GetMenuState(IntPtr hMenu, uint uId, GetMenuStateFlag uFlags);

    //[DllImport("user32.dll", CharSet = CharSet.Auto)]
    //public static extern bool AppendMenu(IntPtr hMenu, MenuItemFlags uFlags, uint uIDNewItem, string lpNewItem);

    //[DllImport("user32.dll")]
    //public static extern uint CheckMenuItem(IntPtr hmenu, uint uIDCheckItem, uint uCheck);

    //[DllImport("shell32.dll", SetLastError = true)]
    //public static extern bool Shell_NotifyIcon(int dwMessage, [In] ref NotifyIconData lpData);

    //[DllImport("user32.dll", CharSet = CharSet.Auto)]
    //public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
}
