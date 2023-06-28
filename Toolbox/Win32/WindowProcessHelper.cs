using System.Runtime.InteropServices;
using static PInvoke.User32;

namespace Toolbox;

internal static class WindowProcessHelper
{
    public delegate IntPtr WndProcDelegate(IntPtr hWnd, WindowMessage message, IntPtr wParam, IntPtr lParam);

    // Make sure to hold a reference to the delegate so it doesn't get garbage
    // collected, or you'll get baffling ExecutionEngineExceptions when
    // Windows tries to call your function pointer which no longer points
    // to anything.
    private static WndProcDelegate _currDelegate = null!;

    public static IntPtr SetWndProc(IntPtr windowHandle, WndProcDelegate newProc)
    {
        _currDelegate = newProc;

        var newWndProcPtr = Marshal.GetFunctionPointerForDelegate(newProc);

        return SetWindowLongPtr(windowHandle, (int)WindowLongIndexFlags.GWL_WNDPROC, newWndProcPtr);
    }


    [DllImport("user32.dll")]
    private static extern IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);
}
