using System.ComponentModel.DataAnnotations;

namespace AIFantasyPremierLeague.API.Repository.Data;

public class PlayerHistoryEntity

{
    [Key]
    public required string Id { get; set; }

    [Required]
    public required string PlayerId { get; set; }

    [Required]
    public required int Season { get; set; }

    [Required]
    public required int Week { get; set; }

    [Required]
    public required string Team { get; set; }

    [Required]
    public required int Points { get; set; }

    [Required]
    public required int Goals { get; set; }

    [Required]
    public required int Assists { get; set; }

    [Required]
    public required int MinsPlayed { get; set; }
}