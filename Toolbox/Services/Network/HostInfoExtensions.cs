namespace Toolbox;

public static class HostInfoExtensions
{
    public static string ToMessage(this IEnumerable<HostInfo> hostInfo)
    {
        var messages =
            hostInfo
            .Where(h => !h.CanConnect)
            .Select(h => $"Cannot connect to {h.Host}.")
            .ToList();

        return string.Join(Environment.NewLine, messages);
    }
}
