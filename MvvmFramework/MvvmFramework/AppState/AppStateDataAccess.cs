using System.Text.Json;

namespace MvvmFramework;

public class AppStateDataAccess
{
    private readonly string _path;

    public AppStateDataAccess(string path)
    {
        _path = path;
    }

    public async Task<AppState?> LoadAsync()
    {
        if(!File.Exists(_path))
        {
            return default;
        }

        await using var fileStream = File.OpenRead(_path);
        return await JsonSerializer.DeserializeAsync<AppState>(fileStream);
    }

    public async Task<AppState> LoadOrCreateAsync()
    {
        if(!File.Exists(_path))
        {
            // Create a new file.
            await SaveAsync(new());
        }

        await using var fileStream = File.OpenRead(_path);
        return await JsonSerializer.DeserializeAsync<AppState>(fileStream);
    }

    public async Task SaveAsync(AppState settings)
    {
        await using var fileStream = new FileStream(_path, FileMode.Create, FileAccess.Write);
        await JsonSerializer.SerializeAsync(fileStream, settings);
    }

    public void Clear()
    {
        if(File.Exists(_path))
        {
            File.Delete(_path);
        }
    }
}
