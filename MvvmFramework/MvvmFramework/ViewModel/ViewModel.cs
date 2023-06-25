namespace MvvmFramework;

public abstract class ViewModel : ObservableObject
{
    public virtual async Task Validate() { }

    // Suspend/Resume
    public virtual async Task Suspend() { }
    public virtual async Task Resume() { }

    // Restore
    public virtual async Task<ViewModelState?> GetState() => default;
    public virtual async Task RestoreState(ViewModelState? state) { }

    // Navigation
    public virtual async Task OnViewLoaded() { }
    public virtual async Task OnNavigatedTo() { }
    public virtual async Task OnNavigatingFrom(ViewModelNavigationArgs args) { }
    public virtual async Task OnNavigatedFrom() { }
}
