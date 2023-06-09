using System.Drawing;

namespace MvvmFramework.WinUI;

public class ScreenCaptureService
{
    public Bitmap? CaptureDesktop()
    {
        return CaptureWindow(PInvoke.User32.GetDesktopWindow());
    }

    public Bitmap? CaptureActiveWindow()
    {
        return CaptureWindow(PInvoke.User32.GetForegroundWindow());
    }

    public Bitmap? CaptureWindow(IntPtr handle)
    {
        PInvoke.User32.GetWindowRect(handle, out var rect);
        var bounds = new Rectangle(rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top);
        var result = new Bitmap(bounds.Width, bounds.Height);

        using(var graphics = Graphics.FromImage(result))
        {
            graphics.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size, CopyPixelOperation.SourceCopy);
        }

        return result;
    }
}
