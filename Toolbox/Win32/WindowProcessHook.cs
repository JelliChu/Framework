using PInvoke;

namespace Toolbox;

public class WindowProcessHook
{
    private static readonly Dictionary<IntPtr, WindowProcessHook> _windowHandles = new();

    private readonly IntPtr _windowHandle;
    private readonly IntPtr _oldWndProc;
    private readonly List<Action<WindowMessageEventArgs>> _callbacks = new();

    public WindowProcessHook(IntPtr windowHandle)
    {
        _windowHandle = windowHandle;
        _oldWndProc = WindowProcessHelper.SetWndProc(_windowHandle, WndProc);
    }

    public static WindowProcessHook GetOrCreate(IntPtr windowHandle)
    {
        if(!_windowHandles.TryGetValue(windowHandle, out var hook))
        {
            hook = new WindowProcessHook(windowHandle);
            _windowHandles.Add(windowHandle, hook);
        }

        return hook;
    }

    public void AddWindowMessageCallback(Action<WindowMessageEventArgs> callback)
    {
        _callbacks.Add(callback);
    }

    private IntPtr WndProc(IntPtr hWnd, User32.WindowMessage message, IntPtr wParam, IntPtr lParam)
    {
        _callbacks.ForEach(action => action.Invoke(new(hWnd, message, wParam, lParam)));

        return Win32.CallWindowProc(_oldWndProc, hWnd, (uint)(int)message, wParam, lParam);
    }
}
