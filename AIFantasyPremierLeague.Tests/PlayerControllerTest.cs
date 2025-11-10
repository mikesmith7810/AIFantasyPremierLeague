using Xunit;
using Moq;
using FluentAssertions;
using AIFantasyPremierLeague.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using AIFantasyPremierLeague.API.Services;

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
    public void GetPlayers_ReturnsAllPlayers()
    {
        _playerService.Setup(playerService => playerService.GetPlayers())
                .Returns([new Player("Johan Cruyff", 2), new Player("Sam Smith", 1)]);

        ActionResult<IEnumerable<Player>> response = _playerController.GetPlayers();

        var result = response.Result as OkObjectResult;
        var players = result?.Value as IEnumerable<Player>;

        players.Should().HaveCount(2);

        players.Should().ContainInOrder(
            new Player("Johan Cruyff", 2), new Player("Sam Smith", 1)
        );

        _playerService.Verify(playerService => playerService.GetPlayers(), Times.Once);
    }
}