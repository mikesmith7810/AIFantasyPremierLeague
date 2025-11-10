using Xunit;
using Moq;
using FluentAssertions;
using AIFantasyPremierLeague.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using AIFantasyPremierLeague.API.Services;
using AIFantasyPremierLeague.API.Models;
using System.Threading.Tasks;

namespace AIFantasyPremierLeague.Tests;

public class PlayerControllerTest
{
    private readonly Mock<IPlayerService> _playerService;
    private readonly PlayerController _playerController;


    public PlayerControllerTest()
    {
        _playerService = new Mock<IPlayerService>();

        _playerController = new PlayerController(_playerService.Object);
    }

    [Fact]
    public async Task GetPlayers_ReturnsAllPlayers()
    {
        var mockPlayers = new List<Player>
        {
            new("player1", "Johan Cruyff", "team2"),
            new("player2", "Sam Smith", "team1")
        };

        _playerService.Setup(playerService => playerService.GetPlayers())
                .ReturnsAsync(mockPlayers);

        ActionResult<IEnumerable<Player>> response = await _playerController.GetPlayers();

        var result = response.Result as OkObjectResult;
        var players = result?.Value as IEnumerable<Player>;

        players.Should().HaveCount(2);

        players.Should().ContainInOrder(
            new Player("player1", "Johan Cruyff", "team2"), new Player("player2", "Sam Smith", "team1")
        );

        _playerService.Verify(playerService => playerService.GetPlayers(), Times.Once);
    }
}