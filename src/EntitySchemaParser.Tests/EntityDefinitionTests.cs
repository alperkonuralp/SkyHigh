using Xunit;
using SkyHigh.EntitySchemaParser.Entities;
using System.Collections.Generic;

namespace SkyHigh.EntitySchemaParser.Tests;

public class EntitySchemaTests
{
    [Fact]
    public void EntitySchema_DefaultConstructor_InitializesCollections()
    {
        // Act
        var schema = new EntitySchema();

        // Assert
        Assert.NotNull(schema);
        Assert.Null(schema.BaseTypes);
        Assert.Null(schema.Interfaces);
        Assert.Null(schema.Entities);
        Assert.Null(schema.Enums);
    }

    [Fact]
    public void EntitySchema_SetProperties_StoresValuesCorrectly()
    {
        // Arrange
        var schema = new EntitySchema();
        var baseTypes = new List<ClassDefinition>();
        var interfaces = new List<InterfaceDefinition>();
        var entities = new List<ClassDefinition>();
        var enums = new List<EnumDefinition>();

        // Act
        schema.Name = "TestSchema";
        schema.Version = "1.0.0";
        schema.Description = "Test description";
        schema.BaseNamespace = "Test.Namespace";
        schema.BaseTypes = baseTypes;
        schema.Interfaces = interfaces;
        schema.Entities = entities;
        schema.Enums = enums;

        // Assert
        Assert.Equal("TestSchema", schema.Name);
        Assert.Equal("1.0.0", schema.Version);
        Assert.Equal("Test description", schema.Description);
        Assert.Equal("Test.Namespace", schema.BaseNamespace);
        Assert.Same(baseTypes, schema.BaseTypes);
        Assert.Same(interfaces, schema.Interfaces);
        Assert.Same(entities, schema.Entities);
        Assert.Same(enums, schema.Enums);
    }
}

public class ClassDefinitionTests
{
    [Fact]
    public void ClassDefinition_DefaultConstructor_InitializesWithDefaults()
    {
        // Act
        var classDefinition = new ClassDefinition();

        // Assert
        Assert.Equal(ClassAccessModifier.Public, classDefinition.AccessModifier);
        Assert.Null(classDefinition.Name);
        Assert.Equal(".", classDefinition.Namespace);
        Assert.Null(classDefinition.BaseType);
        Assert.NotNull(classDefinition.Interfaces);
        Assert.Empty(classDefinition.Interfaces);
        Assert.Equal(SpecialType.None, classDefinition.SpecialType);
        Assert.Null(classDefinition.Summary);
        Assert.False(classDefinition.IsAbstract);
        Assert.False(classDefinition.IsSealed);
        Assert.False(classDefinition.IsPartial);
        Assert.NotNull(classDefinition.TypeParameters);
        Assert.Empty(classDefinition.TypeParameters);
        Assert.NotNull(classDefinition.TypeFilters);
        Assert.Empty(classDefinition.TypeFilters);
        Assert.Null(classDefinition.Properties);
        Assert.Null(classDefinition.Relationships);
    }

    [Fact]
    public void ClassDefinition_SetProperties_StoresValuesCorrectly()
    {
        // Arrange
        var classDefinition = new ClassDefinition();
        var interfaces = new List<string> { "IInterface1", "IInterface2" };
        var typeParameters = new List<string> { "T", "U" };
        var typeFilters = new Dictionary<string, string> { { "T", "class" }, { "U", "struct" } };
        var properties = new List<PropertyDefinition>();
        var relationships = new List<RelationshipDefinition>();

        // Act
        classDefinition.AccessModifier = ClassAccessModifier.Internal;
        classDefinition.Name = "TestClass";
        classDefinition.Namespace = "Test.Namespace";
        classDefinition.BaseType = "BaseClass";
        classDefinition.Interfaces = interfaces;
        classDefinition.SpecialType = SpecialType.Tenant;
        classDefinition.Summary = "Test summary";
        classDefinition.IsAbstract = true;
        classDefinition.IsSealed = true;
        classDefinition.IsPartial = true;
        classDefinition.TypeParameters = typeParameters;
        classDefinition.TypeFilters = typeFilters;
        classDefinition.Properties = properties;
        classDefinition.Relationships = relationships;

        // Assert
        Assert.Equal(ClassAccessModifier.Internal, classDefinition.AccessModifier);
        Assert.Equal("TestClass", classDefinition.Name);
        Assert.Equal("Test.Namespace", classDefinition.Namespace);
        Assert.Equal("BaseClass", classDefinition.BaseType);
        Assert.Same(interfaces, classDefinition.Interfaces);
        Assert.Equal(SpecialType.Tenant, classDefinition.SpecialType);
        Assert.Equal("Test summary", classDefinition.Summary);
        Assert.True(classDefinition.IsAbstract);
        Assert.True(classDefinition.IsSealed);
        Assert.True(classDefinition.IsPartial);
        Assert.Same(typeParameters, classDefinition.TypeParameters);
        Assert.Same(typeFilters, classDefinition.TypeFilters);
        Assert.Same(properties, classDefinition.Properties);
        Assert.Same(relationships, classDefinition.Relationships);
    }
}

public class PropertyDefinitionTests
{
    [Fact]
    public void PropertyDefinition_DefaultConstructor_InitializesWithDefaults()
    {
        // Act
        var property = new PropertyDefinition();

        // Assert
        Assert.Equal(PropertyAccessModifier.Public, property.AccessModifier);
        Assert.Null(property.Name);
        Assert.Null(property.Type);
        Assert.Null(property.Summary);
        Assert.Equal(PropertySpecialType.None, property.SpecialType);
        Assert.False(property.IsRequired);
        Assert.Null(property.DefaultValue);
        Assert.Null(property.MaxLength);
    }

    [Fact]
    public void PropertyDefinition_SetProperties_StoresValuesCorrectly()
    {
        // Arrange
        var property = new PropertyDefinition();

        // Act
        property.AccessModifier = PropertyAccessModifier.Protected;
        property.Name = "TestProperty";
        property.Type = "string";
        property.Summary = "Test property summary";
        property.SpecialType = PropertySpecialType.PrimaryKey;
        property.IsRequired = true;
        property.DefaultValue = "default";
        property.MaxLength = 100;

        // Assert
        Assert.Equal(PropertyAccessModifier.Protected, property.AccessModifier);
        Assert.Equal("TestProperty", property.Name);
        Assert.Equal("string", property.Type);
        Assert.Equal("Test property summary", property.Summary);
        Assert.Equal(PropertySpecialType.PrimaryKey, property.SpecialType);
        Assert.True(property.IsRequired);
        Assert.Equal("default", property.DefaultValue);
        Assert.Equal(100, property.MaxLength);
    }
}