namespace TechCareer.WernerHeisenberg.Northwind.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class ColumnNameAttribute: Attribute
{
    public string Name { get; set; }

    public ColumnNameAttribute(string name, short index,bool ignore = false)
    {
        Name = name;
        Index = index;
        Ignore = ignore;
    }

    public bool Ignore { get; set; }

    public short Index { get; set; }
}