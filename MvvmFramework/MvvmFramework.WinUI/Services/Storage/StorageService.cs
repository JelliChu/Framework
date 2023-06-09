using Windows.Storage;

namespace MvvmFramework.WinUI;

public class StorageService : IStorageService
{
	public static StorageService Default => new();

	public static StorageFolder ApplicationStorageRoot => ApplicationData.Current.LocalFolder;
	public string ApplicationStorageRootPath => ApplicationStorageRoot.Path;


	public async Task<StorageDirectory> GetDirectory(string path)
	{
		var storageFolder = await StorageFolder.GetFolderFromPathAsync(path);

		return new StorageDirectory(
			Path.GetDirectoryName(storageFolder.Path),
			storageFolder.Name);
	}

	public async Task<IEnumerable<StorageDirectory>> GetDirectories(string directoryPath)
	{
		var storageFolder = await StorageFolder.GetFolderFromPathAsync(directoryPath);
		var files = await storageFolder.GetFilesAsync();

		return files.Select(storageFile => new StorageDirectory(
			Path.GetDirectoryName(storageFile.Path),
			storageFile.Name));
	}

	public async Task CreateDirectory(StorageDirectory directory)
	{
		var storageFolder = await StorageFolder.GetFolderFromPathAsync(directory.Path);

		await storageFolder.CreateFolderAsync(directory.Name);
	}

	public async Task DeleteDirectory(StorageDirectory directory)
	{
		var storageFolder = await StorageFolder.GetFolderFromPathAsync(directory.Path);

		await storageFolder.DeleteAsync();
	}


	public async Task<StorageFile> GetFile(string path)
	{
		var directoryPath = Path.GetDirectoryName(path);
		var storageFolder = await StorageFolder.GetFolderFromPathAsync(directoryPath);
		var storageFile = await storageFolder.GetFileAsync(Path.GetFileName(path));

		return new StorageFile(
			Path.GetDirectoryName(storageFile.Path),
			Path.GetFileNameWithoutExtension(storageFile.Name),
			storageFile.FileType);
	}

	public async Task<IEnumerable<StorageFile>> GetFiles(string directoryPath)
	{
		var storageFolder = await StorageFolder.GetFolderFromPathAsync(directoryPath);
		var files = await storageFolder.GetFilesAsync();

		return files.Select(storageFile => new StorageFile(
			Path.GetDirectoryName(storageFile.Path),
			Path.GetFileNameWithoutExtension(storageFile.Name),
			storageFile.FileType));
	}

	public async Task CreateFile(StorageFile file)
	{
		var storageFolder = await StorageFolder.GetFolderFromPathAsync(file.Path);
		var storageFile = await storageFolder.CreateFileAsync(file.Name);

		var contents = await file.GetContents();

		await FileIO.WriteTextAsync(storageFile, contents);
	}

	public async Task DeleteFile(StorageFile file)
	{
		var storageFolder = await StorageFolder.GetFolderFromPathAsync(file.Path);
		var storageFile = await storageFolder.GetFileAsync(file.Name);

		await storageFile.DeleteAsync();
	}


	async Task<IStorageDirectory> IStorageService.GetDirectory(string path) => await GetDirectory(path);
	async Task<IEnumerable<IStorageDirectory>> IStorageService.GetDirectories(string directoryPath) => await GetDirectories(directoryPath);
	async Task IStorageService.CreateDirectory(IStorageDirectory directory) => await CreateDirectory((StorageDirectory)directory);
	async Task IStorageService.DeleteDirectory(IStorageDirectory directory) => await CreateDirectory((StorageDirectory)directory);

	async Task<IStorageFile> IStorageService.GetFile(string path) => await GetFile(path);
	async Task<IEnumerable<IStorageFile>> IStorageService.GetFiles(string directoryPath) => await GetFiles(directoryPath);
	async Task IStorageService.CreateFile(IStorageFile file) => await CreateFile((StorageFile)file);
	async Task IStorageService.DeleteFile(IStorageFile file) => await DeleteFile((StorageFile)file);
}
