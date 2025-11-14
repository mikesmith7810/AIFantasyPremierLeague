using System.Threading.Tasks;
using AIFantasyPremierLeague.API.Repository;
using AIFantasyPremierLeague.API.Repository.Data;
namespace AIFantasyPremierLeague.API.DataGatherers;
public class AverageGoalsCalculator(IPlayerPerformanceRepository playerPerformanceRepository)
{
    public async Task<double> CalculateAverageGoalsForPlayer(string PlayerId, int numberOfWeeks)
    {
        IEnumerable<PlayerPerformanceEntity> playerPerformanceEntities = await playerPerformanceRepository.GetLastNWeeksForPlayerAsync(PlayerId, numberOfWeeks);

        if (!playerPerformanceEntities.Any())
            return 0f;

        return playerPerformanceEntities.Average(p => p.Stats.Goals);
    }
}