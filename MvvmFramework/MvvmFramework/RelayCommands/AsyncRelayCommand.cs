namespace MvvmFramework;

public class AsyncRelayCommand : BaseCommand
{
	private readonly Func<Task> _execute;
	private bool _isExecuting;

	public AsyncRelayCommand(Func<Task> execute)
	{
		_execute = execute;
	}

	public override async void Execute() => await ExecuteAsync();

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

	public override async void Execute(TParameter parameter) => await ExecuteAsync(parameter);

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
