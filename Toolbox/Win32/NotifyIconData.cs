using System.Runtime.InteropServices;

namespace Toolbox;

[StructLayout(LayoutKind.Sequential)]
public struct NotifyIconData
{
    public int cbSize; // DWORD
    public IntPtr hWnd; // HWND
    public int uID; // UINT
    public NotifyIconFlags uFlags; // UINT
    public int uCallbackMessage; // UINT
    public IntPtr hIcon; // HICON
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
    public string szTip; // char[128]
    public int dwState; // DWORD
    public int dwStateMask; // DWORD
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string szInfo; // char[256]
    public int uTimeoutOrVersion; // UINT
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
    public string szInfoTitle; // char[64]
    public int dwInfoFlags; // DWORD
    //GUID guidItem; > IE 6
}