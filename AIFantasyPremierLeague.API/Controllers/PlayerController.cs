using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Threading.Tasks;
using AIFantasyPremierLeague.API.Models;
using AIFantasyPremierLeague.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace AIFantasyPremierLeague.API.Controllers;

[ApiController]
[Route("player")]
public class PlayerController : ControllerBase
{

    private readonly IPlayerService _playerService;

    public PlayerController(IPlayerService playerService)
    {
        _playerService = playerService;
    }

    [HttpPost]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<ActionResult<Player>> AddPlayer([FromBody] Player player)
    {
        Player createdPlayer = await _playerService.AddPlayerAsync(player);

        return CreatedAtAction(
            nameof(GetPlayer),
            new { id = createdPlayer.Id },
            createdPlayer
        );
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Player>> GetPlayer(string id)
    {
        var player = await _playerService.GetPlayerAsync(id);

        if (player == null)
        {
            return NotFound();
        }

        return Ok(player);
    }

    [HttpGet]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<ActionResult<IEnumerable<Player>>> GetPlayers()
    {
        IEnumerable<Player> players = await _playerService.GetPlayersAsync();

        if (players == null || !players.Any())
        {
            return NotFound();
        }
        return Ok(players);
    }
}

