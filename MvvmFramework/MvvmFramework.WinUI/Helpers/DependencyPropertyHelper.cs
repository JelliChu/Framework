using Microsoft.UI.Xaml;
using System.Reflection;
using System.Runtime.CompilerServices;
using Toolbox;

namespace MvvmFramework.WinUI;

public static class DependencyPropertyHelper
{
	// NOTE: new/hidden properties cannot use this helper

	private static readonly Dictionary<string, Type> _cachedTypes = new();
	private const BindingFlags _bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;

	public static DependencyProperty Register(string name, Type ownerType, PropertyMetadata propertyMetadata = default)
	{
		var property = ownerType.GetProperty(name, _bindingFlags);

		var propertyType = property.PropertyType;

		propertyMetadata ??= new(default);

		return DependencyProperty.Register(name, propertyType, ownerType, propertyMetadata);
	}

	public static DependencyProperty Register<TOwnerType>(string name, PropertyMetadata propertyMetadata = default)
	{
		var ownerType = typeof(TOwnerType);

		return Register(name, ownerType, propertyMetadata);
	}

	public static DependencyProperty AutoRegister(string name, PropertyMetadata propertyMetadata = default, [CallerFilePath] string typeOwnerFilePath = null)
	{
		var ownerTypeName = PathHelper.GetFileNameWithoutExtensions(typeOwnerFilePath);

		var ownerType = FindType(ownerTypeName);

		return Register(name, ownerType, propertyMetadata);
	}


    public static DependencyProperty RegisterAttached(string name, Type ownerType, PropertyMetadata propertyMetadata = default)
	{
		var methodName = $"Get{name}";

		var method = ownerType.GetMethod(methodName, _bindingFlags);

		var methodType = method.ReturnType;

		propertyMetadata ??= new(default);

		return DependencyProperty.RegisterAttached(name, methodType, ownerType, propertyMetadata);
	}

	public static DependencyProperty RegisterAttached<TOwnerType>(string name, PropertyMetadata propertyMetadata = default)
	{
		var ownerType = typeof(TOwnerType);

		return RegisterAttached(name, ownerType, propertyMetadata);
	}

	public static DependencyProperty AutoRegisterAttached(string name, PropertyMetadata propertyMetadata = default, [CallerFilePath] string typeOwnerFilePath = null)
	{
		var ownerTypeName = PathHelper.GetFileNameWithoutExtensions(typeOwnerFilePath);

		var ownerType = FindType(ownerTypeName);

		return RegisterAttached(name, ownerType, propertyMetadata);
	}


	private static Type FindType(string ownerTypeName)
	{
		if(_cachedTypes.ContainsKey(ownerTypeName))
		{
			return _cachedTypes[ownerTypeName];
		}

		var types = TypesCache.Types;

		var ownerType = types.First(type => type.Name == ownerTypeName);

		_cachedTypes.Add(ownerTypeName, ownerType);

		return ownerType;
	}
}
