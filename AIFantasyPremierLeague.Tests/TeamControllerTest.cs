using Xunit;
using Moq;
using FluentAssertions;
using AIFantasyPremierLeague.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using AIFantasyPremierLeague.API.Services;

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
    public void GetTeams_ReturnsAllTeams()
    {
        _teamService.Setup(teamService => teamService.GetTeams())
                .Returns([new Team(1, "Mike"), new Team(2, "Sam")]);

        ActionResult<IEnumerable<Team>> response = _teamController.GetTeams();

        var result = response.Result as OkObjectResult;
        var teams = result?.Value as IEnumerable<Team>;

        teams.Should().HaveCount(2);

        teams.Should().ContainInOrder(
            new Team(1, "Mike"),
            new Team(2, "Sam")
        );

        _teamService.Verify(teamService => teamService.GetTeams(), Times.Once);
    }
}