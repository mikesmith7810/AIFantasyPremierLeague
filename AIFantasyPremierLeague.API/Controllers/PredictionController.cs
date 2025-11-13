using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Threading.Tasks;
using AIFantasyPremierLeague.API.Models;
using AIFantasyPremierLeague.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace AIFantasyPremierLeague.API.Controllers;

[ApiController]
[Route("prediction")]
public class PredictionController : ControllerBase
{

    private readonly IPredictionService _predictionService;

    public PredictionController(IPredictionService predictionService)
    {
        _predictionService = predictionService;
    }

    [HttpGet]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<ActionResult<PredictionHighestPoints>> GetPredictions()
    {
        PredictionHighestPoints predictionHighestPoints = await _predictionService.GetPredictionHighestPointsAsync();

        if (predictionHighestPoints == null)
        {
            return NotFound();
        }

        return Ok(predictionHighestPoints);
    }
}

