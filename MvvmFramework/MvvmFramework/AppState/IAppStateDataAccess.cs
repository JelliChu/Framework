namespace MvvmFramework;

public interface IAppStateDataAccess
{
    void Clear();
    Task<AppState?> LoadAsync();
    Task<AppState> LoadOrCreateAsync();
    Task SaveAsync(AppState settings);
}