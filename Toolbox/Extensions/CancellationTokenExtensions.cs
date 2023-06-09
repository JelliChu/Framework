namespace Toolbox;

public static class CancellationTokenExtensions
{
    public static bool WaitCancellationRequested(this CancellationToken token, TimeSpan timeout)
    {
        return token.WaitHandle.WaitOne(timeout);
    }
}
