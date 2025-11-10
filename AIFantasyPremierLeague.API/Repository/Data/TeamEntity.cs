using System.ComponentModel.DataAnnotations;

namespace AIFantasyPremierLeague.API.Repository.Data;

public class TeamEntity

{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public required string Name { get; set; }
}