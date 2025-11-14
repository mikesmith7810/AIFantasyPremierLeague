using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using Microsoft.Net.Http.Headers;

namespace AIFantasyPremierLeague.API.Prediction;

public class PlayerTrainingData
{

    public required string PlayerId { get; set; }

    public float AverageGoalsLast5Games { get; set; }

    public float AveragePointsLast5Games { get; set; }

    public float AverageMinsPlayedLast5Games { get; set; }

    [ColumnName("Label")]
    public float Points { get; set; }
}

