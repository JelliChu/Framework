namespace MvvmFramework.WinUI;

public class SimpleNotification : INotification
{
	private readonly List<ButtonTemplate> _buttons = new();

	public SimpleNotification(string content, TimeSpan duration)
	{
		Id = GetHashCode().ToString();
		Content = content;
		Duration = duration;
	}

	public SimpleNotification(string id, string content, TimeSpan duration)
	{
		Id = id;
		Content = content;
		Duration = duration;
	}

	public string Id { get; }
	public string Content { get; }
	public List<ButtonTemplate> Buttons => _buttons;
	public TimeSpan Duration { get; }


	IEnumerable<ButtonTemplate> INotification.Buttons => _buttons;
}