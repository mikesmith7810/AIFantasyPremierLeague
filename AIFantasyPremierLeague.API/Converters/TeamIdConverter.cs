using System.Text.Json;
using System.Text.Json.Serialization;

namespace AIFantasyPremierLeague.API.Converters;

public class TeamIdConverter : JsonConverter<string>
{
    public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Number)
        {
            int teamId = reader.GetInt32();
            return $"team{teamId}";
        }

        if (reader.TokenType == JsonTokenType.String)
        {
            string? value = reader.GetString();
            return value ?? throw new JsonException("Team value cannot be null");
        }

        throw new JsonException($"Expected number or string for team, got {reader.TokenType}");
    }

    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
    {
        if (value.StartsWith("team") && int.TryParse(value.Substring(4), out int teamId))
        {
            writer.WriteNumberValue(teamId);
        }
        else
        {
            throw new JsonException($"Invalid team format: {value}. Expected format: team[number]");
        }
    }
}