using System.Text.Json;
using System.Text.Json.Serialization;
using AIFantasyPremierLeague.API.Repository.Data;

namespace AIFantasyPremierLeague.API.Converters;

public class StatsConverter : JsonConverter<PlayerStats>
{
    public override PlayerStats Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected StartObject token");
        }

        return JsonSerializer.Deserialize<PlayerStats>(ref reader, options)
               ?? throw new JsonException("Failed to deserialize PlayerStats");
    }

    public override void Write(Utf8JsonWriter writer, PlayerStats value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, options);
    }
}