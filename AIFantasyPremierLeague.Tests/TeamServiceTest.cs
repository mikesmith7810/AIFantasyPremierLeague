using Xunit;
using Moq;
using FluentAssertions;
using AIFantasyPremierLeague.API.Services;
using Microsoft.AspNetCore.Mvc;
using AIFantasyPremierLeague.API.Repository;
using AIFantasyPremierLeague.API.Repository.Data;
using AIFantasyPremierLeague.API.Models;


namespace AIFantasyPremierLeague.Tests;
public class TeamServiceTest
{
    private readonly Mock<IRepository<TeamEntity>> _teamRepository;
    private readonly TeamService _teamService;


    public TeamServiceTest()
    {
        _teamRepository = new Mock<IRepository<TeamEntity>>();

        _teamService = new TeamService(_teamRepository.Object);
    }

    [Fact]
    public async Task GetTeams_ReturnsAllTeams()
    {
        var mockTeamEntities = new List<TeamEntity>
        {
            new() { Id = "team1", Name = "Barcelona" },
            new() { Id = "team2", Name = "Real Madrid" }
        };

        _teamRepository.Setup(teamRepository => teamRepository.GetAllAsync())
                .ReturnsAsync(mockTeamEntities);

        IEnumerable<Team> teams = await _teamService.GetTeamsAsync();

        teams.Should().HaveCount(2);

        teams.Should().ContainInOrder(
            new Team("team1", "Barcelona"),
            new Team("team2", "Real Madrid")
        );

        _teamRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
    }
}