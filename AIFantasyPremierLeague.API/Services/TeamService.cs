namespace AIFantasyPremierLeague.API.Services;
public class TeamService : ITeamService
{
    public IEnumerable<Team> GetTeams()
    {
        List<Team> teams = [new Team(1, "Mike"), new Team(2, "Sam")];
        return teams;
    }
}

