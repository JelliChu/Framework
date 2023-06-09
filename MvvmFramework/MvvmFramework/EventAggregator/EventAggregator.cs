namespace MvvmFramework;

public class EventAggregator : IEventAggregator
{
	private readonly Dictionary<Type, List<IEventCallback>> _recipients = new();

	public void Subscribe<TEvent>(object recipient, Action<TEvent> callback) where TEvent : IEvent
	{
		var eventType = typeof(TEvent);
		var record = new EventCallback<TEvent>(recipient, callback);

		if(_recipients.ContainsKey(eventType))
		{
			_recipients[eventType].Add(record);
		}
		else
		{
			_recipients.Add(eventType, new() { record });
		}
	}

	public void Subscribe<TEvent>(object recipient, Func<TEvent, Task> callback) where TEvent : IEvent
	{
		var eventType = typeof(TEvent);
		var record = new AsyncEventCallback<TEvent>(recipient, callback);

		if(_recipients.ContainsKey(eventType))
		{
			_recipients[eventType].Add(record);
		}
		else
		{
			_recipients.Add(eventType, new() { record });
		}
	}

	public void Subscribe<TEvent>(object recipient, object token, Action<TEvent> callback) where TEvent : IEvent
	{
		var eventType = typeof(TEvent);
		var record = new EventCallback<TEvent>(recipient, token, callback);

		if(_recipients.ContainsKey(eventType))
		{
			_recipients[eventType].Add(record);
		}
		else
		{
			_recipients.Add(eventType, new() { record });
		}
	}

	public void Unsubscribe<TEvent>(object recipient)
	{
		var eventType = typeof(TEvent);

		if(_recipients.ContainsKey(eventType))
		{
			_recipients[eventType].RemoveAll(i => i.Recipient == recipient);
		}
	}

	public void Notify<TEvent>(TEvent message) where TEvent : IEvent
	{
		var eventType = typeof(TEvent);

		if(_recipients.ContainsKey(eventType))
		{
			// Don't fire events with tokens
			_recipients[eventType]
				.Where(i => i.Token is null)
				.ToList()
				.ForEach(async i => await i.Invoke(message));
        }
    }

	public void Notify<TEvent>(TEvent message, object token) where TEvent : IEvent
	{
		var eventType = typeof(TEvent);

		if(_recipients.ContainsKey(eventType))
		{
			_recipients[eventType]
				.Where(i => i.Token == token)
				.ToList()
				.ForEach(async i => await i.Invoke(message));
		}
	}
}
