using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using AIFantasyPremierLeague.API.Converters;

namespace AIFantasyPremierLeague.API.Repository.Data;

public class PlayerPerformanceEntity
{
    [Key]
    [JsonIgnore]
    public string Id { get; set; } = $"perf_{Guid.NewGuid().ToString("N")[..12]}";

    [Required]
    [JsonPropertyName("id")]
    [JsonConverter(typeof(PlayerIdConverter))]
    public required string PlayerId { get; set; }

    public int GameWeek { get; set; }

    [Required]
    [JsonPropertyName("stats")]
    [JsonConverter(typeof(StatsConverter))]
    public required PlayerStats Stats { get; set; }
}

[Owned]
public class PlayerStats
{
    [JsonPropertyName("total_points")]
    public int Points { get; set; }

    [JsonPropertyName("goals_scored")]
    public int Goals { get; set; }

    [JsonPropertyName("assists")]
    public int Assists { get; set; }

    [JsonPropertyName("minutes")]
    public int MinsPlayed { get; set; }

    [JsonPropertyName("bonus")]
    public int Bonus { get; set; }

    [JsonPropertyName("clean_sheets")]
    public int CleanSheets { get; set; }

    [JsonPropertyName("goals_conceded")]
    public int GoalsConceded { get; set; }

    [JsonPropertyName("yellow_cards")]
    public int YellowCards { get; set; }

    [JsonPropertyName("red_cards")]
    public int RedCards { get; set; }

    [JsonPropertyName("saves")]
    public int Saves { get; set; }
}