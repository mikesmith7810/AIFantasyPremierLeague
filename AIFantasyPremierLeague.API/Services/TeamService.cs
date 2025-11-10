using System.Threading.Tasks;
using AIFantasyPremierLeague.API.Models;
using AIFantasyPremierLeague.API.Repository;
using AIFantasyPremierLeague.API.Repository.Data;

namespace AIFantasyPremierLeague.API.Services;
public class TeamService : ITeamService
{
    private readonly IRepository<TeamEntity> _teamRepository;

    public TeamService(IRepository<TeamEntity> teamRepository)
    {
        _teamRepository = teamRepository;
    }

    public async Task<IEnumerable<Team>> GetTeamsAsync()
    {
        IEnumerable<TeamEntity> teams = await _teamRepository.GetAllAsync();

        return teams.Select(teamEntity => new Team(teamEntity.Id, teamEntity.Name));
    }
}

