namespace MvvmFramework;

public class AsyncRelayCommand : BaseCommand
{
	private readonly Func<Task> _execute;
	private bool _isExecuting;

	public AsyncRelayCommand(Func<Task> execute)
	{
		_execute = execute;
	}

	public override void Execute() => ExecuteAsync().ConfigureAwait(false);

	public async Task ExecuteAsync()
	{
		if(_isExecuting)
		{
			return;
		}

		_isExecuting = true;

		try
		{
			await _execute().ConfigureAwait(false);
		}
		finally
		{
			_isExecuting = false;
		}
	}
}

public class AsyncRelayCommand<TParameter> : BaseCommand<TParameter>
{
    private readonly Func<TParameter, Task> _execute;
    private bool _isExecuting;

    public AsyncRelayCommand(Func<TParameter, Task> execute) => _execute = execute;

    public override void Execute(TParameter parameter) => ExecuteAsync(parameter).ConfigureAwait(false);

    public async Task ExecuteAsync(TParameter parameter)
    {
        if(_isExecuting)
        {
            return;
        }

        _isExecuting = true;

        try
        {
            await _execute(parameter).ConfigureAwait(false);
        }
        finally
        {
            _isExecuting = false;
        }
    }
}
