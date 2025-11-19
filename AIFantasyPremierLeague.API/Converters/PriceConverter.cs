using System.Text.Json;
using System.Text.Json.Serialization;

namespace AIFantasyPremierLeague.API.Converters;

public class PriceConverter : JsonConverter<double>
{
    public override double Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Number)
        {
            var nowCost = reader.GetDouble();
            return nowCost / 10.0; // Divide by 10 to get the actual price
        }

        if (reader.TokenType == JsonTokenType.String)
        {
            var stringValue = reader.GetString();
            if (double.TryParse(stringValue, out double cost))
            {
                return cost / 10.0; // Divide by 10 to get the actual price
            }
        }

        throw new JsonException("Unable to convert to price");
    }

    public override void Write(Utf8JsonWriter writer, double value, JsonSerializerOptions options)
    {
        // When serializing back, multiply by 10 to get the original now_cost format
        writer.WriteNumberValue(value * 10);
    }
}