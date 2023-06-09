namespace MvvmFramework.WinUI;

public class StorageDirectory : IStorageDirectory
{
	public StorageDirectory(string fullName)
	{
		Path = System.IO.Path.GetDirectoryName(fullName);
		Name = System.IO.Path.GetFileName(fullName);
	}

	public StorageDirectory(string path, string name)
	{
		Path = path;
		Name = name;
	}

	public string Path { get; }
	public string Name { get; }

	public async Task AddDirectory(StorageDirectory directory) => await StorageService.Default.CreateDirectory(directory);
	public async Task<IEnumerable<StorageFile>> GetFiles() => await StorageService.Default.GetFiles(GetFullPath());
	public async Task AddFile(StorageFile file) => await StorageService.Default.CreateFile(file);

	public override string ToString() => GetFullPath();

	private string GetFullPath() => System.IO.Path.Combine(Path, Name);

	async Task IStorageDirectory.AddDirectory(IStorageDirectory directory) => await AddDirectory((StorageDirectory)directory);
	async Task<IEnumerable<IStorageFile>> IStorageDirectory.GetFiles() => await GetFiles();
	async Task IStorageDirectory.AddFile(IStorageFile file) => await AddFile((StorageFile)file);
}
