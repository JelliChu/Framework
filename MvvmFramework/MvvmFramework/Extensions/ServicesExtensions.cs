using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MvvmFramework.Extensions;

public static class ServicesExtensions
{
	public static IServiceCollection RegisterAllOfType<TType>(this IServiceCollection services, ServiceLifetime lifetime, Assembly? assembly = default)
	{
		assembly ??= Assembly.GetEntryAssembly();

		assembly
			.GetTypes()
			.Where(type => !type.IsAbstract)
			.Where(type => type.IsAssignableTo(typeof(TType)))
			.ToList()
			.ForEach(type => services.Add(new(type, type, lifetime)));

		return services;
	}
}
