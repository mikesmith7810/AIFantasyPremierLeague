public class PlayerPerformanceNotFoundException : Exception
{
    public PlayerPerformanceNotFoundException(string playerPerformanceId)
         : base($"Player Performance with ID '{playerPerformanceId}' was not found")
    {
    }
}
