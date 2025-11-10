using Xunit;
using Moq;
using FluentAssertions;
using AIFantasyPremierLeague.API.Services;
using Microsoft.AspNetCore.Mvc;
using AIFantasyPremierLeague.API.Repository;
using AIFantasyPremierLeague.API.Repository.Data;
using AIFantasyPremierLeague.API.Models;


namespace AIFantasyPremierLeague.Tests;
public class PlayerServiceTest
{
    private readonly Mock<IRepository<PlayerEntity>> _playerRepository;
    private readonly PlayerService _playerService;


    public PlayerServiceTest()
    {
        _playerRepository = new Mock<IRepository<PlayerEntity>>();

        _playerService = new PlayerService(_playerRepository.Object);
    }


    [Fact]
    public async Task GetPlayers_ReturnsAllPlayers()
    {
        var mockPlayerEntities = new List<PlayerEntity>
        {
            new() { Id = "player1", Name = "Harry Kane", Team="team1" },
            new() { Id = "player2", Name = "Declan Rice", Team="team2" }
        };

        _playerRepository.Setup(playerRepository => playerRepository.GetAllAsync())
                .ReturnsAsync(mockPlayerEntities);

        IEnumerable<Player> players = await _playerService.GetPlayers();

        players.Should().HaveCount(2);

        players.Should().ContainInOrder(
            new Player("player1", "Harry Kane", "team1"), new Player("player2", "Declan Rice", "team2")
        );
    }
}