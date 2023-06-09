namespace MvvmFramework;

public class RelayCommand : BaseCommand
{
	private readonly Action _execute;

	public RelayCommand(Action execute) => _execute = execute;

	public override void Execute() => _execute?.Invoke();
}

public class RelayCommand<TParameter> : BaseCommand<TParameter>
{
	private readonly Action<TParameter> _execute;

	public RelayCommand(Action<TParameter> execute) => _execute = execute;

	public override void Execute(TParameter parameter) => _execute?.Invoke(parameter);
}
