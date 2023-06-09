namespace MvvmFramework;

public interface IStorageService
{
	public string ApplicationStorageRootPath { get; }

	Task<IStorageDirectory> GetDirectory(string path);
	Task<IEnumerable<IStorageDirectory>> GetDirectories(string directoryPath);
	Task CreateDirectory(IStorageDirectory directory);
	Task DeleteDirectory(IStorageDirectory directory);

	Task<IStorageFile> GetFile(string path);
	Task<IEnumerable<IStorageFile>> GetFiles(string directoryPath);
	Task CreateFile(IStorageFile file);
	Task DeleteFile(IStorageFile file);
}
