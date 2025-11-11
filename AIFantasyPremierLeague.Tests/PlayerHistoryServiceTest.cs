using Xunit;
using Moq;
using FluentAssertions;
using AIFantasyPremierLeague.API.Services;
using Microsoft.AspNetCore.Mvc;
using AIFantasyPremierLeague.API.Repository;
using AIFantasyPremierLeague.API.Repository.Data;
using AIFantasyPremierLeague.API.Models;


namespace AIFantasyPremierLeague.Tests;
public class PlayerHistoryServiceTest
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
            new() { Id = "playerHistory1", Season = 25, Week =1 ,Team="team1", Points=10 },
            new() { Id = "playerHistory2", Season = 25, Week =3 ,Team="team2", Points=20  }
        };

        _playerHistoryRepository.Setup(playerHistoryRepository => playerHistoryRepository.GetAllAsync())
                .ReturnsAsync(mockPlayerHistoryEntities);

        IEnumerable<PlayerHistory> playerHistorys = await _playerHistoryService.GetPlayerHistorysAsync();

        playerHistorys.Should().HaveCount(2);

        playerHistorys.Should().ContainInOrder(
            new PlayerHistory("playerHistory1", 25, 1, "team1", 10), new PlayerHistory("playerHistory2", 25, 3, "team2", 20)
        );
    }

    [Fact]
    public async Task GetPlayerHistory_ReturnsPlayerHistory()
    {
        PlayerHistoryEntity mockPlayerHistory = new() { Id = "playerHistory1", Season = 25, Week = 1, Team = "team1", Points = 10 };

        _playerHistoryRepository.Setup(playerHistoryRepository => playerHistoryRepository.GetByIdAsync("playerHistory1"))
                .ReturnsAsync(mockPlayerHistory);

        PlayerHistory playerHistory = await _playerHistoryService.GetPlayerHistoryAsync("playerHistory1");

        playerHistory.Should().NotBeNull();
        playerHistory.Should().BeEquivalentTo(
            new PlayerHistory("playerHistory1", 25, 1, "team1", 10));
    }

    [Fact]
    public async Task AddPlayerHistory_ReturnsPlayerHistory()
    {
        PlayerHistory mockPlayerHistory = new PlayerHistory("playerHistory1", 25, 1, "team1", 10);
        PlayerHistoryEntity mockPlayerHistoryEntity = new() { Id = "playerHistory1", Season = 25, Week = 1, Team = "team1", Points = 10 };

        _playerHistoryRepository.Setup(playerHistoryRepository => playerHistoryRepository.AddAsync(It.IsAny<PlayerHistoryEntity>()))
            .ReturnsAsync((PlayerHistoryEntity entity) => entity);

        PlayerHistory playerHistory = await _playerHistoryService.AddPlayerHistoryAsync(mockPlayerHistory);

        playerHistory.Should().NotBeNull();
        playerHistory.Should().BeEquivalentTo(
            new PlayerHistory("playerHistory1", 25, 1, "team1", 10));
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