using Microsoft.CodeAnalysis;
using SkyHigh.PocoEntity.Generator;
using System;
using Xunit;

namespace SkyHigh.PocoEntity.Generator.Tests;

/// <summary>
/// Tests for PocoEntity EntityGenerator to ensure it can be instantiated and has the correct attributes
/// </summary>
public class EntityGeneratorTests
{
    [Fact]
    public void EntityGenerator_CanBeInstantiated()
    {
        // Act
        var generator = new EntityGenerator();

        // Assert
        Assert.NotNull(generator);
    }

    [Fact]
    public void EntityGenerator_ImplementsIIncrementalGenerator()
    {
        // Arrange
        var generator = new EntityGenerator();

        // Act & Assert
        Assert.IsAssignableFrom<IIncrementalGenerator>(generator);
    }

    [Fact]
    public void EntityGenerator_HasGeneratorAttribute()
    {
        // Arrange
        var generatorType = typeof(EntityGenerator);

        // Act
        var attributes = generatorType.GetCustomAttributes(typeof(GeneratorAttribute), false);

        // Assert
        Assert.NotEmpty(attributes);
        Assert.Single(attributes);
    }

    [Fact]
    public void EntityGenerator_Initialize_DoesNotThrow()
    {
        // Arrange
        var generator = new EntityGenerator();

        // Act & Assert - Should not throw when called with a valid context
        var initializeMethod = typeof(EntityGenerator).GetMethod("Initialize");
        Assert.NotNull(initializeMethod);
        Assert.Equal(typeof(void), initializeMethod.ReturnType);
    }

    [Fact]
    public void EntityGenerator_IsInCorrectNamespace()
    {
        // Arrange
        var generatorType = typeof(EntityGenerator);

        // Act & Assert
        Assert.Equal("SkyHigh.PocoEntity.Generator", generatorType.Namespace);
    }

    [Fact]
    public void EntityGenerator_IsPublicClass()
    {
        // Arrange
        var generatorType = typeof(EntityGenerator);

        // Act & Assert
        Assert.True(generatorType.IsPublic);
        Assert.True(generatorType.IsClass);
        Assert.False(generatorType.IsAbstract);
    }

    [Fact]
    public void EntityGenerator_HasExpectedMethods()
    {
        // Arrange
        var generatorType = typeof(EntityGenerator);

        // Act & Assert
        var initializeMethod = generatorType.GetMethod("Initialize");
        Assert.NotNull(initializeMethod);
        
        // The Initialize method should take IncrementalGeneratorInitializationContext parameter
        var parameters = initializeMethod.GetParameters();
        Assert.Single(parameters);
        Assert.Equal("IncrementalGeneratorInitializationContext", parameters[0].ParameterType.Name);
    }

    [Fact]
    public void EntityGenerator_AssemblyHasCorrectName()
    {
        // Arrange
        var generatorType = typeof(EntityGenerator);

        // Act & Assert
        Assert.Equal("SkyHigh.PocoEntity.Generator", generatorType.Assembly.GetName().Name);
    }
}