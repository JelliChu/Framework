using PInvoke;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Toolbox;

public partial class NotifyIconManager
{
    private readonly Dictionary<int, (NotifyIconData, Action<WindowMessageEventArgs>)> _onClickCallbacks = new();
    private readonly IntPtr _windowHandle;
    private readonly WindowProcessHook _windowProcessHook;
    private int _nextCallbackMessage = (int)User32.WindowMessage.WM_APP + 100;

    public NotifyIconManager(IntPtr windowHandle)
    {
        _windowHandle = windowHandle;
        _windowProcessHook = WindowProcessHook.GetOrCreate(_windowHandle);
        _windowProcessHook.AddWindowMessageCallback(WndProc);
    }

    public int AddNotifyIcon(string iconPath, Action<WindowMessageEventArgs> onClick)
    {
        var callbackMessage = _nextCallbackMessage++;

        var notifyIconData = new NotifyIconData
        {
            hWnd = _windowHandle,
            cbSize = Marshal.SizeOf<NotifyIconData>(),
            uFlags = NotifyIconFlags.NIF_ICON | NotifyIconFlags.NIF_MESSAGE,
            hIcon = new Icon(iconPath).Handle,
            uCallbackMessage = callbackMessage,
        };

        _onClickCallbacks.Add(callbackMessage, (notifyIconData, onClick));

        Win32.Shell_NotifyIcon((int)NotifyIconMessage.NIM_ADD, ref notifyIconData);

        return callbackMessage;
    }

    public int AddNotifyIcon(string iconPath, string tooltip, Action<WindowMessageEventArgs> onClick)
    {
        var callbackMessage = _nextCallbackMessage++;

        var notifyIconData = new NotifyIconData
        {
            hWnd = _windowHandle,
            cbSize = Marshal.SizeOf<NotifyIconData>(),
            uFlags = NotifyIconFlags.NIF_ICON | NotifyIconFlags.NIF_MESSAGE | NotifyIconFlags.NIF_TIP,
            szTip = tooltip,
            hIcon = new Icon(iconPath).Handle,
            uCallbackMessage = callbackMessage,
        };

        _onClickCallbacks.Add(callbackMessage, (notifyIconData, onClick));

        Win32.Shell_NotifyIcon((int)NotifyIconMessage.NIM_ADD, ref notifyIconData);

        return callbackMessage;
    }

    public void RemoveNotifyIcon(int id)
    {
        if(!_onClickCallbacks.ContainsKey(id))
        {
            return;
        }

        var data = _onClickCallbacks[id];

        _onClickCallbacks.Remove(id);

        Win32.Shell_NotifyIcon((int)NotifyIconMessage.NIM_DELETE, ref data.Item1);
    }

    private void WndProc(WindowMessageEventArgs args)
    {
        if(_onClickCallbacks.TryGetValue((int)args.WindowMessage, out var value))
        {
            value.Item2.Invoke(args);
        }
    }
}
