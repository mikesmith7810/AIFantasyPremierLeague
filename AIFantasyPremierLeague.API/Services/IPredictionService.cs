using AIFantasyPremierLeague.API.Prediction;

namespace AIFantasyPremierLeague.API.Services;
public interface IPredictionService
{
    Task<string> TrainAndCreateModelAsync();
    Task<IEnumerable<PlayerPrediction>> GetPredictionHighestPoints(int GameWeek, string Position);

    Task<PlayerPrediction> GetPredictionForPlayer(int PlayerId, int GameWeek);
}