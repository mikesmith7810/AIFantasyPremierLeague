namespace AIFantasyPremierLeague.API.Services;
public class PlayerService : IPlayerService
{
    public IEnumerable<Player> GetPlayers()
    {
        List<Player> players = [new Player("Johan Cruyff", 2), new Player("Sam Smith", 1)];
        return players;
    }
}

