using AIFantasyPremierLeague.API.DataGatherers;
using AIFantasyPremierLeague.API.Prediction;
using AIFantasyPremierLeague.API.Repository;
using AIFantasyPremierLeague.API.Repository.Data;
using Microsoft.Extensions.ML;
using Microsoft.ML;

namespace AIFantasyPremierLeague.API.Services;

public class PredictionService(IRepository<PlayerPerformanceEntity> playerPerformanceRepository, PredictionEnginePool<PlayerTrainingData, PlayerPrediction> predictionEnginePool, AverageGoalsCalculator averageGoalsCalculator, AveragePointsCalculator averagePointsCalculator, AverageMinsPlayedCalculator averageMinsPlayedCalculator) : IPredictionService
{
    private readonly MLContext _mlContext = new MLContext();
    private ITransformer? _trainedModel;

    public async Task<string> TrainAndCreateModelAsync()
    {
        var modelPath = "fplModel.zip";

        var playerPerformanceEntities = await playerPerformanceRepository.GetAllAsync();

        List<PlayerTrainingData> playerTrainingData = await MapToTrainingData(playerPerformanceEntities);

        IDataView dataView = _mlContext.Data.LoadFromEnumerable(playerTrainingData);

        var pipeline = _mlContext.Transforms.Categorical
        .OneHotHashEncoding(
          outputColumnName: "PlayerIdEncoded",
          inputColumnName: "PlayerId",
          numberOfBits: 20)
        .Append(_mlContext.Transforms.Concatenate(
            "Features",
            "PlayerIdEncoded",
            "AverageGoalsLast5Games",
            "AveragePointsLast5Games",
            "AverageMinsPlayedLast5Games"
          ))
        .Append(_mlContext.Regression.Trainers.FastTree());

        _trainedModel = pipeline.Fit(dataView);

        var predictions = _trainedModel.Transform(dataView);
        var metrics = _mlContext.Regression.Evaluate(predictions, labelColumnName: "Label", scoreColumnName: "Score");
        Console.WriteLine($"R-Squared: {metrics.RSquared:F2}"); // Closer to 1 is better


        _mlContext.Model.Save(_trainedModel, dataView.Schema, modelPath);
        Console.WriteLine($"Model saved to {modelPath}");

        return modelPath;


    }

    public PlayerPrediction GetPredictionHighestPoints()
    {
        var futureInput = new PlayerTrainingData
        {
            PlayerId = "player414",
            AverageGoalsLast5Games = 3,
            AveragePointsLast5Games = 6,
            AverageMinsPlayedLast5Games = 79,
            Points = 0f
        };

        PlayerPrediction playerPrediction = predictionEnginePool.Predict(futureInput);
        return playerPrediction;
    }

    private async Task<List<PlayerTrainingData>> MapToTrainingData(IEnumerable<PlayerPerformanceEntity> playerPerformanceEntities)
    {
        var trainingDataList = new List<PlayerTrainingData>();
        foreach (var playerPerformanceEntity in playerPerformanceEntities)
        {
            trainingDataList.Add(new PlayerTrainingData
            {
                PlayerId = playerPerformanceEntity.PlayerId,
                AverageGoalsLast5Games = (float)await averageGoalsCalculator.CalculateAverageGoalsForPlayer(playerPerformanceEntity.PlayerId, 5),
                AveragePointsLast5Games = (float)await averagePointsCalculator.CalculateAveragePointsForPlayer(playerPerformanceEntity.PlayerId, 5),
                AverageMinsPlayedLast5Games = (float)await averageMinsPlayedCalculator.CalculateAverageMinsPlayedForPlayer(playerPerformanceEntity.PlayerId, 5),
                Points = playerPerformanceEntity.Stats.Points
            });
        }
        return trainingDataList;
    }

}

