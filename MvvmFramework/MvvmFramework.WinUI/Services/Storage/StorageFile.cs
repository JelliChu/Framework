using Windows.Storage;

namespace MvvmFramework.WinUI;

public class StorageFile : IStorageFile
{
	public StorageFile(string fullName)
	{
		Path = System.IO.Path.GetDirectoryName(fullName);
		Name = System.IO.Path.GetFileNameWithoutExtension(fullName);
		Extension = System.IO.Path.GetExtension(fullName);
	}

	public StorageFile(string path, string name, string extension)
	{
		Path = path;
		Name = name;
		Extension = extension;
	}

	public string Path { get; }
	public string Name { get; }
	public string Extension { get; }

	public virtual async Task<string> GetContents()
	{
		var storageFolder = await StorageFolder.GetFolderFromPathAsync(Path);
		var storageFile = await storageFolder.GetFileAsync(Name);

		return await FileIO.ReadTextAsync(storageFile);
	}

	public virtual async Task SetContents(string contents)
	{
		var storageFolder = await StorageFolder.GetFolderFromPathAsync(Path);
		var storageFile = await storageFolder.GetFileAsync(Name);

		await FileIO.WriteTextAsync(storageFile, contents);
	}

	public override string ToString() => GetFullPath();

	protected virtual string GetFullPath()
	{
		var path = System.IO.Path.Combine(Path, Name);

		var extension = Extension.StartsWith(".")
			? Extension
			: $".{Extension}";

		return path + extension;
	}
}
