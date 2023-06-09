using System.Windows.Input;

namespace MvvmFramework;

public abstract class BaseCommand : ICommand
{
	event EventHandler? ICommand.CanExecuteChanged { add { } remove { } }

	bool ICommand.CanExecute(object parameter) => true;

	void ICommand.Execute(object parameter) => Execute();

	public abstract void Execute();
}

public abstract class BaseCommand<TParameter> : ICommand
{
    event EventHandler? ICommand.CanExecuteChanged { add { } remove { } }

    bool ICommand.CanExecute(object parameter) => true;

    void ICommand.Execute(object parameter) => Execute((TParameter)parameter);

    public abstract void Execute(TParameter parameter);
}
