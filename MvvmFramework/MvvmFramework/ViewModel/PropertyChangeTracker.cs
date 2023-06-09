using System.ComponentModel;

namespace MvvmFramework;

public class PropertyChangeTracker
{
	public Action<object?, string>? OnPropertyChanged { get; set; }

	public void Subscribe(INotifyPropertyChanged recipient)
	{
		recipient.PropertyChanged += Recipient_PropertyChanged;

		recipient
			.GetType()
			.GetProperties()
			.Where(propertyInfo => propertyInfo.PropertyType.IsAssignableTo(typeof(INotifyPropertyChanged)))
			.Select(propertyInfo => (INotifyPropertyChanged)propertyInfo.GetValue(recipient)!)
			.ToList()
			.ForEach(Subscribe);
	}

	private void Recipient_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		var senderName = sender;
		var propertyName = e.PropertyName;

		OnPropertyChanged?.Invoke(senderName, propertyName);
	}
}