using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using AIFantasyPremierLeague.API.Converters;
using AIFantasyPremierLeague.API.Models;

namespace AIFantasyPremierLeague.API.Repository.Data;

public class PlayerEntity

{
    [Key]
    [JsonPropertyName("id")]
    [JsonConverter(typeof(PlayerIdConverter))]
    public required int Id { get; set; }

    [JsonPropertyName("first_name")]
    public string FirstName { get; set; } = string.Empty;

    [JsonPropertyName("second_name")]
    public string SecondName { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    [JsonIgnore]
    public string Name => $"{FirstName?.Trim()} {SecondName?.Trim()}".Trim();

    [Required]
    [JsonPropertyName("team")]
    public required int Team { get; set; }

    [Required]
    [JsonPropertyName("now_cost")]
    [JsonConverter(typeof(PriceConverter))]
    public required double Price { get; set; }

    [Required]
    [JsonPropertyName("element_type")]
    [JsonConverter(typeof(PositionConverter))]
    public required Position Position { get; set; }

    public int PredictedPoints { get; set; }
}