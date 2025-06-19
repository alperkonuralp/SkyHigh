using Xunit;
using SkyHigh.EntitySchemaParser.Entities;
using System.Collections.Generic;

namespace SkyHigh.EntitySchemaParser.Tests;

/// <summary>
/// Tests for public entity model classes and their properties
/// </summary>
public class EntityModelTests
{
    [Fact]
    public void EntitySchema_Properties_GetAndSetCorrectly()
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

    [Fact]
    public void ClassDefinition_DefaultValues_AreCorrect()
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
    public void ClassDefinition_AllProperties_CanBeSetAndRetrieved()
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

    [Fact]
    public void PropertyDefinition_DefaultValues_AreCorrect()
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
    public void PropertyDefinition_AllProperties_CanBeSetAndRetrieved()
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

    [Fact]
    public void InterfaceDefinition_CanBeCreatedAndConfigured()
    {
        // Arrange
        var interfaceDefinition = new InterfaceDefinition();
        var interfaces = new List<string> { "IBaseInterface" };
        var typeParameters = new List<string> { "T" };
        var typeFilters = new Dictionary<string, string> { { "T", "class" } };
        var properties = new List<PropertyDefinition>();
        var relationships = new List<RelationshipDefinition>();

        // Act
        interfaceDefinition.AccessModifier = InterfaceAccessModifier.Public;
        interfaceDefinition.Name = "ITestInterface";
        interfaceDefinition.Namespace = "Test.Interfaces";
        interfaceDefinition.Interfaces = interfaces;
        interfaceDefinition.IsPartial = true;
        interfaceDefinition.TypeParameters = typeParameters;
        interfaceDefinition.TypeFilters = typeFilters;
        interfaceDefinition.Summary = "Test interface";
        interfaceDefinition.Properties = properties;
        interfaceDefinition.Relationships = relationships;

        // Assert
        Assert.Equal(InterfaceAccessModifier.Public, interfaceDefinition.AccessModifier);
        Assert.Equal("ITestInterface", interfaceDefinition.Name);
        Assert.Equal("Test.Interfaces", interfaceDefinition.Namespace);
        Assert.Same(interfaces, interfaceDefinition.Interfaces);
        Assert.True(interfaceDefinition.IsPartial);
        Assert.Same(typeParameters, interfaceDefinition.TypeParameters);
        Assert.Same(typeFilters, interfaceDefinition.TypeFilters);
        Assert.Equal("Test interface", interfaceDefinition.Summary);
        Assert.Same(properties, interfaceDefinition.Properties);
        Assert.Same(relationships, interfaceDefinition.Relationships);
    }

    [Fact]
    public void EnumDefinition_CanBeCreatedAndConfigured()
    {
        // Arrange
        var enumDefinition = new EnumDefinition();
        var values = new Dictionary<string, object>
        {
            { "Value1", 1 },
            { "Value2", 2 }
        };

        // Act
        enumDefinition.AccessModifier = EnumAccessModifier.Public;
        enumDefinition.Name = "TestEnum";
        enumDefinition.Namespace = "Test.Enums";
        enumDefinition.BaseType = EnumBaseType.Int;
        enumDefinition.Values = values;

        // Assert
        Assert.Equal(EnumAccessModifier.Public, enumDefinition.AccessModifier);
        Assert.Equal("TestEnum", enumDefinition.Name);
        Assert.Equal("Test.Enums", enumDefinition.Namespace);
        Assert.Equal(EnumBaseType.Int, enumDefinition.BaseType);
        Assert.Same(values, enumDefinition.Values);
    }

    [Fact]
    public void RelationshipDefinition_CanBeCreatedAndConfigured()
    {
        // Arrange
        var relationship = new RelationshipDefinition();

        // Act
        relationship.Name = "Orders";
        relationship.EntityType = "Order";
        relationship.Type = "List<Order>";
        relationship.IsCollection = true;
        relationship.Summary = "Customer orders";
        relationship.ForeignKeyEntity = "Customer";
        relationship.ForeignKeyProperty = "CustomerId";

        // Assert
        Assert.Equal("Orders", relationship.Name);
        Assert.Equal("Order", relationship.EntityType);
        Assert.Equal("List<Order>", relationship.Type);
        Assert.True(relationship.IsCollection);
        Assert.Equal("Customer orders", relationship.Summary);
        Assert.Equal("Customer", relationship.ForeignKeyEntity);
        Assert.Equal("CustomerId", relationship.ForeignKeyProperty);
    }

    [Theory]
    [InlineData(ClassAccessModifier.Public)]
    [InlineData(ClassAccessModifier.Internal)]
    public void ClassAccessModifier_AllValues_AreValid(ClassAccessModifier modifier)
    {
        // Arrange
        var classDefinition = new ClassDefinition();

        // Act
        classDefinition.AccessModifier = modifier;

        // Assert
        Assert.Equal(modifier, classDefinition.AccessModifier);
    }

    [Theory]
    [InlineData(PropertySpecialType.None)]
    [InlineData(PropertySpecialType.PrimaryKey)]
    [InlineData(PropertySpecialType.CreatedAt)]
    [InlineData(PropertySpecialType.ModifiedAt)]
    [InlineData(PropertySpecialType.IsDeleted)]
    [InlineData(PropertySpecialType.ConcurrencyToken)]
    public void PropertySpecialType_AllValues_AreValid(PropertySpecialType specialType)
    {
        // Arrange
        var property = new PropertyDefinition();

        // Act
        property.SpecialType = specialType;

        // Assert
        Assert.Equal(specialType, property.SpecialType);
    }

    [Theory]
    [InlineData(SpecialType.None)]
    [InlineData(SpecialType.Tenant)]
    [InlineData(SpecialType.User)]
    public void SpecialType_AllValues_AreValid(SpecialType specialType)
    {
        // Arrange
        var classDefinition = new ClassDefinition();

        // Act
        classDefinition.SpecialType = specialType;

        // Assert
        Assert.Equal(specialType, classDefinition.SpecialType);
    }

    [Theory]
    [InlineData(EnumBaseType.Byte)]
    [InlineData(EnumBaseType.SByte)]
    [InlineData(EnumBaseType.Short)]
    [InlineData(EnumBaseType.UShort)]
    [InlineData(EnumBaseType.Int)]
    [InlineData(EnumBaseType.UInt)]
    [InlineData(EnumBaseType.Long)]
    [InlineData(EnumBaseType.ULong)]
    public void EnumBaseType_AllValues_AreValid(EnumBaseType baseType)
    {
        // Arrange
        var enumDefinition = new EnumDefinition();

        // Act
        enumDefinition.BaseType = baseType;

        // Assert
        Assert.Equal(baseType, enumDefinition.BaseType);
    }
}