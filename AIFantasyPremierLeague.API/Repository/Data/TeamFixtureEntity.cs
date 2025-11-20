using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AIFantasyPremierLeague.API.Repository.Data;

public class TeamFixtureEntity

{
    [Key]
    [JsonPropertyName("code")]
    public required int Id { get; set; }

    [Required]
    [JsonPropertyName("event")]
    public required int Week { get; set; }

    [Required]
    [JsonPropertyName("team_h")]
    public required int TeamHome { get; set; }

    [Required]
    [JsonPropertyName("team_a")]
    public required int TeamAway { get; set; }
}