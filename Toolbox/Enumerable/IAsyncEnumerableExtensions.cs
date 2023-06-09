namespace Toolbox;

public static class IAsyncEnumerableExtensions
{
    public static async Task<List<T>> ToListAsync<T>(this IAsyncEnumerable<T> source)
    {
        var list = new List<T>();

        var enumerator = source.GetAsyncEnumerator();

        try
        {
            while(await enumerator.MoveNextAsync())
            {
                var item = enumerator.Current;
                list.Add(item);
            }
        }
        finally
        {
            await enumerator.DisposeAsync();
        }

        return list;
    }
}
