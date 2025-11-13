namespace AIFantasyPremierLeague.API.Models;

public record PlayerPerformance(string Id, string PlayerId, int Points, int Goals, int Assists, int MinsPlayed);