using System.Threading.Tasks;
using AIFantasyPremierLeague.API.DataGatherers;
using AIFantasyPremierLeague.API.Models;
using AIFantasyPremierLeague.API.Prediction;
using AIFantasyPremierLeague.API.Repository;
using AIFantasyPremierLeague.API.Repository.Data;
using Microsoft.Extensions.ML;
using Microsoft.ML;

namespace AIFantasyPremierLeague.API.Services;

public class PredictionService(IRepository<PlayerPerformanceEntity> playerPerformanceRepository, IRepository<PlayerEntity> playerRepository, PredictionEnginePool<PlayerTrainingData, PlayerPrediction> predictionEnginePool, AverageGoalsCalculator averageGoalsCalculator, AverageAssistsCalculator averageAssistsCalculator, AveragePointsCalculator averagePointsCalculator, AverageMinsPlayedCalculator averageMinsPlayedCalculator, AverageBonusCalculator averageBonusCalculator, AverageCleanSheetsCalculator averageCleanSheetsCalculator, AverageGoalsConcededCalculator averageGoalsConcededCalculator, AverageYellowCardsCalculator averageYellowCardsCalculator, AverageRedCardsCalculator averageRedCardsCalculator, AverageSavesCalculator averageSavesCalculator, OppositionAveragePointsConcededCalculator oppositionAveragePointsConcededCalculator, IPlayerService playerService, ILogger<PredictionService> logger) : IPredictionService
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
            "OppositionAveragePointsConcededLast5Games",
            "IsHome",
            "Position"
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

    public async Task<IEnumerable<PlayerPrediction>> GetPredictionHighestPoints(int GameWeek, string Position)
    {
        var players = await playerService.GetPlayersAsync();
        var predictions = new List<PlayerPrediction>();

        foreach (var player in players)
        {
            if (player.Position.ToString().Equals(Position))
            {
                PlayerEntity? playerEntity = await playerRepository.GetByIdAsync(player.Id);
                // get is home from team fixtures based upon gameweek form teamid in above player entity - update below.

                if (playerEntity == null) continue;

                var futureInput = new PlayerTrainingData
                {
                    PlayerId = player.Id,
                    AverageGoalsLast5Games = (float)await averageGoalsCalculator.CalculateForGameWeek(player.Id, 5, GameWeek),
                    AverageAssistsLast5Games = (float)await averageAssistsCalculator.CalculateForGameWeek(player.Id, 5, GameWeek),
                    AveragePointsLast5Games = (float)await averagePointsCalculator.CalculateForGameWeek(player.Id, 5, GameWeek),
                    AverageMinsPlayedLast5Games = (float)await averageMinsPlayedCalculator.CalculateForGameWeek(player.Id, 5, GameWeek),
                    AverageBonusLast5Games = (float)await averageBonusCalculator.CalculateForGameWeek(player.Id, 5, GameWeek),
                    AverageCleanSheetsLast5Games = (float)await averageCleanSheetsCalculator.CalculateForGameWeek(player.Id, 5, GameWeek),
                    AverageGoalsConcededLast5Games = (float)await averageGoalsConcededCalculator.CalculateForGameWeek(player.Id, 5, GameWeek),
                    AverageYellowCardsLast5Games = (float)await averageYellowCardsCalculator.CalculateForGameWeek(player.Id, 5, GameWeek),
                    AverageRedCardsLast5Games = (float)await averageRedCardsCalculator.CalculateForGameWeek(player.Id, 5, GameWeek),
                    AverageSavesLast5Games = (float)await averageSavesCalculator.CalculateForGameWeek(player.Id, 5, GameWeek),
                    OppositionAveragePointsConcededLast5Games = (float)await oppositionAveragePointsConcededCalculator.CalculateForGameWeek(player.Id, 5, GameWeek),
                    IsHome = 1,
                    Position = (float)player.Position,
                    Points = 0f
                };

                PlayerPrediction playerPrediction = predictionEnginePool.Predict(futureInput);
                playerPrediction.PlayerId = player.Id;
                playerPrediction.PlayerFirstName = player.FirstName;
                playerPrediction.PlayerSecondName = player.SecondName;
                playerPrediction.Price = player.Price;
                predictions.Add(playerPrediction);
            }
        }

        return predictions.OrderByDescending(pp => pp.PredictedPoints);
    }

    public async Task<PlayerPrediction> GetPredictionForPlayer(int PlayerId, int GameWeek)
    {
        PlayerEntity? playerEntity = await playerRepository.GetByIdAsync(PlayerId);
        // get is home from team fixtures based upon gameweek form teamid in above player entity - update below.

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
            IsHome = 1,
            Position = (float)playerEntity.Position,
            Points = 0f
        };


        PlayerPrediction playerPrediction = predictionEnginePool.Predict(futureInput);
        playerPrediction.PlayerId = PlayerId;
        playerPrediction.PlayerFirstName = playerEntity.FirstName;
        playerPrediction.PlayerSecondName = playerEntity.SecondName;
        playerPrediction.Price = playerEntity.Price;

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
                IsHome = (float)playerPerformanceEntity.IsHome,
                Position = (float)playerPerformanceEntity.Position,
                Points = playerPerformanceEntity.Stats.Points
            });
        }

        logger.LogInformation("Completed training data mapping for {TotalCount} entities", totalCount);
        return trainingDataList;
    }
}

