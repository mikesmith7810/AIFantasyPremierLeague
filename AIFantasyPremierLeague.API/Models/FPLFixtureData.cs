using System.Text.Json.Serialization;
using AIFantasyPremierLeague.API.Repository.Data;
namespace AIFantasyPremierLeague.API.Models;

public class FPLFixtureData
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
}