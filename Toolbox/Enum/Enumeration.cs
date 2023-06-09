using System.Reflection;

namespace Toolbox;

public abstract class Enumeration<TEnum, TIndex> : IComparable<Enumeration<TEnum, TIndex>>
    where TEnum : Enumeration<TEnum, TIndex>
    where TIndex : struct, IComparable<TIndex>
{
    protected TIndex _Index;

    public Enumeration(TIndex index) => _Index = index;

    public override bool Equals(object? obj) => obj is TEnum other && _Index.Equals(other._Index);

    public override int GetHashCode() => _Index.GetHashCode();

    public static IEnumerable<TEnum> AsEnumerable()
    {
        var type = typeof(TEnum);

        return Assembly
            .GetAssembly(type)!
            .GetTypes()
            .Where(t => type.IsAssignableFrom(t))
            .SelectMany(t =>
                t.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                 .Where(p => type.IsAssignableFrom(p.FieldType))
                 .Select(pi => (TEnum)pi.GetValue(null)!))
            .OrderBy(t => t._Index)
            .ToList();
    }

    public static TEnum? FromIndex(TIndex index) => AsEnumerable().FirstOrDefault(element => element._Index.Equals(index));

    public int CompareTo(Enumeration<TEnum, TIndex>? other) => _Index.CompareTo(other?._Index ?? default);

    public static implicit operator Enumeration<TEnum, TIndex>?(TIndex index) => FromIndex(index);
    public static implicit operator TIndex(Enumeration<TEnum, TIndex> enumeration) => enumeration._Index;
}

public abstract class Enumeration<TEnum> : Enumeration<TEnum, int>
    where TEnum : Enumeration<TEnum>
{
    public Enumeration(int index) : base(index) { }
}
