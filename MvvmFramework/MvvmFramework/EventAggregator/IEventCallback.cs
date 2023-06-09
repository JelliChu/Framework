namespace MvvmFramework;

public interface IEventCallback
{
	object Recipient { get; init; }
	object? Token { get; init; }
	Task Invoke(IEvent message);
}
