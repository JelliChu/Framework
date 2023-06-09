using Windows.Devices.Display;
using Windows.Devices.Enumeration;
using Windows.Graphics;

namespace MvvmFramework.WinUI;

public static class DisplayHelper
{
    public static async Task<MonitorInfo> GetMonitorInfoAsync(string deviceInterfaceId)
    {
        var monitor = await DisplayMonitor.FromInterfaceIdAsync(deviceInterfaceId);

        return new(
            monitor.DeviceId, 
            monitor.DisplayName, 
            monitor.NativeResolutionInRawPixels.Width, 
            monitor.NativeResolutionInRawPixels.Height);
    }

    public static async Task<MonitorInfo> GetMonitorInfoAsync(int index)
    {
        var devices = await DeviceInformation.FindAllAsync(DisplayMonitor.GetDeviceSelector());

        return await GetMonitorInfoAsync(devices[index].Id);
    }

    public static async Task<MonitorInfo> GetPrimaryMonitorInfoAsync()
    {
        return await GetMonitorInfoAsync(0);
    }

    public static async Task<IEnumerable<MonitorInfo>> GetAllMonitorInfoAsync()
    {
        var devices = await DeviceInformation.FindAllAsync(DisplayMonitor.GetDeviceSelector());

        return
            devices
            .Select(async device => await GetMonitorInfoAsync(device.Id))
            .Select(task => task.Result);
    }
}

public class MonitorInfo
{
    public MonitorInfo(string id, string name, int width, int height)
    {
        Id = id;
        Name = name;
        Bounds = new(width, height);
    }

    public string Id { get; set; }
    public string Name { get; set; }
    public SizeInt32 Bounds { get; set; }
}
