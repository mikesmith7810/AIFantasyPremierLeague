using Xunit;
using Moq;
using FluentAssertions;
using AIFantasyPremierLeague.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using AIFantasyPremierLeague.API.Services;
using AIFantasyPremierLeague.API.Models;
using System.Threading.Tasks;

namespace AIFantasyPremierLeague.Tests;

public class PlayerPerformanceControllerTest : DataSupplier
{
    private readonly Mock<IPlayerPerformanceService> _playerPerformanceService;
    private readonly PlayerPerformanceController _playerPerformanceController;


    public PlayerPerformanceControllerTest()
    {
        _playerPerformanceService = new Mock<IPlayerPerformanceService>();

        _playerPerformanceController = new PlayerPerformanceController(_playerPerformanceService.Object);
    }

    [Fact]
    public async Task GetPlayerPerformances_ReturnsAllPlayerPerformances()
    {
        var mockPlayerPerformances = new List<PlayerPerformance>
        {
            PlayerPerformance1(),
            PlayerPerformance2()
        };

        _playerPerformanceService.Setup(playerPerformanceService => playerPerformanceService.GetPlayerPerformancesAsync())
                .ReturnsAsync(mockPlayerPerformances);

        ActionResult<IEnumerable<PlayerPerformance>> response = await _playerPerformanceController.GetPlayerPerformances();
        OkObjectResult? result = response.Result as OkObjectResult;
        IEnumerable<PlayerPerformance>? playerPerformances = result?.Value as IEnumerable<PlayerPerformance>;

        playerPerformances.Should().HaveCount(2);

        playerPerformances.Should().ContainInOrder(
            PlayerPerformance1(), PlayerPerformance2()
        );

        _playerPerformanceService.Verify(playerPerformanceService => playerPerformanceService.GetPlayerPerformancesAsync(), Times.Once);
    }

    [Fact]
    public async Task AddPlayerPerformance_ReturnsCreated()
    {
        PlayerPerformance playerPerformance1 = PlayerPerformance1();

        _playerPerformanceService.Setup(playerPerformanceService => playerPerformanceService.AddPlayerPerformanceAsync(playerPerformance1))
                .ReturnsAsync(playerPerformance1);

        ActionResult<PlayerPerformance> response = await _playerPerformanceController.AddPlayerPerformance(playerPerformance1);
        CreatedAtActionResult? result = response.Result as CreatedAtActionResult;
        var playerPerformance = result?.Value as PlayerPerformance;

        playerPerformance.Should().BeEquivalentTo(playerPerformance1);

        _playerPerformanceService.Verify(playerPerformanceService => playerPerformanceService.AddPlayerPerformanceAsync(It.IsAny<PlayerPerformance>()), Times.Once);
    }

    [Fact]
    public async Task GetPlayerPerformance_ReturnsSuccess()
    {
        PlayerPerformance playerPerformance1 = PlayerPerformance1();

        _playerPerformanceService.Setup(playerPerformanceService => playerPerformanceService.GetPlayerPerformanceAsync(1))
                .ReturnsAsync(playerPerformance1);

        ActionResult<PlayerPerformance> response = await _playerPerformanceController.GetPlayerPerformance(1);
        OkObjectResult? result = response.Result as OkObjectResult;
        var playerPerformance = result?.Value as PlayerPerformance;

        playerPerformance.Should().BeEquivalentTo(playerPerformance1);

        _playerPerformanceService.Verify(playerPerformanceService => playerPerformanceService.GetPlayerPerformanceAsync(It.IsAny<int>()), Times.Once);
    }
}