namespace AIFantasyPremierLeague.API.Services;
public class TeamService : ITeamService
{
    public IEnumerable<Team> GetTeams()
    {
        List<Team> teams = [new Team("Mike"), new Team("Sam")];
        return teams;
    }
}

