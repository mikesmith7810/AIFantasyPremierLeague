using AIFantasyPremierLeague.API.Models;

namespace AIFantasyPremierLeague.API.Services;
public interface ITeamService
{
    Task<IEnumerable<Team>> GetTeamsAsync();

    Task<Team> AddTeamAsync(Team team);

    Task<Team> GetTeamAsync(int teamId);
}