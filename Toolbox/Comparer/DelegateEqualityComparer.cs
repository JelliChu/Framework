namespace Toolbox;

public class DelegateEqualityComparer<T, TKey> : IEqualityComparer<T>
{
    private readonly Func<T, TKey> _keySelector;

    public DelegateEqualityComparer(Func<T, TKey> keySelector)
    {
        _keySelector = keySelector ?? throw new ArgumentNullException(nameof(keySelector));
    }

    public bool Equals(T? x, T? y)
    {
        var keyX = _keySelector(x);
        var keyY = _keySelector(y);
        return EqualityComparer<TKey>.Default.Equals(keyX, keyY);
    }

    public int GetHashCode(T obj)
    {
        var key = _keySelector(obj);
        return EqualityComparer<TKey>.Default.GetHashCode(key);
    }
}

