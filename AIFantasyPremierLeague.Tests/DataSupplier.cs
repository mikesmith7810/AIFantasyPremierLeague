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
        return new() { Id = "player1", Name = "Harry Kane", Team = "team1", Value = 20, Position = "ATT", Age = 29 };
    }

    public static PlayerEntity PlayerEntity2()
    {
        return new() { Id = "player2", Name = "Declan Rice", Team = "team2", Value = 30, Position = "MID", Age = 27 };
    }

    public static PlayerHistory PlayerHistory1()
    {
        return new("playerHistory1", "player1", 25, 1, "team1", 10, 2, 1, 90);
    }

    public static PlayerHistory PlayerHistory2()
    {
        return new("playerHistory2", "player2", 25, 3, "team2", 20, 3, 2, 78);
    }

    public static PlayerHistoryEntity PlayerHistoryEntity1()
    {
        return new() { Id = "playerHistory1", PlayerId = "1", Season = 25, Week = 1, Team = "team1", Points = 10, Goals = 2, Assists = 1, MinsPlayed = 90 };
    }

    public static PlayerHistoryEntity PlayerHistoryEntity2()
    {
        return new() { Id = "playerHistory2", Season = 25, Week = 3, Team = "team2", Points = 20, Goals = 3, Assists = 2, MinsPlayed = 78 };
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