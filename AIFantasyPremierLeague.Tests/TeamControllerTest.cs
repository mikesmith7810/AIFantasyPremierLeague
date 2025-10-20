using Xunit;
using Moq;
using FluentAssertions;
using AIFantasyPremierLeague.API.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace AIFantasyPremierLeague.Tests.Controllers;
public class TeamControllerTest
{
    [Fact]
    public void GetTeams_ReturnsAllTeams()
    {
        var teamController = new TeamController();

        ActionResult<IEnumerable<Team>> response = teamController.GetTeams();

        var result = response.Result as OkObjectResult;
        var teams = result?.Value as IEnumerable<Team>;

        teams.Should().HaveCount(2);

        // Check that teams are in the expected order
        teams.Should().ContainInOrder(
            new Team("Mike"),
            new Team("Sam")
        );
    }
}