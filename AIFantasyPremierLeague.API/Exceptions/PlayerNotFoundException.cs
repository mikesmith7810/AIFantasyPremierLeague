public class PlayerNotFoundException : Exception
{
   public PlayerNotFoundException(string playerId) 
        : base($"Player with ID '{playerId}' was not found")
    {
    }
}
