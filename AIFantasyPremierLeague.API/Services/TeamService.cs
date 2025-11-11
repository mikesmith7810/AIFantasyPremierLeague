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

    public async Task<Team> AddTeamAsync(Team team)
    {
        TeamEntity teamEntity = new() { Id = team.Id, Name = team.Name, };

        TeamEntity response = await _teamRepository.AddAsync(teamEntity);

        return new Team(response.Id, response.Name);

    }

    public async Task<Team> GetTeamAsync(string teamId)
    {
        TeamEntity? teamEntity = await _teamRepository.GetByIdAsync(teamId);

        if (teamEntity == null)
            throw new TeamNotFoundException(teamId);

        return new Team(teamEntity.Id, teamEntity.Name);
    }
}

