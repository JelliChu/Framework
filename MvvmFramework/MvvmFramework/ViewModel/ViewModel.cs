
namespace MvvmFramework;

public abstract class ViewModel : ObservableObject
{
	public virtual async Task Validate() { }

	public virtual async Task OnViewLoaded() { }

	// Suspend/Restore
	public virtual async Task Suspend() { }
	public virtual async Task Resume() { }

	// Navigation
	// To
	public virtual async Task OnNavigateToRequested(ViewModelNavigationRequestArgs args) { }
	public virtual async Task OnNavigatingTo() { }
	public virtual async Task OnNavigatedTo() { }
	// From
	public virtual async Task OnNavigateFromRequested(ViewModelNavigationRequestArgs args) { }
	public virtual async Task OnNavigatingFrom() { }
	public virtual async Task OnNavigatedFrom() { }
}
