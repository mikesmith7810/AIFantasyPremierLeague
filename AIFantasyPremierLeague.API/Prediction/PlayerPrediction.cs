using Microsoft.ML.Data;

namespace AIFantasyPremierLeague.API.Prediction;

public class PlayerPrediction
{
    // ML.NET's regression models output the predicted value into a column named "Score".
    [ColumnName("Score")]
    public float PredictedPoints { get; set; }
}