namespace Toolbox;

public static class EnumExtensions
{
    public static TEnum? TryParse<TEnum>(string s) where TEnum : struct
    {
        return Enum.TryParse<TEnum>(s, out var parsed) ? parsed : default;
    }

    public static List<string> ToList<TEnum>() where TEnum : struct
    {
        return Enum.GetNames(typeof(TEnum)).ToList();
    }

    public static List<string> ToList<TEnum>(this TEnum @enum) where TEnum : struct
    {
        return Enum.GetNames(@enum.GetType()).ToList();
    }

    public static List<string> ToList<TEnum>(Type @enum) where TEnum : struct
    {
        return Enum.GetNames(@enum).ToList();
    }

    public static TEnum Iterate<TEnum>(this TEnum @enum) where TEnum : struct, Enum
    {
        // Iterate enum
        var enumInt = Convert.ToInt32(@enum);
        enumInt++;

        var count = Enum.GetNames<TEnum>().Length;

        if(enumInt >= count)
        {
            enumInt = 0;
        }

        return (TEnum)Enum.ToObject(@enum.GetType(), enumInt);
    }
}
