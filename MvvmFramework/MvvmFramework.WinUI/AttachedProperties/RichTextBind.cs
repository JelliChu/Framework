using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Documents;

namespace MvvmFramework.WinUI.AttachedProperties;

public static class RichTextBind
{
    #region Text
    public static string GetText(DependencyObject obj) => (string)obj.GetValue(TextProperty);
    public static void SetText(DependencyObject obj, string value) => obj.SetValue(TextProperty, value);

    public static readonly DependencyProperty TextProperty = DependencyPropertyHelper.AutoRegisterAttached("Text", new(default(string), OnTextChanged));

	private static void OnTextChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
	{
		if(sender is not RichTextBlock richTextBlock)
		{
			return;
		}

		if(e.NewValue is not string value)
		{
			return;
		}

		richTextBlock.Blocks.Clear();
		richTextBlock.Blocks.Add(new Paragraph
		{
			Inlines =
			{
				new Run { Text = value },
			}
		});
	}
	#endregion
}
