using CommunityToolkit.WinUI.Notifications;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using System;
using System.Linq;

namespace MvvmFramework.WinUI.SampleApp;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : Application
{
    private MainWindow _mainWindow;
    private static IServiceProvider _serviceProvider;
    private static INavigationService _navigationService;
    private static INotificationService _notificationService;

    /// <summary>
    /// Initializes the singleton application object.  This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App()
    {
        this.InitializeComponent();

        ToastNotificationManagerCompat.OnActivated += OnToastActivated;
    }

    public static INavigationService NavigationService => _navigationService ??= _serviceProvider.GetService<INavigationService>();
    public static INotificationService NotificationService => _notificationService ??= _serviceProvider.GetService<INotificationService>();

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        _mainWindow = new MainWindow();
        
        ConfigureServices(new());

        _mainWindow.Activate();
    }

    private void ConfigureServices(ServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<INavigationService, NavigationService>();
        serviceCollection.AddSingleton<INotificationService, NotificationService>(provider => new(_mainWindow.InAppNotificationControl));

        _serviceProvider = serviceCollection.BuildServiceProvider();
    }

    private void OnToastActivated(ToastNotificationActivatedEventArgsCompat args)
    {
        try
        {
            var toastArgs = ToastArguments.Parse(args.Argument);

            if(!toastArgs.Contains("Id") || !toastArgs.Contains("ButtonIndex"))
            {
                return;
            }

            var notificationId = toastArgs.Get("Id");
            var notification = NotificationService.GetNotification(notificationId);
            
            var buttonIndex = toastArgs.GetInt("ButtonIndex");
            var button = notification.Buttons.ElementAt(buttonIndex);

            button.OnClick?.Invoke();
        }
        catch { }
    }
}
