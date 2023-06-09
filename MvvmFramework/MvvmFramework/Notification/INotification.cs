namespace MvvmFramework;

public interface INotification
{
	string Id { get; }
	string Content { get; }
	TimeSpan Duration { get; }
	public IEnumerable<ButtonTemplate> Buttons { get; }
}
