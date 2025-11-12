using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Threading.Tasks;
using AIFantasyPremierLeague.API.Models;
using AIFantasyPremierLeague.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace AIFantasyPremierLeague.API.Controllers;

[ApiController]
[Route("playerPerformance")]
public class PlayerPerformanceController : ControllerBase
{

    private readonly IPlayerPerformanceService _playerPerformanceService;

    public PlayerPerformanceController(IPlayerPerformanceService playerPerformanceService)
    {
        _playerPerformanceService = playerPerformanceService;
    }

    [HttpPost]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<ActionResult<PlayerPerformance>> AddPlayerPerformance([FromBody] PlayerPerformance playerPerformance)
    {
        PlayerPerformance createdPlayerPerformance = await _playerPerformanceService.AddPlayerPerformanceAsync(playerPerformance);

        return CreatedAtAction(
            nameof(GetPlayerPerformance),
            new { id = createdPlayerPerformance.Id },
            createdPlayerPerformance
        );
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PlayerPerformance>> GetPlayerPerformance(string id)
    {
        var playerPerformance = await _playerPerformanceService.GetPlayerPerformanceAsync(id);

        if (playerPerformance == null)
        {
            return NotFound();
        }

        return Ok(playerPerformance);
    }

    [HttpGet]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<ActionResult<IEnumerable<PlayerPerformance>>> GetPlayerPerformances()
    {
        IEnumerable<PlayerPerformance> playerPerformances = await _playerPerformanceService.GetPlayerPerformancesAsync();

        if (playerPerformances == null || !playerPerformances.Any())
        {
            return NotFound();
        }
        return Ok(playerPerformances);
    }
}

