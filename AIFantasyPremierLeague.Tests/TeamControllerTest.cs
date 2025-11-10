using Xunit;
using Moq;
using FluentAssertions;
using AIFantasyPremierLeague.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using AIFantasyPremierLeague.API.Services;
using AIFantasyPremierLeague.API.Models;
using System.Threading.Tasks;

namespace AIFantasyPremierLeague.Tests;

public class TeamControllerTest
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
            new("team1", "Barcelona"),
            new("team2", "Real Madrid")
        };

        _teamService.Setup(teamService => teamService.GetTeamsAsync())
                .ReturnsAsync(mockTeams);

        ActionResult<IEnumerable<Team>> response = await _teamController.GetTeams();

        var result = response.Result as OkObjectResult;
        var teams = result?.Value as IEnumerable<Team>;

        teams.Should().HaveCount(2);

        teams.Should().ContainInOrder(
            new Team("team1", "Barcelona"),
            new Team("team2", "Real Madrid")
        );

        _teamService.Verify(teamService => teamService.GetTeamsAsync(), Times.Once);
    }
}