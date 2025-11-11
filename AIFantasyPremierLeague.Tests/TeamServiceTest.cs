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

    [Fact]
    public async Task GetTeam_ReturnsTeam()
    {
        TeamEntity mockTeam = new() { Id = "team1", Name = "Real Madrid" };

        _teamRepository.Setup(teamRepository => teamRepository.GetByIdAsync("team1"))
                .ReturnsAsync(mockTeam);

        Team team = await _teamService.GetTeamAsync("team1");

        team.Should().NotBeNull();
        team.Should().BeEquivalentTo(
            new Team("team1", "Real Madrid"));
    }

    [Fact]
    public async Task GetNonExistingTeam_ThrowsTeamNotFoundException()
    {
        _teamRepository.Setup(teamRepository => teamRepository.GetByIdAsync("team1"))
         .ReturnsAsync((TeamEntity)null);

        await _teamService.Invoking(s => s.GetTeamAsync("team1"))
                      .Should().ThrowAsync<TeamNotFoundException>()
                      .WithMessage($"Team with ID 'team1' was not found");
    }

    [Fact]
    public async Task AddTeam_ReturnsTeam()
    {
        Team mockTeam = new Team("team1", "Real Madrid");
        TeamEntity mockTeamEntity = new() { Id = "team1", Name = "Real Madrid" };

        _teamRepository.Setup(teamRepository => teamRepository.AddAsync(It.IsAny<TeamEntity>()))
            .ReturnsAsync((TeamEntity entity) => entity);

        Team team = await _teamService.AddTeamAsync(mockTeam);

        team.Should().NotBeNull();
        team.Should().BeEquivalentTo(
            new Team("team1", "Real Madrid"));
    }
}