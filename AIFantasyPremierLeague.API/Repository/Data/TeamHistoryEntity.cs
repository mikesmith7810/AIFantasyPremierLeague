using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AIFantasyPremierLeague.API.Repository.Data;

public class TeamHistoryEntity

{
    [Key]
    public required string Id { get; set; }

    [Required]
    public required int TeamId { get; set; }

    [Required]
    public required int Week { get; set; }

    [Required]
    public required int PointsConceded { get; set; }
}