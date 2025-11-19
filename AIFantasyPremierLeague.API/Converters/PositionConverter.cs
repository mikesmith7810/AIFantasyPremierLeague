using System.Text.Json;
using System.Text.Json.Serialization;
using AIFantasyPremierLeague.API.Models;

namespace AIFantasyPremierLeague.API.Converters;

public class PositionConverter : JsonConverter<Position>
{
    public override Position Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Number)
        {
            var elementType = reader.GetInt32();
            return elementType switch
            {
                1 => Position.GK,
                2 => Position.DEF,
                3 => Position.MID,
                4 => Position.ATT,
                _ => throw new JsonException($"Unknown position element_type: {elementType}")
            };
        }

        if (reader.TokenType == JsonTokenType.String)
        {
            var positionString = reader.GetString()?.ToUpperInvariant();
            return positionString switch
            {
                "GK" or "GOALKEEPER" => Position.GK,
                "DEF" or "DEFENDER" => Position.DEF,
                "MID" or "MIDFIELDER" => Position.MID,
                "ATT" or "ATTACKER" or "FORWARD" => Position.ATT,
                _ => throw new JsonException($"Unknown position string: {positionString}")
            };
        }

        throw new JsonException("Unable to convert to position enum");
    }

    public override void Write(Utf8JsonWriter writer, Position value, JsonSerializerOptions options)
    {
        // Write back as the original element_type number for FPL API compatibility
        writer.WriteNumberValue((int)value);
    }
}