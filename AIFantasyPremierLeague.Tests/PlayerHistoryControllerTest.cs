using Xunit;
using Moq;
using FluentAssertions;
using AIFantasyPremierLeague.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using AIFantasyPremierLeague.API.Services;
using AIFantasyPremierLeague.API.Models;
using System.Threading.Tasks;

namespace AIFantasyPremierLeague.Tests;

public class PlayerHistoryControllerTest
{
    private readonly Mock<IPlayerHistoryService> _playerHistoryService;
    private readonly PlayerHistoryController _playerHistoryController;


    public PlayerHistoryControllerTest()
    {
        _playerHistoryService = new Mock<IPlayerHistoryService>();

        _playerHistoryController = new PlayerHistoryController(_playerHistoryService.Object);
    }

    [Fact]
    public async Task GetPlayerHistorys_ReturnsAllPlayerHistorys()
    {
        var mockPlayerHistorys = new List<PlayerHistory>
        {
            new("playerHistory1",  25, 1 ,"team1", 10 ),
            new("playerHistory2",  25, 1 ,"team3", 20 )
        };

        _playerHistoryService.Setup(playerHistoryService => playerHistoryService.GetPlayerHistorysAsync())
                .ReturnsAsync(mockPlayerHistorys);

        ActionResult<IEnumerable<PlayerHistory>> response = await _playerHistoryController.GetPlayerHistorys();
        OkObjectResult? result = response.Result as OkObjectResult;
        IEnumerable<PlayerHistory>? playerHistorys = result?.Value as IEnumerable<PlayerHistory>;

        playerHistorys.Should().HaveCount(2);

        playerHistorys.Should().ContainInOrder(
            new PlayerHistory("playerHistory1", 25, 1, "team1", 10), new PlayerHistory("playerHistory2", 25, 1, "team3", 20)
        );

        _playerHistoryService.Verify(playerHistoryService => playerHistoryService.GetPlayerHistorysAsync(), Times.Once);
    }

    [Fact]
    public async Task AddPlayerHistory_ReturnsCreated()
    {
        PlayerHistory playerHistory1 = new("playerHistory1", 25, 1, "team1", 10);

        _playerHistoryService.Setup(playerHistoryService => playerHistoryService.AddPlayerHistoryAsync(playerHistory1))
                .ReturnsAsync(playerHistory1);

        ActionResult<PlayerHistory> response = await _playerHistoryController.AddPlayerHistory(playerHistory1);
        CreatedAtActionResult? result = response.Result as CreatedAtActionResult;
        var playerHistory = result?.Value as PlayerHistory;

        playerHistory.Should().BeEquivalentTo(playerHistory1);

        _playerHistoryService.Verify(playerHistoryService => playerHistoryService.AddPlayerHistoryAsync(It.IsAny<PlayerHistory>()), Times.Once);
    }

    [Fact]
    public async Task GetPlayerHistory_ReturnsSuccess()
    {
        PlayerHistory playerHistory1 = new("playerHistory1", 25, 1, "team1", 10);

        _playerHistoryService.Setup(playerHistoryService => playerHistoryService.GetPlayerHistoryAsync("playerHistory1"))
                .ReturnsAsync(playerHistory1);

        ActionResult<PlayerHistory> response = await _playerHistoryController.GetPlayerHistory("playerHistory1");
        OkObjectResult? result = response.Result as OkObjectResult;
        var playerHistory = result?.Value as PlayerHistory;

        playerHistory.Should().BeEquivalentTo(playerHistory1);

        _playerHistoryService.Verify(playerHistoryService => playerHistoryService.GetPlayerHistoryAsync(It.IsAny<string>()), Times.Once);
    }
}