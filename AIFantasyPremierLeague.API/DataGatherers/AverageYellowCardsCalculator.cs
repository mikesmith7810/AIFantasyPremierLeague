using AIFantasyPremierLeague.API.Repository;
using AIFantasyPremierLeague.API.Repository.Data;

namespace AIFantasyPremierLeague.API.DataGatherers;

public class AverageYellowCardsCalculator(IPlayerPerformanceRepository playerPerformanceRepository) : IPlayerCalculator
{
    public async Task<double> Calculate(int PlayerId, int NumberOfWeeks)
    {
        IEnumerable<PlayerPerformanceEntity> playerPerformanceEntities = await playerPerformanceRepository.GetLastNWeeksForPlayerAsync(PlayerId, NumberOfWeeks);

        if (!playerPerformanceEntities.Any())
            return 0f;

        return playerPerformanceEntities.Average(p => p.Stats.YellowCards);
    }

    public async Task<double> CalculateForGameWeek(int PlayerId, int NumberOfWeeks, int GameWeek)
    {
        IEnumerable<PlayerPerformanceEntity> playerPerformanceEntities = await playerPerformanceRepository.GetLastNWeeksForPlayerByGameWeekAsync(PlayerId, NumberOfWeeks, GameWeek);
        if (!playerPerformanceEntities.Any())
            return 0f;

        return playerPerformanceEntities.Average(p => p.Stats.YellowCards);
    }
}