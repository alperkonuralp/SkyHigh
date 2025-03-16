using System.Collections.Generic;

namespace SkyHigh.EntitySchemaParser.Entities;

public class ClassDefinition
{
    public ClassAccessModifier AccessModifier { get; set; } = ClassAccessModifier.Public;
    public string Name { get; set; }

    public string Namespace { get; set; } = ".";

    public string BaseType { get; set; }

    public List<string> Interfaces { get; set; } = [];

    public SpecialType SpecialType { get; set; } = SpecialType.None;

    public string Summary { get; set; }

    public bool IsAbstract { get; set; } = false;

    public bool IsSealed { get; set; } = false;

    public bool IsPartial { get; set; } = false;

    public List<string> TypeParameters { get; set; } = [];

    public Dictionary<string, string> TypeFilters { get; set; } = [];

    public List<PropertyDefinition> Properties { get; set; }

    public List<RelationshipDefinition> Relationships { get; set; }
}