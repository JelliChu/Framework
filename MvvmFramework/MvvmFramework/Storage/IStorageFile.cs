namespace MvvmFramework;

public interface IStorageFile
{
	/// <summary>
	/// Path of the file (without name).
	/// </summary>
	string Path { get; }
	/// <summary>
	/// Name of the file (without path or extension).
	/// </summary>
	string Name { get; }
	/// <summary>
	/// Extension of the file.
	/// </summary>
	string Extension { get; }

	Task<string> GetContents();
	Task SetContents(string contents);
}
