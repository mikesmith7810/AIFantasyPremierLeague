public class TeamNotFoundException : Exception
{
   public TeamNotFoundException(string teamId) 
        : base($"Team with ID '{teamId}' was not found")
    {
    }
}
