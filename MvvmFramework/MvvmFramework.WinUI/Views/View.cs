using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace MvvmFramework.WinUI;

public abstract class View<TViewModel> : Page, IView where TViewModel : ViewModel
{
    public View()
    {
        Loaded += OnViewLoaded;
    }

    public TViewModel ViewModel
    {
        get { return (TViewModel)GetValue(ViewModelProperty); }
        set { SetValue(ViewModelProperty, value); }
    }
    public static readonly DependencyProperty ViewModelProperty = DependencyPropertyHelper.Register<View<TViewModel>>(nameof(ViewModel));

    private async void OnViewLoaded(object sender, RoutedEventArgs e)
    {
        if(ViewModel is not null)
        {
            await ViewModel.OnViewLoaded();
        }
    }

    public virtual async Task Suspend() => await ViewModel.Suspend();
    public virtual async Task Resume() => await ViewModel.Resume();

    protected override async void OnNavigatedTo(NavigationEventArgs e)
    {
        if(e.Parameter is TViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        if(ViewModel is not null)
        {
            await ViewModel.OnNavigatedTo();
        }

        base.OnNavigatedTo(e);
    }

    protected override async void OnNavigatingFrom(NavigatingCancelEventArgs e)
    {
        if(ViewModel is not null)
        {
            await ViewModel.OnNavigatingFrom(new() { RequestCancelled = e.Cancel });
        }

        base.OnNavigatingFrom(e);
    }

    protected override async void OnNavigatedFrom(NavigationEventArgs e)
    {
        if(ViewModel is IDisposable disposable)
        {
            disposable.Dispose();
        }

        if(ViewModel is not null)
        {
            await ViewModel.OnNavigatedFrom();
        }

        base.OnNavigatedFrom(e);
    }

    ViewModel IView.GetViewModel() => ViewModel;
}
