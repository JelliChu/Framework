namespace MvvmFramework;

public class AppState
{
    public string ViewTypeName { get; init; }
    public string? ViewModelTypeName { get; init; }
    public ViewModelState? ViewModelState { get; init; }
}
