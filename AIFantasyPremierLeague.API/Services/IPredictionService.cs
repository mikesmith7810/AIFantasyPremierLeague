using AIFantasyPremierLeague.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace AIFantasyPremierLeague.API.Services;
public interface IPredictionService
{
    Task<string> TrainAndCreateModelAsync();
    Task<PredictionHighestPoints> GetPredictionHighestPointsAsync();
}