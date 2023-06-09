namespace MvvmFramework;

public interface IUIContext
{
    void Enqueue(Action action);
}
