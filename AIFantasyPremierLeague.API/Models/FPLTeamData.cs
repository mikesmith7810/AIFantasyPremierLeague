using System.Text.Json.Serialization;
using AIFantasyPremierLeague.API.Repository.Data;
namespace AIFantasyPremierLeague.API.Models;

public class FPLTeamData
{
    [JsonPropertyName("teams")]
    public List<TeamEntity>? Teams { get; set; }
}