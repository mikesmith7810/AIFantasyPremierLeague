using System.Net.Mime;
using AIFantasyPremierLeague.API.Models;
using AIFantasyPremierLeague.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace AIFantasyPremierLeague.API.Controllers;

[ApiController]
[Route("team")]
public class TeamController(ITeamService teamService) : ControllerBase
{
    [HttpGet]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<ActionResult<IEnumerable<Team>>> GetTeams()
    {
        var teams = await teamService.GetTeamsAsync();

        if (teams == null || !teams.Any())
        {
            return NotFound();
        }

        return Ok(teams);
    }

    [HttpPost]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<ActionResult<Team>> AddTeam([FromBody] Team team)
    {
        var createdTeam = await teamService.AddTeamAsync(team);

        return CreatedAtAction(
            nameof(GetTeam),
            new { id = createdTeam.Id },
            createdTeam
        );
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Team>> GetTeam(int id)
    {
        var team = await teamService.GetTeamAsync(id);

        if (team == null)
        {
            return NotFound();
        }

        return Ok(team);
    }
}

