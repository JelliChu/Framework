﻿using System.Reflection;
using System.Text.RegularExpressions;

namespace XamlForms.WinUI.MultiBinding;

internal static class ReflectionExtensions
{
    private const string IntegerIndexerPattern = @".+\[[0-9]+(,[0-9]+)*\]";


    public static PropertyInfo GetRuntimePropertyFromXamlPath(this Type type, string xamlPropertyPath)
    {
        var propertyPathParts = xamlPropertyPath.Split('.');

        PropertyInfo propertyInfo = null;
        foreach(var propertyPathPart in propertyPathParts)
        {
            if(CheckIfContainsIndexer(propertyPathPart))
            {
                var indexBracketIndex = propertyPathPart.IndexOf("[", StringComparison.Ordinal);
                var propertyNamePart = propertyPathPart.Substring(0, indexBracketIndex);
                var indexerPart = propertyPathPart.Substring(indexBracketIndex);

                propertyInfo = type.GetRuntimeProperty(propertyNamePart);
                type = propertyInfo.PropertyType;

                var indexParametersCount = indexerPart.Split(',').Length;
                var indexers = type.GetRuntimeProperties().Where(pi => pi.GetIndexParameters().Length == indexParametersCount);
                var targetIndexer = indexers.FirstOrDefault(indexer => indexer.GetIndexParameters().All(indexParameter => indexParameter.ParameterType == typeof(int)));
                if(targetIndexer == null)
                {
                    throw new ArgumentException($"Indexer {propertyPathPart} must contain only integer indices.");
                }

                propertyInfo = targetIndexer;
                type = targetIndexer.PropertyType;
            }
            else
            {
                propertyInfo = type.GetRuntimeProperty(propertyPathPart);
                type = propertyInfo.PropertyType;
            }
        }

        return propertyInfo;
    }


    private static bool CheckIfContainsIndexer(string propertyPathPart)
    {
        var propertyWithIndexerRegEx = new Regex(IntegerIndexerPattern);

        return propertyWithIndexerRegEx.IsMatch(propertyPathPart);
    }
}
