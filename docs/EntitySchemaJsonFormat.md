# EntitySchema JSON Format Guide

This document provides a detailed reference for the EntitySchema JSON format, including examples and best practices for creating your own schema files.

## Basic Structure

An EntitySchema JSON file has the following top-level structure:

```json
{
  "name": "MySchema",
  "version": "1.0",
  "description": "My domain model description",
  "baseNamespace": "MyCompany.Domain",
  "baseTypes": [ /* base class definitions */ ],
  "interfaces": [ /* interface definitions */ ],
  "entities": [ /* entity class definitions */ ],
  "enums": [ /* enum definitions */ ]
}
```

## Detailed Elements

### Root Properties

| Property | Type | Description |
|----------|------|-------------|
| `name` | string | Name of the schema |
| `version` | string | Version of the schema, typically in SemVer format |
| `description` | string | Description of the schema and its purpose |
| `baseNamespace` | string | Base namespace for generated code |
| `baseTypes` | array | Array of base class definitions |
| `interfaces` | array | Array of interface definitions |
| `entities` | array | Array of entity class definitions |
| `enums` | array | Array of enum definitions |

### Class Definition

Both `baseTypes` and `entities` arrays contain class definitions with the following structure:

```json
{
  "accessModifier": "Public",
  "name": "Customer",
  "namespace": "MyCompany.Domain.Customers",
  "baseType": "EntityBase",
  "interfaces": ["IAuditable", "ISoftDeletable"],
  "specialType": "None",
  "summary": "Represents a customer in the system",
  "isAbstract": false,
  "isSealed": false,
  "isPartial": true,
  "typeParameters": ["TKey"],
  "typeFilters": {
    "TKey": "struct, IEquatable<TKey>"
  },
  "properties": [ /* property definitions */ ],
  "relationships": [ /* relationship definitions */ ]
}
```

| Property | Type | Description |
|----------|------|-------------|
| `accessModifier` | string | Class visibility: "Public" or "Internal" |
| `name` | string | Name of the class |
| `namespace` | string | Namespace for this class (defaults to "." which means use baseNamespace) |
| `baseType` | string | Base class name if this class inherits from another class |
| `interfaces` | array | List of interfaces this class implements |
| `specialType` | string | Special class role: "None", "Tenant", or "User" |
| `summary` | string | Documentation comment for the class |
| `isAbstract` | boolean | Whether the class is abstract |
| `isSealed` | boolean | Whether the class is sealed |
| `isPartial` | boolean | Whether the class is partial |
| `typeParameters` | array | Generic type parameters for the class |
| `typeFilters` | object | Generic type parameter constraints |
| `properties` | array | Property definitions for the class |
| `relationships` | array | Relationship definitions for the class |

### Interface Definition

The `interfaces` array contains interface definitions with the following structure:

```json
{
  "accessModifier": "Public",
  "name": "IAuditable",
  "namespace": "MyCompany.Domain.Common",
  "interfaces": ["IEntity"],
  "isPartial": false,
  "typeParameters": ["T"],
  "typeFilters": {
    "T": "class"
  },
  "summary": "Interface for entities that track creation and modification",
  "properties": [ /* property definitions */ ],
  "relationships": [ /* relationship definitions */ ]
}
```

| Property | Type | Description |
|----------|------|-------------|
| `accessModifier` | string | Interface visibility: "Public" or "Internal" |
| `name` | string | Name of the interface |
| `namespace` | string | Namespace for this interface (defaults to "." which means use baseNamespace) |
| `interfaces` | array | List of interfaces this interface extends |
| `isPartial` | boolean | Whether the interface is partial |
| `typeParameters` | array | Generic type parameters for the interface |
| `typeFilters` | object | Generic type parameter constraints |
| `summary` | string | Documentation comment for the interface |
| `properties` | array | Property definitions for the interface |
| `relationships` | array | Relationship definitions for the interface |

### Property Definition

The `properties` array contains property definitions with the following structure:

```json
{
  "accessModifier": "Public",
  "name": "FirstName",
  "type": "string",
  "summary": "The customer's first name",
  "specialType": "None",
  "isRequired": true,
  "defaultValue": "\"Unknown\"",
  "maxLength": 100
}
```

| Property | Type | Description |
|----------|------|-------------|
| `accessModifier` | string | Property visibility: "Public", "Internal", "Protected", or "Private" |
| `name` | string | Name of the property |
| `type` | string | Data type of the property |
| `summary` | string | Documentation comment for the property |
| `specialType` | string | Special property role: "None", "PrimaryKey", "CreatedAt", "ModifiedAt", "IsDeleted", "ConcurrencyToken" |
| `isRequired` | boolean | Whether the property is required (non-nullable) |
| `defaultValue` | string | Default value as a string literal |
| `maxLength` | number | Maximum length for string properties |

### Relationship Definition

The `relationships` array contains relationship definitions with the following structure:

```json
{
  "name": "Orders",
  "entityType": "Order",
  "type": "OneToMany",
  "isCollection": true,
  "summary": "Orders placed by this customer",
  "foreignKeyName": "CustomerId",
  "foreignKeyEntity": "Order",
  "foreignKeyProperty": "CustomerId"
}
```

| Property | Type | Description |
|----------|------|-------------|
| `name` | string | Name of the relationship property |
| `entityType` | string | The related entity type |
| `type` | string | Relationship type, e.g., "OneToOne", "OneToMany", "ManyToOne", "ManyToMany" |
| `isCollection` | boolean | Whether the relationship is a collection |
| `summary` | string | Documentation comment for the relationship |
| `foreignKeyName` | string | Name of the foreign key |
| `foreignKeyEntity` | string | Entity containing the foreign key |
| `foreignKeyProperty` | string | Property name of the foreign key |

### Enum Definition

The `enums` array contains enum definitions with the following structure:

```json
{
  "accessModifier": "Public",
  "name": "CustomerStatus",
  "namespace": "MyCompany.Domain.Customers",
  "baseType": "Int",
  "values": {
    "Active": 1,
    "Inactive": 2,
    "Suspended": 3,
    "Closed": 4
  }
}
```

| Property | Type | Description |
|----------|------|-------------|
| `accessModifier` | string | Enum visibility: "Public" or "Internal" |
| `name` | string | Name of the enum |
| `namespace` | string | Namespace for this enum (defaults to "." which means use baseNamespace) |
| `baseType` | string | Base type of the enum: "Byte", "SByte", "Short", "UShort", "Int", "UInt", "Long", "ULong", "String" |
| `values` | object | Dictionary of name-value pairs defining the enum values |

## Complete Example

Here's a complete example of an EntitySchema JSON file:

```json
{
  "name": "OnlineShopping",
  "version": "1.0.0",
  "description": "Domain model for an online shopping application",
  "baseNamespace": "OnlineShopping.Domain",
  
  "baseTypes": [
    {
      "name": "EntityBase",
      "namespace": "OnlineShopping.Domain.Base",
      "isAbstract": true,
      "properties": [
        {
          "name": "Id",
          "type": "int",
          "specialType": "PrimaryKey",
          "summary": "The unique identifier for the entity."
        },
        {
          "name": "CreatedAt",
          "type": "DateTimeOffset",
          "specialType": "CreatedAt",
          "summary": "When the entity was created"
        },
        {
          "name": "ModifiedAt",
          "type": "DateTimeOffset?",
          "specialType": "ModifiedAt",
          "summary": "When the entity was last modified"
        },
        {
          "name": "IsDeleted",
          "type": "bool",
          "specialType": "IsDeleted",
          "defaultValue": "false",
          "summary": "Soft delete flag"
        }
      ]
    }
  ],
  
  "interfaces": [
    {
      "name": "IAuditable",
      "properties": [
        {
          "name": "CreatedBy",
          "type": "string",
          "summary": "User who created this entity"
        },
        {
          "name": "ModifiedBy",
          "type": "string",
          "isRequired": false,
          "summary": "User who last modified this entity"
        }
      ]
    }
  ],
  
  "entities": [
    {
      "name": "Customer",
      "baseType": "EntityBase",
      "interfaces": ["IAuditable"],
      "specialType": "None",
      "summary": "Represents a customer who can place orders",
      "properties": [
        {
          "name": "FirstName",
          "type": "string",
          "isRequired": true,
          "maxLength": 50,
          "summary": "Customer's first name"
        },
        {
          "name": "LastName",
          "type": "string",
          "isRequired": true,
          "maxLength": 50,
          "summary": "Customer's last name"
        },
        {
          "name": "Email",
          "type": "string",
          "isRequired": true,
          "summary": "Customer's email address"
        },
        {
          "name": "Status",
          "type": "CustomerStatus",
          "isRequired": true,
          "defaultValue": "CustomerStatus.Active",
          "summary": "Current status of the customer"
        }
      ],
      "relationships": [
        {
          "name": "Orders",
          "entityType": "Order",
          "type": "OneToMany",
          "isCollection": true,
          "summary": "Orders placed by this customer",
          "foreignKeyEntity": "Order",
          "foreignKeyProperty": "CustomerId"
        },
        {
          "name": "DefaultAddress",
          "entityType": "Address",
          "type": "OneToOne",
          "isCollection": false,
          "summary": "Customer's default address",
          "foreignKeyName": "DefaultAddressId"
        }
      ]
    },
    
    {
      "name": "Order",
      "baseType": "EntityBase",
      "interfaces": ["IAuditable"],
      "properties": [
        {
          "name": "OrderNumber",
          "type": "string",
          "isRequired": true,
          "summary": "Unique order number"
        },
        {
          "name": "OrderDate",
          "type": "DateTimeOffset",
          "isRequired": true,
          "summary": "When the order was placed"
        },
        {
          "name": "Status",
          "type": "OrderStatus",
          "isRequired": true,
          "defaultValue": "OrderStatus.Pending",
          "summary": "Current status of the order"
        },
        {
          "name": "TotalAmount",
          "type": "decimal",
          "isRequired": true,
          "summary": "Total order amount"
        },
        {
          "name": "CustomerId",
          "type": "int",
          "isRequired": true,
          "summary": "Foreign key to the customer"
        }
      ],
      "relationships": [
        {
          "name": "Customer",
          "entityType": "Customer",
          "type": "ManyToOne",
          "isCollection": false,
          "summary": "The customer who placed this order",
          "foreignKeyProperty": "CustomerId"
        },
        {
          "name": "OrderItems",
          "entityType": "OrderItem",
          "type": "OneToMany",
          "isCollection": true,
          "summary": "Items in this order",
          "foreignKeyEntity": "OrderItem",
          "foreignKeyProperty": "OrderId"
        }
      ]
    }
  ],
  
  "enums": [
    {
      "name": "CustomerStatus",
      "baseType": "Int",
      "values": {
        "Active": 1,
        "Inactive": 2,
        "Suspended": 3,
        "Closed": 4
      }
    },
    {
      "name": "OrderStatus",
      "baseType": "Int",
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

## Best Practices

1. **Be Consistent** - Use consistent naming conventions for all entities, properties, and relationships.

2. **Use SpecialTypes** - Leverage `specialType` properties to define standard behavior like primary keys and audit fields.

3. **Document Everything** - Provide summary comments for all classes, properties, and relationships.

4. **Use Namespaces Wisely** - Define specific namespaces for logical groupings of entities.

5. **Define Base Types** - Create base types for common properties to avoid duplication.

6. **Plan Relationships Carefully** - Define relationships with proper foreign keys and navigation properties.

7. **Keep It Simple** - Start with a minimal schema and expand as needed.

## Parser Implementation

The `SkyHigh.EntitySchemaParser` library provides a simple JSON parser that parses the schema JSON into a strongly-typed object model. The parsing is done using the `SimpleJsonParser` class in combination with extension methods like `Parse` in the `ParserExtensions` class.