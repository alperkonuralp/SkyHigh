using System.Collections.Generic;

namespace SkyHigh.EntitySchemaParser.Entities;

public class EntitySchema
{
    public string Name { get; set; }

    public string Version { get; set; }

    public string Description { get; set; }

    public string BaseNamespace { get; set; }

    public List<ClassDefinition> BaseTypes { get; set; }

    public List<InterfaceDefinition> Interfaces { get; set; }

    public List<ClassDefinition> Entities { get; set; }

    public List<EnumDefinition> Enums { get; set; }
}