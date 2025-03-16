using System.Collections.Generic;

namespace SkyHigh.EntitySchemaParser.Entities;

public class EnumDefinition
{
    public EnumAccessModifier AccessModifier { get; set; } = EnumAccessModifier.Public;

    public string Name { get; set; }

    public string Namespace { get; set; } = ".";

    public EnumBaseType BaseType { get; set; } = EnumBaseType.Int;

    public Dictionary<string, object> Values { get; set; }
}