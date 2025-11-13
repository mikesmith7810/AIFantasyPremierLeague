using System.Text.Json.Serialization;
using AIFantasyPremierLeague.API.Repository.Data;
namespace AIFantasyPremierLeague.API.Models;

public class FPLPerformanceData
{
    [JsonPropertyName("elements")]
    public List<PlayerPerformanceEntity>? PlayerPerformances { get; set; }
}