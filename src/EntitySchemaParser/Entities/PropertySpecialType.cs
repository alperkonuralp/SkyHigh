namespace SkyHigh.EntitySchemaParser.Entities;

public enum PropertySpecialType
{
    None,
    PrimaryKey,
    CreatedAt,
    ModifiedAt,
    IsDeleted,
    ConcurrencyToken,
}