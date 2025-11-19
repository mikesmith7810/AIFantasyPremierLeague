using System.Net.Mime;
using System.Threading.Tasks;
using AIFantasyPremierLeague.API.Prediction;
using AIFantasyPremierLeague.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AIFantasyPremierLeague.API.Controllers;

[ApiController]
[Route("prediction")]
public class PredictionController(IPredictionService predictionService) : ControllerBase
{
    [HttpGet("all/{gameWeek}/{position}")]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<ActionResult<IEnumerable<PlayerPrediction>>> GetPredictionsForAll(int GameWeek, string Position)
    {
        var playerPrediction = await predictionService.GetPredictionHighestPoints(GameWeek, Position);

        if (playerPrediction == null)
        {
            return NotFound();
        }

        return Ok(playerPrediction);
    }

    [HttpGet("{playerId}/{gameWeek}")]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<ActionResult<PlayerPrediction>> GetPredictionForPlayer(int PlayerId, int GameWeek)
    {
        var playerPrediction = await predictionService.GetPredictionForPlayer(PlayerId, GameWeek);

        if (playerPrediction == null)
        {
            return NotFound();
        }

        return Ok(playerPrediction);
    }

    [HttpPost("train")]
    public async Task<ActionResult> TrainAndCreateModel()
    {
        var modelPath = await predictionService.TrainAndCreateModelAsync();

        return Created($"/prediction/model/{Path.GetFileName(modelPath)}", new
        {
            message = $"Model successfully trained and saved to: {modelPath}",
            modelPath,
            createdAt = DateTime.UtcNow
        });
    }
}

