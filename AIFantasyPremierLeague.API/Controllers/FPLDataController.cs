using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Threading.Tasks;
using AIFantasyPremierLeague.API.Models;
using AIFantasyPremierLeague.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace AIFantasyPremierLeague.API.Controllers;

[ApiController]
[Route("fpldata")]
public class FPLDataController : ControllerBase
{

    private readonly IFPLDataService _fplDataService;

    public FPLDataController(IFPLDataService fplDataService)
    {
        _fplDataService = fplDataService;
    }

    [HttpPost("/playersKnownData")]
    public async Task<ActionResult<Player>> LoadPlayersKnownData()
    {

        await _fplDataService.LoadPlayersKnownDataAsync();
        return Created();
    }

    [HttpPost("/playersPerformanceData/{gameWeek}")]
    public async Task<ActionResult<Player>> LoadPlayersPerformanceData()
    {

        await _fplDataService.LoadPlayersKnownDataAsync();
        return Created();
    }
}

