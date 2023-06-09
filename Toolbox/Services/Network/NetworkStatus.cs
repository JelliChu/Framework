using System.Net.NetworkInformation;

namespace Toolbox;

public class NetworkStatus : INetworkStatus
{
    private readonly HashSet<string> _hosts = new();

    public NetworkStatus() { }
    public NetworkStatus(HashSet<string> hosts) => _hosts = hosts;

    public async Task<bool> CanConnect() => (await GetHostInfo()).All(h => h.CanConnect);

    public async Task<IEnumerable<HostInfo>> GetHostInfo()
    {
        var hostInfo = new List<HostInfo>();

        foreach(var host in _hosts)
        {
            hostInfo.Add(new(host, await CanConnect(host)));
        }

        return hostInfo;
    }

    private static async Task<bool> CanConnect(string host)
    {
        if(host.StartsWith("localhost:"))
        {
            var port = int.Parse(host.Replace("localhost:", ""));
            return
                IPGlobalProperties
                .GetIPGlobalProperties()
                .GetActiveTcpListeners()
                .Any(ep => ep.Port == port);
        }

        using var ping = new Ping();
        var reply = await ping.SendPingAsync(host);
        return reply.Status is IPStatus.Success;
    }
}
