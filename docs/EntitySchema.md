# EntitySchema Documentation

## Overview
EntitySchema is a flexible metadata format used in the SkyHigh framework to define entities, their properties, relationships, and other structural elements. It provides a schema-driven approach for creating domain models that can be used for code generation, database mapping, and API integration.

## Core Components

### EntitySchema Class
The `EntitySchema` class is the root container for all entity definitions, interfaces, and enums. It provides metadata about the schema itself and contains collections of the various entity types.

Properties:
- `Name`: The name of the schema.
- `Version`: The schema version.
- `Description`: A description of the schema.
- `BaseNamespace`: The base namespace for generated code.
- `BaseTypes`: Base classes that entities can inherit from.
- `Interfaces`: Interface definitions in the schema.
- `Entities`: Class/entity definitions in the schema.
- `Enums`: Enum definitions in the schema.

### Class Definitions
A `ClassDefinition` represents an entity or class in the domain model. It defines properties, relationships, and other class-level metadata.

Key components:
- `AccessModifier`: Defines the class's visibility (Public, Internal).
- `Name`: The name of the class.
- `Namespace`: The specific namespace for this class.
- `BaseType`: The parent class if this class inherits from another class.
- `Interfaces`: List of interfaces this class implements.
- `SpecialType`: Indicates if the class has a special role (None, Tenant, User).
- `Summary`: Documentation for the class.
- `IsAbstract`, `IsSealed`, `IsPartial`: Class modifiers.
- `TypeParameters`: Generic type parameters if the class is generic.
- `TypeFilters`: Constraints for generic type parameters.
- `Properties`: Properties defined on the class.
- `Relationships`: Relationships with other entities.

### Interface Definitions
An `InterfaceDefinition` represents an interface that entities can implement.

Key components:
- `AccessModifier`: Defines the interface's visibility (Public, Internal).
- `Name`: The name of the interface.
- `Namespace`: The specific namespace for this interface.
- `Interfaces`: List of interfaces this interface extends.
- `IsPartial`: Whether the interface is marked as partial.
- `TypeParameters`: Generic type parameters if the interface is generic.
- `TypeFilters`: Constraints for generic type parameters.
- `Properties`: Properties defined by the interface.
- `Relationships`: Relationships defined by the interface.

### Property Definitions
A `PropertyDefinition` represents a property on a class or interface.

Key components:
- `AccessModifier`: Defines the property's visibility (Public, Internal, Protected, Private).
- `Name`: The name of the property.
- `Type`: The data type of the property.
- `Summary`: Documentation for the property.
- `SpecialType`: Indicates if the property has a special role (PrimaryKey, CreatedAt, ModifiedAt, etc.).
- `IsRequired`: Whether the property is required.
- `DefaultValue`: The property's default value, if any.
- `MaxLength`: Maximum length for string properties.

### Relationship Definitions
A `RelationshipDefinition` represents a relationship between entities.

Key components:
- `Name`: The name of the relationship property.
- `EntityType`: The related entity type.
- `Type`: The relationship type.
- `IsCollection`: Whether the relationship is a collection.
- `Summary`: Documentation for the relationship.
- `ForeignKeyName`: The name of the foreign key.
- `ForeignKeyEntity`: The entity containing the foreign key.
- `ForeignKeyProperty`: The foreign key property.

### Enum Definitions
An `EnumDefinition` represents an enumeration type.

Key components:
- `AccessModifier`: Defines the enum's visibility (Public, Internal).
- `Name`: The name of the enum.
- `Namespace`: The specific namespace for this enum.
- `BaseType`: The base type of the enum (Int, Byte, String, etc.).
- `Values`: Dictionary of name-value pairs defining the enum values.

## JSON Schema Format
EntitySchema is typically defined in a JSON format. Here's an example of the structure:

```json
{
  "name": "MyDomainModel",
  "version": "1.0.0",
  "description": "Example domain model for a business application",
  "baseNamespace": "MyCompany.Domain",
  "baseTypes": [
    {
      "name": "EntityBase",
      "namespace": "MyCompany.Domain.Base",
      "properties": [
        {
          "name": "Id",
          "type": "int",
          "specialType": "PrimaryKey",
          "summary": "The unique identifier for the entity."
        }
      ]
    }
  ],
  "entities": [
    {
      "name": "Customer",
      "baseType": "EntityBase",
      "specialType": "None",
      "properties": [
        {
          "name": "Name",
          "type": "string",
          "isRequired": true,
          "maxLength": 100
        },
        {
          "name": "Email",
          "type": "string",
          "isRequired": true
        }
      ],
      "relationships": [
        {
          "name": "Orders",
          "entityType": "Order",
          "isCollection": true,
          "foreignKeyEntity": "Order",
          "foreignKeyProperty": "CustomerId"
        }
      ]
    }
  ],
  "enums": [
    {
      "name": "OrderStatus",
      "values": {
        "Pending": 0,
        "Processing": 1,
        "Shipped": 2,
        "Delivered": 3,
        "Cancelled": 4
      }
    }
  ]
}
```

## Special Types

### SpecialType Enum
The `SpecialType` enum is used to mark classes that have special roles in the system:
- `None`: Regular entity without special behavior.
- `Tenant`: Entity that represents a tenant in multi-tenant applications.
- `User`: Entity that represents a user in the system.

### PropertySpecialType Enum
The `PropertySpecialType` enum is used to mark properties with special behavior:
- `None`: Regular property without special behavior.
- `PrimaryKey`: Identifies the property as the primary key.
- `CreatedAt`: Timestamp for when the entity was created.
- `ModifiedAt`: Timestamp for when the entity was last modified.
- `IsDeleted`: Flag for soft-delete functionality.
- `ConcurrencyToken`: Property used for concurrency control.

## Using EntitySchema

EntitySchema files can be used for:
1. Code generation (entities, DTOs, controllers)
2. Database schema generation
3. API documentation
4. Client code generation
5. Validation rule application

The `SkyHigh.EntitySchemaParser` library provides the tools to parse and process EntitySchema JSON files, making them available for these purposes.

## Integration with Source Generators

The EntitySchema system works particularly well with .NET Source Generators for compile-time code generation. Place the schema JSON file in your project with its "Build Action" set to "AdditionalFiles" to make it accessible to source generators.