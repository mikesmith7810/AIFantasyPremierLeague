using Xunit;
using Moq;
using FluentAssertions;
using AIFantasyPremierLeague.API.Services;
using Microsoft.AspNetCore.Mvc;
using AIFantasyPremierLeague.API.Repository;
using AIFantasyPremierLeague.API.Repository.Data;
using AIFantasyPremierLeague.API.Models;


namespace AIFantasyPremierLeague.Tests;

public class PlayerHistoryServiceTest : DataSupplier
{
    private readonly Mock<IRepository<PlayerHistoryEntity>> _playerHistoryRepository;
    private readonly PlayerHistoryService _playerHistoryService;


    public PlayerHistoryServiceTest()
    {
        _playerHistoryRepository = new Mock<IRepository<PlayerHistoryEntity>>();

        _playerHistoryService = new PlayerHistoryService(_playerHistoryRepository.Object);
    }


    [Fact]
    public async Task GetPlayerHistorys_ReturnsAllPlayerHistorys()
    {
        var mockPlayerHistoryEntities = new List<PlayerHistoryEntity>
        {
            PlayerHistoryEntity1(),
           PlayerHistoryEntity2()
        };

        _playerHistoryRepository.Setup(playerHistoryRepository => playerHistoryRepository.GetAllAsync())
                .ReturnsAsync(mockPlayerHistoryEntities);

        IEnumerable<PlayerHistory> playerHistorys = await _playerHistoryService.GetPlayerHistorysAsync();

        playerHistorys.Should().HaveCount(2);

        playerHistorys.Should().ContainInOrder(
            PlayerHistory1(), PlayerHistory2()
        );
    }

    [Fact]
    public async Task GetPlayerHistory_ReturnsPlayerHistory()
    {
        PlayerHistoryEntity mockPlayerHistory = PlayerHistoryEntity1();

        _playerHistoryRepository.Setup(playerHistoryRepository => playerHistoryRepository.GetByIdAsync("playerHistory1"))
                .ReturnsAsync(mockPlayerHistory);

        PlayerHistory playerHistory = await _playerHistoryService.GetPlayerHistoryAsync("playerHistory1");

        playerHistory.Should().NotBeNull();
        playerHistory.Should().BeEquivalentTo(
            PlayerHistory1());
    }

    [Fact]
    public async Task AddPlayerHistory_ReturnsPlayerHistory()
    {
        PlayerHistory mockPlayerHistory = PlayerHistory1();
        PlayerHistoryEntity mockPlayerHistoryEntity = PlayerHistoryEntity1();

        _playerHistoryRepository.Setup(playerHistoryRepository => playerHistoryRepository.AddAsync(It.IsAny<PlayerHistoryEntity>()))
            .ReturnsAsync((PlayerHistoryEntity entity) => entity);

        PlayerHistory playerHistory = await _playerHistoryService.AddPlayerHistoryAsync(mockPlayerHistory);

        playerHistory.Should().NotBeNull();
        playerHistory.Should().BeEquivalentTo(
            PlayerHistory1());
    }

    [Fact]
    public async Task GetNonExistingPlayerHistory_ThrowsPlayerHistoryNotFoundException()
    {
        _playerHistoryRepository.Setup(playerHistoryRepository => playerHistoryRepository.GetByIdAsync("playerHistory1"))
                .ReturnsAsync((PlayerHistoryEntity)null);

        await _playerHistoryService.Invoking(s => s.GetPlayerHistoryAsync("playerHistory1"))
                      .Should().ThrowAsync<PlayerHistoryNotFoundException>()
                      .WithMessage($"Player History with ID 'playerHistory1' was not found");
    }
}