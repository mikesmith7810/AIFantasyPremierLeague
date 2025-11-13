using System.Text.Json;
using System.Text.Json.Serialization;

namespace AIFantasyPremierLeague.API.Converters;

public class PlayerPerformanceIdConverter : JsonConverter<string>
{
    public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Number)
        {
            var id = reader.GetInt32();
            return $"performance{id}";
        }

        if (reader.TokenType == JsonTokenType.String)
        {
            var stringValue = reader.GetString();
            return stringValue?.StartsWith("performance") == true ? stringValue : $"performance{stringValue}";
        }

        throw new JsonException("Unable to convert to performance ID");
    }

    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
    {
        if (value.StartsWith("performance") && int.TryParse(value.Substring(11), out int numericId))
        {
            writer.WriteNumberValue(numericId);
        }
        else
        {
            writer.WriteStringValue(value);
        }
    }
}