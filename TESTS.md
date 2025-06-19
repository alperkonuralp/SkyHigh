# SkyHigh Unit Tests

This directory contains comprehensive unit tests for the SkyHigh Source Generators project.

## Test Projects

### 1. EntitySchemaParser.Tests
**Location**: `src/EntitySchemaParser.Tests/`  
**Test Count**: 33 tests  
**Coverage**: Tests for entity model classes, property validation, and enum verification

**Key Test Areas**:
- EntitySchema property validation
- ClassDefinition default values and property setting
- PropertyDefinition configuration
- InterfaceDefinition functionality  
- EnumDefinition creation and configuration
- RelationshipDefinition properties
- Access modifier enums validation
- Property special type enums validation
- Enum base type validation

### 2. StaticProxy.Generator.Tests
**Location**: `src/StaticProxy.Generator.Tests/`  
**Test Count**: 6 tests  
**Coverage**: Tests for static proxy source generator functionality

**Key Test Areas**:
- Generator instantiation and interface implementation
- Generator attribute verification
- Namespace and class structure validation
- Public API accessibility
- Method existence verification

### 3. PocoEntity.Generator.Tests  
**Location**: `src/PocoEntity.Generator.Tests/`  
**Test Count**: 8 tests  
**Coverage**: Tests for POCO entity source generator functionality

**Key Test Areas**:
- Generator instantiation and interface implementation
- Generator attribute verification
- Initialize method validation
- Namespace and assembly verification
- Class structure and accessibility

### 4. EF.Generator.Tests
**Location**: `src/EF.Generator.Tests/`  
**Test Count**: 11 tests  
**Coverage**: Tests for Entity Framework source generator functionality  

**Key Test Areas**:
- Generator instantiation and interface implementation
- Generator attribute verification
- Code generation method validation
- Assembly reference verification
- Template usage validation

## Running Tests

### Run All Tests
```bash
# From the repository root
find src -name "*.Tests" -type d | xargs -I {} dotnet test {}
```

### Run Individual Test Projects
```bash
# EntitySchemaParser tests
dotnet test src/EntitySchemaParser.Tests/

# StaticProxy.Generator tests  
dotnet test src/StaticProxy.Generator.Tests/

# PocoEntity.Generator tests
dotnet test src/PocoEntity.Generator.Tests/

# EF.Generator tests
dotnet test src/EF.Generator.Tests/
```

### Build All Test Projects
```bash
# Build all test projects
find src -name "*.Tests" -type d | xargs -I {} dotnet build {}
```

## Test Summary

| Project | Tests | Status | Focus Area |
|---------|-------|--------|------------|
| EntitySchemaParser.Tests | 33 | ✅ Passing | Data model validation and entity schema parsing |
| StaticProxy.Generator.Tests | 6 | ✅ Passing | Static proxy source generator structure |
| PocoEntity.Generator.Tests | 8 | ✅ Passing | POCO entity source generator functionality |
| EF.Generator.Tests | 11 | ✅ Passing | Entity Framework source generator validation |
| **Total** | **58** | **✅ All Passing** | **Complete project coverage** |

## Test Architecture

The test projects follow these principles:

1. **Focused Testing**: Each test project focuses on its specific component without complex integration testing
2. **Public API Testing**: Tests focus on public interfaces and behavior rather than internal implementation details
3. **Structural Validation**: Tests verify that classes have correct attributes, namespaces, and expected methods
4. **Minimal Dependencies**: Test projects avoid complex dependencies to remain maintainable
5. **xUnit Framework**: All tests use xUnit as the testing framework for consistency

## Test Dependencies

- **Microsoft.NET.Test.Sdk**: Test platform
- **xunit**: Testing framework
- **xunit.runner.visualstudio**: Visual Studio test runner
- **coverlet.collector**: Code coverage collection
- **Microsoft.CodeAnalysis**: Required for testing source generators

## Notes

- Tests are designed to be fast and reliable
- Source generator testing is limited to structural validation due to complexity of Roslyn integration testing
- All tests target .NET 8.0 for consistency
- Test projects maintain the same CodeAnalysis package versions as their corresponding source projects to avoid conflicts