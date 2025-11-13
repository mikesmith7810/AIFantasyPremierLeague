using System.Net.NetworkInformation;
using AIFantasyPremierLeague.API.Models;
using AIFantasyPremierLeague.API.Repository.Data;
using Microsoft.AspNetCore.Http;

namespace AIFantasyPremierLeague.Tests;
public class DataSupplier
{
    public static Player Player1()
    {
        return new Player("player1", "Harry Kane", "team1", 20, "ATT", 29);
    }

    public static Player Player2()
    {
        return new Player("player2", "Declan Rice", "team2", 30, "MID", 27);
    }

    public static PlayerEntity PlayerEntity1()
    {
        return new() { Id = "player1", Name = "Harry Kane", Team = "team1", Value = 20, Position = "ATT", PredictedPoints = 29 };
    }

    public static PlayerEntity PlayerEntity2()
    {
        return new() { Id = "player2", Name = "Declan Rice", Team = "team2", Value = 30, Position = "MID", PredictedPoints = 27 };
    }

    public static PlayerPerformance PlayerPerformance1()
    {
        return new("performance1", "player1", 10, 2, 1, 90);
    }

    public static PlayerPerformance PlayerPerformance2()
    {
        return new("performance2", "player2", 20, 3, 2, 78);
    }

    public static PlayerPerformanceEntity PlayerPerformanceEntity1()
    {

        return new()
        {
            Id = "performance1",
            PlayerId = "player1",
            Stats = new() { Points = 10, Goals = 2, Assists = 1, MinsPlayed = 90 }
        };
    }

    public static PlayerPerformanceEntity PlayerPerformanceEntity2()
    {
        return new()
        {
            Id = "performance2",
            PlayerId = "player3",
            Stats = new() { Points = 20, Goals = 3, Assists = 2, MinsPlayed = 78 }
        };
    }

    public static Team Team1()
    {
        return new("team1", "Barcelona");
    }

    public static Team Team2()
    {
        return new("team2", "Real Madrid");
    }

    public static TeamEntity TeamEntity1()
    {
        return new() { Id = "team1", Name = "Barcelona" };
    }

    public static TeamEntity TeamEntity2()
    {
        return new() { Id = "team2", Name = "Real Madrid" };
    }
}