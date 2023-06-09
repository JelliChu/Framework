using System.Reflection;

namespace MvvmFramework.WinUI;

public static class TypesCache
{
	private static HashSet<Type> _types =
		Assembly.GetEntryAssembly().GetTypes()
		.Concat(Assembly.GetCallingAssembly().GetTypes()).ToHashSet();

	public static IReadOnlySet<Type> Types => _types;
}
