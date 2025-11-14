namespace AIFantasyPremierLeague.API.Exceptions;

public class PlayerPerformanceNotFoundException(string playerPerformanceId) : Exception($"Player Performance with ID '{playerPerformanceId}' was not found")
{
}
