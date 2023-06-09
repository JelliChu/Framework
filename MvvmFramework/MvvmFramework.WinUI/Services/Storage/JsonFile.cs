using System.Text.Json;
using Windows.Storage;

namespace MvvmFramework.WinUI;

public class JsonFile : StorageFile
{
	public JsonFile(string fullName) : this(System.IO.Path.GetDirectoryName(fullName), System.IO.Path.GetFileNameWithoutExtension(fullName)) { }

	public JsonFile(string path, string name) : base(path, name, ".json") { }

	public async Task<TContents?> GetContents<TContents>()
	{
		var storageFolder = await StorageFolder.GetFolderFromPathAsync(Path);
		var storageFile = await storageFolder.GetFileAsync(Name);
		using var fileStream = await storageFile.OpenStreamForReadAsync();

		return await JsonSerializer.DeserializeAsync<TContents>(fileStream);
	}

	public async Task SetContents<TContents>(TContents contents)
	{
		var storageFolder = await StorageFolder.GetFolderFromPathAsync(Path);
		var storageFile = await storageFolder.GetFileAsync(Name);
		using var fileStream = await storageFile.OpenStreamForWriteAsync();

		await JsonSerializer.SerializeAsync(fileStream, contents);
	}
}