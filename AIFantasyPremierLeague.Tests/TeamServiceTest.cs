using Xunit;
using Moq;
using FluentAssertions;
using AIFantasyPremierLeague.API.Services;
using Microsoft.AspNetCore.Mvc;


namespace AIFantasyPremierLeague.Tests;
public class TeamServiceTest
{
    [Fact]
    public async Task GetTeams_ReturnsAllTeams()
    {
        var teamService = new TeamService();

        IEnumerable<Team> teams = teamService.GetTeams();

        teams.Should().HaveCount(2);

        teams.Should().ContainInOrder(
            new Team(1, "Mike"),
            new Team(2, "Sam")
        );
    }
}