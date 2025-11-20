namespace AIFantasyPremierLeague.API.Models;

public record Player(int Id, string FirstName, string SecondName, int TeamId, double Price, Position Position, int PredictedPoints);