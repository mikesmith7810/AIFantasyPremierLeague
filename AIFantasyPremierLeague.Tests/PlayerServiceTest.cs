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

public class PlayerServiceTest : DataSupplier
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
            PlayerEntity1(),
            PlayerEntity2()
        };

        _playerRepository.Setup(playerRepository => playerRepository.GetAllAsync())
                .ReturnsAsync(mockPlayerEntities);

        IEnumerable<Player> players = await _playerService.GetPlayersAsync();

        players.Should().HaveCount(2);

        players.Should().ContainInOrder(
            Player1(), Player2()
        );
    }

    [Fact]
    public async Task GetPlayer_ReturnsPlayer()
    {
        PlayerEntity mockPlayer = PlayerEntity1();

        _playerRepository.Setup(playerRepository => playerRepository.GetByIdAsync("player1"))
                .ReturnsAsync(mockPlayer);

        Player player = await _playerService.GetPlayerAsync("player1");

        player.Should().NotBeNull();
        player.Should().BeEquivalentTo(
            Player1());
    }

    [Fact]
    public async Task AddPlayer_ReturnsPlayer()
    {
        Player mockPlayer = Player1();
        PlayerEntity mockPlayerEntity = PlayerEntity1();

        _playerRepository.Setup(playerRepository => playerRepository.AddAsync(It.IsAny<PlayerEntity>()))
            .ReturnsAsync((PlayerEntity entity) => entity);

        Player player = await _playerService.AddPlayerAsync(mockPlayer);

        player.Should().NotBeNull();
        player.Should().BeEquivalentTo(
            Player1());
    }

    [Fact]
    public async Task GetNonExistingPlayer_ThrowsPlayerNotFoundException()
    {
        _playerRepository.Setup(playerRepository => playerRepository.GetByIdAsync("player1"))
                .ReturnsAsync((PlayerEntity)null);

        await _playerService.Invoking(s => s.GetPlayerAsync("player1"))
                      .Should().ThrowAsync<PlayerNotFoundException>()
                      .WithMessage($"Player with ID 'player1' was not found");
    }
}