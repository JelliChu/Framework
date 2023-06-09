namespace MvvmFramework;

public class AsyncEventCallback<TEvent> : IEventCallback where TEvent : IEvent
{
	public AsyncEventCallback(object recipient, Func<TEvent, Task> callback)
	{
		Recipient = recipient;
		Callback = callback;
	}

	public AsyncEventCallback(object recipient, object token, Func<TEvent, Task> callback)
	{
		Recipient = recipient;
		Token = token;
		Callback = callback;
	}

	public object Recipient { get; init; }
	public object? Token { get; init; }
	public Func<TEvent, Task> Callback { get; init; }

	public async Task Invoke(TEvent @event) => await Callback.Invoke(@event);

	Task IEventCallback.Invoke(IEvent @event) => Invoke((TEvent)@event);
}