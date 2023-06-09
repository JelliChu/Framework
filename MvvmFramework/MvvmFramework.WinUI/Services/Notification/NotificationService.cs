using CommunityToolkit.WinUI.Notifications;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Reflection;
using Windows.UI.Notifications;

namespace MvvmFramework.WinUI;

public class NotificationService : INotificationService
{
	private readonly InAppNotification _inAppNotification;
	private Dictionary<string, INotification> _notifications = new();

	public NotificationService(InAppNotification inAppNotification)
	{
		_inAppNotification = inAppNotification;
	}

	public async Task Show(NotificationPlacement notificationPlacement, INotification notification)
	{
		// OS Notification Centre
		if(notificationPlacement is NotificationPlacement.OS)
		{
			// NOTE: Windows desktop notification only checks ExpirationTime every 30 seconds.
			ShowOsNotification(notification);
			return;
		}

		// InApp
		if(notificationPlacement is NotificationPlacement.InApp)
		{
			ShowInAppNotification(notification);
			return;
		}
	}

	private void ShowOsNotification(INotification notification)
	{
		var exists = _notifications.TryAdd(notification.Id, notification);
		if(exists)
		{
			return;
		}

		_notifications.Add(notification.Id, notification);

		var builder = new ToastContentBuilder();

		builder.AddArgument("Id", notification.Id);

		builder.AddText(notification.Content);

		var index = 0;
		foreach(var button in notification.Buttons)
		{
			builder
				.AddButton(new ToastButton()
				.SetContent(button.Content)
				.AddArgument("ButtonIndex", index++));
		}

		builder.Show(options =>
		{
			options.Tag = notification.Id;
			options.Activated += OsNotification_Activated;
			options.Dismissed += OsNotification_Dismissed;
			options.Failed += OsNotification_Failed;
			options.ExpirationTime = DateTimeOffset.Now.Add(notification.Duration);
		});
	}

	private void ShowInAppNotification(INotification notification)
	{
		var exists = _notifications.TryAdd(notification.Id, notification);
		if(exists)
		{
			return;
		}

		_inAppNotification.Closing += InAppNotification_Closing;
		_inAppNotification.Show(new TextBlock 
		{ 
			Text = notification.Content,
			Tag = notification.Id 
		}, 
		(int)notification.Duration.TotalMilliseconds);
	}

	public INotification GetNotification(string id) => _notifications[id];

	private void OsNotification_Activated(ToastNotification sender, object args)
	{
		_notifications.Remove(sender.Tag);
	}

	private void OsNotification_Failed(ToastNotification sender, ToastFailedEventArgs args)
	{
		_notifications.Remove(sender.Tag);
	}

	private void OsNotification_Dismissed(ToastNotification sender, ToastDismissedEventArgs args)
	{
		_notifications.Remove(sender.Tag);
	}

	private void InAppNotification_Closing(object sender, InAppNotificationClosingEventArgs e)
	{
		var type = _inAppNotification.GetType();
		var property = type.GetField("_contentProvider", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);

		if(property is null)
		{
			return;
		}

		var contentProvider = property.GetValue(_inAppNotification) as ContentPresenter;

		var tag = (contentProvider.Content as FrameworkElement).Tag as string;

		_notifications.Remove(tag);
	}
}
