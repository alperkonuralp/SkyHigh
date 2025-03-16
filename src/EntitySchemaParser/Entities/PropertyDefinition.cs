namespace SkyHigh.EntitySchemaParser.Entities;

public class PropertyDefinition
{
    public PropertyAccessModifier AccessModifier { get; set; } = PropertyAccessModifier.Public;

    public string Name { get; set; }

    public string Type { get; set; }

    public string Summary { get; set; }

    public PropertySpecialType SpecialType { get; set; } = PropertySpecialType.None;

    public bool IsRequired { get; set; }

    public string DefaultValue { get; set; }

    public int? MaxLength { get; set; }
}