using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace MvvmFramework.WinUI.AttachedProperties;

public static class TextBlockBind
{
    #region StringModifier
    public static TextBlockStringModifier GetStringModifier(DependencyObject obj) => (TextBlockStringModifier)obj.GetValue(StringModifierProperty);
    public static void SetStringModifier(DependencyObject obj, TextBlockStringModifier value) => obj.SetValue(StringModifierProperty, value);

    public static readonly DependencyProperty StringModifierProperty =
		DependencyPropertyHelper.AutoRegisterAttached("StringModifier", new PropertyMetadata(default(TextBlockStringModifier), OnStringModifierChanged));

	private static void OnStringModifierChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
	{
		var textBlock = (TextBlock)sender;

		textBlock.WhenLoaded(() =>
		{
			var stringModifier = (TextBlockStringModifier?)e.NewValue;

			if(stringModifier is null or TextBlockStringModifier.None)
			{
				return;
			}

			var text = stringModifier switch
			{
				TextBlockStringModifier.ToUpper => textBlock.Text.ToUpper(),
				TextBlockStringModifier.ToLower => textBlock.Text.ToLower(),
				TextBlockStringModifier.Trim => textBlock.Text.Trim(),
				TextBlockStringModifier.TrimStart => textBlock.Text.TrimStart(),
				TextBlockStringModifier.TrimEnd => textBlock.Text.TrimEnd(),
				_ => throw new NotImplementedException(),
			};

			textBlock.Text = text;
		});
	}

	public enum TextBlockStringModifier
	{
		None,
		ToUpper,
		ToLower,
		Trim,
		TrimStart,
		TrimEnd,
	}
	#endregion
}
