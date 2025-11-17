using AIFantasyPremierLeague.API.Repository;
using AIFantasyPremierLeague.API.Repository.Data;

namespace AIFantasyPremierLeague.API.DataGatherers;

public class AverageAssistsCalculator(IPlayerPerformanceRepository playerPerformanceRepository) : IPerformanceCalculator
{
    public async Task<double> Calculate(string PlayerId, int NumberOfWeeks)
    {
        IEnumerable<PlayerPerformanceEntity> playerPerformanceEntities = await playerPerformanceRepository.GetLastNWeeksForPlayerAsync(PlayerId, NumberOfWeeks);

        if (!playerPerformanceEntities.Any())
            return 0f;

        return playerPerformanceEntities.Average(p => p.Stats.Assists);
    }
}