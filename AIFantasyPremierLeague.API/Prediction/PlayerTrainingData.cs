using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using Microsoft.Net.Http.Headers;

namespace AIFantasyPremierLeague.API.Prediction;

public class PlayerTrainingData
{

    public required int PlayerId { get; set; }

    public float AverageGoalsLast5Games { get; set; }

    public float AverageAssistsLast5Games { get; set; }

    public float AveragePointsLast5Games { get; set; }

    public float AverageMinsPlayedLast5Games { get; set; }

    public float AverageBonusLast5Games { get; set; }

    public float AverageCleanSheetsLast5Games { get; set; }

    public float AverageGoalsConcededLast5Games { get; set; }

    public float AverageYellowCardsLast5Games { get; set; }

    public float AverageRedCardsLast5Games { get; set; }

    public float AverageSavesLast5Games { get; set; }

    public float OppositionAveragePointsConcededLast5Games { get; set; }

    public float IsHome { get; set; }

    public float Position { get; set; }

    [ColumnName("Label")]
    public float Points { get; set; }
}

