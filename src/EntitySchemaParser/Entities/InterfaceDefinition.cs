using System.Collections.Generic;

namespace SkyHigh.EntitySchemaParser.Entities;

public class InterfaceDefinition
{
    public InterfaceAccessModifier AccessModifier { get; set; } = InterfaceAccessModifier.Public;

    public string Name { get; set; }

    public string Namespace { get; set; } = ".";

    public List<string> Interfaces { get; set; } = [];

    public bool IsPartial { get; set; } = false;

    public List<string> TypeParameters { get; set; } = [];

    public Dictionary<string, string> TypeFilters { get; set; } = [];

    public string Summary { get; set; }

    public List<PropertyDefinition> Properties { get; set; }

    public List<RelationshipDefinition> Relationships { get; set; }
}