using AIFantasyPremierLeague.API.Models;

namespace AIFantasyPremierLeague.API.DataGatherers;

public interface ITeamCalculator
{
    Task<double> Calculate(int TeamId, int NumberOfWeeks);
}