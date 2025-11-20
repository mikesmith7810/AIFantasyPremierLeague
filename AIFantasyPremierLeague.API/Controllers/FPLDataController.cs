using AIFantasyPremierLeague.API.Models;
using AIFantasyPremierLeague.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace AIFantasyPremierLeague.API.Controllers;

[ApiController]
[Route("fpldata")]
public class FPLDataController(IFPLDataService fplDataService) : ControllerBase
{
    [HttpPost("playersKnownData")]
    public async Task<ActionResult<Player>> LoadPlayersKnownData()
    {

        await fplDataService.LoadPlayersKnownDataAsync();
        return Created();
    }

    [HttpPost("teamsKnownData")]
    public async Task<ActionResult<Team>> LoadTeamsKnownData()
    {

        await fplDataService.LoadTeamsKnownDataAsync();
        return Created();
    }

    [HttpPost("playersPerformanceData/{gameWeek}")]
    public async Task<ActionResult<Player>> LoadPlayersPerformanceData(int gameWeek)
    {
        await fplDataService.LoadPlayersPerformanceDataAsync(gameWeek);
        return Created();
    }

    [HttpPost("teamPerformanceData/{gameWeek}")]
    public async Task<ActionResult<Player>> LoadTeamFixtureHistoryData(int gameWeek)
    {
        await fplDataService.LoadTeamHistoryData(gameWeek);
        return Created();
    }

    [HttpPost("teamFixtureData/{gameWeek}")]
    public async Task<ActionResult<Player>> LoadTeamFixtureData(int gameWeek)
    {
        await fplDataService.LoadTeamFixtureData(gameWeek);
        return Created();
    }
}

