using System.Collections.ObjectModel;

namespace Toolbox;

public static class EnumerableExtensions
{
    public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> source) => new(source);

    //public static void ForEach<T>(this IEnumerable<T> source, Action<T, int> action)
    //{
    //    var index = 0;
    //    foreach(var item in source)
    //    {
    //        action(item, index);
    //        index++;
    //    }
    //}

    //public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    //{
    //    foreach(var item in source)
    //    {
    //        action(item);
    //    }
    //}

    public static bool IsEmpty<T>(this IEnumerable<T> source) => !source.Any();

    public static bool IsNullOrEmpty<T>(this IEnumerable<T> source) => source is null || !source.Any();

    public static IEnumerable<T> NonDistinct<T>(this IEnumerable<T> source)
    {
        return
            source
            .GroupBy(i => i)
            .Where(g => g.Count() > 1)
            .SelectMany(r => r);
    }

    public static IEnumerable<T> NonDistinct<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector)
    {
        return
            source
            .GroupBy(keySelector)
            .Where(g => g.Count() > 1)
            .SelectMany(r => r);
    }

    public static string Join(this IEnumerable<string> source, string separator) => string.Join(separator, source);
}
