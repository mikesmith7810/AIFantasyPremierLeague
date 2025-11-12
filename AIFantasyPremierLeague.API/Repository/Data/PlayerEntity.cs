using System.ComponentModel.DataAnnotations;

namespace AIFantasyPremierLeague.API.Repository.Data;

public class PlayerEntity

{
    [Key]
    public required string Id { get; set; }

    [Required]
    [StringLength(100)]
    public required string Name { get; set; }

    [Required]
    public required string Team { get; set; }

    [Required]
    public required double Value { get; set; }

    [Required]
    public required string Position { get; set; }

    [Required]
    public required int Age { get; set; }
}