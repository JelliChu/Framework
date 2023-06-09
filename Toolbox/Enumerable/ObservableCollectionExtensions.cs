using System.Collections.ObjectModel;

namespace Toolbox;

public static class ObservableCollectionExtensions
{
    public static void RemoveFirst<T>(this ObservableCollection<T> source, Func<T, bool> predicate)
    {
        foreach(var item in source)
        {
            if(predicate(item))
            {
                source.Remove(item);
                return;
            }
        }
    }

    public static void AddRange<T>(this ObservableCollection<T> source, IEnumerable<T> enumerable)
    {
        foreach(var item in enumerable)
        {
            source.Add(item);
        }
    }
}
