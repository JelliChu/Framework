namespace Toolbox;

public static class PathHelper
{
    /// <summary> Returns the file name of the specified path string without the extensions. </summary>
    /// <param name="path"> The path of the file. </param>
    /// <returns> The path minus all extensions. </returns>
    public static string GetFileNameWithoutExtensions(string path)
    {
        path = Path.GetFileNameWithoutExtension(path);

        if(Path.HasExtension(path))
        {
            return GetFileNameWithoutExtensions(path);
        }

        return path;
    }

    /// <summary> Creates all directories in the path. </summary>
    /// <param name="path"> The path of the file. </param>
    public static void EnsurePathExists(string path)
    {
        var directory = Path.GetDirectoryName(path);
        if(directory is null)
        {
            return;
        }

        if(!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }
}