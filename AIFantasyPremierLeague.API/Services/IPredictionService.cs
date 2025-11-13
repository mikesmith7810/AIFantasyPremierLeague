using AIFantasyPremierLeague.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace AIFantasyPremierLeague.API.Services;
public interface IPredictionService
{
    Task<PredictionHighestPoints> GetPredictionHighestPointsAsync();
}