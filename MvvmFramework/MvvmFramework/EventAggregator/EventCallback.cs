namespace MvvmFramework;

public class EventCallback<TEvent> : IEventCallback where TEvent : IEvent
{
	public EventCallback(object recipient, Action<TEvent> callback)
	{
		Recipient = recipient;
		Callback = callback;
	}

	public EventCallback(object recipient, object token, Action<TEvent> callback)
	{
		Recipient = recipient;
		Token = token;
		Callback = callback;
	}

	public object Recipient { get; init; }
	public object? Token { get; init; }
	public Action<TEvent> Callback { get; init; }

	public async Task Invoke(TEvent @event) => Callback.Invoke(@event);

	Task IEventCallback.Invoke(IEvent @event) => Invoke((TEvent)@event);
}
