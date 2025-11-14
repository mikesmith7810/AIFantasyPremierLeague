using AIFantasyPremierLeague.API.Models;
using AIFantasyPremierLeague.API.Prediction;
using Microsoft.AspNetCore.Mvc;

namespace AIFantasyPremierLeague.API.Services;
public interface IPredictionService
{
    Task<string> TrainAndCreateModelAsync();
    PlayerPrediction GetPredictionHighestPointsAsync();
}