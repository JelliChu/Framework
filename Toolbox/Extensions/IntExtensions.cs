namespace Toolbox;

public static class IntExtensions
{
    public static int ParseOrDefault(string s)
    {
        return int.TryParse(s, out var parsed) ? parsed : default;
    }
}
