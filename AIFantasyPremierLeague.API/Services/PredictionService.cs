using AIFantasyPremierLeague.API.Models;
using AIFantasyPremierLeague.API.Prediction;
using AIFantasyPremierLeague.API.Repository;
using AIFantasyPremierLeague.API.Repository.Data;
using Microsoft.ML;

namespace AIFantasyPremierLeague.API.Services;

public class PredictionService : IPredictionService
{
    private readonly IRepository<PlayerEntity> _playerRepository;
    private readonly IRepository<PlayerPerformanceEntity> _playerPerformanceRepository;

    private readonly MLContext _mlContext;
    private ITransformer? _trainedModel;

    public PredictionService(IRepository<PlayerEntity> playerRepository, IRepository<PlayerPerformanceEntity> playerPerformanceRepository)
    {
        _playerRepository = playerRepository;
        _playerPerformanceRepository = playerPerformanceRepository;
        _mlContext = new MLContext();
    }

    public async Task<String> TrainAndCreateModelAsync()
    {
        var modelPath = "fplModel.zip";

        var playerPerformanceEntities = await _playerPerformanceRepository.GetAllAsync();
        List<PlayerTrainingData> playerTrainingData = MapToTrainingData(playerPerformanceEntities);

        // Add sample data if no data exists in database
        if (playerTrainingData.Count == 0)
        {
            playerTrainingData = new List<PlayerTrainingData>
            {
                new() { PlayerId = "player1", Goals = 2, Assists = 1, MinsPlayed = 90, Points = 8 },
                new() { PlayerId = "player2", Goals = 1, Assists = 2, MinsPlayed = 85, Points = 7 },
                new() { PlayerId = "player3", Goals = 0, Assists = 0, MinsPlayed = 45, Points = 2 },
                new() { PlayerId = "player4", Goals = 3, Assists = 1, MinsPlayed = 90, Points = 12 }
            };
            Console.WriteLine("Using sample training data since no database data was found.");
        }

        IDataView dataView = _mlContext.Data.LoadFromEnumerable(playerTrainingData);

        var pipeline = _mlContext.Transforms.Categorical.OneHotHashEncoding(
          outputColumnName: "PlayerIdEncoded",
          inputColumnName: "PlayerId",
          numberOfBits: 20) // Using OneHotHashEncoding for large number of categories

      // Combine ALL feature columns into a single 'Features' vector
      .Append(_mlContext.Transforms.Concatenate(
          "Features",
          "PlayerIdEncoded", // <-- Include the Player Identity
          "Goals",
          "Assists",
          "MinsPlayed"))

      // Choose the Regression trainer
      .Append(_mlContext.Regression.Trainers.FastTree());

        // 3. Train the model
        _trainedModel = pipeline.Fit(dataView);

        // 4. Evaluate the model (Optional, but recommended)
        var predictions = _trainedModel.Transform(dataView);
        var metrics = _mlContext.Regression.Evaluate(predictions, labelColumnName: "Label", scoreColumnName: "Score");
        Console.WriteLine($"R-Squared: {metrics.RSquared:F2}"); // Closer to 1 is better

        // 5. Save the trained model to a file
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

