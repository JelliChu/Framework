namespace MvvmFramework;

public class ButtonTemplate
{
	public ButtonTemplate(string content, Action onClick)
	{
		Content = content;
		OnClick = onClick;
	}

	public string Content { get; }
	public Action OnClick { get; }
}