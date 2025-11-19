using System.Text.Json;
using System.Text.Json.Serialization;
using AIFantasyPremierLeague.API.Models;

namespace AIFantasyPremierLeague.API.Converters;

public class PositionToIntConverter : JsonConverter<int>
{
    public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Number)
        {
            var elementType = reader.GetInt32();
            // FPL API uses: 1=GK, 2=DEF, 3=MID, 4=ATT
            return elementType switch
            {
                1 => (int)Position.GK,
                2 => (int)Position.DEF,
                3 => (int)Position.MID,
                4 => (int)Position.ATT,
                _ => throw new JsonException($"Unknown position element_type: {elementType}")
            };
        }

        if (reader.TokenType == JsonTokenType.String)
        {
            var positionString = reader.GetString()?.ToUpperInvariant();
            return positionString switch
            {
                "GK" or "GOALKEEPER" => (int)Position.GK,
                "DEF" or "DEFENDER" => (int)Position.DEF,
                "MID" or "MIDFIELDER" => (int)Position.MID,
                "ATT" or "ATTACKER" or "FORWARD" => (int)Position.ATT,
                _ => throw new JsonException($"Unknown position string: {positionString}")
            };
        }

        throw new JsonException("Unable to convert to position integer");
    }

    public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value);
    }
}