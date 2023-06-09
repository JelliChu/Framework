namespace Toolbox;

public interface INetworkStatus
{
    Task<bool> CanConnect();
    Task<IEnumerable<HostInfo>> GetHostInfo();
}

