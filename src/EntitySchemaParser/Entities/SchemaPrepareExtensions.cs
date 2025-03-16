using Microsoft.CodeAnalysis;
using System.IO;
using System;

namespace SkyHigh.EntitySchemaParser.Entities;

internal static class SchemaPrepareExtensions
{
    private const string FILE_NAME = "entitySchema.json";

    public static IncrementalValuesProvider<EntitySchema> Prepare(this IncrementalGeneratorInitializationContext context, string fileName = FILE_NAME)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            fileName = FILE_NAME;
        }

        // AdditionalFiles üzerinden JSON şemayı okuma
        var schemaProvider = context.AdditionalTextsProvider
            .Where(file => Path.GetFileName(file.Path).Equals(fileName, StringComparison.OrdinalIgnoreCase))
            .Select((file, cancellationToken) => file.GetText(cancellationToken)?.ToString())
            .Where(content => !string.IsNullOrEmpty(content));

        // JSON deserialize
        IncrementalValuesProvider<EntitySchema> schema = schemaProvider.Select((content, _) =>
        {
            try
            {
                return new EntitySchema().Parse(content);
            }
            catch (Exception)
            {
                // Hata durumunda diagnostic mesaj ekle
                return (EntitySchema)null;
            }
        });

        return schema;
    }
}