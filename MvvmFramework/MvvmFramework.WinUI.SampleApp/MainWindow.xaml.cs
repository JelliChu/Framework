using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml;
using System;
using System.Diagnostics;

namespace MvvmFramework.WinUI.SampleApp;

public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        this.InitializeComponent();
    }

    public InAppNotification InAppNotificationControl => InAppNotification;

    private async void myButton_Click(object sender, RoutedEventArgs e)
    {
        await App.NotificationService.Show(NotificationPlacement.OS, new SimpleNotification("Test", TimeSpan.FromSeconds(3))
        {
            Buttons =
            {
                new ButtonTemplate("Button1", () => Debug.WriteLine("Button1 was clicked")),
                new ButtonTemplate("Button2", () => Debug.WriteLine("Button2 was clicked, innit")),
            },
        });
    }
}
