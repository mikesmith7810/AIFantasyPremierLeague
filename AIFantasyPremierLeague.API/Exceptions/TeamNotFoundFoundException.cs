namespace AIFantasyPremierLeague.API.Exceptions;

public class TeamNotFoundException(string teamId) : Exception($"Team with ID '{teamId}' was not found")
{
}
