using Microsoft.ML.Data;

namespace AIFantasyPremierLeague.API.Prediction;

public class PlayerTrainingData
{

    public required string PlayerId { get; set; }

    public float Goals { get; set; }

    public float Assists { get; set; }


    public float MinsPlayed { get; set; }

    [LoadColumn(5), ColumnName("Label")]
    public float Points { get; set; }
}

