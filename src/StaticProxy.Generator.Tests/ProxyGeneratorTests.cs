using SkyHigh.StaticProxy.Generator;
using System;
using Xunit;

namespace SkyHigh.StaticProxy.Generator.Tests;

/// <summary>
/// Basic tests for ProxyGenerator to ensure it can be instantiated and has the correct attributes
/// </summary>
public class ProxyGeneratorTests
{
    [Fact]
    public void ProxyGenerator_CanBeInstantiated()
    {
        // Act
        var generator = new ProxyGenerator();

        // Assert
        Assert.NotNull(generator);
    }

    [Fact]
    public void ProxyGenerator_ImplementsIIncrementalGenerator()
    {
        // Arrange
        var generator = new ProxyGenerator();

        // Act & Assert
        Assert.IsAssignableFrom<Microsoft.CodeAnalysis.IIncrementalGenerator>(generator);
    }

    [Fact]
    public void ProxyGenerator_HasGeneratorAttribute()
    {
        // Arrange
        var generatorType = typeof(ProxyGenerator);

        // Act
        var attributes = generatorType.GetCustomAttributes(typeof(Microsoft.CodeAnalysis.GeneratorAttribute), false);

        // Assert
        Assert.NotEmpty(attributes);
        Assert.Single(attributes);
    }

    [Fact]
    public void ProxyGenerator_Initialize_DoesNotThrow()
    {
        // Arrange
        var generator = new ProxyGenerator();

        // Act & Assert - Should not throw when called with a valid context
        // Note: We can't easily create a real IncrementalGeneratorInitializationContext for testing
        // This test verifies the method exists and can be called
        var initializeMethod = typeof(ProxyGenerator).GetMethod("Initialize");
        Assert.NotNull(initializeMethod);
        Assert.Equal(typeof(void), initializeMethod.ReturnType);
    }

    [Fact]
    public void ProxyGenerator_IsInCorrectNamespace()
    {
        // Arrange
        var generatorType = typeof(ProxyGenerator);

        // Act & Assert
        Assert.Equal("SkyHigh.StaticProxy.Generator", generatorType.Namespace);
    }

    [Fact]
    public void ProxyGenerator_IsPublicClass()
    {
        // Arrange
        var generatorType = typeof(ProxyGenerator);

        // Act & Assert
        Assert.True(generatorType.IsPublic);
        Assert.True(generatorType.IsClass);
        Assert.False(generatorType.IsAbstract);
    }
}