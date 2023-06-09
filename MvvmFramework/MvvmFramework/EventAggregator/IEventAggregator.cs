namespace MvvmFramework;

public interface IEventAggregator
{
	void Subscribe<TEvent>(object recipient, Action<TEvent> callback) where TEvent : IEvent;
	void Subscribe<TEvent>(object recipient, Func<TEvent, Task> callback) where TEvent : IEvent;
	void Subscribe<TEvent>(object recipient, object token, Action<TEvent> callback) where TEvent : IEvent;
	void Unsubscribe<TEvent>(object recipient);
	void Notify<TEvent>(TEvent message) where TEvent : IEvent;
	void Notify<TEvent>(TEvent message, object token) where TEvent : IEvent;
}
