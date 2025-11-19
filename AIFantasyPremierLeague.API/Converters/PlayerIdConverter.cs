using System.Text.Json;
using System.Text.Json.Serialization;

namespace AIFantasyPremierLeague.API.Converters;

public class PlayerIdConverter : JsonConverter<int>
{
    public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Number)
        {
            return reader.GetInt32();
        }

        if (reader.TokenType == JsonTokenType.String)
        {
            var stringValue = reader.GetString();
            if (stringValue?.StartsWith("player") == true && int.TryParse(stringValue.Substring(6), out int id))
            {
                return id;
            }
            if (int.TryParse(stringValue, out int numericId))
            {
                return numericId;
            }
        }

        throw new JsonException("Unable to convert to player ID integer");
    }

    public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value);
    }
}