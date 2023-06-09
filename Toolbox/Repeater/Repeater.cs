namespace Toolbox;

public class Repeater : IDisposable
{
    private readonly Action _action;
    private readonly PeriodicTimer _timer;
    private CancellationTokenSource? _cancellationTokenSource;
    private bool _isRunning;

    public Repeater(Action action, TimeSpan interval)
    {
        _action = action;
        _timer = new(interval);
    }

    public bool IsRunning => _isRunning;

    public void Start(CancellationToken? cancellationToken = default)
    {
        _cancellationTokenSource =
            cancellationToken is null
            ? new()
            : CancellationTokenSource.CreateLinkedTokenSource(cancellationToken.Value);

        _isRunning = true;
        InvokeAction().ConfigureAwait(false);
    }

    public void Stop()
    {
        _cancellationTokenSource?.Cancel();
        //_cancellationTokenSource?.Dispose();

        _isRunning = false;
    }

    private async Task InvokeAction()
    {
        try
        {
            // check IsRunning pre and post wait
            while(IsRunningInternal() && await _timer.WaitForNextTickAsync(_cancellationTokenSource!.Token) && IsRunningInternal())
            {
                _action();
            }
        }
        catch(OperationCanceledException) { }
    }

    private bool IsRunningInternal()
    {
        return _isRunning
            && _cancellationTokenSource is not null
            && !_cancellationTokenSource.IsCancellationRequested
            && _timer is not null;
    }

    public void Dispose()
    {
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();
        _cancellationTokenSource = null;

        _timer.Dispose();

        GC.SuppressFinalize(this);
    }
}
