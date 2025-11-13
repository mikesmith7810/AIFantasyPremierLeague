using Microsoft.ML.Data;

namespace AIFantasyPremierLeague.API.Prediction;

public class PlayerPrediction
{
    [ColumnName("Score")]
    public float PredictedPoints { get; set; }
}