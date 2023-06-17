using System.Runtime.CompilerServices;

namespace MvvmFramework;

public class ViewModelState : Dictionary<string, object>
{
    protected T? Get<T>([CallerMemberName] string propertyName = default) => (T?)this[propertyName];
    protected void Set<T>(T value, [CallerMemberName] string propertyName = default) => this[propertyName] = value;
}
