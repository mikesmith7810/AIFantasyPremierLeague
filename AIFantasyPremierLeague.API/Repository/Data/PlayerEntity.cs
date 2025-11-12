using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using AIFantasyPremierLeague.API.Converters;

namespace AIFantasyPremierLeague.API.Repository.Data;

public class PlayerEntity

{
    [Key]
    [JsonPropertyName("id")]
    [JsonConverter(typeof(PlayerIdConverter))]
    public required string Id { get; set; }

    [Required]
    [StringLength(100)]
    [JsonPropertyName("second_name")]
    public required string Name { get; set; }

    [Required]
    [JsonPropertyName("team")]
    [JsonConverter(typeof(TeamIdConverter))]
    public required string Team { get; set; }

    [Required]
    [JsonPropertyName("now_cost")]
    public required double Value { get; set; }

    [Required]
    [JsonPropertyName("element_type")]
    [JsonConverter(typeof(PositionConverter))]
    public required string Position { get; set; }

    public int Age { get; set; }
}