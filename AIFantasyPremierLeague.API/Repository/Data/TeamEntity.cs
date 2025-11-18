using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AIFantasyPremierLeague.API.Repository.Data;

public class TeamEntity

{
    [Key]
    [JsonPropertyName("id")]
    public required int Id { get; set; }

    [Required]
    [JsonPropertyName("name")]
    [StringLength(100)]
    public required string Name { get; set; }

    [Required]
    [JsonPropertyName("short_name")]
    [StringLength(100)]
    public required string ShortName { get; set; }
}