using Microsoft.ML.Data;

namespace AIFantasyPremierLeague.API.Prediction;

public class PlayerPrediction
{
    [ColumnName("Score")]
    public float PredictedPoints { get; set; }

    public int PlayerId { get; set; }

    public string PlayerFirstName { get; set; } = string.Empty;

    public string PlayerSecondName { get; set; } = string.Empty;

    public double Price { get; set; }
}