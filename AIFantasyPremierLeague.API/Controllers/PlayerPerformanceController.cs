using System.Net.Mime;
using AIFantasyPremierLeague.API.Models;
using AIFantasyPremierLeague.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace AIFantasyPremierLeague.API.Controllers;

[ApiController]
[Route("playerPerformance")]
public class PlayerPerformanceController(IPlayerPerformanceService playerPerformanceService) : ControllerBase
{
    [HttpPost]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<ActionResult<PlayerPerformance>> AddPlayerPerformance([FromBody] PlayerPerformance playerPerformance)
    {
        var createdPlayerPerformance = await playerPerformanceService.AddPlayerPerformanceAsync(playerPerformance);

        return CreatedAtAction(
            nameof(GetPlayerPerformance),
            new { id = createdPlayerPerformance.Id },
            createdPlayerPerformance
        );
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PlayerPerformance>> GetPlayerPerformance(int id)
    {
        var playerPerformance = await playerPerformanceService.GetPlayerPerformanceAsync(id);

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
        var playerPerformances = await playerPerformanceService.GetPlayerPerformancesAsync();

        if (playerPerformances == null || !playerPerformances.Any())
        {
            return NotFound();
        }
        return Ok(playerPerformances);
    }
}

