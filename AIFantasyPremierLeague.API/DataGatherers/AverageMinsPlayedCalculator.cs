using System.Threading.Tasks;
using AIFantasyPremierLeague.API.Repository;
using AIFantasyPremierLeague.API.Repository.Data;
namespace AIFantasyPremierLeague.API.DataGatherers;
public class AverageMinsPlayedCalculator(IPlayerPerformanceRepository playerPerformanceRepository)
{
    public async Task<double> CalculateAverageMinsPlayedForPlayer(string PlayerId, int numberOfWeeks)
    {
        IEnumerable<PlayerPerformanceEntity> playerPerformanceEntities = await playerPerformanceRepository.GetLastNWeeksForPlayerAsync(PlayerId, numberOfWeeks);

        if (!playerPerformanceEntities.Any())
            return 0f;

        return playerPerformanceEntities.Average(p => p.Stats.MinsPlayed);
    }
}