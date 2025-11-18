using System.Text.Json.Serialization;
using AIFantasyPremierLeague.API.Repository.Data;

namespace AIFantasyPremierLeague.API.Models;

// Your flat object
public class FPLPlayerEventData
{
    public int PlayerId { get; set; }
    public int Week { get; set; }
    public int FixtureId { get; set; }
    public int Minutes { get; set; }
    public int GoalsScored { get; set; }
    public int Assists { get; set; }
    public int TotalPoints { get; set; }
}

public class FPLPlayerEventResponse
{
    [JsonPropertyName("elements")]
    public List<ElementData> Elements { get; set; } = new();
}

public class ElementData
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("stats")]
    public StatsData Stats { get; set; } = new();

    [JsonPropertyName("explain")]
    public List<ExplainData> Explain { get; set; } = new();
}

public class StatsData
{
    [JsonPropertyName("minutes")]
    public int Minutes { get; set; }

    [JsonPropertyName("goals_scored")]
    public int GoalsScored { get; set; }

    [JsonPropertyName("assists")]
    public int Assists { get; set; }

    [JsonPropertyName("total_points")]
    public int TotalPoints { get; set; }
}

public class ExplainData
{
    [JsonPropertyName("fixture")]
    public int Fixture { get; set; }
}