using AIFantasyPremierLeague.API.Models;
using AIFantasyPremierLeague.API.Prediction;
using AIFantasyPremierLeague.API.Repository;
using AIFantasyPremierLeague.API.Repository.Data;
using Microsoft.Extensions.ML;
using Microsoft.ML;

namespace AIFantasyPremierLeague.API.Services;

public class PredictionService(IRepository<PlayerEntity> playerRepository, IRepository<PlayerPerformanceEntity> playerPerformanceRepository, PredictionEnginePool<PlayerTrainingData, PlayerPrediction> predictionEnginePool) : IPredictionService
{
    private readonly IRepository<PlayerEntity> _playerRepository = playerRepository;
    private readonly IRepository<PlayerPerformanceEntity> _playerPerformanceRepository = playerPerformanceRepository;

    private readonly PredictionEnginePool<PlayerTrainingData, PlayerPrediction> _predictionEnginePool = predictionEnginePool;

    private readonly MLContext _mlContext = new MLContext();
    private ITransformer? _trainedModel;

    public async Task<string> TrainAndCreateModelAsync()
    {
        var modelPath = "fplModel.zip";

        var playerPerformanceEntities = await _playerPerformanceRepository.GetAllAsync();
        List<PlayerTrainingData> playerTrainingData = MapToTrainingData(playerPerformanceEntities);

        IDataView dataView = _mlContext.Data.LoadFromEnumerable(playerTrainingData);

        var pipeline = _mlContext.Transforms.Categorical
        .OneHotHashEncoding(
          outputColumnName: "PlayerIdEncoded",
          inputColumnName: "PlayerId",
          numberOfBits: 20)
        .Append(_mlContext.Transforms.Concatenate(
          "Features",
          "PlayerIdEncoded",
          "Goals",
          "Assists",
          "MinsPlayed"))
        .Append(_mlContext.Regression.Trainers.FastTree());

        _trainedModel = pipeline.Fit(dataView);

        var predictions = _trainedModel.Transform(dataView);
        var metrics = _mlContext.Regression.Evaluate(predictions, labelColumnName: "Label", scoreColumnName: "Score");
        Console.WriteLine($"R-Squared: {metrics.RSquared:F2}"); // Closer to 1 is better


        _mlContext.Model.Save(_trainedModel, dataView.Schema, modelPath);
        Console.WriteLine($"Model saved to {modelPath}");

        return modelPath;


    }

    private static List<PlayerTrainingData> MapToTrainingData(IEnumerable<PlayerPerformanceEntity> playerPerformanceEntities)
    {
        return [.. playerPerformanceEntities.Select(playerPerformanceEntity => new PlayerTrainingData
        {
            PlayerId = playerPerformanceEntity.PlayerId,
            Goals = playerPerformanceEntity.Stats.Goals,
            Assists = playerPerformanceEntity.Stats.Assists,
            MinsPlayed = playerPerformanceEntity.Stats.MinsPlayed,
            Points = playerPerformanceEntity.Stats.Points
        })];
    }
    public PlayerPrediction GetPredictionHighestPointsAsync()
    {
        var futureInput = new PlayerTrainingData
        {
            PlayerId = "player414",
            Goals = 1f,
            Assists = 1f,
            MinsPlayed = 78f,
            Points = 0f
        };

        PlayerPrediction playerPrediction = _predictionEnginePool.Predict(futureInput);
        return playerPrediction;
    }
}

