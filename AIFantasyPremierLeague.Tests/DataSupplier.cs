using System.Net.NetworkInformation;
using AIFantasyPremierLeague.API.Models;
using AIFantasyPremierLeague.API.Repository.Data;
using Microsoft.AspNetCore.Http;

namespace AIFantasyPremierLeague.Tests;
public class DataSupplier
{
    public static Player Player1()
    {
        return new Player(1, "Harry", "Kane", 1, 20, Position.ATT, 29);
    }

    public static Player Player2()
    {
        return new Player(2, "Declan", "Rice", 2, 30, Position.MID, 27);
    }

    public static PlayerEntity PlayerEntity1()
    {
        return new() { Id = 1, FirstName = "Harry", SecondName = "Kane", Team = 1, Price = 20, Position = Position.ATT, PredictedPoints = 29 };
    }

    public static PlayerEntity PlayerEntity2()
    {
        return new() { Id = 2, FirstName = "Declan", SecondName = "Rice", Team = 2, Price = 30, Position = Position.MID, PredictedPoints = 27 };
    }

    public static PlayerPerformance PlayerPerformance1()
    {
        return new("performance1", 1, 10, 2, 1, 90);
    }

    public static PlayerPerformance PlayerPerformance2()
    {
        return new("performance2", 2, 20, 3, 2, 78);
    }

    public static PlayerPerformanceEntity PlayerPerformanceEntity1()
    {

        return new()
        {
            Id = "performance1",
            PlayerId = 1,
            Stats = new() { Points = 10, Goals = 2, Assists = 1, MinsPlayed = 90 }
        };
    }

    public static PlayerPerformanceEntity PlayerPerformanceEntity2()
    {
        return new()
        {
            Id = "performance2",
            PlayerId = 2,
            Stats = new() { Points = 20, Goals = 3, Assists = 2, MinsPlayed = 78 }
        };
    }

    public static Team Team1()
    {
        return new(1, "Barcelona", "BAR");
    }

    public static Team Team2()
    {
        return new(2, "Real Madrid", "RMA");
    }

    public static TeamEntity TeamEntity1()
    {
        return new() { Id = 1, Name = "Barcelona", ShortName = "BAR" };
    }

    public static TeamEntity TeamEntity2()
    {
        return new() { Id = 2, Name = "Real Madrid", ShortName = "RMA" };
    }
}