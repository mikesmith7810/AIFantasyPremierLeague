using System.Text.Json;
using System.Text.Json.Serialization;

namespace AIFantasyPremierLeague.API.Converters;

public class PlayerNameConverter : JsonConverter<string>
{
    public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // This converter is meant to be used differently - see PlayerEntity for proper implementation
        if (reader.TokenType == JsonTokenType.String)
        {
            return reader.GetString() ?? string.Empty;
        }

        throw new JsonException("Expected string value for player name");
    }

    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value);
    }
}