using PInvoke;
using System.Runtime.InteropServices;

namespace Toolbox;

public class WindowSystemMenuManager
{
    private readonly Dictionary<int, Action<bool>> _menuIds = new();
    private readonly IntPtr _windowHandle;
    private readonly IntPtr _sysMenuHandle;
    private readonly WindowProcessHook _windowProcessHook;
    private int _nextId = 1001;

    public WindowSystemMenuManager(IntPtr windowHandle)
    {
        _windowHandle = windowHandle;
        _sysMenuHandle = User32.GetSystemMenu(_windowHandle, false);
        _windowProcessHook = WindowProcessHook.GetOrCreate(_windowHandle);
        _windowProcessHook.AddWindowMessageCallback(WndProc);
    }

    public WindowSystemMenuManager AppendMenuItemSeparator()
    {
        User32.AppendMenu(_sysMenuHandle, User32.MenuItemFlags.MF_SEPARATOR, default, null);

        return this;
    }

    public WindowSystemMenuManager AppendMenuItemCheckable(string text, Action<bool> onCheckedChanged)
    {
        var id = _nextId++;

        User32.AppendMenu(_sysMenuHandle, User32.MenuItemFlags.MF_UNCHECKED, id, text);

        _menuIds.Add(id, onCheckedChanged);

        return this;
    }

    private void WndProc(WindowMessageEventArgs args)
    {
        if(args.WindowMessage == User32.WindowMessage.WM_SYSCOMMAND)
        {
            foreach(var element in _menuIds)
            {
                var id = element.Key;
                var callback = element.Value;

                if((int)args.WParam != id)
                {
                    continue;
                }

                var state = (User32.MenuItemState)User32.GetMenuState(_sysMenuHandle, (uint)id, User32.GetMenuStateFlags.MF_BYCOMMAND);
                if(state == User32.MenuItemState.MFS_CHECKED)
                {
                    callback(false);
                    _ = CheckMenuItem(_sysMenuHandle, (uint)id, (uint)User32.MenuItemState.MFS_UNCHECKED);
                }
                else if(state == User32.MenuItemState.MFS_UNCHECKED)
                {
                    callback(true);
                    _ = CheckMenuItem(_sysMenuHandle, (uint)id, (uint)User32.MenuItemState.MFS_CHECKED);
                }
            }
        }
    }


    [DllImport("user32.dll")]
    static extern uint CheckMenuItem(IntPtr hmenu, uint uIDCheckItem, uint uCheck);
}
