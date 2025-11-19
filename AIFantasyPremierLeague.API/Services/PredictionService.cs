using System.Threading.Tasks;
using AIFantasyPremierLeague.API.DataGatherers;
using AIFantasyPremierLeague.API.Prediction;
using AIFantasyPremierLeague.API.Repository;
using AIFantasyPremierLeague.API.Repository.Data;
using Microsoft.Extensions.ML;
using Microsoft.ML;

namespace AIFantasyPremierLeague.API.Services;

public class PredictionService(IRepository<PlayerPerformanceEntity> playerPerformanceRepository, PredictionEnginePool<PlayerTrainingData, PlayerPrediction> predictionEnginePool, AverageGoalsCalculator averageGoalsCalculator, AverageAssistsCalculator averageAssistsCalculator, AveragePointsCalculator averagePointsCalculator, AverageMinsPlayedCalculator averageMinsPlayedCalculator, AverageBonusCalculator averageBonusCalculator, AverageCleanSheetsCalculator averageCleanSheetsCalculator, AverageGoalsConcededCalculator averageGoalsConcededCalculator, AverageYellowCardsCalculator averageYellowCardsCalculator, AverageRedCardsCalculator averageRedCardsCalculator, AverageSavesCalculator averageSavesCalculator, OppositionAveragePointsConcededCalculator oppositionAveragePointsConcededCalculator, ILogger<PredictionService> logger) : IPredictionService
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
            "AverageAssistsLast5Games",
            "AveragePointsLast5Games",
            "AverageMinsPlayedLast5Games",
            "AverageBonusLast5Games",
            "AverageCleanSheetsLast5Games",
            "AverageGoalsConcededLast5Games",
            "AverageYellowCardsLast5Games",
            "AverageRedCardsLast5Games",
            "AverageSavesLast5Games",
            "OppositionAveragePointsConcededLast5Games"
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
            PlayerId = 414,
            AverageGoalsLast5Games = 3,
            AverageAssistsLast5Games = 0,
            AveragePointsLast5Games = 6,
            AverageMinsPlayedLast5Games = 79,
            AverageBonusLast5Games = 0,
            AverageCleanSheetsLast5Games = 0,
            AverageGoalsConcededLast5Games = 0,
            AverageYellowCardsLast5Games = 0,
            AverageRedCardsLast5Games = 0,
            AverageSavesLast5Games = 0,
            OppositionAveragePointsConcededLast5Games = 0,
            Points = 0f
        };

        PlayerPrediction playerPrediction = predictionEnginePool.Predict(futureInput);
        return playerPrediction;
    }

    public async Task<PlayerPrediction> GetPredictionForPlayer(int PlayerId, int GameWeek)
    {


        var futureInput = new PlayerTrainingData
        {
            PlayerId = PlayerId,
            AverageGoalsLast5Games = (float)await averageGoalsCalculator.CalculateForGameWeek(PlayerId, 5, GameWeek),
            AverageAssistsLast5Games = (float)await averageAssistsCalculator.CalculateForGameWeek(PlayerId, 5, GameWeek),
            AveragePointsLast5Games = (float)await averagePointsCalculator.CalculateForGameWeek(PlayerId, 5, GameWeek),
            AverageMinsPlayedLast5Games = (float)await averageMinsPlayedCalculator.CalculateForGameWeek(PlayerId, 5, GameWeek),
            AverageBonusLast5Games = (float)await averageBonusCalculator.CalculateForGameWeek(PlayerId, 5, GameWeek),
            AverageCleanSheetsLast5Games = (float)await averageCleanSheetsCalculator.CalculateForGameWeek(PlayerId, 5, GameWeek),
            AverageGoalsConcededLast5Games = (float)await averageGoalsConcededCalculator.CalculateForGameWeek(PlayerId, 5, GameWeek),
            AverageYellowCardsLast5Games = (float)await averageYellowCardsCalculator.CalculateForGameWeek(PlayerId, 5, GameWeek),
            AverageRedCardsLast5Games = (float)await averageRedCardsCalculator.CalculateForGameWeek(PlayerId, 5, GameWeek),
            AverageSavesLast5Games = (float)await averageSavesCalculator.CalculateForGameWeek(PlayerId, 5, GameWeek),
            OppositionAveragePointsConcededLast5Games = (float)await oppositionAveragePointsConcededCalculator.CalculateForGameWeek(PlayerId, 5, GameWeek),
            Points = 0f
        };

        PlayerPrediction playerPrediction = predictionEnginePool.Predict(futureInput);
        return playerPrediction;
    }

    private async Task<List<PlayerTrainingData>> MapToTrainingData(IEnumerable<PlayerPerformanceEntity> playerPerformanceEntities)
    {
        var trainingDataList = new List<PlayerTrainingData>();
        var entityList = playerPerformanceEntities.ToList();
        var totalCount = entityList.Count;
        var progressInterval = Math.Max(1, totalCount / 10); // Every 10%

        logger.LogInformation("Starting training data mapping for {TotalCount} player performance entities", totalCount);

        for (int i = 0; i < entityList.Count; i++)
        {
            var playerPerformanceEntity = entityList[i];

            // Log progress every 10%
            if ((i + 1) % progressInterval == 0 || i == entityList.Count - 1)
            {
                var progressPercentage = Math.Round(((double)(i + 1) / totalCount) * 100, 1);
                logger.LogInformation("Processing training data: {Progress}% ({Current}/{Total})",
                    progressPercentage, i + 1, totalCount);
            }

            trainingDataList.Add(new PlayerTrainingData
            {
                PlayerId = playerPerformanceEntity.PlayerId,
                AverageGoalsLast5Games = (float)await averageGoalsCalculator.Calculate(playerPerformanceEntity.PlayerId, 5),
                AverageAssistsLast5Games = (float)await averageAssistsCalculator.Calculate(playerPerformanceEntity.PlayerId, 5),
                AveragePointsLast5Games = (float)await averagePointsCalculator.Calculate(playerPerformanceEntity.PlayerId, 5),
                AverageMinsPlayedLast5Games = (float)await averageMinsPlayedCalculator.Calculate(playerPerformanceEntity.PlayerId, 5),
                AverageBonusLast5Games = (float)await averageBonusCalculator.Calculate(playerPerformanceEntity.PlayerId, 5),
                AverageCleanSheetsLast5Games = (float)await averageCleanSheetsCalculator.Calculate(playerPerformanceEntity.PlayerId, 5),
                AverageGoalsConcededLast5Games = (float)await averageGoalsConcededCalculator.Calculate(playerPerformanceEntity.PlayerId, 5),
                AverageYellowCardsLast5Games = (float)await averageYellowCardsCalculator.Calculate(playerPerformanceEntity.PlayerId, 5),
                AverageRedCardsLast5Games = (float)await averageRedCardsCalculator.Calculate(playerPerformanceEntity.PlayerId, 5),
                AverageSavesLast5Games = (float)await averageSavesCalculator.Calculate(playerPerformanceEntity.PlayerId, 5),
                OppositionAveragePointsConcededLast5Games = (float)await oppositionAveragePointsConcededCalculator.Calculate(playerPerformanceEntity.OpponentTeam, 5),
                Points = playerPerformanceEntity.Stats.Points
            });
        }

        logger.LogInformation("Completed training data mapping for {TotalCount} entities", totalCount);
        return trainingDataList;
    }
}

