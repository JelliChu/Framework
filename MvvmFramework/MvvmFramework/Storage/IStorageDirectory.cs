namespace MvvmFramework;

public interface IStorageDirectory
{
	/// <summary>
	/// Path of the directory (without name).
	/// </summary>
	string Path { get; }
	/// <summary>
	/// Name of the directory (without path).
	/// </summary>
	string Name { get; }

	Task AddDirectory(IStorageDirectory directory);
	Task<IEnumerable<IStorageFile>> GetFiles();
	Task AddFile(IStorageFile file);
}
