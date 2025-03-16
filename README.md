# SkyHigh Source Generators

This GitHub project contains multiple source code generators. With these tools, you will be able to prepare your projects easily and quickly.

## Projects

### EntitySchemaParser
- **Project File**: `SkyHigh.EntitySchemaParser.csproj`
- **Description**: This project is a .NET Standard 2.0 library that includes various entity schema parsing functionalities. It uses Roslyn for code analysis and generation.
- **Dependencies**:
  - `Microsoft.CodeAnalysis.CSharp`
  - `Microsoft.CodeAnalysis.Analyzers`

### PocoEntity
- **Solution File**: `SkyHigh.PocoEntity.sln`
- **Projects**:
  - **SkyHigh.PocoEntity.Generator**:
    - **Project File**: `SkyHigh.PocoEntity.Generator.csproj`
    - **Description**: This project is responsible for generating POCO (Plain Old CLR Object) entities. It also uses Roslyn for code analysis and generation.
    - **Dependencies**:
      - `Microsoft.CodeAnalysis.CSharp`
      - `Microsoft.CodeAnalysis.Analyzers`
      - `Scriban`
  - **SkyHigh.PocoEntity.Demo**:
    - **Project File**: `SkyHigh.PocoEntity.Demo.csproj`
    - **Description**: This project serves as a demo application to showcase the usage of the generated POCO entities.

