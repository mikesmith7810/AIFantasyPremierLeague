using AIFantasyPremierLeague.API.Prediction;

namespace AIFantasyPremierLeague.API.Services;
public interface IPredictionService
{
    Task<string> TrainAndCreateModelAsync();
    Task<PlayerPrediction> GetPredictionHighestPointsAsync();
}