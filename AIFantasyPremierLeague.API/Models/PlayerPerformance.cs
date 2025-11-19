namespace AIFantasyPremierLeague.API.Models;

public record PlayerPerformance(string Id, int PlayerId, int Points, int Goals, int Assists, int MinsPlayed);