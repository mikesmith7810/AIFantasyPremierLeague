using System.Text.Json.Serialization;
using AIFantasyPremierLeague.API.Repository.Data;
namespace AIFantasyPremierLeague.API.Models;

public class FPLPlayerDetailedData
{
    public int PlayerId { get; set; }
    public int Fixture { get; set; }
    public int OpponentTeam { get; set; }
    public int TotalPoints { get; set; }
    public int Round { get; set; }
    public int WasHome { get; set; }
}
public class FPLPlayerDetailedDataResponse
{
    [JsonPropertyName("history")]
    public List<HistoryData>? Historys { get; set; }
}

public class HistoryData
{
    [JsonPropertyName("element")]
    public int PlayerId { get; set; }

    [JsonPropertyName("fixture")]
    public int Fixture { get; set; }

    [JsonPropertyName("opponent_team")]
    public int OpponentTeam { get; set; }

    [JsonPropertyName("total_points")]
    public int TotalPoints { get; set; }

    [JsonPropertyName("round")]
    public int Round { get; set; }

    [JsonPropertyName("was_home")]
    public bool WasHome { get; set; }

    [JsonPropertyName("minutes")]
    public int Minutes { get; set; }

    [JsonPropertyName("goals_scored")]
    public int GoalsScored { get; set; }

    [JsonPropertyName("assists")]
    public int Assists { get; set; }

    [JsonPropertyName("clean_sheets")]
    public int CleanSheets { get; set; }

    [JsonPropertyName("goals_conceded")]
    public int GoalsConceded { get; set; }

    [JsonPropertyName("yellow_cards")]
    public int YellowCards { get; set; }

    [JsonPropertyName("red_cards")]
    public int RedCards { get; set; }

    [JsonPropertyName("saves")]
    public int Saves { get; set; }

    [JsonPropertyName("bonus")]
    public int Bonus { get; set; }
}