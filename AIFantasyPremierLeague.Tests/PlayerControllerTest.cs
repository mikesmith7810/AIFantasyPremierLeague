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
            new("player1", "Johan Cruyff", "team2",10),
            new("player2", "Sam Smith", "team1",20)
        };

        _playerService.Setup(playerService => playerService.GetPlayersAsync())
                .ReturnsAsync(mockPlayers);

        ActionResult<IEnumerable<Player>> response = await _playerController.GetPlayers();
        OkObjectResult? result = response.Result as OkObjectResult;
        IEnumerable<Player>? players = result?.Value as IEnumerable<Player>;

        players.Should().HaveCount(2);

        players.Should().ContainInOrder(
            new Player("player1", "Johan Cruyff", "team2", 10), new Player("player2", "Sam Smith", "team1", 20)
        );

        _playerService.Verify(playerService => playerService.GetPlayersAsync(), Times.Once);
    }

    [Fact]
    public async Task AddPlayer_ReturnsCreated()
    {
        Player player1 = new("player1", "Lamine Yamal", "team2", 10);

        _playerService.Setup(playerService => playerService.AddPlayerAsync(player1))
                .ReturnsAsync(player1);

        ActionResult<Player> response = await _playerController.AddPlayer(player1);
        CreatedAtActionResult? result = response.Result as CreatedAtActionResult;
        var player = result?.Value as Player;

        player.Should().BeEquivalentTo(player1);

        _playerService.Verify(playerService => playerService.AddPlayerAsync(It.IsAny<Player>()), Times.Once);
    }

    [Fact]
    public async Task GetPlayer_ReturnsSuccess()
    {
        Player player1 = new("player1", "Lamine Yamal", "team2", 10);

        _playerService.Setup(playerService => playerService.GetPlayerAsync("player1"))
                .ReturnsAsync(player1);

        ActionResult<Player> response = await _playerController.GetPlayer("player1");
        OkObjectResult? result = response.Result as OkObjectResult;
        var player = result?.Value as Player;

        player.Should().BeEquivalentTo(player1);

        _playerService.Verify(playerService => playerService.GetPlayerAsync(It.IsAny<string>()), Times.Once);
    }
}