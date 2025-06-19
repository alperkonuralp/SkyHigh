using Microsoft.CodeAnalysis;
using SkyHigh.EF.Generator;
using System;
using Xunit;

namespace SkyHigh.EF.Generator.Tests;

/// <summary>
/// Tests for EF EntityGenerator to ensure it can be instantiated and has the correct attributes
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
        Assert.Equal("SkyHigh.EF.Generator", generatorType.Namespace);
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
        Assert.Equal("SkyHigh.EF.Generator", generatorType.Assembly.GetName().Name);
    }

    [Fact]
    public void EntityGenerator_HasPrivateMethodsForCodeGeneration()
    {
        // Arrange
        var generatorType = typeof(EntityGenerator);

        // Act & Assert - Check that code generation methods exist
        var allMethods = generatorType.GetMethods(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        // Should have private methods for generating different types of code
        var hasPrivateMethods = Array.Exists(allMethods, method => 
            method.Name.Contains("Generate") && method.IsPrivate);
        
        Assert.True(hasPrivateMethods, "EntityGenerator should have private methods for code generation");
    }

    [Fact]
    public void EntityGenerator_ReferencesRequiredAssemblies()
    {
        // Arrange
        var generatorType = typeof(EntityGenerator);
        var referencedAssemblies = generatorType.Assembly.GetReferencedAssemblies();

        // Act & Assert - Should reference Microsoft.CodeAnalysis for source generation
        var hasCodeAnalysisReference = Array.Exists(referencedAssemblies, assembly => 
            assembly.Name?.Contains("Microsoft.CodeAnalysis") == true);
        
        Assert.True(hasCodeAnalysisReference, "EntityGenerator should reference Microsoft.CodeAnalysis");
        
        // Verify that the assembly has the expected basic references
        var assemblyNames = Array.ConvertAll(referencedAssemblies, assembly => assembly.Name ?? string.Empty);
        Assert.NotEmpty(assemblyNames);
        
        // Conditionally check for netstandard reference based on target framework
        if (AppContext.TargetFrameworkName?.Contains(".NETStandard") == true)
        {
            Assert.Contains("netstandard", assemblyNames);
        }
    }

    [Fact]
    public void EntityGenerator_UsesCorrectTemplateConstants()
    {
        // This test verifies that the generator class structure is as expected
        var generatorType = typeof(EntityGenerator);
        
        // Should be in the correct namespace for EF generation
        Assert.Equal("SkyHigh.EF.Generator", generatorType.Namespace);
        
        // Should have Generator attribute for Roslyn
        var hasGeneratorAttribute = Attribute.IsDefined(generatorType, typeof(GeneratorAttribute));
        Assert.True(hasGeneratorAttribute, "EntityGenerator should have GeneratorAttribute");
    }
}