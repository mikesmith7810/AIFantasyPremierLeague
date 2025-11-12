namespace AIFantasyPremierLeague.API.Models;

public record PlayerHistory(string Id, string PlayerId, int Season, int Week, string TeamId, int Points, int Goals, int Assists, int MinsPlayed);