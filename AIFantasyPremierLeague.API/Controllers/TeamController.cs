using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Threading.Tasks;
using AIFantasyPremierLeague.API.Models;
using AIFantasyPremierLeague.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace AIFantasyPremierLeague.API.Controllers;

[ApiController]
[Route("team")]
public class TeamController : ControllerBase
{

    private readonly ITeamService _teamService;

    public TeamController(ITeamService teamService)
    {
        _teamService = teamService;
    }

    [HttpGet]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<ActionResult<IEnumerable<Team>>> GetTeams()
    {
        IEnumerable<Team> teams = await _teamService.GetTeamsAsync();

        if (teams == null || !teams.Any())
        {
            return NotFound();
        }

        return Ok(teams);
    }
}

