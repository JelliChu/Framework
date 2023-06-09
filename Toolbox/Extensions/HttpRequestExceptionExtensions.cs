using System.Net.Sockets;

namespace Toolbox;

public static class HttpRequestExceptionExtensions
{
    public static bool FailedToConnect(this HttpRequestException httpRequestException)
    {
        if(httpRequestException.InnerException is SocketException socketException)
        {
            if(socketException.SocketErrorCode is SocketError.ConnectionRefused or SocketError.NotConnected or SocketError.TimedOut)
            {
                return true;
            }
        }

        return false;
    }
}
