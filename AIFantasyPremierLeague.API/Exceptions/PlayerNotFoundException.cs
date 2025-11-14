namespace AIFantasyPremierLeague.API.Exceptions;

public class PlayerNotFoundException(string playerId) : Exception($"Player with ID '{playerId}' was not found")
{
}
