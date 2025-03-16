# SkyHigh Demo Solution

The Demo Solution (`src/Demo/SkyHigh.Demo.sln`) is a comprehensive example that showcases how all SkyHigh generator components work together. This solution uses NuGet packages to integrate all the source generators, providing a complete end-to-end example of schema-driven development.

## Overview

The Demo Solution demonstrates how to:

1. Define an entity schema using JSON
2. Generate POCO entities from the schema
3. Set up Entity Framework Core with generated configurations
4. Create OData controllers for your entities
5. Run a fully functional API based on your schema

## Project Structure

- **SkyHigh.Demo** (`SkyHigh.Demo.csproj`)
  - Main application project that references all the required NuGet packages
  - Contains the entity schema definition in `entitySchema.json`
  - Includes a sample weather forecast controller for demonstration

## Entity Schema

The Demo Solution includes a sample entity schema file (`entitySchema.json`) that defines:

- Domain entities with properties and relationships
- Enumerations
- Base types and interfaces

This schema file serves as the single source of truth for the entire application. All code generation is based on this schema.

## Integration with Source Generators

The Demo Solution is configured to use the following source generators via NuGet packages:

1. **POCO Entity Generator**
   - Generates C# class files for all entities defined in the schema
   - Creates properly typed properties and relationships
   - Adds appropriate annotations and documentation

2. **Entity Framework Generator**
   - Creates EF Core entity configurations
   - Generates DbContext with proper entity mappings
   - Sets up relationships, keys, and indexes

3. **OData Generator**
   - Builds OData controllers for each entity
   - Creates proper routing and CRUD operations
   - Configures OData query options

## How It Works

1. When you build the solution, the source generators read the `entitySchema.json` file.
2. Each generator produces its respective code files:
   - Entity classes in the `Generated/Entities` folder
   - EF configurations in the `Generated/EntityFramework` folder
   - OData controllers in the `Generated/Controllers` folder
3. These generated files are included in the compilation and can be used just like hand-written code.

## Getting Started with the Demo

To run the Demo Solution:

1. Open `src/Demo/SkyHigh.Demo.sln` in Visual Studio or your preferred IDE
2. Build the solution (this triggers the source generators)
3. Run the application
4. Navigate to the Swagger UI at `/swagger` to test the API endpoints

## Customizing the Demo

You can customize the demo by modifying the `entitySchema.json` file:

1. Add new entities, properties, or relationships
2. Change data types or constraints
3. Add new enumerations
4. Rebuild the solution to regenerate all code

## Key Files

- `entitySchema.json` - The entity schema definition
- `Program.cs` - Application startup and service configuration
- `WeatherForecast.cs` - Example domain model (not generated)
- `Controllers/WeatherForecastController.cs` - Example controller (not generated)

## Benefits of the Schema-Driven Approach

The Demo Solution demonstrates several benefits of using SkyHigh's schema-driven approach:

1. **Single Source of Truth** - The entity schema defines your domain model in one place
2. **Consistency** - All generated code follows the same patterns and conventions
3. **Productivity** - Changes to the schema automatically propagate to all layers
4. **Maintainability** - Less boilerplate code to maintain
5. **Type Safety** - All relationships and properties are strongly typed

## Next Steps

After exploring the Demo Solution, you can:

1. Create your own entity schema based on your domain requirements
2. Start a new project and reference the SkyHigh NuGet packages
3. Configure your project to use the source generators with your schema
4. Extend the generated code with custom business logic

The Demo Solution serves as a reference implementation that you can use as a starting point for your own projects.