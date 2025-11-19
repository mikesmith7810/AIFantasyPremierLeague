using AIFantasyPremierLeague.API.Prediction;

namespace AIFantasyPremierLeague.API.Services;
public interface IPredictionService
{
    Task<string> TrainAndCreateModelAsync();
    PlayerPrediction GetPredictionHighestPoints();

    Task<PlayerPrediction> GetPredictionForPlayer(int PlayerId, int GameWeek);
}