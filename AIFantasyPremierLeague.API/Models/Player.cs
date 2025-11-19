namespace AIFantasyPremierLeague.API.Models;

public record Player(int Id, string FirstName, string SecondName, string TeamId, double Price, Position Position, int PredictedPoints);