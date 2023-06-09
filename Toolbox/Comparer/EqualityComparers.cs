namespace Toolbox;

public static class EqualityComparers
{
    public static DelegateEqualityComparer<T, TKey> Delegate<T, TKey>(Func<T, TKey> keySelector) => new(keySelector);
}

