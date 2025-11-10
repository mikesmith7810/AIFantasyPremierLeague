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

    [HttpGet]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<ActionResult<IEnumerable<Player>>> GetPlayers()
    {
        IEnumerable<Player> players = await _playerService.GetPlayers();

        if (players == null || !players.Any())
        {
            return NotFound();
        }
        return Ok(players);
    }
}

