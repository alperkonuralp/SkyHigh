namespace SkyHigh.EntitySchemaParser.Entities;

public class RelationshipDefinition
{
    public string Name { get; set; }

    public string EntityType { get; set; }

    public string Type { get; set; }

    public bool IsCollection { get; set; }

    public string Summary { get; set; }

    public string ForeignKeyName { get; set; }

    public string ForeignKeyEntity { get; set; }

    public string ForeignKeyProperty { get; set; }
}