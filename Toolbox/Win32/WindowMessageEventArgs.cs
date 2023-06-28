using static PInvoke.User32;

namespace Toolbox;

public class WindowMessageEventArgs : EventArgs
{
    public WindowMessageEventArgs(IntPtr windowHandle, WindowMessage windowMessage, IntPtr wParam, IntPtr lParam)
    {
        WindowHandle = windowHandle;
        WindowMessage = windowMessage;
        WParam = wParam;
        LParam = lParam;
    }

    public IntPtr WindowHandle { get; }
    public WindowMessage WindowMessage { get; }
    public IntPtr WParam { get; }
    public IntPtr LParam { get; }
}
