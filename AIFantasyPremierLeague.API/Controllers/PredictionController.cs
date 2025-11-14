using System.Net.Mime;
using AIFantasyPremierLeague.API.Prediction;
using AIFantasyPremierLeague.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace AIFantasyPremierLeague.API.Controllers;

[ApiController]
[Route("prediction")]
public class PredictionController(IPredictionService predictionService) : ControllerBase
{
    [HttpGet]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<ActionResult<PlayerPrediction>> GetPredictions()
    {
        var playerPrediction = predictionService.GetPredictionHighestPointsAsync();

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

