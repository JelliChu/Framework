namespace MvvmFramework;

public interface INotificationService
{
	Task Show(NotificationPlacement notificationPlacement, INotification notification);
	INotification GetNotification(string id);
}
