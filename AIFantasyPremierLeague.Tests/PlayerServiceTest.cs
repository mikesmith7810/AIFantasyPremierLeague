using Xunit;
using Moq;
using FluentAssertions;
using AIFantasyPremierLeague.API.Services;
using Microsoft.AspNetCore.Mvc;


namespace AIFantasyPremierLeague.Tests;
public class PlayerServiceTest
{
    [Fact]
    public async Task GetPlayers_ReturnsAllPlayers()
    {
        var playerService = new PlayerService();

        IEnumerable<Player> players = playerService.GetPlayers();

        players.Should().HaveCount(2);

        players.Should().ContainInOrder(
            new Player("Johan Cruyff", 2), new Player("Sam Smith", 1)
        );
    }
}