using Xunit;
using Moq;
using FluentAssertions;
using AIFantasyPremierLeague.API.Services;
using Microsoft.AspNetCore.Mvc;
using AIFantasyPremierLeague.API.Repository;
using AIFantasyPremierLeague.API.Repository.Data;
using AIFantasyPremierLeague.API.Models;
using AIFantasyPremierLeague.API.Exceptions;


namespace AIFantasyPremierLeague.Tests;

public class PlayerPerformanceServiceTest : DataSupplier
{
    private readonly Mock<IRepository<PlayerPerformanceEntity>> _playerPerformanceRepository;
    private readonly PlayerPerformanceService _playerPerformanceService;


    public PlayerPerformanceServiceTest()
    {
        _playerPerformanceRepository = new Mock<IRepository<PlayerPerformanceEntity>>();

        _playerPerformanceService = new PlayerPerformanceService(_playerPerformanceRepository.Object);
    }


    [Fact]
    public async Task GetPlayerPerformances_ReturnsAllPlayerPerformances()
    {
        var mockPlayerPerformanceEntities = new List<PlayerPerformanceEntity>
        {
            PlayerPerformanceEntity1(),
           PlayerPerformanceEntity2()
        };

        _playerPerformanceRepository.Setup(playerPerformanceRepository => playerPerformanceRepository.GetAllAsync())
                .ReturnsAsync(mockPlayerPerformanceEntities);

        IEnumerable<PlayerPerformance> playerPerformances = await _playerPerformanceService.GetPlayerPerformancesAsync();

        playerPerformances.Should().HaveCount(2);

        playerPerformances.Should().ContainInOrder(
            PlayerPerformance1(), PlayerPerformance2()
        );
    }

    [Fact]
    public async Task GetPlayerPerformance_ReturnsPlayerPerformance()
    {
        PlayerPerformanceEntity mockPlayerPerformance = PlayerPerformanceEntity1();

        _playerPerformanceRepository.Setup(playerPerformanceRepository => playerPerformanceRepository.GetByIdAsync(1))
                .ReturnsAsync(mockPlayerPerformance);

        PlayerPerformance playerPerformance = await _playerPerformanceService.GetPlayerPerformanceAsync(1);

        playerPerformance.Should().NotBeNull();
        playerPerformance.Should().BeEquivalentTo(
            PlayerPerformance1());
    }

    [Fact]
    public async Task AddPlayerPerformance_ReturnsPlayerPerformance()
    {
        PlayerPerformance mockPlayerPerformance = PlayerPerformance1();
        PlayerPerformanceEntity mockPlayerPerformanceEntity = PlayerPerformanceEntity1();

        _playerPerformanceRepository.Setup(playerPerformanceRepository => playerPerformanceRepository.AddAsync(It.IsAny<PlayerPerformanceEntity>()))
            .ReturnsAsync((PlayerPerformanceEntity entity) => entity);

        PlayerPerformance playerPerformance = await _playerPerformanceService.AddPlayerPerformanceAsync(mockPlayerPerformance);

        playerPerformance.Should().NotBeNull();
        playerPerformance.Should().BeEquivalentTo(
            PlayerPerformance1());
    }

    [Fact]
    public async Task GetNonExistingPlayerPerformance_ThrowsPlayerPerformanceNotFoundException()
    {
        _playerPerformanceRepository.Setup(playerPerformanceRepository => playerPerformanceRepository.GetByIdAsync(1))
                .ReturnsAsync((PlayerPerformanceEntity)null);

        await _playerPerformanceService.Invoking(s => s.GetPlayerPerformanceAsync(1))
                      .Should().ThrowAsync<PlayerPerformanceNotFoundException>()
                      .WithMessage($"Player Performance with ID '1' was not found");
    }
}