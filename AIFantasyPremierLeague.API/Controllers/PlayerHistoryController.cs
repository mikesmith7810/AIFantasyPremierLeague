using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Threading.Tasks;
using AIFantasyPremierLeague.API.Models;
using AIFantasyPremierLeague.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace AIFantasyPremierLeague.API.Controllers;

[ApiController]
[Route("playerHistory")]
public class PlayerHistoryController : ControllerBase
{

    private readonly IPlayerHistoryService _playerHistoryService;

    public PlayerHistoryController(IPlayerHistoryService playerHistoryService)
    {
        _playerHistoryService = playerHistoryService;
    }

    [HttpPost]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<ActionResult<PlayerHistory>> AddPlayerHistory([FromBody] PlayerHistory playerHistory)
    {
        PlayerHistory createdPlayerHistory = await _playerHistoryService.AddPlayerHistoryAsync(playerHistory);

        return CreatedAtAction(
            nameof(GetPlayerHistory),
            new { id = createdPlayerHistory.Id },
            createdPlayerHistory
        );
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PlayerHistory>> GetPlayerHistory(string id)
    {
        var playerHistory = await _playerHistoryService.GetPlayerHistoryAsync(id);

        if (playerHistory == null)
        {
            return NotFound();
        }

        return Ok(playerHistory);
    }

    [HttpGet]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<ActionResult<IEnumerable<PlayerHistory>>> GetPlayerHistorys()
    {
        IEnumerable<PlayerHistory> playerHistorys = await _playerHistoryService.GetPlayerHistorysAsync();

        if (playerHistorys == null || !playerHistorys.Any())
        {
            return NotFound();
        }
        return Ok(playerHistorys);
    }
}

