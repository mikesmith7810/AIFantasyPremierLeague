using AIFantasyPremierLeague.API.Migrations;
using AIFantasyPremierLeague.API.Repository;
using AIFantasyPremierLeague.API.Repository.Data;

namespace AIFantasyPremierLeague.API.DataGatherers;

public class OppositionAveragePointsConcededCalculator(ITeamHistoryRepository teamHistoryRepository) : ITeamCalculator
{
    public async Task<double> Calculate(int TeamId, int NumberOfWeeks)
    {
        IEnumerable<TeamHistoryEntity> teamHistoryEntities = await teamHistoryRepository.GetLastNWeeksForTeamHistoryAsync(TeamId, NumberOfWeeks);

        if (!teamHistoryEntities.Any())
            return 0f;

        return teamHistoryEntities.Average(th => th.PointsConceded);
    }
}