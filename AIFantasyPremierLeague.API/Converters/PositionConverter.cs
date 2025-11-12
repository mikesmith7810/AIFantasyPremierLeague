using System.Text.Json;
using System.Text.Json.Serialization;

namespace AIFantasyPremierLeague.API.Converters;

public class PositionConverter : JsonConverter<string>
{
    private static readonly Dictionary<int, string> PositionMap = new()
    {
        { 1, "GK" },
        { 2, "DEF" },
        { 3, "MID" },
        { 4, "ATT" }
    };

    private static readonly Dictionary<string, int> ReversePositionMap = new()
    {
        { "GK", 1 },
        { "DEF", 2 },
        { "MID", 3 },
        { "ATT", 4 }
    };

    public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Number)
        {
            int elementType = reader.GetInt32();
            if (PositionMap.TryGetValue(elementType, out string? position))
            {
                return position;
            }
            throw new JsonException($"Unknown element_type value: {elementType}");
        }

        if (reader.TokenType == JsonTokenType.String)
        {
            string? value = reader.GetString();
            return value ?? throw new JsonException("Position value cannot be null");
        }

        throw new JsonException($"Expected number or string for element_type, got {reader.TokenType}");
    }

    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
    {
        if (ReversePositionMap.TryGetValue(value, out int elementType))
        {
            writer.WriteNumberValue(elementType);
        }
        else
        {
            throw new JsonException($"Unknown position value: {value}");
        }
    }
}