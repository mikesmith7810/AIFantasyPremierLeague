public class PlayerHistoryNotFoundException : Exception
{
    public PlayerHistoryNotFoundException(string playerHistoryId)
         : base($"Player History with ID '{playerHistoryId}' was not found")
    {
    }
}
