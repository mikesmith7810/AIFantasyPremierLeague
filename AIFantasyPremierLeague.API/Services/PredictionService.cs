using AIFantasyPremierLeague.API.Models;
using AIFantasyPremierLeague.API.Prediction;
using AIFantasyPremierLeague.API.Repository;
using AIFantasyPremierLeague.API.Repository.Data;
using Microsoft.Extensions.ML;
using Microsoft.ML;

namespace AIFantasyPremierLeague.API.Services;

public class PredictionService : IPredictionService
{
    private readonly IRepository<PlayerEntity> _playerRepository;
    private readonly IRepository<PlayerPerformanceEntity> _playerPerformanceRepository;

    private readonly PredictionEnginePool<PlayerTrainingData, PlayerPrediction> _predictionEnginePool;

    private readonly MLContext _mlContext;
    private ITransformer? _trainedModel;

    public PredictionService(IRepository<PlayerEntity> playerRepository, IRepository<PlayerPerformanceEntity> playerPerformanceRepository, PredictionEnginePool<PlayerTrainingData, PlayerPrediction> predictionEnginePool)
    {
        _playerRepository = playerRepository;
        _playerPerformanceRepository = playerPerformanceRepository;
        _predictionEnginePool = predictionEnginePool;
        _mlContext = new MLContext();
    }

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
        return playerPerformanceEntities.Select(playerPerformanceEntity => new PlayerTrainingData
        {
            PlayerId = playerPerformanceEntity.PlayerId,
            Goals = playerPerformanceEntity.Stats.Goals,
            Assists = playerPerformanceEntity.Stats.Assists,
            MinsPlayed = playerPerformanceEntity.Stats.MinsPlayed,
            Points = playerPerformanceEntity.Stats.Points
        }).ToList();
    }

    public Task<PredictionHighestPoints> GetPredictionHighestPointsAsync()
    {
        throw new NotImplementedException();
    }
}

