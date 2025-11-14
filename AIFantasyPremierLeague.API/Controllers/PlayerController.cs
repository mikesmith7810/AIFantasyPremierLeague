using System.Net.Mime;
using AIFantasyPremierLeague.API.Models;
using AIFantasyPremierLeague.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace AIFantasyPremierLeague.API.Controllers;

[ApiController]
[Route("player")]
public class PlayerController(IPlayerService playerService) : ControllerBase
{
    [HttpPost]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<ActionResult<Player>> AddPlayer([FromBody] Player player)
    {
        var createdPlayer = await playerService.AddPlayerAsync(player);

        return CreatedAtAction(
            nameof(GetPlayer),
            new { id = createdPlayer.Id },
            createdPlayer
        );
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Player>> GetPlayer(string id)
    {
        var player = await playerService.GetPlayerAsync(id);

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
        var players = await playerService.GetPlayersAsync();

        if (players == null || !players.Any())
        {
            return NotFound();
        }
        return Ok(players);
    }
}

