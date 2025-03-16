# SkyHigh.PocoEntity.Generator

A .NET source generator that creates POCO entity classes based on JSON schema definitions.

## Features

- Generates entity classes from JSON schema
- Creates interfaces for entity contracts
- Supports enum generation with different base types
- Handles relationships between entities
- Supports inheritance and interfaces

## Installation

```shell
dotnet add package SkyHigh.PocoEntity.Generator
```

## Usage

1. Add a reference to the package in your project:

```xml
<PackageReference Include="SkyHigh.PocoEntity.Generator" Version="1.0.0" OutputItemType="Analyzer" ReferenceOutputAssembly="false" />
```

2. Create an `entitySchema.json` file in your project root with your entity definitions.

3. The source generator will automatically create class files for your entities at compile time.

## Schema Example

```json
{
  "name": "YourProject",
  "version": "1.0.0",
  "baseNamespace": "YourNamespace",
  "entities": [
    {
      "name": "User",
      "namespace": ".Entities",
      "properties": [
        {
          "name": "Id",
          "type": "Guid",
          "isRequired": true
        },
        {
          "name": "Name",
          "type": "string",
          "maxLength": 100
        }
      ]
    }
  ]
}
```

## License

MIT