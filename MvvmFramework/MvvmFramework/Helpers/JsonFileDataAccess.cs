using System.Text.Json;

namespace MvvmFramework.Helpers;

public interface IJsonFileDataAccess<TValue> where TValue : class
{
	Task<TValue?> FirstAsync(Func<TValue, bool>? predicate = null);
	Task<List<TValue>> ListAsync(Func<TValue, bool>? predicate = null);

	void Add(TValue value);
	void Update(TValue value);
	void Delete(TValue value);
	Task SaveChangesAsync();
}

public class JsonFileDataAccess<TValue> : IJsonFileDataAccess<TValue> where TValue : class
{
	private readonly string _path;
	private readonly Dictionary<string, TValue> _changeTracker = new();
	private bool _isWorking;

	public JsonFileDataAccess(string path)
	{
		_path = path;
	}

	// Queries
	public async Task<TValue?> FirstAsync(Func<TValue, bool>? predicate = default)
	{
		var values = await LoadAsync();

		return predicate is null
			? values.FirstOrDefault()
			: values.FirstOrDefault(predicate);
	}

	public async Task<List<TValue>> ListAsync(Func<TValue, bool>? predicate = default)
	{
		var values = await LoadAsync();

		return predicate is null
			? values.ToList()
			: values.Where(predicate).ToList();
	}

	// Commands
	public void Add(TValue value)
	{
		_changeTracker.Add("add", value);
	}

	public void Update(TValue value)
	{
		_changeTracker.Add("update", value);
	}

	public void Delete(TValue value)
	{
		_changeTracker.Add("delete", value);
	}

	public async Task SaveChangesAsync()
	{
		if(_isWorking)
		{
			return;
		}

		if(_changeTracker.IsEmpty())
		{
			return;
		}

		try
		{
			_isWorking = true;

			var values = await LoadAsync();

			foreach(var change in _changeTracker)
			{
                switch(change.Key)
                {
                    case "add":
                        Add(values, change);
                        break;
                    case "update":
                        Update(values, change);
                        break;
                    case "delete":
                        Delete(values, change);
                        break;
                }
            }

			await SaveAsync(values);

			_changeTracker.Clear();
		}
		finally
		{
			_isWorking = false;
		}
	}

    // Helpers
    private static void Add(List<TValue> values, KeyValuePair<string, TValue> change)
    {
        values.Add(change.Value);
    }

    private static void Update(List<TValue> values, KeyValuePair<string, TValue> change)
    {
        var value = values.FirstOrDefault(value => value == change.Value);
        if(value is null)
        {
            throw new Exception($"{change.Value} cannot be updated.");
        }

        var index = values.IndexOf(value);
        values.RemoveAt(index);
        values.Insert(index, change.Value);
    }

    private static void Delete(List<TValue> values, KeyValuePair<string, TValue> change)
    {
        var value = values.FirstOrDefault(value => value == change.Value);
        if(value is null)
        {
            throw new Exception($"{change.Value} cannot be deleted.");
        }

        values.Remove(value);
    }

    private async Task<List<TValue>> LoadAsync()
	{
		if(!File.Exists(_path))
		{
			return new();
		}

		await using var fileStream = new FileStream(_path, FileMode.Open, FileAccess.Read);
		var values = await JsonSerializer.DeserializeAsync<List<TValue>>(fileStream);

		return values ?? new();
	}

	private async Task SaveAsync(List<TValue>? values = default)
	{
		if(!Directory.Exists(Path.GetDirectoryName(_path)))
		{
			Directory.CreateDirectory(Path.GetDirectoryName(_path));
		}

		values ??= await LoadAsync();

		await using var fileStream = new FileStream(_path, FileMode.Create, FileAccess.Write);
		await JsonSerializer.SerializeAsync(fileStream, values);
	}
}
