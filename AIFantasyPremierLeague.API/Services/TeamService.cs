using AIFantasyPremierLeague.API.Exceptions;
using AIFantasyPremierLeague.API.Models;
using AIFantasyPremierLeague.API.Repository;
using AIFantasyPremierLeague.API.Repository.Data;

namespace AIFantasyPremierLeague.API.Services;

public class TeamService(IRepository<TeamEntity> teamRepository) : ITeamService
{
    public async Task<IEnumerable<Team>> GetTeamsAsync()
    {
        var teams = await teamRepository.GetAllAsync();

        return teams.Select(teamEntity => new Team(teamEntity.Id, teamEntity.Name, teamEntity.ShortName));
    }

    public async Task<Team> AddTeamAsync(Team team)
    {
        TeamEntity teamEntity = new() { Id = team.Id, Name = team.Name, ShortName = team.ShortName };

        var response = await teamRepository.AddAsync(teamEntity);

        return new Team(response.Id, response.Name, response.ShortName);

    }

    public async Task<Team> GetTeamAsync(int teamId)
    {
        TeamEntity? teamEntity = await teamRepository.GetByIdAsync(teamId) ?? throw new TeamNotFoundException(teamId.ToString());

        return new Team(teamEntity.Id, teamEntity.Name, teamEntity.ShortName);
    }
}

