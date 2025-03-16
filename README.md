# SkyHigh Source Generators

This GitHub project contains multiple source code generators. With these tools, you will be able to prepare your projects easily and quickly.

## Documentation

Comprehensive documentation for the SkyHigh framework is available in the `docs` folder:

- [EntitySchema Documentation](docs/EntitySchema.md) - Overview of the EntitySchema system, its components, and usage
- [EntitySchema JSON Format Guide](docs/EntitySchemaJsonFormat.md) - Detailed reference for the EntitySchema JSON format with examples and best practices
- [Demo Solution Guide](docs/DemoSolution.md) - In-depth guide to the comprehensive Demo Solution that showcases all generators working together

## Projects

### Demo Solution (Main Example)
- **Solution File**: `src/Demo/SkyHigh.Demo.sln`
- **Description**: This is a special comprehensive demo solution that showcases all generator projects integrated together with NuGet packages. This is the best starting point to understand how all components work together.
- **Documentation**: [Demo Solution Guide](docs/DemoSolution.md)
- **Projects**:
  - **SkyHigh.Demo**:
    - **Project File**: `SkyHigh.Demo.csproj`
    - **Description**: Sample application demonstrating the integration of all generators using NuGet packages.
    - **Features**:
      - Complete entity schema implementation
      - POCO entity generation
      - Entity Framework integration
      - OData controllers

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

### Entity Framework Integration
- **Solution File**: `SkyHigh.EF.sln`
- **Projects**:
  - **SkyHigh.EF.Generator**:
    - **Project File**: `SkyHigh.EF.Generator.csproj`
    - **Description**: This project generates Entity Framework Core configurations and DbContext based on entity schema definitions.
    - **Dependencies**:
      - `Microsoft.CodeAnalysis.CSharp`
      - `Microsoft.CodeAnalysis.Analyzers`
  - **SkyHigh.EF.Demo**:
    - **Project File**: `SkyHigh.EF.Demo.csproj`
    - **Description**: Demo application demonstrating the usage of generated Entity Framework configurations.

### OData
- **Description**: This project contains OData integration for schema-based entities.
- **Path**: `src/OData/`

## Getting Started

To get started with SkyHigh Source Generators:

1. Define your domain model using the EntitySchema JSON format (see documentation).
2. Reference the appropriate generator project in your solution.
3. Set your schema file's build action to "AdditionalFiles".
4. Build your project, and the generators will produce the necessary code.

Refer to the Demo projects in each solution for examples of how to set up and use the generators.

## License

This project is licensed under the GNU General Public License v3.0 (GPL-3.0) - see the [LICENSE](LICENSE) file for details.

Copyright (C) 2024 SkyHigh Contributors

This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

