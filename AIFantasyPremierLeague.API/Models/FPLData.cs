using System.Text.Json.Serialization;
using AIFantasyPremierLeague.API.Repository.Data;
namespace AIFantasyPremierLeague.API.Models;

public class FPLData
{
    [JsonPropertyName("elements")]
    public List<PlayerEntity>? Players { get; set; }
}