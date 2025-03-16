using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SkyHigh.EntitySchemaParser;

internal static class SimpleJsonParser
{
    // Dictionary'den güvenli şekilde değer alma
    internal static object TryGetValue(Dictionary<string, object> dict, string key)
    {
        if (dict != null && dict.TryGetValue(key, out var value))
            return value;
        return null;
    }

    // JSON değerini ayrıştıran ana metot
    internal static object ParseJsonValue(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return null;

        json = json.Trim();

        // Null değer kontrolü
        if (json == "null")
            return null;

        // Boolean değer kontrolü
        if (json == "true")
            return true;
        if (json == "false")
            return false;

        // Sayı kontrolü
        if (decimal.TryParse(json, out decimal number))
            return number;

        // String kontrolü
        if (json.StartsWith("\"") && json.EndsWith("\""))
            return json.Substring(1, json.Length - 2);

        // Array kontrolü
        if (json.StartsWith("[") && json.EndsWith("]"))
            return ParseJsonArray(json);

        // Object kontrolü
        if (json.StartsWith("{") && json.EndsWith("}"))
            return ParseJsonObject(json);

        // Tanımlanamayan değer
        return json;
    }

    // JSON nesnesini ayrıştırma
    internal static Dictionary<string, object> ParseJsonObject(string json)
    {
        var result = new Dictionary<string, object>();
#pragma warning disable S125 // Sections of code should not be commented out
        if (json.Length <= 2) // Boş nesne: {}
            return result;
#pragma warning restore S125 // Sections of code should not be commented out

        // İç kısımları ayıklama: {} parantezlerini kaldırma
        var content = json.Substring(1, json.Length - 2).Trim();

        // Key-value çiftlerini ayrıştırma
        var pairs = SplitJsonPairs(content);

        foreach (var pair in pairs)
        {
            // Key ve value ayırma
            var colonIndex = FindUnquotedChar(pair, ':');
            if (colonIndex < 0)
                continue;

            var key = pair.Substring(0, colonIndex).Trim();
            var value = pair.Substring(colonIndex + 1).Trim();

            // Key'den tırnak işaretlerini kaldırma
            if (key.StartsWith("\"") && key.EndsWith("\""))
                key = key.Substring(1, key.Length - 2);

            // Value'yu ayrıştırma
            result[key] = ParseJsonValue(value);
        }

        return result;
    }

    // JSON array'ini ayrıştırma
    internal static List<object> ParseJsonArray(string json)
    {
        var result = new List<object>();
        if (json.Length <= 2) // Boş dizi: []
            return result;

        // İç kısımları ayıklama: [] parantezlerini kaldırma
        var content = json.Substring(1, json.Length - 2).Trim();

        // Dizi elemanlarını ayrıştırma
        var elements = SplitJsonElements(content);

        foreach (var element in elements)
        {
            if (!string.IsNullOrWhiteSpace(element))
                result.Add(ParseJsonValue(element.Trim()));
        }

        return result;
    }

    // JSON string içinde virgülle ayrılmış elemanları bölme
    private static List<string> SplitJsonElements(string json)
    {
        var result = new List<string>();
        int startIndex = 0;
        bool inQuotes = false;
        int bracketLevel = 0;
        int braceLevel = 0;

        for (int i = 0; i < json.Length; i++)
        {
            var c = json[i];

            // Tırnak işareti kontrolü (escape edilmemiş)
            if (c == '"' && (i == 0 || json[i - 1] != '\\'))
                inQuotes = !inQuotes;

            // İç içe geçmiş yapı seviyesi kontrolü
            if (!inQuotes)
            {
                if (c == '{') braceLevel++;
                if (c == '}') braceLevel--;
                if (c == '[') bracketLevel++;
                if (c == ']') bracketLevel--;

                // Virgül bulunduğunda ve en üst seviyedeyken elemanı ayır
                if (c == ',' && bracketLevel == 0 && braceLevel == 0)
                {
                    result.Add(json.Substring(startIndex, i - startIndex).Trim());
                    startIndex = i + 1;
                }
            }
        }

        // Son elemanı ekle
        if (startIndex < json.Length)
            result.Add(json.Substring(startIndex).Trim());

        return result;
    }

    // JSON string içinde key-value çiftlerini bölme
    private static List<string> SplitJsonPairs(string json)
    {
        return SplitJsonElements(json);
    }

    // JSON string içinde tırnak işaretleri dışındaki belirli bir karakteri bulma
    private static int FindUnquotedChar(string text, char target)
    {
        bool inQuotes = false;
        for (int i = 0; i < text.Length; i++)
        {
            var c = text[i];

            // Tırnak işareti kontrolü (escape edilmemiş)
            if (c == '"' && (i == 0 || text[i - 1] != '\\'))
                inQuotes = !inQuotes;

            // Tırnak işaretleri dışındayken hedef karakteri bul
            if (!inQuotes && c == target)
                return i;
        }
        return -1;
    }

    // Dictionary'den string değeri güvenli şekilde alma
    internal static string GetStringValue(Dictionary<string, object> dict, string key)
    {
        if (dict != null && dict.TryGetValue(key, out var value))
            return value?.ToString();
        return null;
    }

    // Dictionary'den string değeri güvenli şekilde alma
    internal static bool? GetBooleanValue(Dictionary<string, object> dict, string key)
    {
        if (dict != null && dict.TryGetValue(key, out var value) && value is bool b)
            return b;
        return null;
    }

    // Bool değeri güvenli şekilde alma
    internal static bool GetBoolValue(Dictionary<string, object> dict, string key, bool defaultValue = false)
    {
        if (dict != null && dict.TryGetValue(key, out var value))
        {
            if (value is bool boolValue)
                return boolValue;

            if (value is string strValue && bool.TryParse(strValue, out bool result))
                return result;
        }
        return defaultValue;
    }

    internal static T? GetEnum<T>(Dictionary<string, object> dict, string key)
        where T : struct
    {
        if (dict != null && dict.TryGetValue(key, out var value))
        {
            if (value is T enumValue)
                return enumValue;
            if (value is string strValue && Enum.TryParse(strValue, true, out T result))
                return result;
        }
        return null;
    }

    // Int değeri güvenli şekilde alma
    internal static int? GetIntValue(Dictionary<string, object> dict, string key)
    {
        if (dict != null && dict.TryGetValue(key, out var value))
        {
            if (value is int intValue)
                return intValue;

            if (value is decimal decValue)
                return (int)decValue;

            if (value is string strValue && int.TryParse(strValue, out int result))
                return result;
        }
        return null;
    }

    // String listesi ayrıştırma
    internal static List<string> ParseStringList(List<object> stringList)
    {
        if (stringList == null)
            return [];

        return [.. stringList.Select(item => item?.ToString()).Where(s => s != null)];
    }
}