using System.Collections.Generic;
using System.Linq;
using static SkyHigh.EntitySchemaParser.SimpleJsonParser;

namespace SkyHigh.EntitySchemaParser.Entities;

internal static class ParserExtensions
{
    internal static EntitySchema Parse(this EntitySchema entitySchema, string json)
    {
        entitySchema ??= new EntitySchema();
        if (ParseJsonValue(json) is not Dictionary<string, object> rootObject)
            return entitySchema;

        entitySchema.Name = GetStringValue(rootObject, "name");
        entitySchema.Version = GetStringValue(rootObject, "version");
        entitySchema.Description = GetStringValue(rootObject, "description");
        entitySchema.BaseNamespace = GetStringValue(rootObject, "baseNamespace");
        entitySchema.BaseTypes = ParseClasses(TryGetValue(rootObject, "baseTypes") as List<object>);
        entitySchema.Interfaces = ParseInterfaces(TryGetValue(rootObject, "interfaces") as List<object>);
        entitySchema.Entities = ParseClasses(TryGetValue(rootObject, "entities") as List<object>);
        entitySchema.Enums = ParseEnums(TryGetValue(rootObject, "enums") as List<object>);
        return entitySchema;
    }

    // Entity listesini ayrıştırma
    private static List<ClassDefinition> ParseClasses(List<object> classesList)
    {
        var result = new List<ClassDefinition>();

        if (classesList == null || classesList.Count == 0)
            return result;

        foreach (var item in classesList)
        {
            if (item is Dictionary<string, object> dict)
            {
                result.Add(new ClassDefinition
                {
                    AccessModifier = GetEnum<ClassAccessModifier>(dict, "accessModifier") ?? ClassAccessModifier.Public,
                    Name = GetStringValue(dict, "name"),
                    Namespace = GetStringValue(dict, "namespace") ?? ".",
                    BaseType = GetStringValue(dict, "baseType"),
                    Interfaces = ParseStringList(TryGetValue(dict, "interfaces") as List<object>),
                    SpecialType = GetEnum<SpecialType>(dict, "specialType") ?? SpecialType.None,
                    Summary = GetStringValue(dict, "summary"),
                    IsAbstract = GetBooleanValue(dict, "isAbstract") ?? false,
                    IsSealed = GetBooleanValue(dict, "isSealed") ?? false,
                    IsPartial = GetBooleanValue(dict, "isPartial") ?? false,
                    TypeParameters = ParseStringList(TryGetValue(dict, "typeParameters") as List<object>),
                    TypeFilters = (TryGetValue(dict, "typeFilters") as Dictionary<string, object> ?? []).Where(x => x.Key != null && x.Value != null).ToDictionary(x => x.Key, x => x.Value as string),
                    Properties = ParseProperties(TryGetValue(dict, "properties") as List<object>),
                    Relationships = ParseRelationships(TryGetValue(dict, "relationships") as List<object>)
                });
            }
        }

        return result;
    }

    // Entity listesini ayrıştırma
    private static List<InterfaceDefinition> ParseInterfaces(List<object> interfaceList)
    {
        var result = new List<InterfaceDefinition>();
        if (interfaceList == null || interfaceList.Count == 0)
            return result;

        foreach (var item in interfaceList)
        {
            if (item is Dictionary<string, object> dict)
            {
                result.Add(new InterfaceDefinition
                {
                    AccessModifier = GetEnum<InterfaceAccessModifier>(dict, "accessModifier") ?? InterfaceAccessModifier.Public,
                    Name = GetStringValue(dict, "name"),
                    Namespace = GetStringValue(dict, "namespace") ?? ".",
                    Interfaces = ParseStringList(TryGetValue(dict, "interfaces") as List<object>),
                    IsPartial = GetBooleanValue(dict, "isPartial") ?? false,
                    TypeParameters = ParseStringList(TryGetValue(dict, "typeParameters") as List<object>),
                    TypeFilters = (TryGetValue(dict, "typeFilters") as Dictionary<string, object> ?? []).Where(x => x.Key != null && x.Value != null).ToDictionary(x => x.Key, x => x.Value as string),
                    Summary = GetStringValue(dict, "summary"),
                    Properties = ParseProperties(TryGetValue(dict, "properties") as List<object>),
                    Relationships = ParseRelationships(TryGetValue(dict, "relationships") as List<object>)
                });
            }
        }

        return result;
    }

    // Enum listesini ayrıştırma
    private static List<EnumDefinition> ParseEnums(List<object> enumsList)
    {
        if (enumsList == null)
            return [];

        var result = new List<EnumDefinition>();

        foreach (var item in enumsList)
        {
            if (item is Dictionary<string, object> dict)
            {
                var enumDef = new EnumDefinition
                {
                    AccessModifier = GetEnum<EnumAccessModifier>(dict, "accessModifier") ?? EnumAccessModifier.Public,
                    Name = GetStringValue(dict, "name"),
                    Namespace = GetStringValue(dict, "namespace") ?? ".",
                    // BaseType ayrıştırma
                    BaseType = GetEnum<EnumBaseType>(dict, "baseType") ?? EnumBaseType.Int
                };

                // Values ayrıştırma
                if (TryGetValue(dict, "values") is Dictionary<string, object> valuesDict)
                {
                    enumDef.Values = [];
                    foreach (var kvp in valuesDict)
                    {
                        enumDef.Values[kvp.Key] = kvp.Value;
                    }
                }

                result.Add(enumDef);
            }
        }

        return result;
    }

    // Property listesini ayrıştırma
    private static List<PropertyDefinition> ParseProperties(List<object> propertiesList)
    {
        if (propertiesList == null)
            return [];

        var result = new List<PropertyDefinition>();

        foreach (var item in propertiesList)
        {
            if (item is Dictionary<string, object> dict)
            {
                result.Add(new PropertyDefinition
                {
                    AccessModifier = GetEnum<PropertyAccessModifier>(dict, "accessModifier") ?? PropertyAccessModifier.Public,
                    Name = GetStringValue(dict, "name"),
                    Type = GetStringValue(dict, "type"),
                    Summary = GetStringValue(dict, "summary"),
                    SpecialType = GetEnum<PropertySpecialType>(dict, "specialType") ?? PropertySpecialType.None,
                    IsRequired = GetBoolValue(dict, "isRequired"),
                    DefaultValue = GetStringValue(dict, "defaultValue"),
                    MaxLength = GetIntValue(dict, "maxLength")
                });
            }
        }

        return result;
    }

    // Relationship listesini ayrıştırma
    private static List<RelationshipDefinition> ParseRelationships(List<object> relationshipsList)
    {
        if (relationshipsList == null)
            return [];

        var result = new List<RelationshipDefinition>();

        foreach (var item in relationshipsList)
        {
            if (item is Dictionary<string, object> dict)
            {
                result.Add(new RelationshipDefinition
                {
                    Name = GetStringValue(dict, "name"),
                    EntityType = GetStringValue(dict, "entityType"),
                    Type = GetStringValue(dict, "type"),
                    IsCollection = GetBoolValue(dict, "isCollection"),
                    Summary = GetStringValue(dict, "summary"),
                    ForeignKeyEntity = GetStringValue(dict, "foreignKeyEntity"),
                    ForeignKeyProperty = GetStringValue(dict, "foreignKeyProperty")
                });
            }
        }

        return result;
    }
}