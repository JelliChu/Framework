namespace Toolbox;

public static class ListExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="element"></param>
    /// <returns>True if the item was added successfully.</returns>
    public static bool TryAddUnique<T>(this IList<T> list, T element)
    {
        if(list.Contains(element))
        {
            return false;
        }

        list.Add(element);
        return true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="element"></param>
    /// <returns>True if the item was added successfully.</returns>
    public static bool TryAddUniqueBy<T, TKey>(this IList<T> list, T element, Func<T, TKey> keySelector)
    {
        if(list.Any(e => Equals(keySelector(e), keySelector(element))))
        {
            return false;
        }

        list.Add(element);
        return true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="index"></param>
    /// <param name="element"></param>
    /// <returns>True if the item was added successfully.</returns>
    public static bool TryInsertUnique<T>(this IList<T> list, int index, T element)
    {
        if(list.Contains(element))
        {
            list.Remove(element);
            list.Insert(index, element);
            return false;
        }

        list.Insert(index, element);
        return true;
    }
}
