using System.Collections;
using System.Reflection;

namespace Toolbox.Reflection;

public static class ReflectionExtensions
{
    public static bool HasProperty(this object @object, string propertyName, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance)
    {
        var type = @object.GetType();
        var property = type.GetProperty(propertyName, bindingFlags);

        return property is not null;
    }

    public static object? TryGetProperty(this object @object, string propertyName, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance)
    {
        var type = @object.GetType();
        var property = type.GetProperty(propertyName, bindingFlags);

        if(property is null)
        {
            return default;
        }

        return property.GetValue(@object);
    }

    public static TValue? TryGetProperty<TValue>(this object @object, string propertyName, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance)
    {
        var type = @object.GetType();
        var property = type.GetProperty(propertyName, bindingFlags);

        if(property is null)
        {
            return default;
        }

        return (TValue?)property.GetValue(@object);
    }

    public static IEnumerable<TType> GetProperties<TType>(this object @object, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance)
    {
        var type = @object.GetType();

        return
            type
            .GetProperties(bindingFlags)
            .Where(i => i.PropertyType.IsAssignableTo(typeof(TType)))
            .Select(i => i.GetValue(@object))
            .Cast<TType>()
            .ToList();
    }

    public static IEnumerable GetProperties(this object @object, Type propertyType, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance)
    {
        var type = @object.GetType();

        return
            type
            .GetProperties(bindingFlags)
            .Where(i => i.PropertyType.IsAssignableTo(propertyType))
            .Select(i => i.GetValue(@object))
            .ToList();
    }

    public static IEnumerable<PropertyInfo> GetPropertyInfos<TType>(this object @object, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance)
    {
        var type = @object.GetType();

        return
            type
            .GetProperties(bindingFlags)
            .Where(i => i.PropertyType.IsAssignableTo(typeof(TType)))
            .ToList();
    }

    public static IEnumerable<PropertyInfo> GetPropertyInfos(this object @object, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance)
    {
        var type = @object.GetType();

        return
            type
            .GetProperties(bindingFlags)
            .ToList();
    }

    public static void TrySetProperty(this object @object, string propertyName, object value)
    {
        var type = @object.GetType();
        var property = type.GetProperty(propertyName);

        if(property is null)
        {
            return;
        }

        property.SetValue(@object, value);
    }

    public static void TryInvokeMethod(this object @object, string methodName)
    {
        var type = @object.GetType();
        var method = type.GetMethod(methodName);

        if(method is null)
        {
            return;
        }

        method.Invoke(@object, null);
    }
}
