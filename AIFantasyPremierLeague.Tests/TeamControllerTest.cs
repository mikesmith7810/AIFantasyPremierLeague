using Xunit;
using Moq;
using FluentAssertions;
using AIFantasyPremierLeague.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using AIFantasyPremierLeague.API.Services;
using AIFantasyPremierLeague.API.Models;
using System.Threading.Tasks;

namespace AIFantasyPremierLeague.Tests;

public class TeamControllerTest : DataSupplier
{
    private readonly Mock<ITeamService> _teamService;
    private readonly TeamController _teamController;


    public TeamControllerTest()
    {
        _teamService = new Mock<ITeamService>();

        _teamController = new TeamController(_teamService.Object);
    }

    [Fact]
    public async Task GetTeams_ReturnsAllTeams()
    {
        var mockTeams = new List<Team>
        {
            Team1(),
            Team2()
        };

        _teamService.Setup(teamService => teamService.GetTeamsAsync())
                .ReturnsAsync(mockTeams);

        ActionResult<IEnumerable<Team>> response = await _teamController.GetTeams();

        var result = response.Result as OkObjectResult;
        var teams = result?.Value as IEnumerable<Team>;

        teams.Should().HaveCount(2);

        teams.Should().ContainInOrder(
            Team1(),
            Team2()
        );

        _teamService.Verify(teamService => teamService.GetTeamsAsync(), Times.Once);
    }

    [Fact]
    public async Task GetTeam_ReturnsTeam()
    {
        Team mockTeam = Team1();

        _teamService.Setup(teamService => teamService.GetTeamAsync(1))
                .ReturnsAsync(mockTeam);

        ActionResult<Team> response = await _teamController.GetTeam(1);

        var result = response.Result as OkObjectResult;
        var team = result?.Value as Team;

        team.Should().NotBeNull();

        team.Should().BeEquivalentTo(mockTeam);

        _teamService.Verify(teamService => teamService.GetTeamAsync(1), Times.Once);
    }


    [Fact]
    public async Task AddTeam_ReturnsNewTeam()
    {
        Team mockTeam = Team1();

        _teamService.Setup(teamService => teamService.AddTeamAsync(It.IsAny<Team>()))
                .ReturnsAsync((Team team) => team);

        ActionResult<Team> response = await _teamController.AddTeam(mockTeam);

        var result = response.Result as CreatedAtActionResult;
        var team = result?.Value as Team;

        team.Should().NotBeNull();

        team.Should().BeEquivalentTo(mockTeam);

        _teamService.Verify(teamService => teamService.AddTeamAsync(mockTeam), Times.Once);
    }
}